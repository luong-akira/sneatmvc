using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sneat.MVC.Models.DTO.Document
{
    public class DocumentInputModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string DocumentAttachment { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public DateTime? CreateDate { get; set; }
    }

    public class DocumentOutputModel : DocumentInputModel
    {
        public int ID { get; set; }
    }
}