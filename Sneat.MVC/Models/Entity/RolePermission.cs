using System.ComponentModel.DataAnnotations.Schema;

namespace Sneat.MVC.Models.Entity
{
    public class RolePermission
    {
        public int ID { get; set; }

        // Navigation property
        [ForeignKey(nameof(Role))]
        public int RoleID { get; set; }
        public virtual Role Role { get; set; }

        // Navigation property
        [ForeignKey(nameof(Permission))]
        public int PermissionID { get; set; }
        public virtual Permission Permission { get; set; }
    }
}