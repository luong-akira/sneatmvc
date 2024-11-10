using Sneat.MVC.Common;
using Sneat.MVC.Models.APIModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Sneat.MVC.Controllers.API
{
    public class UploadController : ApiController
    {
        /// <summary>
        /// Tải file lên
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResultModel UploadFile()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                List<string> listImage = new List<string>();
                if (httpRequest.Files.Count > 0)
                {
                    for (int i = 0; i < httpRequest.Files.Count; i++)
                    {
                        var random = new Random();
                        var file = httpRequest.Files[i];
                        string name = i.ToString() + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + Path.GetExtension(file.FileName);
                        var filePath = HttpContext.Current.Server.MapPath(@"/Uploads/files/" + name);
                        var path = Utils.getFullUrl() + "/Uploads/files/" + name;
                        file.SaveAs(filePath);
                        listImage.Add(path);
                    }
                    return JsonResponse.Response(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE, SystemParam.SUCCESS_MESSAGE, listImage);
                }
                else
                {
                    return JsonResponse.ServerError();
                }
            }
            catch (Exception ex)
            {

                return JsonResponse.ServerError();
            }

        }
    }
}
