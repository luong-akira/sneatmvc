using Sneat.MVC.App_Start;
using Sneat.MVC.Common;
using Sneat.MVC.Models.DTO.User;
using Sneat.MVC.Models.DTO.WorkPackage;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sneat.MVC.Controllers
{
    public class WorkPackageController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [UserAuthenticationFilter]
        public async Task<PartialViewResult> SearchWorkPackage(
            int page = SystemParam.PAGE_DEFAULT, 
            int limit = SystemParam.MAX_ROW_IN_LIST_WEB, 
            string search = "",
            int? projectID = null,
            int? priorityType = null
            )
        {
            ViewBag.ListUserProject = _projectService.GetListUserProject((int)projectID);
            var result = await _workPackageService.SearchWorkPackage(page, limit, search, projectID, priorityType);  
            return PartialView("_ListWorkPackage", result);
        }

        [HttpPost]
        public async Task<int> CreateWorkPackage(WorkPackageInputModel input)
        {
            UserDetailOutputModel userLogin = UserLogins;
            input.AssignorID = userLogin.ID;
            return await _workPackageService.CreateWorkPackage(input);
        }

        public async Task<JsonResult> DetailWorkPackage(int ID)
        {
            var result = await _workPackageService.DetailWorkPackage(ID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<int> UpdateWorkPackage(WorkPackageOutputModel input)
        {
            return await _workPackageService.UpdateWorkPackage(input);
        }
    }
}