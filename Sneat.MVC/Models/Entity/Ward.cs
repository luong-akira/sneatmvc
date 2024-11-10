using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sneat.MVC.Models.Entity
{
    public class Ward
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }

        [ForeignKey(nameof(District))]
        public int DistrictID { get; set; }
        public virtual District District { get; set; }
    }
}