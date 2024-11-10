using Sneat.MVC.Common;
using Sneat.MVC.DAL;
using Sneat.MVC.Models.APIModel;
using Sneat.MVC.Models.DTO.Home;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sneat.MVC.Services
{
    public class HomeService
    {
        private readonly SneatContext _dbContext;
        public HomeService(SneatContext dbContext = null)
        {
            if (dbContext == null)
            {
                dbContext = new SneatContext();
            }
            _dbContext = dbContext;
        }

        public async Task<JsonResultModel> GetHome()
        {
            try
            {
                var countProject = _dbContext.Projects.Where(x => x.IsDeleted == 0).Count();
                var countUsers = _dbContext.Users.Where(x => x.IsDeleted == 0).Count();
                var countBacklog = _dbContext.Sprints.Where(x => x.IsDeleted == 0).Count();
                var countWorkpackage = _dbContext.WorkPackages.Where(x => x.IsDeleted == 0 && x.WorkPackageID == null).Count();

                return JsonResponse.Success(new { countProject, countUsers, countBacklog, countWorkpackage });
            }
            catch (Exception ex)
            {
                return JsonResponse.Error(SystemParam.ERROR, ex.ToString());
            }
        }

        public async Task<JsonResultModel> GetBacklog()
        {
            try
            {
                var backlogs = _dbContext.Sprints.Where(x => x.IsDeleted == 0).OrderByDescending(x => x.ID).Take(15).ToList();

                var listBacklogs = new List<ChartDataModel>();

                foreach (var backlog in backlogs)
                {
                    var data = new ChartDataModel
                    {
                        Label = backlog.Title,
                        Number = _dbContext.WorkPackages.Where(x => x.IsDeleted == 0 && x.WorkPackageID == null && x.SprintID == backlog.ID).Count(),
                    };

                    listBacklogs.Add(data);
                }

                return JsonResponse.Success(listBacklogs);
            }
            catch (Exception ex)
            {
                return JsonResponse.Error(SystemParam.ERROR, ex.ToString());
            }
        }

        public async Task<JsonResultModel> GetWorkpackages()
        {
            try
            {
                var workPackages = _dbContext.WorkPackages.Where(x => x.IsDeleted == 0 && x.WorkPackageID == null).OrderByDescending(x => x.ID).Take(15).ToList();

                var listWorkpackages = new List<ChartDataModel>();

                foreach (var workPackage in workPackages)
                {
                    var data = new ChartDataModel
                    {
                        Label = workPackage.Subject,
                        Number = _dbContext.WorkPackages.Where(x => x.IsDeleted == 0 && x.WorkPackageID == workPackage.ID).Count(),
                    };

                    listWorkpackages.Add(data);
                }

                return JsonResponse.Success(listWorkpackages);
            }
            catch (Exception ex)
            {
                return JsonResponse.Error(SystemParam.ERROR, ex.ToString());
            }
        }
    }
}