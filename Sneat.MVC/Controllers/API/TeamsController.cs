using Sneat.MVC.Common;
using Sneat.MVC.DAL;
using Sneat.MVC.Models.APIModel;
using Sneat.MVC.Models.DTO.Team;
using Sneat.MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Sneat.MVC.Controllers.API
{
    public class TeamsController : ApiController
    {
        protected SneatContext Context;
        public TeamService _teamService;
        public ResponseService _responseService;

        public SneatContext GetContext()
        {
            if (Context == null)
            {
                Context = new SneatContext();
            }
            return Context;
        }

        public TeamsController()
        {
            _teamService = new TeamService(this.GetContext());
            _responseService = new ResponseService();
        }

        [HttpGet]
        public async Task<JsonResultModel> GetListTechStack()
        {
            var result = Helper.GetTechStackModels();
            return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, result);
        }

        [HttpGet]
        public async Task<JsonResultModel> GetListTeamPaging(
            int page = SystemParam.PAGE_DEFAULT, 
            int limit = SystemParam.MAX_ROW_IN_LIST_WEB, 
            string search = ""
            )
        {
            var listPaging = await _teamService.SearchTeam(page, limit, search);
            var paging = new PagingModel
            {
                Page = page,
                Limit = limit,
                TotalItemCount = listPaging.TotalItemCount,
            };

            return _responseService.SuccessPaging(SystemParam.MESSAGE_SUCCESS, listPaging, paging);
        }

        [HttpPost]
        public async Task<JsonResultModel> CreateTeam(TeamInputModel input)
        {
            var result = await _teamService.CreateTeam(input);
            if (result == SystemParam.EXISTED_TEAM_NAME_ERR)
                return _responseService.ErrorResult(SystemParam.EXISTED_TEAM_NAME_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.RETURN_TRUE)
                return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, result);
            else
                return _responseService.serverError();
        }

        [HttpPost]
        public async Task<JsonResultModel> UpdateTeam(TeamOutputModel input)
        {
            var result = await _teamService.UpdateTeam(input);
            if (result == SystemParam.EXISTED_TEAM_NAME_ERR)
                return _responseService.ErrorResult(SystemParam.EXISTED_TEAM_NAME_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.TEAM_NOT_FOUND_ERR)
                return _responseService.ErrorResult(SystemParam.TEAM_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.RETURN_TRUE)
                return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, result);
            else
                return _responseService.serverError();
        }

        [HttpGet]
        public async Task<JsonResultModel> DetailTeam(int ID)
        {
            var result = await _teamService.DetailTeam(ID);
            return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, result);
        }

        [HttpPost]
        public async Task<JsonResultModel> RemoveTeam(int ID)
        {
            var result = await _teamService.RemoveTeam(ID);
            if (result == SystemParam.TEAM_NOT_FOUND_ERR)
                return _responseService.ErrorResult(SystemParam.TEAM_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.RETURN_TRUE)
                return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, result);
            else
                return _responseService.serverError();
        }
    }
}