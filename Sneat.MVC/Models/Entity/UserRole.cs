using System.ComponentModel.DataAnnotations.Schema;

namespace Sneat.MVC.Models.Entity
{
    public class UserRole
    {
        public int ID { get; set; }

        // Navigation property
        [ForeignKey(nameof(Role))]
        public int RoleID { get; set; }
        public virtual Role Role { get; set; }

        // Navigation property
        [ForeignKey(nameof(User))]
        public int UserID { get; set; }
        public virtual User User { get; set; }
    }
}