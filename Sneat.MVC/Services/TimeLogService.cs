using Microsoft.Ajax.Utilities;
using PagedList;
using Sneat.MVC.Common;
using Sneat.MVC.DAL;
using Sneat.MVC.Models.APIModel;
using Sneat.MVC.Models.DTO.TimeLog;
using Sneat.MVC.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sneat.MVC.Services
{
    public class TimeLogService
    {
        private readonly SneatContext _dbContext;
        public TimeLogService(SneatContext dbContext = null)
        {
            if (dbContext == null)
            {
                dbContext = new SneatContext();
            }
            _dbContext = dbContext;
        }

        public async Task<JsonResultModel> GetListTimeLogPaging(
            int? taskID,
            int? memberID,
            int page,
            int limit
            )
        {
            try
            {
                var list = await GetListTimeLog(taskID, memberID);
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

        public async Task<List<TimeLogOutputModel>> GetListTimeLog(
            int? taskID,
            int? memberID
            )
        {
            try
            {
                var listTimeLog = _dbContext.TimeLogs
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                        && (taskID.HasValue ? x.WorkPackageID == taskID : true)
                        && (memberID.HasValue ? x.MemberID == memberID : true)
                        );

                var listResult = listTimeLog
                    .Select(x => new TimeLogOutputModel
                    {
                        ID = x.ID,
                        TotalWorkingTime = x.TotalWorkingTime,
                        LogDate = x.LogDate,
                        MemberID = x.MemberID,
                        UserName = x.User.UserName,
                        CreatedByID = x.CreatedByID,
                        WorkPackageID = x.WorkPackageID,
                        Hours = x.Hours,
                        CreateDate = x.CreatedDate,
                        WorkPackageName = x.WorkPackage != null ? x.WorkPackage.Subject : string.Empty,
                        ProjectID = x.WorkPackage != null ? (x.WorkPackage.WorkPackageID.HasValue ? x.WorkPackage.ParentWorkPackage.Sprint.Project.ID : x.WorkPackage.Sprint.Project.ID) : (int?)null,
                        ProjectName = x.WorkPackage != null ? (x.WorkPackage.WorkPackageID.HasValue ? x.WorkPackage.ParentWorkPackage.Sprint.Project.Name : x.WorkPackage.Sprint.Project.Name) : null,
                        UserID = x.MemberID,
                    })
                    .OrderByDescending(x => x.ID)
                    .ToList();

                return listResult;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new List<TimeLogOutputModel>();
            }
        }

        public async Task<JsonResultModel> CreateTimeLog(TimeLogInputModel input)
        {
            try
            {
                var newTimeLog = new TimeLog
                {
                    Hours = input.Hours,
                    TotalWorkingTime = input.TotalWorkingTime,
                    LogDate = input.LogDate,
                    MemberID = input.MemberID,
                    CreatedByID = input.CreatedByID,
                    WorkPackageID = input.WorkPackageID,

                    IsDeleted = SystemParam.IS_NOT_DELETED,
                    CreatedDate = DateTime.Now
                };
                _dbContext.TimeLogs.Add(newTimeLog);

                await _dbContext.SaveChangesAsync();
                return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, newTimeLog);
            }
            catch (Exception ex)
            {
                return JsonResponse.Exception(ex.ToString());
            }
        }

        public async Task<JsonResultModel> UpdateTimeLog(UpdateTimeLogModel input)
        {
            try
            {
                var timeLog = await _dbContext.TimeLogs
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                        && x.ID == input.ID)
                    .FirstOrDefaultAsync();
                if (timeLog == null)
                    return JsonResponse.ErrorResult(SystemParam.TIME_LOG_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);

                timeLog.Hours = input.Hours;
                timeLog.LogDate = input.LogDate;
                timeLog.UpdatedDate = DateTime.Now;

                await _dbContext.SaveChangesAsync();
                return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, null);
            }
            catch (Exception ex)
            {
                return JsonResponse.Exception(ex.ToString());
            }
        }

        public async Task<JsonResultModel> DetailTimeLog(int ID)
        {
            try
            {
                var timeLog = await _dbContext.TimeLogs
                   .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                       && x.ID == ID)
                   .FirstOrDefaultAsync();
                if (timeLog == null)
                    return JsonResponse.ErrorResult(SystemParam.TIME_LOG_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);

                var timeLogDetail = new TimeLogOutputModel
                {
                    ID = ID,
                    Hours = timeLog.Hours,
                    TotalWorkingTime = timeLog.TotalWorkingTime,
                    LogDate = timeLog.LogDate,
                    MemberID = timeLog.MemberID,
                    CreatedByID = timeLog.CreatedByID,
                    WorkPackageID = timeLog.WorkPackageID,
                    CreateDate = timeLog.CreatedDate,
                    UpdatedDate = timeLog.UpdatedDate,
                };
                return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, timeLogDetail);
            }
            catch (Exception ex)
            {
                return JsonResponse.Exception(ex.ToString());
            }
        }

        public async Task<JsonResultModel> DeleteTimeLog(int ID)
        {
            try
            {
                var timeLog = await _dbContext.TimeLogs
                   .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                       && x.ID == ID)
                   .FirstOrDefaultAsync();
                if (timeLog == null)
                    return JsonResponse.ErrorResult(SystemParam.TIME_LOG_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);

                timeLog.IsDeleted = SystemParam.IS_DELETED;

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