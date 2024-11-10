using PagedList;
using Sneat.MVC.Common;
using Sneat.MVC.DAL;
using Sneat.MVC.Models.DTO.Team;
using Sneat.MVC.Models.DTO.User;
using Sneat.MVC.Models.Entity;
using Sneat.MVC.Models.Enum;
using Sneat.MVC.Templates;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Sneat.MVC.Services
{
    public class UserService
    {
        private readonly SneatContext _dbContext;

        public UserService(SneatContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region User management

        public IPagedList<UserDetailOutputModel> Search(int page, int limit, string search = "", int? projectID = null, string teamIDs = null)
        {
            try
            {
                var list = GetListUser(search, projectID, teamIDs);
                var listPaging = list.ToPagedList(page, limit);
                return listPaging;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new List<UserDetailOutputModel>().ToPagedList(1, 1);
            }
        }

        public List<UserDetailOutputModel> GetListUser(string search = "", int? projectID = null, string teamIDs = null)
        {
            try
            {
                List<int> teamIDList = teamIDs?.Split(',')
                                .Select(int.Parse)
                                .ToList() ?? new List<int>();
                search = Utils.RemoveDiacritics(search);
                var userTeams = _dbContext.UserTeams
                    .Where(ur => ur.Team.IsDeleted == SystemParam.IS_NOT_DELETED)
                    .Select(ur => new TeamUserOutputModel
                    {
                        UserID = ur.UserID,
                        TeamID = ur.TeamID,
                        TeamName = ur.Team.Name,
                    })
                    .Distinct()
                    .ToList();
                var query = (from u in _dbContext.Users
                             where u.IsDeleted == SystemParam.IS_NOT_DELETED
                             && (projectID.HasValue ? u.UserProjects.Where(x => x.ProjectID == projectID.Value).FirstOrDefault() != null : true)
                             orderby u.ID descending
                             select new
                             {
                                 IsDeleted = u.IsDeleted,
                                 ID = u.ID,
                                 Status = (int?)u.Status,
                                 UserName = u.UserName,
                                 Phone = u.Phone,
                                 CreateDate = u.CreatedDate,
                                 Email = u.Email,
                                 Avatar = u.Avatar,
                             })
                             .AsEnumerable()
                             .Select(u => new UserDetailOutputModel
                             {
                                 ID = u.ID,
                                 IsDeleted = u.IsDeleted,
                                 Status = (int?)u.Status,
                                 UserName = u.UserName,
                                 Phone = u.Phone,
                                 CreateDate = u.CreateDate,
                                 Email = u.Email,
                                 Avatar = u.Avatar,
                                 UserTeams = userTeams.Where(ur => ur.UserID == u.ID).ToList(),
                             })
                            .Where(x => string.IsNullOrEmpty(search)
                                || Utils.RemoveDiacritics(x.UserName).Contains(search)
                                || Utils.RemoveDiacritics(x.Phone).Contains(search)
                                || Utils.RemoveDiacritics(x.Email).Contains(search))
                            .Where(u => teamIDList.Count > 0
                                ? u.UserTeams.Select(ur => ur.TeamID).Any(teamID => teamIDList.Contains(teamID))
                                : true)
                            .ToList();

                return query;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new List<UserDetailOutputModel>();
            }
        }

        /*  public async Task<ExcelPackage> ExportListUser(string search = "", int? teamID = null)
          {
              try
              {
                  var list = await GetListUser(search, teamID);
                  FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath(@"/Template/ListCustomerReport.xlsx"));
                  ExcelPackage pack = new ExcelPackage(file);
                  ExcelWorksheet sheet = pack.Workbook.Worksheets[1];
                  int row = 3;
                  int stt = 1;
                  if (list != null && list.Count > 0)
                      foreach (var dt in data)
                      {
                          sheet.Cells[row, 1].Value = stt;
                          sheet.Cells[row, 2].Value = dt.CustomerName;
                          sheet.Cells[row, 3].Value = dt.PhoneNumber;
                          sheet.Cells[row, 4].Value = string.Format("{0:#,0}", Convert.ToDecimal(dt.RankingPoint));
                          sheet.Cells[row, 5].Value = dt.RankingName;
                          sheet.Cells[row, 6].Value = dt.ProvinceName;
                          sheet.Cells[row, 7].Value = dt.DistrictName;
                          switch (dt.Status)
                          {
                              case SystemParam.ACTIVE:
                                  sheet.Cells[row, 8].Value = "Hoạt động";
                                  break;
                              case SystemParam.DEACTIVE:
                                  sheet.Cells[row, 8].Value = "Ngừng hoạt động";
                                  break;
                          }
                          sheet.Cells[row, 9].Value = dt.CreateDate.GetValueOrDefault().ToString(SystemParam.CONVERT_DATETIME);
                          row++;
                          stt++;
                      }
                  return pack;
              }
              catch (Exception ex)
              {
                  ravenClient.Capture(new SentryEvent(ex));
                  return new ExcelPackage();
              }
          }*/

        public async Task<int> CreateUser(UserInputModel input)
        {
            try
            {
                var users = await _dbContext.Users
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED)
                    .ToListAsync();

                var checkMail = users.Count(u => u.Email == input.Email);
                var checkPhone = users.Count(u => u.Phone == input.Phone);
                if (checkMail > 0)
                    return SystemParam.EMAIL_USED_ERR;
                if (checkPhone > 0)
                    return SystemParam.PHONE_USED_ERR;

                var user = new User
                {
                    UserName = input.Name,
                    Email = input.Email,
                    Phone = input.Phone,
                    Avatar = input.Avatar,
                    Password = Utils.GenPass(input.Password),
                    CreatedDate = DateTime.Now,
                    IsDeleted = SystemParam.IS_NOT_DELETED,
                    Status = Status.ACTIVE,
                };
                _dbContext.Users.Add(user);

                var bank = await _dbContext.Banks.Where(x => x.Bin == input.BankBin).FirstOrDefaultAsync();

                // Validate DateOfBirth
                var validDOB = Utils.DefaultDateTime(input.DOB);

                // Validate IdentityReceivedDate
                var validIdentityReceivedDate = Utils.DefaultDateTime(input.IdentityReceivedDate);

                var userDetail = new UserDetail
                {
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    DateOfBirth = input.DOB != null ? input.DOB : validDOB,
                    Gender = input.Gender,
                    Identity = input.Identity,
                    IdentityReceivedDate = input.IdentityReceivedDate != null ? input.IdentityReceivedDate : validIdentityReceivedDate,
                    IdentityReceivedPlace = input.IdentityReceivedPlace,
                    IdentityImages = input.IdentityImages,
                    BankID = bank != null ? bank.ID : default(int),
                    BankAccountName = input.BankAccountName,
                    BankAccountNo = input.BankAccountNo,
                    BankQRImage = input.BankQRImage,
                    DistrictHomeID = input.DistrictHomeID,
                    HomeAddress = input.HomeAddress,
                    DistrictOfficeID = input.DistrictOfficeID,
                    OfficeAddress = input.OfficeAddress,
                    UserID = user.ID,
                };
                _dbContext.UserDetails.Add(userDetail);

                // Add User authorization roles
                if (input.RoleIds.Count > 0)
                {
                    foreach (var roleId in input.RoleIds)
                    {
                        var userRole = new UserRole
                        {
                            RoleID = roleId,
                            UserID = user.ID,
                        };
                        _dbContext.UserRoles.Add(userRole);
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

        public async Task<int> UpdateUser(UpdateUserInputModel input)
        {
            try
            {
                var user = await _dbContext.Users
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED && x.ID == input.ID)
                    .FirstOrDefaultAsync();
                if (user == null)
                    return SystemParam.ACCOUNT_NOT_FOUND_ERR;

                var users = await _dbContext.Users
                    .Where(x => x.IsDeleted == SystemParam.IS_NOT_DELETED)
                    .ToListAsync();

                var checkMail = users.Count(u => u.Email == input.Email && u.ID != input.ID);
                var checkPhone = users.Count(u => u.Phone == input.Phone && u.ID != input.ID);
                if (checkMail > 0)
                    return SystemParam.EMAIL_USED_ERR;
                if (checkPhone > 0)
                    return SystemParam.PHONE_USED_ERR;

                user.UserName = input.Name;
                user.Email = input.Email;
                user.Phone = input.Phone;
                user.Avatar = input.Avatar;
                user.UpdatedDate = DateTime.Now;
                user.Status = (Status)input.Status;

                // Validate DateOfBirth
                var validDOB = Utils.DefaultDateTime(input.DOB);

                // Validate IdentityReceivedDate
                var validIdentityReceivedDate = Utils.DefaultDateTime(input.IdentityReceivedDate);

                var bank = await _dbContext.Banks.Where(x => x.Bin == input.BankBin).FirstOrDefaultAsync();
                var userDetail = await _dbContext.UserDetails.Where(x => x.UserID == input.ID).FirstOrDefaultAsync();
                if (userDetail != null)
                {
                    userDetail.FirstName = input.FirstName;
                    userDetail.LastName = input.LastName;
                    userDetail.DateOfBirth = input.DOB != null ? input.DOB : validDOB;
                    userDetail.Gender = input.Gender;
                    userDetail.Identity = input.Identity;
                    userDetail.IdentityReceivedDate = input.IdentityReceivedDate != null ? input.IdentityReceivedDate : validIdentityReceivedDate;
                    userDetail.IdentityReceivedPlace = input.IdentityReceivedPlace;
                    userDetail.IdentityImages = input.IdentityImages;
                    userDetail.BankID = bank != null ? bank.ID : (int?)null;
                    userDetail.BankAccountName = input.BankAccountName;
                    userDetail.BankAccountNo = input.BankAccountNo;
                    userDetail.BankQRImage = input.BankQRImage;
                    userDetail.DistrictHomeID = input.DistrictHomeID;
                    userDetail.HomeAddress = input.HomeAddress;
                    userDetail.DistrictOfficeID = input.DistrictOfficeID;
                    userDetail.OfficeAddress = input.OfficeAddress;
                }
                else
                {
                    var newUserDetail = new UserDetail
                    {
                        FirstName = input.FirstName,
                        LastName = input.LastName,
                        DateOfBirth = input.DOB,
                        Gender = input.Gender,
                        Identity = input.Identity,
                        IdentityReceivedDate = input.IdentityReceivedDate,
                        IdentityReceivedPlace = input.IdentityReceivedPlace,
                        IdentityImages = input.IdentityImages,
                        BankID = bank != null ? bank.ID : (int?)null,
                        BankAccountName = input.BankAccountName,
                        BankAccountNo = input.BankAccountNo,
                        BankQRImage = input.BankQRImage,
                        DistrictHomeID = input.DistrictHomeID,
                        HomeAddress = input.HomeAddress,
                        DistrictOfficeID = input.DistrictOfficeID,
                        OfficeAddress = input.OfficeAddress,
                        UserID = user.ID,
                    };
                    _dbContext.UserDetails.Add(newUserDetail);
                }

                // Update user authorization roles
                _dbContext.UserRoles.RemoveRange(user.UserRoles);
                if (input.RoleIds.Count > 0)
                {
                    foreach (var roleId in input.RoleIds)
                    {
                        var userRole = new UserRole
                        {
                            RoleID = roleId,
                            UserID = user.ID,
                        };
                        _dbContext.UserRoles.Add(userRole);
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

        public async Task<UpdateUserInputModel> DetailUser(int ID)
        {
            try
            {
                var user = await _dbContext.Users
                    .Where(u => u.ID == ID && u.IsDeleted == SystemParam.IS_NOT_DELETED)
                    .FirstOrDefaultAsync();
                if (user == null)
                    return new UpdateUserInputModel();

                var userDetal = await _dbContext.UserDetails
                    .Where(ud => ud.UserID == ID)
                    .FirstOrDefaultAsync();
                if (userDetal == null)
                    userDetal = new UserDetail();

                var roleIds = user.UserRoles.Select(u => u.RoleID).ToList();
                var permissionIds = await _dbContext.RolePermissions
                    .Where(rp => roleIds.Contains(rp.RoleID))
                    .Select(rp => rp.PermissionID)
                    .Distinct()
                    .ToListAsync();
                var listPermissionTabs = await _dbContext.Permissions
                    .Where(p => permissionIds.Contains(p.ID))
                    .Select(p => p.TabID)
                    .ToListAsync();

                var projectIDs = user.UserProjects
                       .Where(x => x.Project.IsDeleted == SystemParam.IS_NOT_DELETED)
                       .Select(x => x.ProjectID)
                       .ToList();
                var userProjects = _dbContext.Projects
                        .Where(x => projectIDs.Contains(x.ID))
                        .Select(x => new ProjectUserOutputModel
                        {
                            ProjectID = x.ID,
                            ProjectName = x.Name,
                        })
                        .ToList();

                var result = new UpdateUserInputModel
                {
                    ID = ID,
                    Status = (int)user.Status,
                    Name = user.UserName,
                    Phone = user.Phone,
                    Email = user.Email,
                    Avatar = user.Avatar,
                    CreateDate = user.CreatedDate,
                    RoleIds = user.UserRoles.Select(u => u.RoleID).ToList(),

                    FirstName = userDetal.FirstName,
                    LastName = userDetal.LastName,
                    DOB = userDetal.DateOfBirth,
                    Gender = userDetal.Gender,

                    Identity = userDetal.Identity,
                    IdentityReceivedDate = userDetal.IdentityReceivedDate,
                    IdentityReceivedPlace = userDetal.IdentityReceivedPlace,
                    IdentityImages = userDetal.IdentityImages,

                    BankBin = userDetal.Bank != null ? userDetal.Bank.Bin : "",
                    BankAccountName = userDetal.BankAccountName,
                    BankAccountNo = userDetal.BankAccountNo,
                    BankQRImage = userDetal.BankQRImage,
                    ProvinceHomeID = userDetal.DistrictHome != null ? userDetal.DistrictHome.ProvinceID : (int?)null,
                    DistrictHomeID = userDetal.DistrictHomeID,
                    HomeAddress = userDetal.HomeAddress,

                    ProvinceOfficeID = userDetal.DistrictOffice != null ? userDetal.DistrictOffice.ProvinceID : (int?)null,
                    DistrictOfficeID = userDetal.DistrictOfficeID,
                    OfficeAddress = userDetal.OfficeAddress,

                    PermissionTabs = listPermissionTabs,
                    ListProjects = userProjects,
                    TotalProjects = userProjects != null ? userProjects.Count : 0,
                    Token = user.Token
                };

                return result;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return new UpdateUserInputModel();
            }
        }

        public async Task<int> DeleteUser(int ID)
        {
            try
            {
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(x => x.IsDeleted == SystemParam.IS_NOT_DELETED && x.ID == ID);
                if (user == null)
                    return SystemParam.ACCOUNT_NOT_FOUND_ERR;
                user.IsDeleted = SystemParam.IS_DELETED;
                await _dbContext.SaveChangesAsync();

                return SystemParam.RETURN_TRUE;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return SystemParam.RETURN_FALSE;
            }
        }

        public async Task<int> ChangeUserStatus(int ID, int isAcive)
        {
            try
            {
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(x => x.ID == ID && x.IsDeleted == SystemParam.IS_NOT_DELETED);
                if (user == null)
                    return SystemParam.ACCOUNT_NOT_FOUND_ERR;

                if (isAcive == SystemParam.ACTIVE)
                    user.Status = Status.ACTIVE;
                else
                    user.Status = Status.IN_ACTIVE;

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

    }
}