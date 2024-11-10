using Sneat.MVC.Common;
using Sneat.MVC.Models.APIModel;
using Sneat.MVC.Models.DTO.TimeLog;
using Sneat.MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Sneat.MVC.Controllers.API
{
    public class TimeLogController : ApiController
    {
        private readonly TimeLogService _timeLogService;

        public TimeLogController()
        {
            _timeLogService = new TimeLogService();
        }

        [HttpGet]
        public async Task<JsonResultModel> GetListTimeLogPaging(
            int? taskID = null,
            int? memberID = null,
            int page = SystemParam.PAGE_DEFAULT,
            int limit = SystemParam.MAX_ROW_IN_LIST_WEB
            )
        {
            return await _timeLogService.GetListTimeLogPaging(taskID, memberID, page, limit);
        }

        [HttpPost]
        public async Task<JsonResultModel> CreateTimeLog(TimeLogInputModel input)
        {
            return await _timeLogService.CreateTimeLog(input);
        }

        [HttpPost]
        public async Task<JsonResultModel> UpdateTimeLog(UpdateTimeLogModel input)
        {
            return await _timeLogService.UpdateTimeLog(input);
        }

        [HttpGet]
        public async Task<JsonResultModel> DetailTimeLog(int ID)
        {
            return await _timeLogService.DetailTimeLog(ID);
        }

        [HttpPost]
        public async Task<JsonResultModel> DeleteTimeLog(int ID)
        {
            return await _timeLogService.DeleteTimeLog(ID);
        }
    }
}