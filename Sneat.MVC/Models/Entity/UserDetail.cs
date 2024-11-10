using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Sneat.MVC.Models.Enum;

namespace Sneat.MVC.Models.Entity
{
    public class UserDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender Gender { get; set; }

        public string Identity { get; set; }
        public DateTime? IdentityReceivedDate { get; set; }
        public string IdentityReceivedPlace { get; set; }
        public string IdentityImages { get; set; }

        [ForeignKey(nameof(Bank))]
        public int? BankID { get; set; }
        public virtual Bank Bank { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNo { get; set; }
        public string BankQRImage { get; set; }


        [ForeignKey(nameof(DistrictHome))]
        public int? DistrictHomeID { get; set; }
        public virtual District DistrictHome { get; set; }
        public string HomeAddress { get; set; }


        [ForeignKey(nameof(DistrictOffice))]
        public int? DistrictOfficeID { get; set; }
        public virtual District DistrictOffice { get; set; }
        public string OfficeAddress { get; set; }

        // Navigation property
        [ForeignKey(nameof(User))]
        public int UserID { get; set; }

        public virtual User User { get; set; }
    }
}