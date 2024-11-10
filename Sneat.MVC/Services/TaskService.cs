using PagedList;
using Sneat.MVC.Common;
using Sneat.MVC.DAL;
using Sneat.MVC.Models.APIModel;
using Sneat.MVC.Models.DTO.Project;
using Sneat.MVC.Models.DTO.Task;
using Sneat.MVC.Models.DTO.WorkPackage;
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
    public class TaskService
    {
        private readonly SneatContext _dbContext;
        public TaskService(SneatContext dbContext = null)
        {
            if (dbContext == null)
            {
                dbContext = new SneatContext();
            }
            _dbContext = dbContext;
        }

        public async Task<JsonResultModel> GetListTaskPaging(string search, int? projectID, int? sprintID, int? assigneeID, int? parentID, int? status,
            string assigneeIds, string memberIds, int page, int limit)
        {
            try
            {
                var list = await GetListTask(search, projectID, sprintID, assigneeID, parentID, status, assigneeIds, memberIds);
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
                return JsonResponse.Exception(ex.ToString());
            }
        }

        public async Task<List<WorkPackageOutputModel>> GetListTask(
             string search,
            int? projectID,
            int? sprintID,
            int? assigneeID,
            int? parentID,
            int? status,
            string assigneeIds,
            string memberIds
            )
        {
            try
            {
                List<int> listAssigneeIds = assigneeIds?.Split(',')
                    .Select(int.Parse)
                    .ToList() ?? new List<int>();

                List<int> listMemberIds = memberIds?.Split(',')
                    .Select(int.Parse)
                    .ToList() ?? new List<int>();

                search = Utils.RemoveDiacritics(search);

                var listTask = _dbContext.WorkPackages
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                        && x.Type == WorkPackageType.Task
                        )
                    .Include(x => x.UserWorkPackages)
                    .Include(x => x.Sprint.Project.UserProjects)
                    .Select(x => new
                    {
                        x.ID,
                        x.Subject,
                        x.Type,
                        x.Status,
                        x.EstimateTime,
                        x.RemainingTime,
                        x.PriorityPoint,
                        x.StartDate,
                        x.FinishDate,
                        x.Description,
                        x.CreatedDate,
                        x.CompletePercent,
                        Assignee = x.UserWorkPackages
                          .Where(a => a.AssignType == WorkAssignType.Assignee)
                          .Select(a => new
                          {
                              AssigneeID = a.UserID,
                              AssigneeName = !string.IsNullOrEmpty(a.User.UserName) ? a.User.UserName : "---",
                              AssigneeAvatar = !string.IsNullOrEmpty(a.User.Avatar) ? a.User.Avatar : SystemParam.DEFAULT_SYSTEM_IMAGE,
                              AssigneeEmail = !string.IsNullOrEmpty(a.User.Email) ? a.User.Email : "---",
                          })
                          .FirstOrDefault(),
                        AssigneeID = x.UserWorkPackages.Where(a => a.AssignType == WorkAssignType.Assignee).FirstOrDefault() != null ?
                            x.UserWorkPackages.Where(a => a.AssignType == WorkAssignType.Assignee).Select(a => a.UserID).FirstOrDefault() : (int?)null,
                        SpentTime = x.TimeLogs.Where(a => a.IsDeleted == 0).Count() > 0 ? x.TimeLogs.Where(a => a.IsDeleted == 0).Sum(a => a.Hours) : 0,
                        x.WorkPackageID,
                        ProjectID = x.Sprint != null ? x.Sprint.ProjectID : (int?)null,
                        x.SprintID,
                        ListUsers = x.Sprint.Project.UserProjects.Select(a => new ProjectUserModel
                        {
                            UserID = a.UserID,
                            UserAvatar = a.User.Avatar,
                            UserName = a.User.UserName
                        }
                      ).ToList(),
                        MemberIds = x.UserWorkPackages.Where(a => a.AssignType == WorkAssignType.Member).Select(a => a.UserID).ToList()
                    })
                  .AsEnumerable()
                  .Select(x => new WorkPackageOutputModel
                  {
                      ID = x.ID,
                      Subject = x.Subject,
                      Type = (int)x.Type,
                      Status = (int)x.Status,
                      EstimateTime = x.EstimateTime,
                      SpentTime = x.SpentTime,
                      RemainingTime = x.RemainingTime,
                      PriorityPoint = x.PriorityPoint,
                      StartDate = x.StartDate,
                      FinishDate = x.FinishDate,
                      Description = x.Description,
                      CreateDate = x.CreatedDate,
                      CompletePercent = x.CompletePercent,
                      AssigneeID = x.AssigneeID,
                      AssigneeName = x.Assignee != null ? x.Assignee.AssigneeName : null,
                      AssigneeAvatar = x.Assignee != null ? x.Assignee.AssigneeAvatar : null,
                      AssigneeEmail = x.Assignee != null ? x.Assignee.AssigneeEmail : null,
                      ProjectID = x.ProjectID,
                      WorPackageID = x.WorkPackageID,
                      ListUsers = x.ListUsers,
                      MemberIds = x.MemberIds
                  }).ToList();

                var list = _dbContext.WorkPackages
                  .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED && (parentID.HasValue == false ? x.Type == WorkPackageType.UserStory : true))
                  .Where(x =>
                  (projectID.HasValue ? x.Sprint.ProjectID == projectID.Value : true)
                  && (sprintID.HasValue ? x.SprintID == sprintID.Value : true)
                  && (parentID.HasValue ? (x.WorkPackageID == parentID && x.Type == WorkPackageType.Task) : true)
                  && (status.HasValue ? (x.Status == (WorkPackageStatus)status.Value) : true)
                  && (listMemberIds.Count() > 0 ?
                        _dbContext.UserWorkPackages.Where(a => a.AssignType == WorkAssignType.Member && listMemberIds.Contains(a.UserID) == true && (
                             a.WorkPackage.WorkPackageID.HasValue ? a.WorkPackage.WorkPackageID == x.ID : a.WorkPackage.ID == x.ID)).FirstOrDefault() != null
                        : true)

                  && (listAssigneeIds.Count() > 0 ?
                        _dbContext.UserWorkPackages.Where(a => a.AssignType == WorkAssignType.Assignee && listAssigneeIds.Contains(a.UserID) == true && (
                             a.WorkPackage.WorkPackageID.HasValue ? a.WorkPackage.WorkPackageID == x.ID : a.WorkPackage.ID == x.ID)).FirstOrDefault() != null
                        : true)

                    )
                  .Include(x => x.UserWorkPackages)
                  .Select(x => new
                  {
                      x.ID,
                      x.Subject,
                      x.Type,
                      x.Status,
                      x.EstimateTime,
                      x.RemainingTime,
                      x.PriorityPoint,
                      x.StartDate,
                      x.FinishDate,
                      x.Description,
                      x.CreatedDate,
                      x.CompletePercent,
                      Assignee = x.UserWorkPackages
                          .Where(a => a.AssignType == WorkAssignType.Assignee)
                          .Select(a => new
                          {
                              AssigneeID = a.UserID,
                              AssigneeName = !string.IsNullOrEmpty(a.User.UserName) ? a.User.UserName : "---",
                              AssigneeAvatar = !string.IsNullOrEmpty(a.User.Avatar) ? a.User.Avatar : SystemParam.DEFAULT_SYSTEM_IMAGE,
                              AssigneeEmail = !string.IsNullOrEmpty(a.User.Email) ? a.User.Email : "---",
                          })
                          .FirstOrDefault(),
                      AssigneeID = x.UserWorkPackages.Where(a => a.AssignType == WorkAssignType.Assignee).FirstOrDefault() != null ?
                            x.UserWorkPackages.Where(a => a.AssignType == WorkAssignType.Assignee).Select(a => a.UserID).FirstOrDefault() : (int?)null,
                      SpentTime = x.TimeLogs.Where(a => a.IsDeleted == 0).Count() > 0 ? x.TimeLogs.Where(a => a.IsDeleted == 0).Sum(a => a.Hours) : 0,
                      x.WorkPackageID,
                      x.SprintID,
                      ProjectID = x.Sprint != null ? x.Sprint.ProjectID : (int?)null,
                      ListUsers = x.Sprint.Project.UserProjects.Select(a => new ProjectUserModel
                      {
                          UserID = a.UserID,
                          UserAvatar = a.User.Avatar,
                          UserName = a.User.UserName
                      }
                      ).ToList(),
                      MemberIds = x.UserWorkPackages.Where(a => a.AssignType == WorkAssignType.Member).Select(a => a.UserID).ToList()
                  })
                  .AsEnumerable()
                  .Select(x => new WorkPackageOutputModel
                  {
                      ID = x.ID,
                      Subject = x.Subject,
                      Type = (int)x.Type,
                      Status = (int)x.Status,
                      EstimateTime = x.EstimateTime,
                      SpentTime = x.SpentTime,
                      RemainingTime = x.RemainingTime,
                      PriorityPoint = x.PriorityPoint,
                      StartDate = x.StartDate,
                      FinishDate = x.FinishDate,
                      Description = x.Description,
                      CreateDate = x.CreatedDate,
                      CompletePercent = x.CompletePercent,
                      AssigneeID = x.AssigneeID,
                      AssigneeName = x.Assignee != null ? x.Assignee.AssigneeName : null,
                      AssigneeAvatar = x.Assignee != null ? x.Assignee.AssigneeAvatar : null,
                      AssigneeEmail = x.Assignee != null ? x.Assignee.AssigneeEmail : null,
                      ProjectID = x.ProjectID,
                      SprintID = x.SprintID,
                      ListTasks = listTask.Where(t => t.WorPackageID == x.ID).OrderByDescending(t => t.ID).ToList(),
                      ListUsers = x.ListUsers,
                      MemberIds = x.MemberIds
                  })
                  .Where(x => string.IsNullOrEmpty(search)
                   || Utils.RemoveDiacritics(x.Subject).Contains(search)
                   || Utils.RemoveDiacritics(x.AssigneeName).Contains(search)
                   || Utils.RemoveDiacritics(x.AssigneeEmail).Contains(search))
                  .Where(x => assigneeID.HasValue ? x.AssigneeID == assigneeID : true)
                  .OrderByDescending(t => t.ID)
                  .ToList();

                return list;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new List<WorkPackageOutputModel>();
            }
        }

        public async Task<JsonResultModel> CreateTask(WorkPackageInputModel input)
        {
            try
            {
                var newTask = new WorkPackage
                {
                    Subject = input.Subject,
                    Type = (WorkPackageType)input.Type,
                    Status = WorkPackageStatus.New,
                    EstimateTime = input.EstimateTime,
                    SpentTime = input.SpentTime,
                    RemainingTime = input.RemainingTime,
                    CompletePercent = input.EstimateTime.HasValue && input.SpentTime.HasValue ?
                        (input.SpentTime / input.EstimateTime.Value) * 100 : 0,
                    PriorityPoint = input.PriorityPoint,
                    Description = input.Description,
                    StartDate = input.StartDate,
                    FinishDate = input.FinishDate,
                    IsDeleted = SystemParam.IS_NOT_DELETED,
                    CreatedDate = DateTime.Now,
                    WorkPackageID = input.WorPackageID,
                    ProjectID = input.ProjectID,
                    SprintID = input.SprintID,
                };

                _dbContext.WorkPackages.Add(newTask);

                if (input.AssigneeID.HasValue)
                {
                    var assigneeTask = new UserWorkPackage
                    {
                        UserID = input.AssigneeID.Value,
                        WorkPackageID = newTask.ID,
                        AssignType = WorkAssignType.Assignee,
                    };
                    _dbContext.UserWorkPackages.Add(assigneeTask);
                }

                if (input.AssignorID.HasValue)
                {
                    var assignorTask = new UserWorkPackage
                    {
                        UserID = input.AssignorID.Value,
                        WorkPackageID = newTask.ID,
                        AssignType = WorkAssignType.Assignor,
                    };
                    _dbContext.UserWorkPackages.Add(assignorTask);
                }

                if (input.MemberIds != null)
                {
                    if (input.MemberIds.Count > 0)
                    {
                        foreach (var memberID in input.MemberIds)
                        {
                            var memberTask = new UserWorkPackage
                            {
                                UserID = memberID,
                                WorkPackageID = newTask.ID,
                                AssignType = WorkAssignType.Member,
                            };
                            _dbContext.UserWorkPackages.Add(memberTask);
                        }
                    }
                }

                await _dbContext.SaveChangesAsync();
                return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, null);
            }
            catch (Exception ex)
            {
                return JsonResponse.Exception(ex.ToString());
            }
        }

        public async Task<JsonResultModel> UpdateTask(WorkPackageOutputModel input)
        {
            try
            {
                var task = await _dbContext.WorkPackages
                  .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                      && x.ID == input.ID)
                  .FirstOrDefaultAsync();
                if (task == null)
                    return JsonResponse.ErrorResult(SystemParam.TASK_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);

                task.Subject = input.Subject;
                task.Status = (WorkPackageStatus)input.Status;
                task.EstimateTime = input.EstimateTime;
                task.SpentTime = input.SpentTime;
                task.RemainingTime = input.RemainingTime;
                task.PriorityPoint = input.PriorityPoint;
                task.StartDate = input.StartDate;
                task.FinishDate = input.FinishDate;
                task.Description = input.Description;
                task.UpdatedDate = DateTime.Now;

                var workPackage = task.UserWorkPackages.FirstOrDefault(x => x.AssignType == WorkAssignType.Assignee);

                if (workPackage != null)
                {
                    _dbContext.UserWorkPackages.Remove(workPackage);
                }
                if (input.AssigneeID.HasValue)
                {
                    var assigneeTask = new UserWorkPackage
                    {
                        UserID = input.AssigneeID.Value,
                        WorkPackageID = task.ID,
                        AssignType = WorkAssignType.Assignee,
                    };
                    _dbContext.UserWorkPackages.Add(assigneeTask);
                }

                _dbContext.UserWorkPackages.RemoveRange(task.UserWorkPackages.Where(x => x.AssignType == WorkAssignType.Member));
                if (input.MemberIds.Count > 0)
                {
                    foreach (var memberID in input.MemberIds)
                    {
                        var memberTask = new UserWorkPackage
                        {
                            UserID = memberID,
                            WorkPackageID = task.ID,
                            AssignType = WorkAssignType.Member,
                        };
                        _dbContext.UserWorkPackages.Add(memberTask);
                    }
                }

                await _dbContext.SaveChangesAsync();
                return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, null);
            }
            catch (Exception ex)
            {
                return JsonResponse.Exception(ex.ToString());
            }
        }

        public async Task<JsonResultModel> UpdateTaskStatus(UpdateTaskStatusModel model)
        {
            try
            {
                var task = await _dbContext.WorkPackages
                      .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                          && x.ID == model.ID)
                      .FirstOrDefaultAsync();
                if (task == null)
                    return JsonResponse.ErrorResult(SystemParam.TASK_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);

                task.Status = (WorkPackageStatus)model.Status;
                await _dbContext.SaveChangesAsync();
                return JsonResponse.Success(null);
            }
            catch (Exception ex)
            {
                return JsonResponse.Exception(ex.ToString());
            }
        }

        public async Task<JsonResultModel> UpdateUserStoryGeneralInfo(UpdateUserStoryGeneralInfoModel model)
        {
            try
            {
                var task = await _dbContext.WorkPackages
                      .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                          && x.ID == model.ID)
                      .FirstOrDefaultAsync();
                if (task == null)
                    return JsonResponse.ErrorResult(SystemParam.TASK_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);

                task.Subject = model.Subject;
                task.Status = (WorkPackageStatus)model.Status;
                await _dbContext.SaveChangesAsync();
                return JsonResponse.Success(null);
            }
            catch (Exception ex)
            {
                return JsonResponse.Exception(ex.ToString());
            }
        }

        public async Task<JsonResultModel> DetailTask(int ID)
        {
            try
            {
                var task = await _dbContext.WorkPackages
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                        && x.ID == ID)
                    .FirstOrDefaultAsync();

                var assignee = task.UserWorkPackages
                            .Where(a => a.AssignType == WorkAssignType.Assignee)
                            .Select(a => new
                            {
                                AssigneeID = a.UserID,
                                AssigneeName = !string.IsNullOrEmpty(a.User.UserName) ? a.User.UserName : "---",
                                AssigneeAvatar = !string.IsNullOrEmpty(a.User.Avatar) ? a.User.Avatar : SystemParam.DEFAULT_SYSTEM_IMAGE,
                                AssigneeEmail = !string.IsNullOrEmpty(a.User.Email) ? a.User.Email : "---",
                            })
                            .FirstOrDefault();

                var taskDetail = new WorkPackageOutputModel
                {
                    ID = task.ID,
                    Subject = task.Subject,
                    Type = (int)task.Type,
                    Status = (int)task.Status,
                    EstimateTime = task.EstimateTime,
                    SpentTime = task.SpentTime,
                    RemainingTime = task.RemainingTime,
                    PriorityPoint = task.PriorityPoint,
                    StartDate = task.StartDate,
                    FinishDate = task.FinishDate,
                    Description = task.Description,
                    CreateDate = task.CreatedDate,
                    CompletePercent = task.CompletePercent,
                    AssigneeID = assignee.AssigneeID,
                    AssigneeName = assignee.AssigneeName,
                    AssigneeEmail = assignee.AssigneeEmail,
                    AssigneeAvatar = assignee.AssigneeAvatar,
                    ProjectID = task.ProjectID,
                };

                return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, task);
            }
            catch (Exception ex)
            {
                return JsonResponse.Exception(ex.ToString());
            }
        }

        public async Task<JsonResultModel> DeleteTask(int ID)
        {
            try
            {
                var task = await _dbContext.WorkPackages
                   .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                       && x.ID == ID)
                   .FirstOrDefaultAsync();
                if (task == null)
                    return JsonResponse.ErrorResult(SystemParam.TASK_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);

                task.IsDeleted = SystemParam.IS_DELETED;

                await _dbContext.SaveChangesAsync();
                return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, SystemParam.RETURN_TRUE);
            }
            catch (Exception ex)
            {
                return JsonResponse.Exception(ex.ToString());
            }
        }

    }
}