using Sneat.MVC.Common;
using Sneat.MVC.Models.APIModel;
using Sneat.MVC.Models.DTO.Sprint;
using Sneat.MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Sneat.MVC.Controllers.API
{
    public class SprintController : ApiController
    {
        private readonly SprintService _sprintService;

        public SprintController()
        {
            _sprintService = new SprintService();
        }

        [HttpGet]
        public async Task<JsonResultModel> GetListSprintPaging(
           string search = "",
           int? projectID = null,
           int page = SystemParam.PAGE_DEFAULT,
           int limit = SystemParam.MAX_ROW_IN_LIST_WEB
          )
        {
            return await _sprintService.GetListSprintPaging( search, projectID, page, limit );
        }

        [HttpPost]
        public async Task<JsonResultModel> CreateSprint(SprintInputModel input)
        {
            return await _sprintService.CreateSprint( input );
        }

        [HttpPost]
        public async Task<JsonResultModel> UpdateSprint(SprintOutputModel input)
        {
            return await _sprintService.UpdateSprint(input);
        }

        [HttpGet]
        public async Task<JsonResultModel> DetailSprint(int ID)
        {
            return await _sprintService.DetailSprint(ID);
        }

        [HttpPost]
        public async Task<JsonResultModel> DeleteSprint(int ID)
        {
            return await _sprintService.DeleteSprint(ID);
        }
    }
}