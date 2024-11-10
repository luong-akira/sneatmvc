using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sneat.MVC.Models.APIModel
{
    public class PagingModel
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public int TotalItemCount { get; set; }
        public object Extras { get; set; } = null;

    }
}