using PagedList;
using Sneat.MVC.Common;
using Sneat.MVC.DAL;
using Sneat.MVC.Models.APIModel;
using Sneat.MVC.Models.DTO.Sprint;
using Sneat.MVC.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace Sneat.MVC.Services
{
    public class SprintService
    {
        private readonly SneatContext _dbContext;

        public SprintService(SneatContext dbContext = null)
        {
            if (dbContext == null)
            {
                dbContext = new SneatContext();
            }
            _dbContext = dbContext;
        }

        public async Task<JsonResultModel> GetListSprintPaging(
             string search = "",
             int? projectID = null,
             int page = SystemParam.PAGE_DEFAULT,
             int limit = SystemParam.MAX_ROW_IN_LIST_WEB
            )
        {
            try
            {
                var list = await GetListSprint(search, projectID);
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

        public async Task<List<SprintOutputModel>> GetListSprint(string search, int? projectID)
        {
            try
            {
                search = Utils.RemoveDiacritics(search);
                var listSprint = _dbContext.Sprints
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                        && (!string.IsNullOrEmpty(search) ?
                         x.Title.Contains(search) : true)
                        && (projectID.HasValue ? x.ProjectID == projectID.Value : true)
                        );

                var listResult = listSprint
                    .Select(x => new SprintOutputModel
                    {
                        ID = x.ID,
                        Title = x.Title,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        Status = x.Status,
                        CreatedDate = x.CreatedDate,
                        UpdatedDate = x.UpdatedDate,
                        ProjectID = x.ProjectID,
                        ProjectName = x.Project != null ? x.Project.Name : string.Empty
                    })
                    .OrderByDescending(x => x.ID)
                    .ToList();

                return listResult;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new List<SprintOutputModel>();
            }
        }

        public async Task<JsonResultModel> CreateSprint(SprintInputModel input)
        {
            try
            {
                var existedSprint = await _dbContext.Sprints
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                        && x.Title.ToLower() == input.Title.ToLower())
                    .CountAsync();
                if (existedSprint > 0)
                    input.Title = $"{input.Title}({existedSprint + 1})";

                var newSprint = new Sprint
                {
                    Title = input.Title,
                    StartDate = input.StartDate,
                    EndDate = input.EndDate,
                    Status = input.Status,
                    ProjectID = input.ProjectID,

                    IsDeleted = SystemParam.IS_NOT_DELETED,
                    CreatedDate = DateTime.Now
                };
                _dbContext.Sprints.Add(newSprint);

                await _dbContext.SaveChangesAsync();
                return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, newSprint);
            }
            catch (Exception ex)
            {
                return JsonResponse.Exception(ex.ToString());
            }
        }

        public async Task<JsonResultModel> UpdateSprint(SprintOutputModel input)
        {
            try
            {
                var existedSprint = await _dbContext.Sprints
                  .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                      && x.Title.ToLower() == input.Title.ToLower()
                      && x.ID != input.ID)
                  .CountAsync();
                if (existedSprint > 0)
                    input.Title = $"{input.Title}({existedSprint + 1})";

                var sprint = await _dbContext.Sprints
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                        && x.ID == input.ID)
                    .FirstOrDefaultAsync();
                if (sprint == null)
                    return JsonResponse.ErrorResult(SystemParam.SPRINT_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);

                sprint.Title = input.Title;
                sprint.StartDate = input.StartDate;
                sprint.EndDate = input.EndDate;
                sprint.Status = input.Status;
                sprint.UpdatedDate = DateTime.Now;

                await _dbContext.SaveChangesAsync();
                return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, sprint);
            }
            catch (Exception ex)
            {
                return JsonResponse.Exception(ex.ToString());
            }
        }

        public async Task<JsonResultModel> DetailSprint(int ID)
        {
            try
            {
                var sprint = await _dbContext.Sprints
                    .FirstOrDefaultAsync(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                    && x.ID == ID);
                if (sprint == null)
                    return JsonResponse.ErrorResult(SystemParam.SPRINT_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);

                var sprintDetail = new SprintOutputModel
                {
                    ID = ID,
                    Title = sprint.Title,
                    StartDate = sprint.StartDate,
                    EndDate = sprint.EndDate,
                    Status = sprint.Status,
                    ProjectID = sprint.ProjectID,
                    CreatedDate = sprint.CreatedDate,
                    UpdatedDate = sprint.UpdatedDate,
                };
                return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, sprintDetail);
            }
            catch (Exception ex)
            {
                return JsonResponse.Exception(ex.ToString());
            }
        }

        public async Task<JsonResultModel> DeleteSprint(int ID)
        {
            try
            {
                var sprint = await _dbContext.Sprints
                   .FirstOrDefaultAsync(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                   && x.ID == ID);
                if (sprint == null)
                    return JsonResponse.ErrorResult(SystemParam.SPRINT_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);

                sprint.IsDeleted = SystemParam.IS_DELETED;

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