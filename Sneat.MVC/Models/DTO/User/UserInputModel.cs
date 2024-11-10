using Sneat.MVC.Common;
using Sneat.MVC.Models.Enum;
using System;
using System.Collections.Generic;
namespace Sneat.MVC.Models.DTO.User
{
    public class UserInputModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public List<int> RoleIds { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public Gender Gender { get; set; }

        public string Identity { get; set; }
        public DateTime? IdentityReceivedDate { get; set; }
        public string IdentityReceivedPlace { get; set; }
        public string IdentityImages { get; set; }

        public string BankBin { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountNo { get; set; }
        public string BankQRImage { get; set; }
        public int? ProvinceHomeID { get; set; }

        public int? DistrictHomeID { get; set; }
        public string HomeAddress { get; set; }

        public int? ProvinceOfficeID { get; set; }
        public int? DistrictOfficeID { get; set; }
        public string OfficeAddress { get; set; }

        public int Status { get; set; }
    }

    public class UpdateUserInputModel : UserInputModel
    {
        public int ID { get; set; }
        public DateTime CreateDate { get; set; }
        public string IdentityReceivedDateStr 
        {
            get
            {
                if (IdentityReceivedDate != null)
                    return IdentityReceivedDate.Value.ToString(SystemParam.CONVERT_DATETIME_DATE_PICKER_BASIC);
                else
                    return null;
            }
        }
        public string DOBStr
        {
            get
            {
                if (DOB != null)
                    return DOB.Value.ToString(SystemParam.CONVERT_DATETIME_DATE_PICKER_BASIC);
                else
                    return null;
            }
        }

        public string StatusStr
        {
            get
            {
                switch (Status)
                {
                    case SystemParam.ACTIVE:
                        return SystemParam.STATUS_ACTIVE_STR;
                    case SystemParam.IN_ACTIVE:
                        return SystemParam.STATUS_IN_ACTIVE_STR;
                    default:
                        return "Undefined Status";
                }
            }
        }

        public List<string> PermissionTabs { get; set; }
        public List<TeamUserOutputModel> UserTeams { get; set; }
        public List<ProjectUserOutputModel> ListProjects { get; set; }
        public int TotalProjects { get; set; }
        public string Token { get; set; }
    }
}