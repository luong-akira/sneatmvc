using PagedList;
using Sneat.MVC.Common;
using Sneat.MVC.DAL;
using Sneat.MVC.Models.Common;
using Sneat.MVC.Models.DTO.Permission;
using Sneat.MVC.Models.DTO.Role;
using Sneat.MVC.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sneat.MVC.Services
{
    public class RoleService
    {
        private readonly SneatContext _dbContext;

        public RoleService(SneatContext dbContext = null)
        {
            if(dbContext == null)
            {
                dbContext = new SneatContext();
            }
            _dbContext = dbContext;
        }

        #region Role management

        public async Task<IPagedList<RoleOutputModel>> SearchRole(int page, int limit, string search = "")
        {
            try
            {
                search = Utils.RemoveDiacritics(search);
                var list = await ListRoleAuthorization();
                var query = list
                    .Where(x => string.IsNullOrEmpty(search)
                        || Utils.RemoveDiacritics(x.Name).Contains(search)
                    )
                    .ToPagedList(page, limit);
                    
                return query;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new List<RoleOutputModel>().ToPagedList(1, 1);
            }
        }

        public async Task<List<RoleOutputModel>> ListRoleAuthorization()
        {
            try
            {
                var listRoles = _dbContext.Roles
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED)
                    .Select(x => new
                    {
                        x.ID,
                        x.Name,
                        x.Description,
                        x.CreatedDate,
                        x.UpdatedDate,
                        UserRoles = x.UserRoles
                        .Select(ur => new UserRoleOutputModel
                        {
                            UserID = ur.User.ID,
                            UserName = ur.User.UserName,
                            UserAvatar = !string.IsNullOrEmpty(ur.User.Avatar) ? ur.User.Avatar : SystemParam.DEFAULT_SYSTEM_IMAGE,
                        })
                    })
                    .AsEnumerable()
                    .Select(x => new RoleOutputModel
                    {
                        ID = x.ID,
                        Name = x.Name,
                        Description = x.Description,
                        CreatedDate = x.CreatedDate,
                        UpdatedDate = x.UpdatedDate,
                        UserRoles = x.UserRoles.ToList()
                    })
                    .OrderByDescending(x => x.ID)
                    .ToList();

                return listRoles;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new List<RoleOutputModel>();
            }
        }

        public async Task<int> CreateRole(RoleInputModel input)
        {
            try
            {
                var role = await _dbContext.Roles
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED 
                        && x.Name.ToLower() == input.Name.ToLower())
                    .FirstOrDefaultAsync();
                if (role != null)
                    return SystemParam.EXISTED_ROLE_NAME_ERR;

                var newRole = new Role
                {
                    Name = input.Name,
                    Description = input.Description,
                    IsDeleted = SystemParam.IS_NOT_DELETED,
                    CreatedDate = DateTime.Now
                };
                _dbContext.Roles.Add(newRole);

                if(input.PermissionIDs.Count > 0)
                {
                    foreach (var permissionID in input.PermissionIDs)
                    {
                        if (permissionID != 0)
                        {
                            var rolePermission = new RolePermission
                            {
                                RoleID = newRole.ID,
                                PermissionID = permissionID
                            };
                            _dbContext.RolePermissions.Add(rolePermission);
                        }
                    }
                }

                await _dbContext.SaveChangesAsync();
                return SystemParam.RETURN_TRUE;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return SystemParam.RETURN_FALSE;
            }
        }

        public async Task<RoleOutputModel> DetailRole(int ID)
        {
            try
            {
                var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.IsDeleted == SystemParam.IS_NOT_DELETED && x.ID == ID);
                var permissinIds = role.RolePermissions.Select(x => x.PermissionID).ToList();

                var roleDetail = new RoleOutputModel
                {
                    ID = role.ID,
                    Name = role.Name,
                    Description = role.Description,
                    PermissionIDs = permissinIds,
                    CreatedDate = role.CreatedDate,
                    UpdatedDate = role.UpdatedDate,
                };

                return roleDetail;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new RoleOutputModel();
            }
        }

        public async Task<int> UpdateRole(RoleOutputModel input)
        {
            try
            {
                var existedRole = await _dbContext.Roles
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                        && x.Name.ToLower() == input.Name.ToLower()
                        && x.ID != input.ID)
                    .FirstOrDefaultAsync();
                if (existedRole != null)
                    return SystemParam.EXISTED_ROLE_NAME_ERR;

                var role = await _dbContext.Roles
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                        && x.ID == input.ID)
                    .FirstOrDefaultAsync();
                if (role == null)
                    return SystemParam.ROLE_NOT_FOUND_ERR;

                role.Name = input.Name;
                role.UpdatedDate = DateTime.Now;
                role.Description = input.Description;

                // Remove the old permission
                _dbContext.RolePermissions.RemoveRange(role.RolePermissions);
                if (input.PermissionIDs.Count > 0)
                {
                    foreach (var permissionID in input.PermissionIDs)
                    {
                        if (permissionID != 0)
                        {
                            var rolePermission = new RolePermission
                            {
                                RoleID = role.ID,
                                PermissionID = permissionID
                            };
                            _dbContext.RolePermissions.Add(rolePermission);
                        }
                    }
                }

                await _dbContext.SaveChangesAsync();
                return SystemParam.RETURN_TRUE;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return SystemParam.RETURN_FALSE;
            }
        }

        public async Task<int> RemoveRole(int ID)
        {
            try
            {
                var role = await _dbContext.Roles
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED
                        && x.ID == ID)
                    .FirstOrDefaultAsync();
                if (role == null)
                    return SystemParam.ROLE_NOT_FOUND_ERR;

                role.IsDeleted = SystemParam.IS_DELETED;
                _dbContext.RolePermissions.RemoveRange (role.RolePermissions);
                _dbContext.UserRoles.RemoveRange (role.UserRoles);

                await _dbContext.SaveChangesAsync();
                return SystemParam.RETURN_TRUE;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return SystemParam.RETURN_FALSE;
            }
        }
        #endregion

        #region Permission management

        public async Task<PermissionTreeModel> GetAllPermissions()
        {
            try
            {
                var listPermission = await _dbContext.Permissions
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED)
                    .ToListAsync();
                var listTreePermission = listPermission
                    .Select(x => new PermissionOutputModel
                    {
                        ID = x.ID,
                        Name = x.Name,
                        Level = x.Level,
                        IsLeaf = x.IsLeaf,
                        ParentID = x.ParentID,
                        TabIcon = x.TabIcon,
                    })
                    .ToList();

                var listPermissionIds = listPermission.Select(x => x.ID).ToList();
                var resultIds = new List<int>();

                foreach ( var id in listPermissionIds )
                {
                    resultIds.AddRange(GetAllNodeAndLeafIdById(id, listPermission, 1));
                }
                resultIds = resultIds.Distinct().ToList();
                var filteredTreePermission = new PermissionTreeModel
                {
                    Childrens = listTreePermission
                        .Where(x => resultIds.Contains(x.ID))
                        .ToList()
                        .GenerateTree(c => c.ID, c => c.ParentID),
                };

                return filteredTreePermission;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new PermissionTreeModel();
            }
        }

        public List<int> GetAllNodeAndLeafIdById(int Id, List<Permission> list, int isGetParent = 0)
        {
            var listIds = new List<int>();
            var PC = list.Where(x => x.ID == Id).FirstOrDefault();
            if(PC.IsLeaf == SystemParam.ACTIVE || isGetParent == SystemParam.ACTIVE)
            {
                listIds.AddRange(GetAllParentId(PC.ID, list));
            }
            if(PC.IsLeaf == SystemParam.ACTIVE)
            {
                listIds.Add(PC.ID);
            }
            else
            {
                listIds.Add(PC.ID);
                var listPCs = list.Where(x => x.ParentID == Id).ToList();
                foreach (var child in listPCs)
                {
                    listIds.AddRange(GetAllChildId(child.ID, list));
                }
            }
            return listIds;
        }

        public List<int> GetAllParentId(int Id, List<Permission> list)
        {
            var result = new List<int>();
            var item = list.Where(x => x.ID == Id).FirstOrDefault();
            if(item == null) { return result; }
            if(item != null && item.ParentID.HasValue)
            {
                result.AddRange(GetAllParentId((int)item.ParentID, list));
            }
            result.Add((int)item.ID);
            return result;
        }

        public List<int> GetAllChildId(int Id, List<Permission> list)
        {
            var result = new List<int>();
            result.Add(Id);
            var items = list.Where(x => x.ParentID == Id).Select(x => x.ID).ToList();
            if (items.Count > 0) { result.AddRange(items); }
            items.ForEach(x =>
            {
                result.AddRange(GetAllChildId(x, list));
            });
            return result;
        }

        #endregion

    }
}