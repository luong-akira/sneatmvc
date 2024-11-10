using Sneat.MVC.Common;
using Sneat.MVC.DAL;
using Sneat.MVC.Models.APIModel;
using Sneat.MVC.Models.APIModel.Authentication;
using Sneat.MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sneat.MVC.Controllers.API
{
    public class AuthenticationController : ApiController
    {
        protected SneatContext Context;
        public UserService _userService;
        public AddressService _addressService;
        public BankService _bankService;
        public RoleService _roleService;
        public TeamService _teamService;
        public ProjectService _projectService;
        public WorkPackageService _workPackageService;

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

        public AuthenticationController()
        {
            _userService = new UserService(this.GetContext());
            _addressService = new AddressService(this.GetContext());
            _bankService = new BankService(this.GetContext());
            _roleService = new RoleService(this.GetContext());
            _teamService = new TeamService(this.GetContext());
            _projectService = new ProjectService(this.GetContext());
            _workPackageService = new WorkPackageService(this.GetContext());
            _authenticationService = new AuthenticationService(this.GetContext());

            _responseService = new ResponseService();
        }

        [HttpGet]
        public async Task<JsonResultModel> GetDetails()
        {
            var token = Utils.getTokenApp(Request.Headers);
            return await _authenticationService.GetDetails(token);
        }

        [HttpPost]
        public async Task<JsonResultModel> LoginWeb(WebLoginModel model)
        {
            return await _authenticationService.CheckLoginWeb(model.Phone, model.Password);
        }

        [HttpPost]
        public async Task<JsonResultModel> ForgotPasswordWeb([FromBody] string email)
        {
            return await _authenticationService.ForgotPasswordWeb(email);
        }

        [HttpPost]
        public async Task<JsonResultModel> ChangePasswordWeb(ChangePasswordModel model)
        {
            var userId = Utils.getUserFromToken(Request.Headers);

            return await _authenticationService.ChangePasswordWeb(userId.Value, model.OldPassword, model.NewPassword);
        }

        [HttpGet]
        public async Task<JsonResultModel> GetListRole()
        {
            var result = await _roleService.ListRoleAuthorization();
            return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, result);
        }
    }
}