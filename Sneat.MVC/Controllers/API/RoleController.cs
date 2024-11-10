using Sneat.MVC.Common;
using Sneat.MVC.Models.APIModel;
using Sneat.MVC.Models.DTO.Role;
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
    public class RoleController : ApiController
    {
        private readonly RoleService _roleService;
        public RoleController()
        {
            _roleService = new RoleService();
        }

        [HttpGet]
        public async Task<JsonResultModel> GetListRoles(
            int page = SystemParam.PAGE_DEFAULT,
            int limit = SystemParam.MAX_ROW_IN_LIST_WEB,
            string search = null)
        {
            var list =  await _roleService.SearchRole(page, limit, search);
            var paging = new PagingModel
            {
                Page = page,
                Limit = limit,
                TotalItemCount = list.TotalItemCount,
            };

            return JsonResponse.SucessPaging(list, paging);
        }

        [HttpPost]
        public async Task<JsonResultModel> CreateRole(RoleInputModel input)
        {
            var result = await _roleService.CreateRole(input);

            if (result == SystemParam.EXISTED_ROLE_NAME_ERR)
                return JsonResponse.ErrorResult(SystemParam.EXISTED_ROLE_NAME_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.RETURN_TRUE)
                return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, result);
            else
                return JsonResponse.ServerError();
        }

        [HttpPost]
        public async Task<JsonResultModel> UpdateRole(RoleOutputModel input)
        {
            var result = await _roleService.UpdateRole(input);

            if (result == SystemParam.EXISTED_ROLE_NAME_ERR)
                return JsonResponse.ErrorResult(SystemParam.EXISTED_ROLE_NAME_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.ROLE_NOT_FOUND_ERR)
                return JsonResponse.ErrorResult(SystemParam.ROLE_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.RETURN_TRUE)
                return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, result);
            else
                return JsonResponse.ServerError();
        }

        [HttpGet]
        public async Task<JsonResultModel> DetailRole(int ID)
        {
            var result = await _roleService.DetailRole(ID);
            return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, result);
        }

        [HttpPost]
        public async Task<JsonResultModel> RemoveRole(int ID)
        {
            var result = await _roleService.RemoveRole(ID);
            if (result == SystemParam.ROLE_NOT_FOUND_ERR)
                return JsonResponse.ErrorResult(SystemParam.ROLE_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.RETURN_TRUE)
                return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, result);
            else
                return JsonResponse.ServerError();
        }

        [HttpGet]
        public async Task<JsonResultModel> GetAllPermission()
        {
            var result = await _roleService.GetAllPermissions();
            return JsonResponse.Success(SystemParam.MESSAGE_SUCCESS, result);
        }
    }
}
