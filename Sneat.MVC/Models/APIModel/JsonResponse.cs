using Sneat.MVC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sneat.MVC.Models.APIModel
{
    public class JsonResponse
    {
        public static JsonResultModel Response(int status, int code, string message, object data)
        {
            JsonResultModel result = new JsonResultModel();
            result.Status = status;
            result.Code = code;
            result.Message = message;
            result.Data = data;
            return result;
        }


        public static JsonResultModel ServerError()
        {
            JsonResultModel result = new JsonResultModel();
            result.Status = SystemParam.ERROR;
            result.Code = SystemParam.ERROR;
            result.Message = SystemParam.SERVER_ERROR;
            result.Data = "";
            return result;
        }

        public static JsonResultModel Success(string message, object data)
        {
            JsonResultModel result = new JsonResultModel();
            result.Status = SystemParam.SUCCESS;
            result.Code = SystemParam.SUCCESS_CODE;
            result.Message = message;
            result.Data = data;
            return result;
        }

        public static JsonResultModel Success(object data)
        {
            JsonResultModel result = new JsonResultModel();
            result.Status = SystemParam.SUCCESS;
            result.Code = SystemParam.SUCCESS_CODE;
            result.Message = SystemParam.SUCCESS_MESSAGE;
            result.Data = data;
            return result;
        }

        public static JsonResultModel SucessPaging(object data, object paging)
        {
            JsonResultModel result = new JsonResultModel();
            result.Status = SystemParam.SUCCESS;
            result.Code = SystemParam.SUCCESS_CODE;
            result.Message = SystemParam.SUCCESS_MESSAGE;
            result.Data = data;
            result.Paging = paging;
            return result;
        }

        public static JsonResultModel Error(int code, string message)
        {
            return new JsonResultModel(SystemParam.ERROR, code, message, "");
        }

        public static JsonResultModel ErrorResult(string mess, int code)
        {
            JsonResultModel result = new JsonResultModel();
            result.Message = mess;
            result.Status = SystemParam.ERROR;
            result.Code = code;
            result.Data = null;
            return result;
        }
        public static JsonResultModel Exception(string mess)
        {
            JsonResultModel result = new JsonResultModel();
            result.Message = mess;
            result.Status = SystemParam.ERROR;
            result.Code = SystemParam.SERVER_ERROR_CODE;
            result.Data = null;
            // oneSignal.SaveLog(mess, "");
            return result;
        }
    }
}