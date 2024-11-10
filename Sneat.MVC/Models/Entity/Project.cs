using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Sneat.MVC.Models.Enum;
using System;
using System.Collections.Generic;

namespace Sneat.MVC.Models.Entity
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int IsDeleted { get; set; }
        public Status? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string TeamIds { get; set; }
        public virtual ICollection<UserProject> UserProjects { get; set; }
    }
}