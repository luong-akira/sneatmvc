using Sneat.MVC.Common;
using Sneat.MVC.Models.APIModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sneat.MVC.Services
{
    public class ResponseService
    {
        public JsonResultModel response(int status, int code, string message, object data)
        {
            JsonResultModel result = new JsonResultModel();
            result.Status = status;
            result.Code = code;
            result.Message = message;
            result.Data = data;
            return result;
        }

        public JsonResultModel tokenError()
        {
            JsonResultModel result = new JsonResultModel();
            result.Status = SystemParam.ERROR;
            result.Code = SystemParam.TOKEN_ERROR;
            result.Message = SystemParam.MESSAGE_TOKEN_ERROR;
            result.Data = "";
            return result;
        }
        public JsonResultModel serverError()
        {
            JsonResultModel result = new JsonResultModel();
            result.Status = SystemParam.ERROR;
            result.Message = SystemParam.SERVER_ERROR;
            result.Data = "";
            return result;
        }
        public JsonResultModel SuccessResult(string mess, object data)
        {
            JsonResultModel result = new JsonResultModel();
            result.Message = mess;
            result.Status = SystemParam.SUCCESS;
            result.Code = SystemParam.SUCCESS_CODE;
            result.Data = data;
            return result;
        }

        public JsonResultModel SuccessPaging(string mess, object data, object paging)
        {
            JsonResultModel result = new JsonResultModel();
            result.Message = mess;
            result.Status = SystemParam.SUCCESS;
            result.Code = SystemParam.SUCCESS_CODE;
            result.Data = data;
            result.Paging = paging;
            return result;
        }

        public JsonResultModel ErrorResult(string mess, int code)
        {
            JsonResultModel result = new JsonResultModel();
            result.Message = mess;
            result.Status = SystemParam.ERROR;
            result.Code = code;
            result.Data = null;
            return result;
        }
        public JsonResultModel Exception(string mess)
        {
            JsonResultModel result = new JsonResultModel();
            result.Message = SystemParam.SERVER_ERROR;
            result.Status = SystemParam.ERROR;
            result.Code = SystemParam.SERVER_ERROR_CODE;
            result.Data = null;
            // oneSignal.SaveLog(mess, "");
            return result;
        }
    }
}