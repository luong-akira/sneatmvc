using Sneat.MVC.Common;
using System;
using System.Collections.Generic;

namespace Sneat.MVC.Models.DTO.Project
{
    public class ProjectInputModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int PMId { get; set; }
        public List<int> UserIds { get; set; }
        public List<int> TeamIds { get; set; }
    }

    public class ProjectUserModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
    }

    public class ProjectOutputModel : ProjectInputModel
    {
        public int ID { get; set; }
        public int? Status { get; set; }
        public List<ProjectUserModel> ListUsers { get; set; }
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
                        return "Unknown status"; 
                }
            }
        }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

}