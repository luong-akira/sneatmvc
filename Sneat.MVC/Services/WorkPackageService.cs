using PagedList;
using Sneat.MVC.Common;
using Sneat.MVC.DAL;
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
    public class WorkPackageService
    {
        private readonly SneatContext _dbContext;
        public WorkPackageService(SneatContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IPagedList<WorkPackageOutputModel>> SearchWorkPackage(
            int page, int limit, string search = "", int? projectID = null, int? priorityType = null
            )
        {
            try
            {
                var list = await GetListWorkPackage(search, projectID, priorityType);
                var listPaging = list.ToPagedList(page, limit);

                return listPaging;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new List<WorkPackageOutputModel>().ToPagedList(1,1);
            }
        }

        public async Task<List<WorkPackageOutputModel>> GetListWorkPackage(
            string search = "",
            int? projectID = null,
            int? priorityType = null,
            string asigneeIDs = null)
        {
            try
            {
                search = Utils.RemoveDiacritics(search);
                var list = _dbContext.WorkPackages
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED)
                    .Where(x => projectID.HasValue ? x.ProjectID == projectID.Value : true)
                    .Where(x => priorityType.HasValue ? x.PriorityPoint == priorityType.Value : true)
                    .Select(x => new
                    {
                        x.ID,
                        x.Subject,
                        x.Type,
                        x.Status,
                        x.EstimateTime,
                        x.SpentTime,
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
                        x.ProjectID,
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
                        AssigneeID = x.Assignee.AssigneeID,
                        AssigneeName = x.Assignee.AssigneeName,
                        AssigneeAvatar = x.Assignee.AssigneeAvatar,
                        AssigneeEmail = x.Assignee.AssigneeEmail,
                        ProjectID = x.ProjectID,
                    })
                    .Where(x => string.IsNullOrEmpty(search)
                     || Utils.RemoveDiacritics(x.Subject).Contains(search)
                     || Utils.RemoveDiacritics(x.AssigneeName).Contains(search)
                     || Utils.RemoveDiacritics(x.AssigneeEmail).Contains(search))
                    .ToList();

                return list;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new List<WorkPackageOutputModel>();
            }
        }

        public async Task<int> CreateWorkPackage(WorkPackageInputModel input)
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

                    ProjectID = input.ProjectID,
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

                await _dbContext.SaveChangesAsync();
                return SystemParam.RETURN_TRUE;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return SystemParam.RETURN_FALSE;
            }
        }

        public async Task<WorkPackageOutputModel> DetailWorkPackage(int ID)
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

                return taskDetail;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new WorkPackageOutputModel();
            }
        }

        public async Task<int> UpdateWorkPackage(WorkPackageOutputModel input)
        {
            try
            {
                var task = await _dbContext.WorkPackages
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                        && x.ID == input.ID)
                    .FirstOrDefaultAsync();
                if (task == null)
                    return SystemParam.TASK_NOT_FOUND_ERR;

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

                _dbContext.UserWorkPackages.Remove(task.UserWorkPackages.FirstOrDefault(x => x.AssignType == WorkAssignType.Assignee));
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