using Sneat.MVC.Common;
using Sneat.MVC.DAL;
using Sneat.MVC.Models.APIModel;
using Sneat.MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Sneat.MVC.Controllers.API
{
    public class SystemController : ApiController
    {
        protected SneatContext Context;
        public AddressService _addressService;
        public BankService _bankService;
        public ResponseService _responseService;

        public SneatContext GetContext()
        {
            if (Context == null)
            {
                Context = new SneatContext();
            }
            return Context;
        }

        public SystemController()
        {
            _addressService = new AddressService(this.GetContext());
            _bankService = new BankService(this.GetContext());
            _responseService = new ResponseService();
        }

        [HttpGet]
        public async Task<JsonResultModel> GetListBank()
        {
            var result = await _bankService.GetListBanks();
            return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, result);
        }

        [HttpGet]
        public async Task<JsonResultModel> GetListProvince()
        {
            var result = await _addressService.ListProvince();
            return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, result);
        }
    }
}