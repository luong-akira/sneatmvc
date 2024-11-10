using Sneat.MVC.Models.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sneat.MVC.Models.Entity
{
    public class UserProject
    {
        public int ID { get; set; }

        [ForeignKey(nameof(Project))]
        public int ProjectID { get; set; }
        public virtual Project Project { get; set; }

        [ForeignKey(nameof(User))]
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public ProjectRole ProjectRole { get; set; }
    }
}