using Sneat.MVC.App_Start;
using Sneat.MVC.Common;
using Sneat.MVC.Models.DTO.Team;
using Sneat.MVC.Models.DTO.User;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sneat.MVC.Controllers
{
    public class TeamsController : BaseController
    {
        [UserAuthenticationFilter]
        public ActionResult Index()
        {
            ViewBag.TechStack = Helper.GetTechStackModels();
            return View();
        }

        [UserAuthenticationFilter]
        public async Task<PartialViewResult> Search(int page, string search = "")
        {
            var result = await _teamService.SearchTeam(page, SystemParam.MAX_ROW_IN_LIST_WEB, search);
            return PartialView("_ListTeam", result);
        }

        [UserAuthenticationFilter]
        public PartialViewResult SearchUser(int page, int limit = SystemParam.MAX_ROW_IN_LIST_WEB, string search = "", string targetId = "list_user_option")
        {
            var result = _userService.Search(page, limit, search);
            ViewBag.TargetId = targetId; // Pass the targetId to the view
            return PartialView("_ListUserOption", result);
        }


        [HttpPost]
        public async Task<int> CreateTeam(TeamInputModel input)
        {
            return await _teamService.CreateTeam(input);
        }

        [HttpPost]
        public async Task<int> UpdateTeam(TeamOutputModel input)
        {
            return await _teamService.UpdateTeam(input);
        }

        [HttpPost]
        public async Task<int> RemoveTeam(int ID)
        {
            return await _teamService.RemoveTeam(ID);
        }

        public async Task<JsonResult> DetailTeam(int ID)
        {
            var result = await _teamService.DetailTeam(ID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}