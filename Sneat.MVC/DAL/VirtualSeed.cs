using Newtonsoft.Json;
using Sneat.MVC.Common;
using Sneat.MVC.Models.DTO.Bank;
using Sneat.MVC.Models.Entity;
using Sneat.MVC.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Sneat.MVC.DAL
{
    public class VirtualSeed
    {
        // This is just a model file to seed in Migrations/Configuration.cs file
        // Copy this content to that file for seeding
        private static readonly HttpClient client = new HttpClient();
       
        protected void Seed(Sneat.MVC.DAL.SneatContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            SeedProvinces(context);
            SeedDistricts(context);
            SeedUsers(context);
            SeedRoles(context);
            SeedPermission(context);

            SeedBanks(context).GetAwaiter().GetResult();
        }

        private void SeedUsers(Sneat.MVC.DAL.SneatContext context)
        {
            if (context.Users.Any())
                return;

            var users = new List<User>()
            {
                  new User
                    {
                        UserName = "admin",
                        Phone = "admin",
                        Email = "admin@gmail.com",
                        Password = Utils.GenPass("123456"),
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        IsDeleted = 0,
                        Status = Status.ACTIVE,
                    }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        private void SeedRoles(Sneat.MVC.DAL.SneatContext context)
        {
            var adminRole = context.Roles.FirstOrDefault(x => x.Name.Equals("Admin"));

            if (adminRole == null)
            {
                var newRole = new Role
                {
                    Name = "Admin",
                    Description = "This role has full permission in this system.",
                    IsDeleted = SystemParam.IS_NOT_DELETED,
                    CreatedDate = DateTime.Now
                };
                context.Roles.Add(newRole);
            }

            context.SaveChanges();
        }

        private void SeedPermission(Sneat.MVC.DAL.SneatContext context)
        {
            // Clear existing data
            var existingPermissions = context.Permissions.ToList();
            var existingPermissionRoles = context.RolePermissions.ToList();
            context.RolePermissions.RemoveRange(existingPermissionRoles);
            context.Permissions.RemoveRange(existingPermissions);
            context.SaveChanges();

            // Define the permissions to seed
            var permissions = new List<Permission>
            {
                new Permission { Name = "HomePage", TabID = "homePageTab", TabIcon = "home", Level = 1, IsLeaf = 0, ParentID = null, IsDeleted = SystemParam.IS_NOT_DELETED, CreatedDate = DateTime.Now },
                #region User
                new Permission { Name = "User management", TabID = "userListTab", TabIcon = "user", Level = 1, IsLeaf = 0, ParentID = null, IsDeleted = SystemParam.IS_NOT_DELETED, CreatedDate = DateTime.Now },
                new Permission { Name = "View", TabID = "viewUserTab", TabIcon = "user", Level = 2, IsLeaf = 1, ParentID = null, IsDeleted = SystemParam.IS_NOT_DELETED, CreatedDate = DateTime.Now },
                new Permission { Name = "Create", TabID = "createUserTab", TabIcon = "user", Level = 2, IsLeaf = 1, ParentID = null, IsDeleted = SystemParam.IS_NOT_DELETED, CreatedDate = DateTime.Now },
                new Permission { Name = "Update", TabID = "updateUserTab", TabIcon = "user", Level = 2, IsLeaf = 1, ParentID = null, IsDeleted = SystemParam.IS_NOT_DELETED, CreatedDate = DateTime.Now },
                new Permission { Name = "Update user info", TabID = "updateInfoUserTab", TabIcon = "user", Level = 3, IsLeaf = 1, ParentID = null, IsDeleted = SystemParam.IS_NOT_DELETED, CreatedDate = DateTime.Now },
                new Permission { Name = "Update user status", TabID = "updateStatusUserTab", TabIcon = "user", Level = 3, IsLeaf = 1, ParentID = null, IsDeleted = SystemParam.IS_NOT_DELETED, CreatedDate = DateTime.Now },
                new Permission { Name = "Delete", TabID = "deleteUserTab", TabIcon = "user", Level = 2, IsLeaf = 1, ParentID = null, IsDeleted = SystemParam.IS_NOT_DELETED, CreatedDate = DateTime.Now },
                #endregion

                #region Role
                new Permission { Name = "Role management", TabID = "roleListTab", TabIcon = "role", Level = 1, IsLeaf = 0, ParentID = null, IsDeleted = SystemParam.IS_NOT_DELETED, CreatedDate = DateTime.Now },
                new Permission { Name = "View", TabID = "viewRoleTab", TabIcon = "role", Level = 2, IsLeaf = 1, ParentID = null, IsDeleted = SystemParam.IS_NOT_DELETED, CreatedDate = DateTime.Now },
                new Permission { Name = "Create", TabID = "createRoleTab", TabIcon = "role", Level = 2, IsLeaf = 1, ParentID = null, IsDeleted = SystemParam.IS_NOT_DELETED, CreatedDate = DateTime.Now },
                new Permission { Name = "Update", TabID = "updateRoleTab", TabIcon = "role", Level = 2, IsLeaf = 1, ParentID = null, IsDeleted = SystemParam.IS_NOT_DELETED, CreatedDate = DateTime.Now },
                new Permission { Name = "Delete", TabID = "deleteRoleTab", TabIcon = "role", Level = 2, IsLeaf = 1, ParentID = null, IsDeleted = SystemParam.IS_NOT_DELETED, CreatedDate = DateTime.Now },
                #endregion

                #region Team
                new Permission { Name = "Team management", TabID = "teamListTab", TabIcon = "team", Level = 1, IsLeaf = 0, ParentID = null, IsDeleted = SystemParam.IS_NOT_DELETED, CreatedDate = DateTime.Now },
                new Permission { Name = "View", TabID = "viewTeamTab", TabIcon = "team", Level = 2, IsLeaf = 1, ParentID = null, IsDeleted = SystemParam.IS_NOT_DELETED, CreatedDate = DateTime.Now },
                new Permission { Name = "Create", TabID = "createTeamTab", TabIcon = "team", Level = 2, IsLeaf = 1, ParentID = null, IsDeleted = SystemParam.IS_NOT_DELETED, CreatedDate = DateTime.Now },
                new Permission { Name = "Update", TabID = "updateTeamTab", TabIcon = "team", Level = 2, IsLeaf = 1, ParentID = null, IsDeleted = SystemParam.IS_NOT_DELETED, CreatedDate = DateTime.Now },
                new Permission { Name = "Delete", TabID = "deleteTeamTab", TabIcon = "team", Level = 2, IsLeaf = 1, ParentID = null, IsDeleted = SystemParam.IS_NOT_DELETED, CreatedDate = DateTime.Now },
                #endregion
            };

            // Add permissions to context
            context.Permissions.AddRange(permissions);
            context.SaveChanges();

            // Retrieve permissions and update parent-child relationships
            var homePage = context.Permissions.FirstOrDefault(p => p.TabID == "homePageTab");

            var userManagement = context.Permissions.FirstOrDefault(p => p.TabID == "userListTab");
            var createUser = context.Permissions.FirstOrDefault(p => p.TabID == "createUserTab");
            var updateUser = context.Permissions.FirstOrDefault(p => p.TabID == "updateUserTab");
            var updateStatusUser = context.Permissions.FirstOrDefault(p => p.TabID == "updateStatusUserTab");
            var viewUser = context.Permissions.FirstOrDefault(p => p.TabID == "viewUserTab");
            var deleteUser = context.Permissions.FirstOrDefault(p => p.TabID == "deleteUserTab");
            var updateUserInfo = context.Permissions.FirstOrDefault(p => p.TabID == "updateInfoUserTab");

            var roleManagement = context.Permissions.FirstOrDefault(p => p.TabID == "roleListTab");
            var viewRole = context.Permissions.FirstOrDefault(p => p.TabID == "viewRoleTab");
            var createRole = context.Permissions.FirstOrDefault(p => p.TabID == "createRoleTab");
            var updateRole = context.Permissions.FirstOrDefault(p => p.TabID == "updateRoleTab");
            var deleteRole = context.Permissions.FirstOrDefault(p => p.TabID == "deleteRoleTab");

            var teamManagement = context.Permissions.FirstOrDefault(p => p.TabID == "teamListTab");
            var viewTeam = context.Permissions.FirstOrDefault(p => p.TabID == "viewTeamTab");
            var createTeam = context.Permissions.FirstOrDefault(p => p.TabID == "createTeamTab");
            var updateTeam = context.Permissions.FirstOrDefault(p => p.TabID == "updateTeamTab");
            var deleteTeam = context.Permissions.FirstOrDefault(p => p.TabID == "deleteTeamTab");

            if (userManagement != null)
            {
                createUser.ParentID = userManagement.ID;
                updateUser.ParentID = userManagement.ID;
                viewUser.ParentID = userManagement.ID;
                deleteUser.ParentID = userManagement.ID;
            }
            if (updateUser != null)
            {
                updateStatusUser.ParentID = updateUser.ID;
                updateUserInfo.ParentID = updateUser.ID;
            }

            if (roleManagement != null)
            {
                viewRole.ParentID = roleManagement.ID;
                createRole.ParentID = roleManagement.ID;
                updateRole.ParentID = roleManagement.ID;
                deleteRole.ParentID = roleManagement.ID;
            }

            if (teamManagement != null)
            {
                viewTeam.ParentID = teamManagement.ID;
                createTeam.ParentID = teamManagement.ID;
                updateTeam.ParentID = teamManagement.ID;
                deleteTeam.ParentID = teamManagement.ID;
            }

            // Add full permission for Admin
            var adminRole = context.Roles.FirstOrDefault(x => x.Name.Equals("Admin"));
            if (adminRole != null)
            {
                var existedPermissionIds = context.Permissions.Select(x => x.ID).ToList();
                foreach (var permissionId in existedPermissionIds)
                {
                    var permissionAdminRole = new RolePermission
                    {
                        RoleID = adminRole.ID,
                        PermissionID = permissionId,
                    };
                    context.RolePermissions.Add(permissionAdminRole);
                }
            }

            context.SaveChanges();
        }

        private async Task SeedBanks(Sneat.MVC.DAL.SneatContext context)
        {
            if (context.Banks.Any())
                return;

            string url = SystemParam.VIET_QR_API_ROOT_V2 + SystemParam.VIET_QR_API_LIST_BANK_V2;
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var bankApiResponse = JsonConvert.DeserializeObject<BankApiResponse>(responseBody);

            var banks = new List<Bank>();
            foreach (var bank in bankApiResponse.Data)
            {
                banks.Add(new Bank
                {
                    Name = bank.Name,
                    ShortName = bank.ShortName,
                    Bin = bank.Bin,
                    Logo = bank.Logo,
                    Code = bank.Code,
                    SwiftCode = bank.SwiftCode
                });
            }

            context.Banks.AddRange(banks);
            await context.SaveChangesAsync();
        }

        private void SeedProvinces(Sneat.MVC.DAL.SneatContext context)
        {
            if (context.Provinces.Any())
                return;

            var provinces = new List<Province>
            {
                new Province { Name = "Hà Nội" },
                new Province { Name = "Hồ Chí Minh" },
                new Province { Name = "Hải Phòng" },
                new Province { Name = "Đà Nẵng" },
                new Province { Name = "Cần Thơ" },
                new Province { Name = "An Giang" },
                new Province { Name = "Bà Rịa - Vũng Tàu" },
                new Province { Name = "Bạc Liêu" },
                new Province { Name = "Bắc Kạn" },
                new Province { Name = "Bắc Giang" },
                new Province { Name = "Bắc Ninh" },
                new Province { Name = "Bến Tre" },
                new Province { Name = "Bình Dương" },
                new Province { Name = "Bình Định" },
                new Province { Name = "Bình Phước" },
                new Province { Name = "Bình Thuận" },
                new Province { Name = "Cà Mau" },
                new Province { Name = "Cao Bằng" },
                new Province { Name = "Đắk Lắk" },
                new Province { Name = "Đắk Nông" },
                new Province { Name = "Điện Biên" },
                new Province { Name = "Đồng Nai" },
                new Province { Name = "Đồng Tháp" },
                new Province { Name = "Gia Lai" },
                new Province { Name = "Hà Giang" },
                new Province { Name = "Hà Nam" },
                new Province { Name = "Hà Tĩnh" },
                new Province { Name = "Hải Dương" },
                new Province { Name = "Hậu Giang" },
                new Province { Name = "Hòa Bình" },
                new Province { Name = "Hưng Yên" },
                new Province { Name = "Khánh Hòa" },
                new Province { Name = "Kiên Giang" },
                new Province { Name = "Kon Tum" },
                new Province { Name = "Lai Châu" },
                new Province { Name = "Lạng Sơn" },
                new Province { Name = "Lào Cai" },
                new Province { Name = "Lâm Đồng" },
                new Province { Name = "Long An" },
                new Province { Name = "Nam Định" },
                new Province { Name = "Nghệ An" },
                new Province { Name = "Ninh Bình" },
                new Province { Name = "Ninh Thuận" },
                new Province { Name = "Phú Thọ" },
                new Province { Name = "Phú Yên" },
                new Province { Name = "Quảng Bình" },
                new Province { Name = "Quảng Nam" },
                new Province { Name = "Quảng Ngãi" },
                new Province { Name = "Quảng Ninh" },
                new Province { Name = "Quảng Trị" },
                new Province { Name = "Sóc Trăng" },
                new Province { Name = "Sơn La" },
                new Province { Name = "Tây Ninh" },
                new Province { Name = "Thái Bình" },
                new Province { Name = "Thái Nguyên" },
                new Province { Name = "Thanh Hóa" },
                new Province { Name = "Thừa Thiên Huế" },
                new Province { Name = "Tiền Giang" },
                new Province { Name = "Trà Vinh" },
                new Province { Name = "Tuyên Quang" },
                new Province { Name = "Vĩnh Long" },
                new Province { Name = "Vĩnh Phúc" },
                new Province { Name = "Yên Bái" }
            };

            context.Provinces.AddRange(provinces);
            context.SaveChanges();
        }

        private void SeedDistricts(Sneat.MVC.DAL.SneatContext context)
        {
            if (context.Districts.Any())
                return;

            var districts = new List<District>()
            {
                   // Districts of Hà Nội
                    new District { Name = "Ba Đình", ProvinceID = 1 },
                    new District { Name = "Hoàn Kiếm", ProvinceID = 1 },
                    new District { Name = "Tây Hồ", ProvinceID = 1 },
                    new District { Name = "Long Biên", ProvinceID = 1 },
                    new District { Name = "Cầu Giấy", ProvinceID = 1 },
                    new District { Name = "Đống Đa", ProvinceID = 1 },
                    new District { Name = "Hai Bà Trưng", ProvinceID = 1 },
                    new District { Name = "Hoàng Mai", ProvinceID = 1 },
                    new District { Name = "Thanh Xuân", ProvinceID = 1 },
                    new District { Name = "Sóc Sơn", ProvinceID = 1 },
                    new District { Name = "Đông Anh", ProvinceID = 1 },
                    new District { Name = "Gia Lâm", ProvinceID = 1 },
                    new District { Name = "Nam Từ Liêm", ProvinceID = 1 },
                    new District { Name = "Thanh Trì", ProvinceID = 1 },
                    new District { Name = "Bắc Từ Liêm", ProvinceID = 1 },

                    // Districts of Hồ Chí Minh
                    new District { Name = "Quận 1", ProvinceID = 2 },
                    new District { Name = "Quận 2", ProvinceID = 2 },
                    new District { Name = "Quận 3", ProvinceID = 2 },
                    new District { Name = "Quận 4", ProvinceID = 2 },
                    new District { Name = "Quận 5", ProvinceID = 2 },
                    new District { Name = "Quận 6", ProvinceID = 2 },
                    new District { Name = "Quận 7", ProvinceID = 2 },
                    new District { Name = "Quận 8", ProvinceID = 2 },
                    new District { Name = "Quận 9", ProvinceID = 2 },
                    new District { Name = "Quận 10", ProvinceID = 2 },
                    new District { Name = "Quận 11", ProvinceID = 2 },
                    new District { Name = "Quận 12", ProvinceID = 2 },
                    new District { Name = "Bình Tân", ProvinceID = 2 },
                    new District { Name = "Bình Thạnh", ProvinceID = 2 },
                    new District { Name = "Gò Vấp", ProvinceID = 2 },
                    new District { Name = "Phú Nhuận", ProvinceID = 2 },
                    new District { Name = "Tân Bình", ProvinceID = 2 },
                    new District { Name = "Tân Phú", ProvinceID = 2 },
                    new District { Name = "Thủ Đức", ProvinceID = 2 },
                    new District { Name = "Củ Chi", ProvinceID = 2 },
                    new District { Name = "Hóc Môn", ProvinceID = 2 },
                    new District { Name = "Bình Chánh", ProvinceID = 2 },
                    new District { Name = "Nhà Bè", ProvinceID = 2 },
                    new District { Name = "Cần Giờ", ProvinceID = 2 },

                    // Districts of Hải Phòng
                    new District { Name = "Hồng Bàng", ProvinceID = 3 },
                    new District { Name = "Ngô Quyền", ProvinceID = 3 },
                    new District { Name = "Lê Chân", ProvinceID = 3 },
                    new District { Name = "Hải An", ProvinceID = 3 },
                    new District { Name = "Kiến An", ProvinceID = 3 },
                    new District { Name = "Đồ Sơn", ProvinceID = 3 },
                    new District { Name = "Dương Kinh", ProvinceID = 3 },
                    new District { Name = "Thuỷ Nguyên", ProvinceID = 3 },
                    new District { Name = "An Dương", ProvinceID = 3 },
                    new District { Name = "An Lão", ProvinceID = 3 },
                    new District { Name = "Kiến Thuỵ", ProvinceID = 3 },
                    new District { Name = "Tiên Lãng", ProvinceID = 3 },
                    new District { Name = "Vĩnh Bảo", ProvinceID = 3 },
                    new District { Name = "Cát Hải", ProvinceID = 3 },
                    new District { Name = "Bạch Long Vĩ", ProvinceID = 3 },

                    // Districts of Đà Nẵng
                    new District { Name = "Liên Chiểu", ProvinceID = 4 },
                    new District { Name = "Thanh Khê", ProvinceID = 4 },
                    new District { Name = "Hải Châu", ProvinceID = 4 },
                    new District { Name = "Sơn Trà", ProvinceID = 4 },
                    new District { Name = "Ngũ Hành Sơn", ProvinceID = 4 },
                    new District { Name = "Cẩm Lệ", ProvinceID = 4 },
                    new District { Name = "Hoà Vang", ProvinceID = 4 },
                    new District { Name = "Hoàng Sa", ProvinceID = 4 },

                    // Districts of Cần Thơ
                    new District { Name = "Ninh Kiều", ProvinceID = 5 },
                    new District { Name = "Ô Môn", ProvinceID = 5 },
                    new District { Name = "Bình Thủy", ProvinceID = 5 },
                    new District { Name = "Cái Răng", ProvinceID = 5 },
                    new District { Name = "Thốt Nốt", ProvinceID = 5 },
                    new District { Name = "Vĩnh Thạnh", ProvinceID = 5 },
                    new District { Name = "Cờ Đỏ", ProvinceID = 5 },
                    new District { Name = "Phong Điền", ProvinceID = 5 },
                    new District { Name = "Thới Lai", ProvinceID = 5 },

                    // Districts of An Giang
                    new District { Name = "Long Xuyên", ProvinceID = 6 },
                    new District { Name = "Châu Đốc", ProvinceID = 6 },
                    new District { Name = "An Phú", ProvinceID = 6 },
                    new District { Name = "Tân Châu", ProvinceID = 6 },
                    new District { Name = "Phú Tân", ProvinceID = 6 },
                    new District { Name = "Châu Phú", ProvinceID = 6 },
                    new District { Name = "Tịnh Biên", ProvinceID = 6 },
                    new District { Name = "Tri Tôn", ProvinceID = 6 },
                    new District { Name = "Châu Thành", ProvinceID = 6 },
                    new District { Name = "Chợ Mới", ProvinceID = 6 },
                    new District { Name = "Thoại Sơn", ProvinceID = 6 },

                    // Districts of Bà Rịa - Vũng Tàu
                    new District { Name = "Vũng Tàu", ProvinceID = 7 },
                    new District { Name = "Bà Rịa", ProvinceID = 7 },
                    new District { Name = "Châu Đức", ProvinceID = 7 },
                    new District { Name = "Xuyên Mộc", ProvinceID = 7 },
                    new District { Name = "Long Điền", ProvinceID = 7 },
                    new District { Name = "Đất Đỏ", ProvinceID = 7 },
                    new District { Name = "Tân Thành", ProvinceID = 7 },
                    new District { Name = "Côn Đảo", ProvinceID = 7 },

                    // Districts of Bạc Liêu
                    new District { Name = "Bạc Liêu", ProvinceID = 8 },
                    new District { Name = "Hồng Dân", ProvinceID = 8 },
                    new District { Name = "Phước Long", ProvinceID = 8 },
                    new District { Name = "Vĩnh Lợi", ProvinceID = 8 },
                    new District { Name = "Giá Rai", ProvinceID = 8 },
                    new District { Name = "Đông Hải", ProvinceID = 8 },
                    new District { Name = "Hòa Bình", ProvinceID = 8 },

                    // Districts of Bắc Kạn
                    new District { Name = "Bắc Kạn", ProvinceID = 9 },
                    new District { Name = "Pác Nặm", ProvinceID = 9 },
                    new District { Name = "Ba Bể", ProvinceID = 9 },
                    new District { Name = "Ngân Sơn", ProvinceID = 9 },
                    new District { Name = "Bạch Thông", ProvinceID = 9 },
                    new District { Name = "Chợ Đồn", ProvinceID = 9 },
                    new District { Name = "Chợ Mới", ProvinceID = 9 },
                    new District { Name = "Na Rì", ProvinceID = 9 },

                    // Districts of Bắc Giang
                    new District { Name = "Bắc Giang", ProvinceID = 10 },
                    new District { Name = "Yên Thế", ProvinceID = 10 },
                    new District { Name = "Tân Yên", ProvinceID = 10 },
                    new District { Name = "Lạng Giang", ProvinceID = 10 },
                    new District { Name = "Lục Nam", ProvinceID = 10 },
                    new District { Name = "Lục Ngạn", ProvinceID = 10 },
                    new District { Name = "Sơn Động", ProvinceID = 10 },
                    new District { Name = "Yên Dũng", ProvinceID = 10 },
                    new District { Name = "Việt Yên", ProvinceID = 10 },
                    new District { Name = "Hiệp Hòa", ProvinceID = 10 },

                    // Districts of Bắc Ninh
                    new District { Name = "Bắc Ninh", ProvinceID = 11 },
                    new District { Name = "Yên Phong", ProvinceID = 11 },
                    new District { Name = "Quế Võ", ProvinceID = 11 },
                    new District { Name = "Tiên Du", ProvinceID = 11 },
                    new District { Name = "Từ Sơn", ProvinceID = 11 },
                    new District { Name = "Thuận Thành", ProvinceID = 11 },
                    new District { Name = "Gia Bình", ProvinceID = 11 },
                    new District { Name = "Lương Tài", ProvinceID = 11 },

                    // Districts of Bến Tre
                    new District { Name = "Bến Tre", ProvinceID = 12 },
                    new District { Name = "Châu Thành", ProvinceID = 12 },
                    new District { Name = "Chợ Lách", ProvinceID = 12 },
                    new District { Name = "Mỏ Cày Nam", ProvinceID = 12 },
                    new District { Name = "Mỏ Cày Bắc", ProvinceID = 12 },
                    new District { Name = "Giồng Trôm", ProvinceID = 12 },
                    new District { Name = "Bình Đại", ProvinceID = 12 },
                    new District { Name = "Ba Tri", ProvinceID = 12 },
                    new District { Name = "Thạnh Phú", ProvinceID = 12 },

                    // Districts of Bình Dương
                    new District { Name = "Thủ Dầu Một", ProvinceID = 13 },
                    new District { Name = "Bến Cát", ProvinceID = 13 },
                    new District { Name = "Tân Uyên", ProvinceID = 13 },
                    new District { Name = "Dĩ An", ProvinceID = 13 },
                    new District { Name = "Thuận An", ProvinceID = 13 },
                    new District { Name = "Bàu Bàng", ProvinceID = 13 },
                    new District { Name = "Bắc Tân Uyên", ProvinceID = 13 },
                    new District { Name = "Phú Giáo", ProvinceID = 13 },
                    new District { Name = "Dầu Tiếng", ProvinceID = 13 },

                    // Districts of Bình Định
                    new District { Name = "Quy Nhơn", ProvinceID = 14 },
                    new District { Name = "An Lão", ProvinceID = 14 },
                    new District { Name = "Hoài Ân", ProvinceID = 14 },
                    new District { Name = "Hoài Nhơn", ProvinceID = 14 },
                    new District { Name = "Phù Mỹ", ProvinceID = 14 },
                    new District { Name = "Phù Cát", ProvinceID = 14 },
                    new District { Name = "Vĩnh Thạnh", ProvinceID = 14 },
                    new District { Name = "Tây Sơn", ProvinceID = 14 },
                    new District { Name = "Vân Canh", ProvinceID = 14 },
                    new District { Name = "An Nhơn", ProvinceID = 14 },
                    new District { Name = "Tuy Phước", ProvinceID = 14 },

                    // Districts of Bình Phước
                    new District { Name = "Đồng Xoài", ProvinceID = 15 },
                    new District { Name = "Phước Long", ProvinceID = 15 },
                    new District { Name = "Bình Long", ProvinceID = 15 },
                    new District { Name = "Bù Gia Mập", ProvinceID = 15 },
                    new District { Name = "Lộc Ninh", ProvinceID = 15 },
                    new District { Name = "Bù Đốp", ProvinceID = 15 },
                    new District { Name = "Hớn Quản", ProvinceID = 15 },
                    new District { Name = "Đồng Phú", ProvinceID = 15 },
                    new District { Name = "Bù Đăng", ProvinceID = 15 },
                    new District { Name = "Chơn Thành", ProvinceID = 15 },
                    new District { Name = "Phú Riềng", ProvinceID = 15 },

                    // Districts of Bình Thuận
                    new District { Name = "Phan Thiết", ProvinceID = 16 },
                    new District { Name = "La Gi", ProvinceID = 16 },
                    new District { Name = "Tuy Phong", ProvinceID = 16 },
                    new District { Name = "Bắc Bình", ProvinceID = 16 },
                    new District { Name = "Hàm Thuận Bắc", ProvinceID = 16 },
                    new District { Name = "Hàm Thuận Nam", ProvinceID = 16 },
                    new District { Name = "Tánh Linh", ProvinceID = 16 },
                    new District { Name = "Đức Linh", ProvinceID = 16 },
                    new District { Name = "Hàm Tân", ProvinceID = 16 },
                    new District { Name = "Phú Quí", ProvinceID = 16 },

                    // Districts of Cà Mau
                    new District { Name = "Cà Mau", ProvinceID = 17 },
                    new District { Name = "U Minh", ProvinceID = 17 },
                    new District { Name = "Thới Bình", ProvinceID = 17 },
                    new District { Name = "Trần Văn Thời", ProvinceID = 17 },
                    new District { Name = "Cái Nước", ProvinceID = 17 },
                    new District { Name = "Đầm Dơi", ProvinceID = 17 },
                    new District { Name = "Năm Căn", ProvinceID = 17 },
                    new District { Name = "Phú Tân", ProvinceID = 17 },
                    new District { Name = "Ngọc Hiển", ProvinceID = 17 },

                    // Districts of Cao Bằng
                    new District { Name = "Cao Bằng", ProvinceID = 18 },
                    new District { Name = "Bảo Lâm", ProvinceID = 18 },
                    new District { Name = "Bảo Lạc", ProvinceID = 18 },
                    new District { Name = "Thông Nông", ProvinceID = 18 },
                    new District { Name = "Hà Quảng", ProvinceID = 18 },
                    new District { Name = "Trà Lĩnh", ProvinceID = 18 },
                    new District { Name = "Trùng Khánh", ProvinceID = 18 },
                    new District { Name = "Hạ Lang", ProvinceID = 18 },
                    new District { Name = "Quảng Uyên", ProvinceID = 18 },
                    new District { Name = "Phục Hòa", ProvinceID = 18 },
                    new District { Name = "Hòa An", ProvinceID = 18 },
                    new District { Name = "Nguyên Bình", ProvinceID = 18 },
                    new District { Name = "Thạch An", ProvinceID = 18 },

                    // Districts of Đắk Lắk
                    new District { Name = "Buôn Ma Thuột", ProvinceID = 19 },
                    new District { Name = "Buôn Đôn", ProvinceID = 19 },
                    new District { Name = "Cư Kuin", ProvinceID = 19 },
                    new District { Name = "Cư M'gar", ProvinceID = 19 },
                    new District { Name = "Ea H'leo", ProvinceID = 19 },
                    new District { Name = "Ea Kar", ProvinceID = 19 },
                    new District { Name = "Ea Súp", ProvinceID = 19 },
                    new District { Name = "Krông Ana", ProvinceID = 19 },
                    new District { Name = "Krông Bông", ProvinceID = 19 },
                    new District { Name = "Krông Búk", ProvinceID = 19 },
                    new District { Name = "Krông Năng", ProvinceID = 19 },
                    new District { Name = "Krông Pắc", ProvinceID = 19 },
                    new District { Name = "Lắk", ProvinceID = 19 },
                    new District { Name = "M'Đrắk", ProvinceID = 19 },

                    // Districts of Đắk Nông
                    new District { Name = "Gia Nghĩa", ProvinceID = 20 },
                    new District { Name = "Cư Jút", ProvinceID = 20 },
                    new District { Name = "Đắk Glong", ProvinceID = 20 },
                    new District { Name = "Đắk Mil", ProvinceID = 20 },
                    new District { Name = "Đắk R'Lấp", ProvinceID = 20 },
                    new District { Name = "Đắk Song", ProvinceID = 20 },
                    new District { Name = "Krông Nô", ProvinceID = 20 },
                    new District { Name = "Tuy Đức", ProvinceID = 20 },

                    // Districts of Điện Biên
                    new District { Name = "Điện Biên Phủ", ProvinceID = 21 },
                    new District { Name = "Mường Lay", ProvinceID = 21 },
                    new District { Name = "Mường Nhé", ProvinceID = 21 },
                    new District { Name = "Mường Chà", ProvinceID = 21 },
                    new District { Name = "Tủa Chùa", ProvinceID = 21 },
                    new District { Name = "Tuần Giáo", ProvinceID = 21 },
                    new District { Name = "Điện Biên", ProvinceID = 21 },
                    new District { Name = "Điện Biên Đông", ProvinceID = 21 },
                    new District { Name = "Mường Ảng", ProvinceID = 21 },
                    new District { Name = "Nậm Pồ", ProvinceID = 21 },

                    // Districts of Đồng Nai
                    new District { Name = "Biên Hòa", ProvinceID = 22 },
                    new District { Name = "Long Khánh", ProvinceID = 22 },
                    new District { Name = "Tân Phú", ProvinceID = 22 },
                    new District { Name = "Vĩnh Cửu", ProvinceID = 22 },
                    new District { Name = "Định Quán", ProvinceID = 22 },
                    new District { Name = "Trảng Bom", ProvinceID = 22 },
                    new District { Name = "Thống Nhất", ProvinceID = 22 },
                    new District { Name = "Cẩm Mỹ", ProvinceID = 22 },
                    new District { Name = "Long Thành", ProvinceID = 22 },
                    new District { Name = "Xuân Lộc", ProvinceID = 22 },
                    new District { Name = "Nhơn Trạch", ProvinceID = 22 },

                    // Districts of Đồng Tháp
                    new District { Name = "Cao Lãnh", ProvinceID = 23 },
                    new District { Name = "Sa Đéc", ProvinceID = 23 },
                    new District { Name = "Hồng Ngự", ProvinceID = 23 },
                    new District { Name = "Tân Hồng", ProvinceID = 23 },
                    new District { Name = "Hồng Ngự", ProvinceID = 23 },
                    new District { Name = "Tam Nông", ProvinceID = 23 },
                    new District { Name = "Tháp Mười", ProvinceID = 23 },
                    new District { Name = "Cao Lãnh", ProvinceID = 23 },
                    new District { Name = "Thanh Bình", ProvinceID = 23 },
                    new District { Name = "Lấp Vò", ProvinceID = 23 },
                    new District { Name = "Lai Vung", ProvinceID = 23 },
                    new District { Name = "Châu Thành", ProvinceID = 23 },

                    // Districts of Gia Lai
                    new District { Name = "Pleiku", ProvinceID = 24 },
                    new District { Name = "An Khê", ProvinceID = 24 },
                    new District { Name = "Ayun Pa", ProvinceID = 24 },
                    new District { Name = "Chư Păh", ProvinceID = 24 },
                    new District { Name = "Chư Prông", ProvinceID = 24 },
                    new District { Name = "Chư Sê", ProvinceID = 24 },
                    new District { Name = "Đắk Đoa", ProvinceID = 24 },
                    new District { Name = "Đắk Pơ", ProvinceID = 24 },
                    new District { Name = "Đức Cơ", ProvinceID = 24 },
                    new District { Name = "Ia Grai", ProvinceID = 24 },
                    new District { Name = "Ia Pa", ProvinceID = 24 },
                    new District { Name = "KBang", ProvinceID = 24 },
                    new District { Name = "Kông Chro", ProvinceID = 24 },
                    new District { Name = "Krông Pa", ProvinceID = 24 },
                    new District { Name = "Mang Yang", ProvinceID = 24 },

                    // Districts of Hà Giang
                    new District { Name = "Hà Giang", ProvinceID = 25 },
                    new District { Name = "Bắc Mê", ProvinceID = 25 },
                    new District { Name = "Bắc Quang", ProvinceID = 25 },
                    new District { Name = "Đồng Văn", ProvinceID = 25 },
                    new District { Name = "Hoàng Su Phì", ProvinceID = 25 },
                    new District { Name = "Mèo Vạc", ProvinceID = 25 },
                    new District { Name = "Quản Bạ", ProvinceID = 25 },
                    new District { Name = "Quang Bình", ProvinceID = 25 },
                    new District { Name = "Vị Xuyên", ProvinceID = 25 },
                    new District { Name = "Xín Mần", ProvinceID = 25 },
                    new District { Name = "Yên Minh", ProvinceID = 25 },

                    // Districts of Hà Nam
                    new District { Name = "Phủ Lý", ProvinceID = 26 },
                    new District { Name = "Duy Tiên", ProvinceID = 26 },
                    new District { Name = "Kim Bảng", ProvinceID = 26 },
                    new District { Name = "Lý Nhân", ProvinceID = 26 },
                    new District { Name = "Thanh Liêm", ProvinceID = 26 },

                    // Districts of Hà Tĩnh
                    new District { Name = "Hà Tĩnh", ProvinceID = 27 },
                    new District { Name = "Hồng Lĩnh", ProvinceID = 27 },
                    new District { Name = "Hương Khê", ProvinceID = 27 },
                    new District { Name = "Hương Sơn", ProvinceID = 27 },
                    new District { Name = "Kỳ Anh", ProvinceID = 27 },
                    new District { Name = "Lộc Hà", ProvinceID = 27 },
                    new District { Name = "Nghi Xuân", ProvinceID = 27 },
                    new District { Name = "Thạch Hà", ProvinceID = 27 },
                    new District { Name = "Cẩm Xuyên", ProvinceID = 27 },
                    new District { Name = "Can Lộc", ProvinceID = 27 },
                    new District { Name = "Đức Thọ", ProvinceID = 27 },
                    new District { Name = "Hồng Lĩnh", ProvinceID = 27 },

                    // Districts of Hải Dương
                    new District { Name = "Hải Dương", ProvinceID = 28 },
                    new District { Name = "Chí Linh", ProvinceID = 28 },
                    new District { Name = "Nam Sách", ProvinceID = 28 },
                    new District { Name = "Kinh Môn", ProvinceID = 28 },
                    new District { Name = "Kim Thành", ProvinceID = 28 },
                    new District { Name = "Thanh Hà", ProvinceID = 28 },
                    new District { Name = "Cẩm Giàng", ProvinceID = 28 },
                    new District { Name = "Thanh Miện", ProvinceID = 28 },
                    new District { Name = "Ninh Giang", ProvinceID = 28 },

                    // Districts of Hậu Giang
                    new District { Name = "Vị Thanh", ProvinceID = 29 },
                    new District { Name = "Ngã Bảy", ProvinceID = 29 },
                    new District { Name = "Châu Thành A", ProvinceID = 29 },
                    new District { Name = "Châu Thành", ProvinceID = 29 },
                    new District { Name = "Phụng Hiệp", ProvinceID = 29 },
                    new District { Name = "Long Mỹ", ProvinceID = 29 },
                    new District { Name = "Vị Thủy", ProvinceID = 29 },

                    // Districts of Hòa Bình
                    new District { Name = "Hòa Bình", ProvinceID = 30 },
                    new District { Name = "Cao Phong", ProvinceID = 30 },
                    new District { Name = "Đà Bắc", ProvinceID = 30 },
                    new District { Name = "Kim Bôi", ProvinceID = 30 },
                    new District { Name = "Kỳ Sơn", ProvinceID = 30 },
                    new District { Name = "Lạc Sơn", ProvinceID = 30 },
                    new District { Name = "Lạc Thủy", ProvinceID = 30 },
                    new District { Name = "Lương Sơn", ProvinceID = 30 },
                    new District { Name = "Mai Châu", ProvinceID = 30 },
                    new District { Name = "Tân Lạc", ProvinceID = 30 },
                    new District { Name = "Yên Thủy", ProvinceID = 30 },

                    // Districts of Hưng Yên
                    new District { Name = "Hưng Yên", ProvinceID = 31 },
                    new District { Name = "Văn Lâm", ProvinceID = 31 },
                    new District { Name = "Văn Giang", ProvinceID = 31 },
                    new District { Name = "Yên Mỹ", ProvinceID = 31 },
                    new District { Name = "Mỹ Hào", ProvinceID = 31 },
                    new District { Name = "Ân Thi", ProvinceID = 31 },
                    new District { Name = "Khoái Châu", ProvinceID = 31 },
                    new District { Name = "Kim Động", ProvinceID = 31 },
                    new District { Name = "Tiên Lữ", ProvinceID = 31 },

                    // Districts of Khánh Hòa
                    new District { Name = "Nha Trang", ProvinceID = 32 },
                    new District { Name = "Cam Ranh", ProvinceID = 32 },
                    new District { Name = "Ninh Hòa", ProvinceID = 32 },
                    new District { Name = "Khánh Vĩnh", ProvinceID = 32 },
                    new District { Name = "Diên Khánh", ProvinceID = 32 },
                    new District { Name = "Khánh Sơn", ProvinceID = 32 },
                    new District { Name = "Trường Sa", ProvinceID = 32 },
                    new District { Name = "Cam Lâm", ProvinceID = 32 },
                    new District { Name = "Vạn Ninh", ProvinceID = 32 },
                    new District { Name = "Xuân Lộc", ProvinceID = 32 },

                    // Districts of Kiên Giang
                    new District { Name = "Rạch Giá", ProvinceID = 33 },
                    new District { Name = "Hà Tiên", ProvinceID = 33 },
                    new District { Name = "Phú Quốc", ProvinceID = 33 },
                    new District { Name = "Kiên Lương", ProvinceID = 33 },
                    new District { Name = "Hòn Đất", ProvinceID = 33 },
                    new District { Name = "Tân Hiệp", ProvinceID = 33 },
                    new District { Name = "Châu Thành", ProvinceID = 33 },
                    new District { Name = "Giồng Riềng", ProvinceID = 33 },
                    new District { Name = "Gò Quao", ProvinceID = 33 },
                    new District { Name = "An Biên", ProvinceID = 33 },
                    new District { Name = "An Minh", ProvinceID = 33 },
                    new District { Name = "Vĩnh Thuận", ProvinceID = 33 },
                    new District { Name = "Phú Quốc", ProvinceID = 33 },

                    // Districts of Kon Tum
                    new District { Name = "Kon Tum", ProvinceID = 34 },
                    new District { Name = "Đắk Glei", ProvinceID = 34 },
                    new District { Name = "Ngọc Hồi", ProvinceID = 34 },
                    new District { Name = "Đắk Tô", ProvinceID = 34 },
                    new District { Name = "Kon Plông", ProvinceID = 34 },
                    new District { Name = "Kon Rẫy", ProvinceID = 34 },
                    new District { Name = "Sa Thầy", ProvinceID = 34 },
                    new District { Name = "Tu Mơ Rông", ProvinceID = 34 },

                    // Districts of Lai Châu
                    new District { Name = "Lai Châu", ProvinceID = 35 },
                    new District { Name = "Tam Đường", ProvinceID = 35 },
                    new District { Name = "Mường Tè", ProvinceID = 35 },
                    new District { Name = "Phong Thổ", ProvinceID = 35 },
                    new District { Name = "Sìn Hồ", ProvinceID = 35 },
                    new District { Name = "Than Uyên", ProvinceID = 35 },
                    new District { Name = "Tân Uyên", ProvinceID = 35 },

                    // Districts of Lạng Sơn
                    new District { Name = "Lạng Sơn", ProvinceID = 36 },
                    new District { Name = "Tràng Định", ProvinceID = 36 },
                    new District { Name = "Bình Gia", ProvinceID = 36 },
                    new District { Name = "Văn Lãng", ProvinceID = 36 },
                    new District { Name = "Bắc Sơn", ProvinceID = 36 },
                    new District { Name = "Hữu Lũng", ProvinceID = 36 },
                    new District { Name = "Chi Lăng", ProvinceID = 36 },
                    new District { Name = "Lộc Bình", ProvinceID = 36 },
                    new District { Name = "Đình Lập", ProvinceID = 36 },

                    // Districts of Lào Cai
                    new District { Name = "Lào Cai", ProvinceID = 37 },
                    new District { Name = "Bát Xát", ProvinceID = 37 },
                    new District { Name = "Bảo Thắng", ProvinceID = 37 },
                    new District { Name = "Bảo Yên", ProvinceID = 37 },
                    new District { Name = "Sa Pa", ProvinceID = 37 },
                    new District { Name = "Văn Bàn", ProvinceID = 37 },
                    new District { Name = "Bắc Hà", ProvinceID = 37 },
                    new District { Name = "Mường Khương", ProvinceID = 37 },

                    // Districts of Lâm Đồng
                    new District { Name = "Đà Lạt", ProvinceID = 38 },
                    new District { Name = "Bảo Lộc", ProvinceID = 38 },
                    new District { Name = "Đức Trọng", ProvinceID = 38 },
                    new District { Name = "Di Linh", ProvinceID = 38 },
                    new District { Name = "Bảo Lâm", ProvinceID = 38 },
                    new District { Name = "Đam Rông", ProvinceID = 38 },
                    new District { Name = "Lạc Dương", ProvinceID = 38 },
                    new District { Name = "Lâm Hà", ProvinceID = 38 },

                    // Districts of Long An
                    new District { Name = "Tân An", ProvinceID = 39 },
                    new District { Name = "Kiến Tường", ProvinceID = 39 },
                    new District { Name = "Tân Hưng", ProvinceID = 39 },
                    new District { Name = "Vĩnh Hưng", ProvinceID = 39 },
                    new District { Name = "Mộc Hóa", ProvinceID = 39 },
                    new District { Name = "Tân Thạnh", ProvinceID = 39 },
                    new District { Name = "Thạnh Hóa", ProvinceID = 39 },
                    new District { Name = "Đức Huệ", ProvinceID = 39 },
                    new District { Name = "Đức Hòa", ProvinceID = 39 },
                    new District { Name = "Bến Lức", ProvinceID = 39 },
                    new District { Name = "Thủ Thừa", ProvinceID = 39 },
                    new District { Name = "Châu Thành", ProvinceID = 39 },

                    // Districts of Nam Định
                    new District { Name = "Nam Định", ProvinceID = 40 },
                    new District { Name = "Mỹ Lộc", ProvinceID = 40 },
                    new District { Name = "Vụ Bản", ProvinceID = 40 },
                    new District { Name = "Ý Yên", ProvinceID = 40 },
                    new District { Name = "Nghĩa Hưng", ProvinceID = 40 },
                    new District { Name = "Nam Trực", ProvinceID = 40 },
                    new District { Name = "Trực Ninh", ProvinceID = 40 },
                    new District { Name = "Xuân Trường", ProvinceID = 40 },
                    new District { Name = "Giao Thủy", ProvinceID = 40 },
                    new District { Name = "Hải Hậu", ProvinceID = 40 },

                    // Districts of Nghệ An
                    new District { Name = "Vinh", ProvinceID = 41 },
                    new District { Name = "Cửa Lò", ProvinceID = 41 },
                    new District { Name = "Thái Hoà", ProvinceID = 41 },
                    new District { Name = "Quế Phong", ProvinceID = 41 },
                    new District { Name = "Quỳ Châu", ProvinceID = 41 },
                    new District { Name = "Kỳ Sơn", ProvinceID = 41 },
                    new District { Name = "Tương Dương", ProvinceID = 41 },
                    new District { Name = "Con Cuông", ProvinceID = 41 },
                    new District { Name = "Anh Sơn", ProvinceID = 41 },
                    new District { Name = "Diễn Châu", ProvinceID = 41 },
                    new District { Name = "Yên Thành", ProvinceID = 41 },
                    new District { Name = "Đô Lương", ProvinceID = 41 },
                    new District { Name = "Thanh Chương", ProvinceID = 41 },
                    new District { Name = "Nam Đàn", ProvinceID = 41 },
                    new District { Name = "Hưng Nguyên", ProvinceID = 41 },
                    new District { Name = "Hoàng Mai", ProvinceID = 41 },
                    new District { Name = "Quỳ Hợp", ProvinceID = 41 },

                    // Districts of Ninh Bình
                    new District { Name = "Ninh Bình", ProvinceID = 42 },
                    new District { Name = "Tam Điệp", ProvinceID = 42 },
                    new District { Name = "Nho Quan", ProvinceID = 42 },
                    new District { Name = "Gia Viễn", ProvinceID = 42 },
                    new District { Name = "Hoa Lư", ProvinceID = 42 },
                    new District { Name = "Yên Khánh", ProvinceID = 42 },
                    new District { Name = "Kim Sơn", ProvinceID = 42 },

                    // Districts of Ninh Thuận
                    new District { Name = "Phan Rang - Tháp Chàm", ProvinceID = 43 },
                    new District { Name = "Bác Ái", ProvinceID = 43 },
                    new District { Name = "Ninh Sơn", ProvinceID = 43 },
                    new District { Name = "Ninh Hải", ProvinceID = 43 },
                    new District { Name = "Ninh Phước", ProvinceID = 43 },
                    new District { Name = "Thuận Bắc", ProvinceID = 43 },
                    new District { Name = "Thuận Nam", ProvinceID = 43 },

                    // Districts of Phú Thọ
                    new District { Name = "Việt Trì", ProvinceID = 44 },
                    new District { Name = "Phú Thọ", ProvinceID = 44 },
                    new District { Name = "Đoan Hùng", ProvinceID = 44 },
                    new District { Name = "Hạ Hoà", ProvinceID = 44 },
                    new District { Name = "Thanh Ba", ProvinceID = 44 },
                    new District { Name = "Phù Ninh", ProvinceID = 44 },
                    new District { Name = "Yên Lập", ProvinceID = 44 },
                    new District { Name = "Cẩm Khê", ProvinceID = 44 },
                    new District { Name = "Tam Nông", ProvinceID = 44 },
                    new District { Name = "Lâm Thao", ProvinceID = 44 },
                    new District { Name = "Thanh Sơn", ProvinceID = 44 },
                    new District { Name = "Thanh Thuỷ", ProvinceID = 44 },

                    // Districts of Phú Yên
                    new District { Name = "Tuy Hòa", ProvinceID = 45 },
                    new District { Name = "Sông Cầu", ProvinceID = 45 },
                    new District { Name = "Đồng Xuân", ProvinceID = 45 },
                    new District { Name = "Tuy An", ProvinceID = 45 },
                    new District { Name = "Sơn Hòa", ProvinceID = 45 },
                    new District { Name = "Sông Hinh", ProvinceID = 45 },
                    new District { Name = "Đông Hòa", ProvinceID = 45 },

                    // Districts of Quảng Bình
                    new District { Name = "Đồng Hới", ProvinceID = 46 },
                    new District { Name = "Minh Hóa", ProvinceID = 46 },
                    new District { Name = "Tuyên Hóa", ProvinceID = 46 },
                    new District { Name = "Quảng Trạch", ProvinceID = 46 },
                    new District { Name = "Bố Trạch", ProvinceID = 46 },
                    new District { Name = "Quảng Ninh", ProvinceID = 46 },
                    new District { Name = "Lệ Thủy", ProvinceID = 46 },
                    new District { Name = "Ba Đồn", ProvinceID = 46 },

                    // Districts of Quảng Nam
                    new District { Name = "Tam Kỳ", ProvinceID = 47 },
                    new District { Name = "Hội An", ProvinceID = 47 },
                    new District { Name = "Duy Xuyên", ProvinceID = 47 },
                    new District { Name = "Đại Lộc", ProvinceID = 47 },
                    new District { Name = "Điện Bàn", ProvinceID = 47 },
                    new District { Name = "Quế Sơn", ProvinceID = 47 },
                    new District { Name = "Nam Giang", ProvinceID = 47 },
                    new District { Name = "Phước Sơn", ProvinceID = 47 },
                    new District { Name = "Hiệp Đức", ProvinceID = 47 },
                    new District { Name = "Thăng Bình", ProvinceID = 47 },
                    new District { Name = "Tiên Phước", ProvinceID = 47 },
                    new District { Name = "Bắc Trà My", ProvinceID = 47 },
                    new District { Name = "Nam Trà My", ProvinceID = 47 },
                    new District { Name = "Núi Thành", ProvinceID = 47 },

                    // Districts of Quảng Ngãi
                    new District { Name = "Quảng Ngãi", ProvinceID = 48 },
                    new District { Name = "Bình Sơn", ProvinceID = 48 },
                    new District { Name = "Trà Bồng", ProvinceID = 48 },
                    new District { Name = "Tây Trà", ProvinceID = 48 },
                    new District { Name = "Sơn Tây", ProvinceID = 48 },
                    new District { Name = "Sơn Hà", ProvinceID = 48 },
                    new District { Name = "Tư Nghĩa", ProvinceID = 48 },
                    new District { Name = "Nghĩa Hành", ProvinceID = 48 },
                    new District { Name = "Mộ Đức", ProvinceID = 48 },
                    new District { Name = "Đức Phổ", ProvinceID = 48 },
                    new District { Name = "Ba Tơ", ProvinceID = 48 },
                    new District { Name = "Lý Sơn", ProvinceID = 48 },

                    // Districts of Quảng Ninh
                    new District { Name = "Uông Bí", ProvinceID = 49 },
                    new District { Name = "Móng Cái", ProvinceID = 49 },
                    new District { Name = "Đông Triều", ProvinceID = 49 },
                    new District { Name = "Quảng Yên", ProvinceID = 49 },
                    new District { Name = "Hải Hà", ProvinceID = 49 },
                    new District { Name = "Ba Chẽ", ProvinceID = 49 },
                    new District { Name = "Bình Liêu", ProvinceID = 49 },
                    new District { Name = "Tiên Yên", ProvinceID = 49 },
                    new District { Name = "Đầm Hà", ProvinceID = 49 },
                    new District { Name = "Hòn Đất", ProvinceID = 49 },
                    new District { Name = "Vân Đồn", ProvinceID = 49 },
                    new District { Name = "Cô Tô", ProvinceID = 49 },

                    // Districts of Quảng Trị
                    new District { Name = "Đông Hà", ProvinceID = 50 },
                    new District { Name = "Quảng Trị", ProvinceID = 50 },
                    new District { Name = "Vĩnh Linh", ProvinceID = 50 },
                    new District { Name = "Hướng Hóa", ProvinceID = 50 },
                    new District { Name = "Gio Linh", ProvinceID = 50 },
                    new District { Name = "Cam Lộ", ProvinceID = 50 },
                    new District { Name = "Triệu Phong", ProvinceID = 50 },
                    new District { Name = "Hải Lăng", ProvinceID = 50 },
                    new District { Name = "Cồn Cỏ", ProvinceID = 50 },

                    // Districts of Sóc Trăng
                    new District { Name = "Sóc Trăng", ProvinceID = 51 },
                    new District { Name = "Châu Thành", ProvinceID = 51 },
                    new District { Name = "Kế Sách", ProvinceID = 51 },
                    new District { Name = "Mỹ Tú", ProvinceID = 51 },
                    new District { Name = "Cù Lao Dung", ProvinceID = 51 },
                    new District { Name = "Long Phú", ProvinceID = 51 },
                    new District { Name = "Mỹ Xuyên", ProvinceID = 51 },
                    new District { Name = "Ngã Năm", ProvinceID = 51 },
                    new District { Name = "Thạnh Trị", ProvinceID = 51 },
                    new District { Name = "Vĩnh Châu", ProvinceID = 51 },

                    // Districts of Sơn La
                    new District { Name = "Sơn La", ProvinceID = 52 },
                    new District { Name = "Quỳnh Nhai", ProvinceID = 52 },
                    new District { Name = "Thuận Châu", ProvinceID = 52 },
                    new District { Name = "Mường La", ProvinceID = 52 },
                    new District { Name = "Phù Yên", ProvinceID = 52 },
                    new District { Name = "Bắc Yên", ProvinceID = 52 },
                    new District { Name = "Mai Sơn", ProvinceID = 52 },
                    new District { Name = "Yên Châu", ProvinceID = 52 },
                    new District { Name = "Mộc Châu", ProvinceID = 52 },
                    new District { Name = "Sông Mã", ProvinceID = 52 },
                    new District { Name = "Vân Hồ", ProvinceID = 52 },

                    // Districts of Tây Ninh
                    new District { Name = "Tây Ninh", ProvinceID = 53 },
                    new District { Name = "Tân Biên", ProvinceID = 53 },
                    new District { Name = "Tân Châu", ProvinceID = 53 },
                    new District { Name = "Dương Minh Châu", ProvinceID = 53 },
                    new District { Name = "Châu Thành", ProvinceID = 53 },
                    new District { Name = "Hòa Thành", ProvinceID = 53 },
                    new District { Name = "Bến Cầu", ProvinceID = 53 },
                    new District { Name = "Gò Dầu", ProvinceID = 53 },
                    new District { Name = "Trảng Bàng", ProvinceID = 53 },

                    // Districts of Thái Bình
                    new District { Name = "Thái Bình", ProvinceID = 54 },
                    new District { Name = "Quỳnh Phụ", ProvinceID = 54 },
                    new District { Name = "Hưng Hà", ProvinceID = 54 },
                    new District { Name = "Đông Hưng", ProvinceID = 54 },
                    new District { Name = "Vũ Thư", ProvinceID = 54 },
                    new District { Name = "Kiến Xương", ProvinceID = 54 },
                    new District { Name = "Tiền Hải", ProvinceID = 54 },

                    // Districts of Thái Nguyên
                    new District { Name = "Thái Nguyên", ProvinceID = 55 },
                    new District { Name = "Sông Công", ProvinceID = 55 },
                    new District { Name = "Định Hóa", ProvinceID = 55 },
                    new District { Name = "Phú Lương", ProvinceID = 55 },
                    new District { Name = "Đồng Hỷ", ProvinceID = 55 },
                    new District { Name = "Võ Nhai", ProvinceID = 55 },
                    new District { Name = "Đại Từ", ProvinceID = 55 },

                    // Districts of Thanh Hóa
                    new District { Name = "Thanh Hóa", ProvinceID = 56 },
                    new District { Name = "Sầm Sơn", ProvinceID = 56 },
                    new District { Name = "Bỉm Sơn", ProvinceID = 56 },
                    new District { Name = "Quan Hóa", ProvinceID = 56 },
                    new District { Name = "Quan Sơn", ProvinceID = 56 },
                    new District { Name = "Mường Lát", ProvinceID = 56 },
                    new District { Name = "Bá Thước", ProvinceID = 56 },
                    new District { Name = "Thiệu Hóa", ProvinceID = 56 },
                    new District { Name = "Hà Trung", ProvinceID = 56 },
                    new District { Name = "Nga Sơn", ProvinceID = 56 },
                    new District { Name = "Như Thanh", ProvinceID = 56 },
                    new District { Name = "Như Xuân", ProvinceID = 56 },
                    new District { Name = "Thạch Thành", ProvinceID = 56 },
                    new District { Name = "Ngọc Lặc", ProvinceID = 56 },
                    new District { Name = "Như Hòa", ProvinceID = 56 },
                    new District { Name = "Vĩnh Lộc", ProvinceID = 56 },
                    new District { Name = "Yên Định", ProvinceID = 56 },
                    new District { Name = "Thọ Xuân", ProvinceID = 56 },
                    new District { Name = "Triệu Sơn", ProvinceID = 56 },
                    new District { Name = "Thiệu Hóa", ProvinceID = 56 },
                    new District { Name = "Hậu Lộc", ProvinceID = 56 },
                    new District { Name = "Quảng Xương", ProvinceID = 56 },
                    new District { Name = "Tĩnh Gia", ProvinceID = 56 },

                    // Districts of Thừa Thiên Huế
                    new District { Name = "Huế", ProvinceID = 57 },
                    new District { Name = "Hương Thủy", ProvinceID = 57 },
                    new District { Name = "Hương Trà", ProvinceID = 57 },
                    new District { Name = "Phong Điền", ProvinceID = 57 },
                    new District { Name = "Quảng Điền", ProvinceID = 57 },
                    new District { Name = "Phú Vang", ProvinceID = 57 },
                    new District { Name = "A Lưới", ProvinceID = 57 },
                    new District { Name = "Phú Lộc", ProvinceID = 57 },

                    // Districts of Tiền Giang
                    new District { Name = "Mỹ Tho", ProvinceID = 58 },
                    new District { Name = "Cái Bè", ProvinceID = 58 },
                    new District { Name = "Cai Lậy", ProvinceID = 58 },
                    new District { Name = "Châu Thành", ProvinceID = 58 },
                    new District { Name = "Gò Công", ProvinceID = 58 },
                    new District { Name = "Gò Công Đông", ProvinceID = 58 },
                    new District { Name = "Gò Công Tây", ProvinceID = 58 },
                    new District { Name = "Tân Phước", ProvinceID = 58 },
                    new District { Name = "Tân Phú Đông", ProvinceID = 58 },

                    // Districts of Trà Vinh
                    new District { Name = "Trà Vinh", ProvinceID = 59 },
                    new District { Name = "Duyên Hải", ProvinceID = 59 },
                    new District { Name = "Càng Long", ProvinceID = 59 },
                    new District { Name = "Châu Thành", ProvinceID = 59 },
                    new District { Name = "Cầu Kè", ProvinceID = 59 },
                    new District { Name = "Tiểu Cần", ProvinceID = 59 },
                    new District { Name = "Trà Cú", ProvinceID = 59 },

                    // Districts of Tuyên Quang
                    new District { Name = "Tuyên Quang", ProvinceID = 60 },
                    new District { Name = "Lâm Bình", ProvinceID = 60 },
                    new District { Name = "Nà Hang", ProvinceID = 60 },
                    new District { Name = "Chiêm Hóa", ProvinceID = 60 },
                    new District { Name = "Hàm Yên", ProvinceID = 60 },
                    new District { Name = "Yên Sơn", ProvinceID = 60 },
                    new District { Name = "Sơn Dương", ProvinceID = 60 },

                    // Districts of Vĩnh Long
                    new District { Name = "Vĩnh Long", ProvinceID = 61 },
                    new District { Name = "Vũng Liêm", ProvinceID = 61 },
                    new District { Name = "Long Hồ", ProvinceID = 61 },
                    new District { Name = "Mang Thít", ProvinceID = 61 },
                    new District { Name = "Bình Minh", ProvinceID = 61 },
                    new District { Name = "Trà Ôn", ProvinceID = 61 },
                    new District { Name = "Tam Bình", ProvinceID = 61 },

                    // Districts of Vĩnh Phúc
                    new District { Name = "Vĩnh Yên", ProvinceID = 62 },
                    new District { Name = "Phúc Yên", ProvinceID = 62 },
                    new District { Name = "Lập Thạch", ProvinceID = 62 },
                    new District { Name = "Tam Dương", ProvinceID = 62 },
                    new District { Name = "Tam Đảo", ProvinceID = 62 },
                    new District { Name = "Bình Xuyên", ProvinceID = 62 },
                    new District { Name = "Yên Lạc", ProvinceID = 62 },

                    // Districts of Yên Bái
                    new District { Name = "Yên Bái", ProvinceID = 63 },
                    new District { Name = "Nghĩa Lộ", ProvinceID = 63 },
                    new District { Name = "Lục Yên", ProvinceID = 63 },
                    new District { Name = "Văn Yên", ProvinceID = 63 },
                    new District { Name = "Mù Căng Chải", ProvinceID = 63 },
                    new District { Name = "Trấn Yên", ProvinceID = 63 },
                    new District { Name = "Trạm Tấu", ProvinceID = 63 },
                    new District { Name = "Văn Chấn", ProvinceID = 63 },
                    new District { Name = "Yên Bình", ProvinceID = 63 }
            };

            context.Districts.AddRange(districts);
            context.SaveChanges();
        }
    }
}