using Sneat.MVC.Common;
using Sneat.MVC.DAL;
using Sneat.MVC.Models.DTO.User;
using Sneat.MVC.Services;
using System.Web.Mvc;

namespace Sneat.MVC.Controllers
{
    public class BaseController : Controller
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

        public BaseController() : base()
        {
            _userService = new UserService(this.GetContext());
            _addressService = new AddressService(this.GetContext());
            _bankService = new BankService(this.GetContext());
            _roleService = new RoleService(this.GetContext());
            _teamService = new TeamService(this.GetContext());
            _projectService = new ProjectService(this.GetContext());
            _workPackageService = new WorkPackageService(this.GetContext());
            _authenticationService = new AuthenticationService(this.GetContext());
        }

        public SneatContext GetContext()
        {
            if (Context == null)
            {
                Context = new SneatContext();
            }
            return Context;
        }

        public UserDetailOutputModel UserLogins
        {
            get
            {
                return Session[SystemParam.SESSION_LOGIN] as UserDetailOutputModel;

            }
        }
    }
}