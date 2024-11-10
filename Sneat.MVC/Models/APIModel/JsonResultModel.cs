using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sneat.MVC.Models.APIModel
{
    public class JsonResultModel
    {
        public int Status { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public object Paging { get; set; }

        public JsonResultModel()
        {

        }

        public JsonResultModel(int status, int code, string message, object data, object paging = null)
        {
            this.Status = status;
            this.Code = code;
            this.Message = message;
            this.Data = data;
            this.Paging = paging;
        }
    }
}