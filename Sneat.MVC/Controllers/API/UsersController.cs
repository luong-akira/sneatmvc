using Sneat.MVC.Common;
using Sneat.MVC.DAL;
using Sneat.MVC.Models.APIModel;
using Sneat.MVC.Models.DTO.User;
using Sneat.MVC.Models.Entity;
using Sneat.MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace Sneat.MVC.Controllers.API
{
    public class UsersController : ApiController
    {
        protected SneatContext Context;
        public UserService _userService;
        public AuthenticationService _authenticationService;
        public ResponseService _responseService;

        public SneatContext GetContext()
        {
            if (Context == null)
            {
                Context = new SneatContext();
            }
            return Context;
        }

        public UsersController()
        {
            _userService = new UserService(this.GetContext());
            _responseService = new ResponseService();
            _authenticationService = new AuthenticationService(this.GetContext());
        }

        [HttpGet]
        public async Task<JsonResultModel> GetListUserPaging(int page = SystemParam.PAGE_DEFAULT, int limit = SystemParam.MAX_ROW_IN_LIST_WEB, string search = "", string teamIDs = null, int? projectID = null)
        {
            try
            {
                var listPaging = _userService.Search(page, limit, search, projectID, teamIDs);
                var paging = new PagingModel
                {
                    Page = page,
                    Limit = limit,
                    TotalItemCount = listPaging.TotalItemCount,
                };

                return _responseService.SuccessPaging(SystemParam.MESSAGE_SUCCESS, listPaging, paging);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return _responseService.serverError();
            }
        }

        [HttpPost]
        public async Task<JsonResultModel> CreateUser(UserInputModel input)
        {
            var result = await _userService.CreateUser(input);
            if (result == SystemParam.EMAIL_USED_ERR)
                return _responseService.ErrorResult(SystemParam.EMAIL_USED_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.PHONE_USED_ERR)
                return _responseService.ErrorResult(SystemParam.PHONE_USED_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.RETURN_TRUE)
                return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, result);
            else
                return _responseService.serverError();
        }

        [HttpGet]
        public async Task<JsonResultModel> DetailUser(int ID)
        {
            var result = await _userService.DetailUser(ID);
            return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, result);
        }

        [HttpPost]
        public async Task<JsonResultModel> UpdateUser(UpdateUserInputModel input)
        {
            var result = await _userService.UpdateUser(input);
            if (result == SystemParam.EMAIL_USED_ERR)
                return _responseService.ErrorResult(SystemParam.EMAIL_USED_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.PHONE_USED_ERR)
                return _responseService.ErrorResult(SystemParam.PHONE_USED_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.ACCOUNT_NOT_FOUND_ERR)
                return _responseService.ErrorResult(SystemParam.ACCOUNT_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.RETURN_TRUE)
                return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, result);
            else
                return _responseService.serverError();
        }

        [HttpPost]
        public async Task<JsonResultModel> DeleteUser(int ID)
        {
            var result = await _userService.DeleteUser(ID);
            if (result == SystemParam.ACCOUNT_NOT_FOUND_ERR)
                return _responseService.ErrorResult(SystemParam.ACCOUNT_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.RETURN_TRUE)
                return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, result);
            else
                return _responseService.serverError();
        }

        [HttpPost]
        public async Task<JsonResultModel> DeactiveUser(int ID)
        {
            var result = await _userService.ChangeUserStatus(ID, SystemParam.IN_ACTIVE);
            if (result == SystemParam.ACCOUNT_NOT_FOUND_ERR)
                return _responseService.ErrorResult(SystemParam.ACCOUNT_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.RETURN_TRUE)
                return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, result);
            else
                return _responseService.serverError();
        }

        [HttpPost]
        public async Task<JsonResultModel> ActivateUser(int ID)
        {
            var result = await _userService.ChangeUserStatus(ID, SystemParam.ACTIVE);
            if (result == SystemParam.ACCOUNT_NOT_FOUND_ERR)
                return _responseService.ErrorResult(SystemParam.ACCOUNT_NOT_FOUND_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.RETURN_TRUE)
                return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, result);
            else
                return _responseService.serverError();
        }

        [HttpPost]
        public async Task<JsonResultModel> ChangePassword(int ID, string currentPass, string newPass)
        {
            var result = await _authenticationService.ChangePassword(ID, currentPass, newPass);
            if (result == SystemParam.INVALID_PASSWORD_ERR)
                return _responseService.ErrorResult(SystemParam.INVALID_PASSWORD_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            else if (result == SystemParam.RETURN_TRUE)
                return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, result);
            else
                return _responseService.serverError();
        }
    }
}