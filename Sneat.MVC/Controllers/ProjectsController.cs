using Sneat.MVC.App_Start;
using Sneat.MVC.Common;
using Sneat.MVC.Models.DTO.Project;
using Sneat.MVC.Models.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sneat.MVC.Controllers
{
    public class ProjectsController : BaseController
    {
        #region Project management
        [UserAuthenticationFilter]
        public ActionResult Index()
        {
            return View();
        }

        [UserAuthenticationFilter]
        public async Task<PartialViewResult> Search(int page, string search = "")
        {
            var result = await _projectService.SearchProject(page, SystemParam.MAX_ROW_IN_LIST_WEB, search);
            return PartialView("_ListProject", result);
        }

        [UserAuthenticationFilter]
        public async Task<ActionResult> Create()
        {
            ViewBag.ListTeam = await _teamService.GetListTeam();
            return View();
        }

        [HttpPost]
        public async Task<int> CreateProject(ProjectInputModel input)
        {
            UserDetailOutputModel userLogin = UserLogins;
            input.PMId = userLogin.ID;
            return await _projectService.CreateProject(input);
        }

        [UserAuthenticationFilter]
        public async Task<ActionResult> Update(int ID)
        {
            ViewBag.ListTeam = await _teamService.GetListTeam();
            var detail = _projectService.DetailProject(ID);
            return View(detail);
        }

        [UserAuthenticationFilter]
        public ActionResult ProjectDetail(int projectID)
        {
            var detail = _projectService.DetailProject(projectID);
            ViewBag.ListUserProject = _projectService.GetListUserProject(projectID);
            return View(detail);
        }

        [HttpPost]
        public async Task<int> RemoveProject(int ID)
        {
            return await _projectService.RemoveProject(ID);
        }

        [HttpPost]
        public async Task<int> UpdateProject(ProjectOutputModel input)
        {
            return await _projectService.UpdateProject(input);
        }

        [UserAuthenticationFilter]
        public PartialViewResult SearchUserProject(int page, int projectID, int limit = SystemParam.MAX_ROW_IN_LIST_WEB, string search = "")
        {
            var result = _projectService.SearchUserProject(page, limit, projectID, search);
            return PartialView("_ListUserProject", result);
        }

        [UserAuthenticationFilter]
        public async Task<int> AddUserProject(List<int> userIds, int projectID)
        {
            return await _projectService.AddUserProject(userIds, projectID);
        }
       
        [HttpPost]
        public async Task<JsonResult> ListUserByTeam(List<int> teamIds)
        {
            var listSPs = await _teamService.ListUserByTeam(teamIds);
            return Json(listSPs, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Personal Project management
        [UserAuthenticationFilter]
        public async Task<ActionResult> PersonalProjects()
        {
            UserDetailOutputModel userLogin = UserLogins;
            var listProject = await _projectService.PersonalProjects(userLogin.ID);
            return View(listProject);
        }

        [UserAuthenticationFilter]
        public PartialViewResult SearchUserProjectDetail(int page, int projectID, int limit = SystemParam.MAX_ROW_IN_LIST_WEB, string search = "")
        {
            var result = _projectService.SearchUserProject(page, limit, projectID, search);
            return PartialView("_ListUserProjectDetail", result);
        }

        [UserAuthenticationFilter]
        public PartialViewResult ProjectOverview(int projectID)
        {
            var detail = _projectService.DetailProject(projectID);
            return PartialView("_ProjectOverview", detail);
        }
        #endregion
    }
}