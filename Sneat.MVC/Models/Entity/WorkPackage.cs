using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Sneat.MVC.Models.Enum;
using System;
using System.Collections.Generic;

namespace Sneat.MVC.Models.Entity
{
    public class WorkPackage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Subject { get; set; }
        public WorkPackageType Type { get; set; }
        public WorkPackageStatus Status { get; set; }
        public int? EstimateTime { get; set; }
        public double? SpentTime { get; set; }
        public int? RemainingTime { get; set; }
        public double? CompletePercent { get; set; }
        public int? PriorityPoint { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int IsDeleted { get; set; }

        public int? ProjectID { get; set; }
        public int? WorkPackageID { get; set; }
        public int? SprintID { get; set; }

        public virtual WorkPackage ParentWorkPackage { get; set; }
        public virtual ICollection<WorkPackage> WorkPackages { get; set; }
        public virtual Sprint Sprint { get; set; }
        public virtual ICollection<UserWorkPackage> UserWorkPackages { get; set; }
        public virtual ICollection<TimeLog> TimeLogs { get; set; }
    }
}