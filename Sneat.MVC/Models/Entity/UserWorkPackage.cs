using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Sneat.MVC.Models.Enum;

namespace Sneat.MVC.Models.Entity
{
    public class UserWorkPackage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public WorkAssignType AssignType { get; set; }

        [ForeignKey(nameof(User))]
        public int UserID { get; set; }
        public virtual User User { get; set; }

        [ForeignKey(nameof(WorkPackage))]
        public int WorkPackageID { get; set; }
        public virtual WorkPackage WorkPackage { get; set; }
    }
}