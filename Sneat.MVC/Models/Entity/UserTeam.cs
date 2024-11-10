using System.ComponentModel.DataAnnotations.Schema;

namespace Sneat.MVC.Models.Entity
{
    public class UserTeam
    {
        public int ID { get; set; }

        [ForeignKey(nameof(Team))]
        public int TeamID { get; set; }
        public virtual Team Team { get; set; }

        [ForeignKey(nameof(User))]
        public int UserID { get; set; }
        public virtual User User { get; set; }
    }
}