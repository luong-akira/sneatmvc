using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Sneat.MVC.Models.Entity
{
    public class Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public int IsDeleted { get; set; }
        public string TabID { get; set; }
        public string TabIcon { get; set; }
        public int Level { get; set; }
        public int IsLeaf { get; set; }
        public int? ParentID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}