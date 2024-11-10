using Sneat.MVC.DAL;
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
    public class BankController : ApiController
    {
        private readonly BankService _bankService;

        public BankController()
        {
            _bankService = new BankService();
        }


        [HttpGet]
        public async Task<JsonResultModel> GetListBanks()
        {
            var list = await _bankService.GetListBanks();
            return JsonResponse.Success(list);
        }
    }
}
