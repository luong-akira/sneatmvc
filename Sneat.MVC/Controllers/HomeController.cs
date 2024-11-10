using Sneat.MVC.App_Start;
using Sneat.MVC.Common;
using Sneat.MVC.Models.DTO.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sneat.MVC.Controllers
{
    public class HomeController : BaseController
    {
        public string fullUrl = Utils.getFullUrl();

        [UserAuthenticationFilter]
        public ActionResult Index()
        {
            UserDetailOutputModel userLogin = UserLogins;
            return View();
        }

        public ActionResult Login()
        {
            ViewData["isLoginPage"] = true;
            return View();
        }

        public int Logout()
        {
            try
            {
                Session[SystemParam.SESSION_LOGIN] = null;
                return SystemParam.SUCCESS;
            }
            catch
            {
                return SystemParam.ERROR;
            }
        }

        public async Task<int> UserLogin(string email, string password)
        {
            return await _authenticationService.UserLogin(email, password);
        }

        public async Task<int> ForgotPassword(string email)
        {
            return await _authenticationService.ForgotPassword(email);
        }

        [HttpPost]
        public JsonResult UploadFiles(HttpPostedFileBase[] files)
        {
            var listImages = new List<string>();
            if (ModelState.IsValid)
            {
                foreach (HttpPostedFileBase file in files)
                {
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        string name = DateTime.Now.ToString("ddMMyyyyHHmmssfff") + "-" + InputFileName;
                        var ServerSavePath = Path.Combine(Server.MapPath(@"/Uploads/files/"), name);
                        file.SaveAs(ServerSavePath);

                        // Construct the full URL to access the uploaded file
                        var imageUrl = fullUrl + "/Uploads/files/" + name;
                        listImages.Add(imageUrl);
                    }
                }
            }
            return Json(listImages);
        }

        [HttpPost]
        public JsonResult DeleteFile(string fileName)
        {
            try
            {
                var filePath = Path.Combine(Server.MapPath("~/Uploads/files/"), fileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "File not found" });
            }
            catch (Exception ex)
            {
                ex.ToString();
                return Json(new { success = false, message = ex.ToString() });
            }

        }
    } 
}