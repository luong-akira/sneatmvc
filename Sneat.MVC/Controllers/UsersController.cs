using Sneat.MVC.App_Start;
using Sneat.MVC.Common;
using Sneat.MVC.Models.DTO.User;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sneat.MVC.Controllers
{
    public class UsersController : BaseController
    {
        [UserAuthenticationFilter]
        public async Task<ActionResult> Index()
        {
            ViewBag.ListTeams = await _teamService.GetListTeam();
            return View();
        }

        [UserAuthenticationFilter]
        public PartialViewResult Search(int page, int limit = SystemParam.MAX_ROW_IN_LIST_WEB, string search = "", int? teamID = null)
        {
            var result = _userService.Search(page, limit, search, teamID);
            return PartialView("_ListUser", result);
        }

        [UserAuthenticationFilter]
        public async Task<ActionResult> Create()
        {
            ViewBag.ListProvince = await _addressService.ListProvince();
            ViewBag.ListBank =  _bankService.GetListBanks();
            ViewBag.ListRole = await _roleService.ListRoleAuthorization();
            return View();
        }

        [HttpPost]
        public async Task<int> CreateUser(UserInputModel input)
        {
            UserDetailOutputModel userLogin = UserLogins;
            return await _userService.CreateUser(input);
        }

        [UserAuthenticationFilter]
        [HttpGet]
        public async Task<ActionResult> Update(int ID)
        {
            ViewBag.ListProvince = await _addressService.ListProvince();
            ViewBag.ListBank = _bankService.GetListBanks();
            ViewBag.ListRole = await _roleService.ListRoleAuthorization();
            var userDetail = await _userService.DetailUser(ID);
            return View(userDetail);
        }

        [UserAuthenticationFilter]
        [HttpGet]
        public async Task<ActionResult> UserProfile(int? ID)
        {
            var userDetail = await _userService.DetailUser((int)ID);
            return View(userDetail);
        }

        [HttpPost]
        public async Task<int> UpdateUser(UpdateUserInputModel input)
        {
            return await _userService.UpdateUser(input);
        }

        [HttpPost]
        public async Task<int> DeleteUser(int ID)
        {
            return await _userService.DeleteUser(ID);
        }

        [HttpPost]
        public async Task<int> DeactiveUser(int ID)
        {
            return await _userService.ChangeUserStatus(ID, SystemParam.IN_ACTIVE);
        }

        [HttpPost]
        public async Task<int> ActivateUser(int ID)
        {
            return await _userService.ChangeUserStatus(ID, SystemParam.ACTIVE);
        }

        [HttpPost]
        public async Task<int> ChangePassword(string currentPass, string newPass)
        {
            UserDetailOutputModel userLogin = UserLogins;
            return await _authenticationService.ChangePassword(userLogin.ID, currentPass, newPass);
        }

    }
}