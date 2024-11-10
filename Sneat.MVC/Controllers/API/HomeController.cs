using Sneat.MVC.Models.APIModel;
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
    public class HomeController : ApiController
    {
        public HomeService _homeService;

        public HomeController()
        {
            _homeService = new HomeService();
        }

        [HttpGet]
        public async Task<JsonResultModel> GetHome()
        {
            var result = await _homeService.GetHome();
            return result;
        }

        [HttpGet]
        public async Task<JsonResultModel> GetBacklog()
        {
            var result = await _homeService.GetBacklog();
            return result;
        }

        [HttpGet]
        public async Task<JsonResultModel> GetWorkpackages()
        {
            var result = await _homeService.GetWorkpackages();
            return result;
        }
    }
}
