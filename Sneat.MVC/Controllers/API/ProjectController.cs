using Sneat.MVC.Common;
using Sneat.MVC.Models.APIModel;
using Sneat.MVC.Models.DTO.Project;
using Sneat.MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sneat.MVC.Controllers.API
{
    public class ProjectController : ApiController
    {
        private readonly ProjectService _projectService;
        private readonly AuthenticationService _authenticationService;

        public ProjectController()
        {
            _projectService = new ProjectService();
            _authenticationService = new AuthenticationService();
        }

        [HttpGet]
        public async Task<JsonResultModel> GetListProject(
            int page = SystemParam.PAGE_DEFAULT, 
            int limit = SystemParam.MAX_ROW_IN_LIST_WEB, 
            string search = "")
        {
            return await _projectService.GetListProject(page, limit, search);
        }

        [HttpPost]
        public async Task<JsonResultModel> CreateProject(ProjectInputModel input)
        {
            var token = Utils.getTokenApp(Request.Headers);
            var userID = await _authenticationService.GetUserLoginID(token);
            if (userID > SystemParam.RETURN_FALSE)
                input.PMId = userID;
            var result = await _projectService.CreateProject(input);

            if (result == SystemParam.EXISTED_PROJECT_NAME_ERR)
                return JsonResponse.ErrorResult(SystemParam.EXISTED_PROJECT_NAME_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.RETURN_TRUE)
                return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, result);
            else
                return JsonResponse.ServerError();

        }

        [HttpPost]
        public async Task<JsonResultModel> UpdateProject(ProjectOutputModel input)
        {
            var result = await _projectService.UpdateProject(input);

            if (result == SystemParam.EXISTED_PROJECT_NAME_ERR)
                return JsonResponse.ErrorResult(SystemParam.EXISTED_PROJECT_NAME_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.PROJECT_NOT_FOUND_ERR)
                return JsonResponse.ErrorResult(SystemParam.PROJECT_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.RETURN_TRUE)
                return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, result);
            else
                return JsonResponse.ServerError();
        }

        [HttpGet]
        public JsonResultModel ProjectDetail(int ID)
        {
            var result = _projectService.DetailProject(ID);
            return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, result);
        }

        [HttpGet]
        public JsonResultModel ListUserProject(int projectID)
        {
            var result = _projectService.GetListUserProject(projectID);
            return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, result);
        }

        [HttpPost]
        public async Task<JsonResultModel> RemoveProject(int ID)
        {
            var result = await _projectService.RemoveProject(ID);
            if (result == SystemParam.PROJECT_NOT_FOUND_ERR)
                return JsonResponse.ErrorResult(SystemParam.PROJECT_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.RETURN_TRUE)
                return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, result);
            else
                return JsonResponse.ServerError();
        }

        #region Personal Project management
        [HttpGet]
        public async Task<JsonResultModel> PersonalProjects()
        {
            var token = Utils.getTokenApp(Request.Headers);
            var userID = await _authenticationService.GetUserLoginID(token);
            var result = new List<ProjectOutputModel>();
            if (userID > SystemParam.RETURN_FALSE)
                result = await _projectService.PersonalProjects(userID);

            return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, result);
        }

        #endregion
    }
}
