using PagedList;
using Sneat.MVC.Common;
using Sneat.MVC.DAL;
using Sneat.MVC.Models.APIModel;
using Sneat.MVC.Models.DTO.Project;
using Sneat.MVC.Models.DTO.User;
using Sneat.MVC.Models.Entity;
using Sneat.MVC.Models.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sneat.MVC.Services
{
    public class ProjectService
    {
        private readonly SneatContext _dbContext;
        public ProjectService(SneatContext dbContext = null)
        {
            if (dbContext == null)
            {
                dbContext = new SneatContext();
            }
            _dbContext = dbContext;
        }

        public async Task<IPagedList<ProjectOutputModel>> SearchProject(int page, int limit, string search = "")
        {
            try
            {
                var list = await GetListProject(search);
                var listPaging = list.ToPagedList(page, limit);

                return listPaging;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new List<ProjectOutputModel>().ToPagedList(1, 1);
            }
        }

        public async Task<JsonResultModel> GetListProject(int page, int limit, string search = "")
        {
            try
            {
                var list = await GetListProject(search);
                var listPaging = list.ToPagedList(page, limit);
                var paging = new PagingModel
                {
                    Page = page,
                    Limit = limit,
                    TotalItemCount = listPaging.TotalItemCount,
                };

                return JsonResponse.SucessPaging(listPaging, paging);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return JsonResponse.Error(SystemParam.ERROR, ex.ToString());
            }
        }

        public async Task<List<ProjectOutputModel>> PersonalProjects(int userID)
        {
            try
            {
                var projectIds = await _dbContext.UserProjects
                    .Where(x => x.UserID == userID
                        && x.Project.IsDeleted == SystemParam.IS_NOT_DELETED)
                    .Select(x => x.ProjectID)
                    .ToListAsync();

                var list = await GetListProject(null);
                var listUserProjects = list.Where(x => projectIds.Contains(x.ID)).ToList();

                return listUserProjects;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new List<ProjectOutputModel>();
            }
        }

        public async Task<List<ProjectOutputModel>> GetListProject(string search = "")
        {
            try
            {
                search = Utils.RemoveDiacritics(search);
                var listProjects = _dbContext.Projects
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED)
                    .Select(x => new
                    {
                        x.ID,
                        x.Name,
                        x.Status,
                        x.Description,
                        x.CreatedDate,
                        x.UpdatedDate,
                        UserIds = x.UserProjects.Select(a => a.UserID).ToList(),
                        ListUsers = x.UserProjects.Select(a => new ProjectUserModel
                        {
                            UserID = a.UserID,
                            UserName = a.User.UserName,
                            UserAvatar = a.User.Avatar
                        }).ToList()
                    })
                    .AsEnumerable()
                    .Select(x => new ProjectOutputModel
                    {
                        ID = x.ID,
                        Name = x.Name,
                        Description = x.Description,
                        Status = (int)x.Status,
                        CreatedDate = x.CreatedDate,
                        UpdatedDate = x.UpdatedDate,
                        UserIds = x.UserIds,
                        ListUsers = x.ListUsers
                    })
                    .Where(x => string.IsNullOrEmpty(search)
                        || Utils.RemoveDiacritics(x.Name).Contains(search))
                    .OrderByDescending(x => x.ID)
                    .ToList();

                return listProjects;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new List<ProjectOutputModel>();
            }
        }

        public async Task<int> CreateProject(ProjectInputModel input)
        {
            try
            {
                var existedProject = await _dbContext.Projects
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                         && x.Name.ToLower() == input.Name.ToLower())
                    .FirstOrDefaultAsync();
                if (existedProject != null)
                    return SystemParam.EXISTED_PROJECT_NAME_ERR;

                var newProject = new Project
                {
                    Name = input.Name,
                    Description = input.Description,
                    Status = Status.ACTIVE,
                    IsDeleted = SystemParam.IS_NOT_DELETED,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };

                if (input.TeamIds != null)
                {
                    newProject.TeamIds = String.Join(",", input.TeamIds);
                }

                _dbContext.Projects.Add(newProject);

                if (input.UserIds.Count > 0)
                {
                    foreach (var id in input.UserIds)
                    {
                        var userProject = new UserProject
                        {
                            UserID = id,
                            ProjectID = newProject.ID,
                            ProjectRole = ProjectRole.Member
                        };
                        _dbContext.UserProjects.Add(userProject);
                    }
                }

                var pmProject = new UserProject
                {
                    UserID = input.PMId,
                    ProjectID = newProject.ID,
                    ProjectRole = ProjectRole.PM
                };
                _dbContext.UserProjects.Add(pmProject);

                await _dbContext.SaveChangesAsync();
                return SystemParam.RETURN_TRUE;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return SystemParam.RETURN_FALSE;
            }
        }

        public async Task<int> UpdateProject(ProjectOutputModel input)
        {
            try
            {
                var existedProject = await _dbContext.Projects
                     .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                          && x.Name.ToLower() == input.Name.ToLower()
                          && x.ID != input.ID)
                     .FirstOrDefaultAsync();
                if (existedProject != null)
                    return SystemParam.EXISTED_PROJECT_NAME_ERR;

                var project = await _dbContext.Projects
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                        && x.ID == input.ID)
                    .FirstOrDefaultAsync();
                if (project == null)
                    return SystemParam.PROJECT_NOT_FOUND_ERR;

                project.Name = input.Name;
                project.Description = input.Description;
                project.UpdatedDate = DateTime.Now;
                if (input.TeamIds != null)
                {
                    project.TeamIds = String.Join(",", input.TeamIds);
                }

                _dbContext.UserProjects.RemoveRange(project.UserProjects);

                if (input.UserIds.Count > 0)
                {
                    foreach (var id in input.UserIds)
                    {
                        var userProject = new UserProject
                        {
                            UserID = id,
                            ProjectID = project.ID
                        };
                        _dbContext.UserProjects.Add(userProject);
                    }
                }

                await _dbContext.SaveChangesAsync();
                return SystemParam.RETURN_TRUE;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return SystemParam.RETURN_FALSE;
            }
        }

        public ProjectOutputModel DetailProject(int ID)
        {
            try
            {
                var project = _dbContext.Projects
                    .FirstOrDefault(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                        && x.ID == ID);
                var projectDetail = new ProjectOutputModel
                {
                    ID = ID,
                    Name = project.Name,
                    Description = project.Description,
                    Status = (int)project.Status,
                    CreatedDate = project.CreatedDate,
                    UpdatedDate = project.UpdatedDate,
                    UserIds = project.UserProjects.Select(a => a.UserID).ToList(),
                    TeamIds = !string.IsNullOrEmpty(project.TeamIds) ? project.TeamIds.Split(',').Select(int.Parse).ToList() : new List<int>()
                };

                return projectDetail;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new ProjectOutputModel();
            }
        }

        public async Task<int> RemoveProject(int ID)
        {
            try
            {
                var project = await _dbContext.Projects
                    .Where(x => x.ID == ID && x.IsDeleted == SystemParam.IS_NOT_DELETED)
                    .FirstOrDefaultAsync();
                if (project == null)
                    return SystemParam.PROJECT_NOT_FOUND_ERR;

                project.IsDeleted = SystemParam.IS_DELETED;
                _dbContext.UserProjects.RemoveRange(project.UserProjects);

                await _dbContext.SaveChangesAsync();
                return SystemParam.RETURN_TRUE;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return SystemParam.RETURN_FALSE;
            }
        }

        public IPagedList<UserDetailOutputModel> SearchUserProject(int page, int limit, int projectId, string search = "")
        {
            try
            {
                search = Utils.RemoveDiacritics(search);
                var list = GetListUserProject(projectId, search);
                var listPaging = list.ToPagedList(page, limit);

                return listPaging;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new List<UserDetailOutputModel>().ToPagedList(1, 1);
            }
        }

        public List<UserDetailOutputModel> GetListUserProject(int projectId, string search = "")
        {
            try
            {
                var userIds = _dbContext.UserProjects
                    .AsNoTracking()
                    .Where(x => x.User.IsDeleted == SystemParam.IS_NOT_DELETED
                        && x.User.Status == Status.ACTIVE
                        && x.ProjectID == projectId)
                    .Select(x => x.UserID)
                    .ToList();

                var users = (from u in _dbContext.Users
                             where userIds.Contains(u.ID)
                             orderby u.ID descending
                             select new
                             {
                                 IsDeleted = u.IsDeleted,
                                 ID = u.ID,
                                 Status = (int?)u.Status,
                                 UserName = u.UserName,
                                 Phone = u.Phone,
                                 CreateDate = u.CreatedDate,
                                 Email = u.Email,
                                 Avatar = u.Avatar
                             })
                             .AsEnumerable()
                             .Select(u => new UserDetailOutputModel
                             {
                                 ID = u.ID,
                                 IsDeleted = u.IsDeleted,
                                 Status = (int?)u.Status,
                                 UserName = u.UserName,
                                 Phone = u.Phone,
                                 CreateDate = u.CreateDate,
                                 Email = u.Email,
                                 Avatar = u.Avatar
                             })
                            .Where(x => string.IsNullOrEmpty(search)
                                || Utils.RemoveDiacritics(x.UserName).Contains(search)
                                || Utils.RemoveDiacritics(x.Phone).Contains(search)
                                || Utils.RemoveDiacritics(x.Email).Contains(search))
                            .ToList();

                return users;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new List<UserDetailOutputModel>();
            }
        }

        public async Task<int> AddUserProject(List<int> userIds, int projectID)
        {
            try
            {
                var project = await _dbContext.Projects
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED && x.ID == projectID)
                    .FirstOrDefaultAsync();

                var existedUserProject = await _dbContext.UserProjects
                    .Where(x => x.ProjectID == projectID)
                    .Select(x => x.UserID)
                    .ToListAsync();

                if (project == null)
                    return SystemParam.PROJECT_NOT_FOUND_ERR;

                if (userIds.Count > 0)
                {
                    foreach (var userId in userIds)
                    {
                        if (!existedUserProject.Contains(userId))
                        {
                            var userProject = new UserProject
                            {
                                UserID = userId,
                                ProjectID = projectID,
                            };
                            _dbContext.UserProjects.Add(userProject);
                        }
                    }
                }

                await _dbContext.SaveChangesAsync();
                return SystemParam.RETURN_TRUE;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return SystemParam.RETURN_FALSE;
            }
        }

    }
}