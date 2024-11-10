using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sneat.MVC.Models.Entity
{
    public class District
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }

        [ForeignKey(nameof(Province))]
        public int ProvinceID { get; set; }
        public virtual Province Province { get; set; }
    }
}