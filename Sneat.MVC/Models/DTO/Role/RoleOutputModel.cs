using System;
using System.Collections.Generic;

namespace Sneat.MVC.Models.DTO.Role
{
    public class RoleOutputModel : RoleInputModel
    {
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<UserRoleOutputModel> UserRoles { get; set; }
    }

    public class RoleInputModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> PermissionIDs { get; set; }

    }

    public class UserRoleOutputModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }
    }
}