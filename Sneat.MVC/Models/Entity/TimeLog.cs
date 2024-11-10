using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sneat.MVC.Models.Entity
{
    public class TimeLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public double Hours { get; set; }
        public int TotalWorkingTime { get; set; }
        public int IsDeleted { get; set; }
        public DateTime LogDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey(nameof(User))]
        public int MemberID { get; set; }
        public int CreatedByID { get; set; }

        [ForeignKey(nameof(WorkPackage))]
        public int WorkPackageID { get; set; }
        public virtual WorkPackage WorkPackage { get; set; }
        public virtual User User { get; set; }
    }

}