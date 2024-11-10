using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sneat.MVC.Models.DTO.TimeLog
{
    public class TimeLogInputModel
    {
        public double Hours { get; set; }
        public int TotalWorkingTime { get; set; }
        public DateTime LogDate { get; set; }
        public int MemberID { get; set; }
        public int CreatedByID { get; set; }
        public int WorkPackageID { get; set; }
    }

    public class TimeLogOutputModel : TimeLogInputModel
    {
        public int ID { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UserName { get; set; }
        public int? UserID { get; set; }
        public string WorkPackageName { get; set; }
        public string ProjectName { get; set; }
        public int? ProjectID { get; set; }
    }

    public class UpdateTimeLogModel
    {
        public int ID { get; set; }
        public double Hours { get; set; }
        public DateTime LogDate { get; set; }
    }
}