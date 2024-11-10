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
    public class AddresssController : ApiController
    {
        private readonly AddressService _addressService;


        public AddresssController()
        {
            _addressService = new AddressService();
        }

        [HttpGet]
        public async Task<JsonResultModel> GetListProvinces()
        {
            var list =  await _addressService.ListProvince();
            return JsonResponse.Success(list);
        }

        [HttpGet]
        public async Task<JsonResultModel> GetListDistricts(int provinceId)
        {
            var list = await _addressService.ListDistrict(provinceId);
            return JsonResponse.Success(list);
        }

        [HttpGet]
        public async Task<JsonResultModel> GetListWards(int districtId)
        {
            var list = await _addressService.ListWards(districtId);
            return JsonResponse.Success(list);
        }
    }
}