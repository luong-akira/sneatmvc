using Sneat.MVC.Common;
using Sneat.MVC.DAL;
using Sneat.MVC.Models.APIModel;
using Sneat.MVC.Models.DTO.User;
using Sneat.MVC.Models.Entity;
using Sneat.MVC.Models.Enum;
using Sneat.MVC.Templates;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sneat.MVC.Services
{
    public class AuthenticationService
    {
        private readonly SneatContext _dbContext;
        public ResponseService _responseService = new ResponseService();
        public AuthenticationService(SneatContext dbContext = null)
        {
            if (dbContext == null)
            {
                dbContext = new SneatContext();
            }
            _dbContext = dbContext;
        }

        #region Authentication WebAdmin MVC
        public async Task<int> UserLogin(string phone, string password)
        {
            try
            {
                var user = await _dbContext.Users
                    .Where(u => u.IsDeleted == SystemParam.IS_NOT_DELETED
                        && (u.Phone.Equals(phone) || u.Email.Equals(phone)))
                    .FirstOrDefaultAsync();

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

                if (user == null)
                    return SystemParam.INVALID_EMAIL_OR_PASSWORD_ERR;
                if (user.Status == Status.IN_ACTIVE)
                    return SystemParam.ACCOUNT_HAD_BEEN_BLOCKED_ERR;

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
                var userDetail = new UserDetailOutputModel
                {
                    UserName = user.UserName,
                    ID = user.ID,
                    Phone = user.Phone,
                    Email = user.Email,
                    Avatar = user.Avatar,
                    Status = (int?)user.Status,
                    PermissionTabs = listPermissionTabs,
                    ListProjects = userProjects,
                    TotalProjects = userProjects != null ? userProjects.Count : 0,
                };
                HttpContext.Current.Session[SystemParam.SESSION_LOGIN] = userDetail;
                return SystemParam.RETURN_TRUE;

                return SystemParam.INVALID_EMAIL_OR_PASSWORD_ERR;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return SystemParam.RETURN_FALSE;
            }
        }

        public async Task<int> ForgotPassword(string email)
        {
            try
            {
                var emailService = new EmailService();
                var user = await _dbContext.Users
                    .Where(u => u.IsDeleted == SystemParam.IS_NOT_DELETED && u.Email.Equals(email))
                    .FirstOrDefaultAsync();
                if (user == null)
                    return SystemParam.INVALID_EMAIL_ERR;

                var random = new Random();
                string newPass = random.Next(100000, 999999).ToString();
                user.Password = Utils.GenPass(newPass);
                await _dbContext.SaveChangesAsync();

                var loginUrl = Utils.getFullUrl();

                // HTML content for the email
                string htmlContent = SendMailTemplate.ForgotPasswordTemplate(user.UserName, newPass, loginUrl);

                // Send the email asynchronously
                emailService.configClient(email, SystemParam.EMAIL_TITLE, htmlContent);

                return SystemParam.RETURN_TRUE;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return SystemParam.RETURN_FALSE;
            }
        }

        public async Task<int> ChangePassword(int ID, string currentPass, string newPass)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(ID);
                if (Utils.CheckPass(currentPass, user.Password))
                {
                    user.Password = Utils.GenPass(newPass);
                    await _dbContext.SaveChangesAsync();
                    return SystemParam.RETURN_TRUE;
                }
                return SystemParam.INVALID_PASSWORD_ERR;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return SystemParam.RETURN_FALSE;
            }
        }

        #endregion

        #region Authentication WebAdmin React
        public async Task<JsonResultModel> CheckLoginWeb(string phone, string password)
        {
            UserService _userService = new UserService(_dbContext);
            try
            {
                string token = Utils.CreateMD5(DateTime.Now.ToString());

                var user = await _dbContext.Users
                   .Where(u => u.IsDeleted == SystemParam.IS_NOT_DELETED
                       && (u.Phone.Equals(phone) || u.Email.Equals(phone)))
                   .FirstOrDefaultAsync();

                if (user == null)
                    return _responseService.ErrorResult(SystemParam.INVALID_EMAIL_OR_PASSWORD_ERR_STR, SystemParam.INVALID_EMAIL_OR_PASSWORD_ERR);
                if (user.Status == Status.IN_ACTIVE)
                    return _responseService.ErrorResult(SystemParam.ACCOUNT_HAD_BEEN_BLOCKED_ERR_STR, SystemParam.ACCOUNT_HAD_BEEN_BLOCKED_ERR);

                if (Utils.CheckPass(password, user.Password))
                {
                    user.Token = token;
                    user.Token = Utils.GenerateJWTAuthetication(user.ID, user.Email);
                    await _dbContext.SaveChangesAsync();

                    var userDetail = await _userService.DetailUser(user.ID);
                    return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, userDetail);
                }

                return _responseService.ErrorResult(SystemParam.INVALID_EMAIL_OR_PASSWORD_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return _responseService.serverError();
            }
        }

        public async Task<JsonResultModel> ForgotPasswordWeb(string email)
        {
            try
            {
                var emailService = new EmailService();
                var user = await _dbContext.Users
                    .Where(u => u.IsDeleted == SystemParam.IS_NOT_DELETED && u.Email.Equals(email))
                    .FirstOrDefaultAsync();
                if (user == null)
                    return _responseService.ErrorResult(SystemParam.INVALID_EMAIL_OR_PASSWORD_ERR_STR, SystemParam.INVALID_EMAIL_ERR);

                var random = new Random();
                string newPass = random.Next(100000, 999999).ToString();
                user.Password = Utils.GenPass(newPass);
                await _dbContext.SaveChangesAsync();

                var loginUrl = Utils.getFullUrl();

                // HTML content for the email
                string htmlContent = SendMailTemplate.ForgotPasswordTemplate(user.UserName, newPass, loginUrl);

                // Send the email asynchronously
                emailService.configClient(email, SystemParam.EMAIL_TITLE, htmlContent);

                return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, "");
            }
            catch (Exception ex)
            {
                ex.ToString();
                return _responseService.serverError();
            }
        }

        public async Task<JsonResultModel> ChangePasswordWeb(int ID, string currentPass, string newPass)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(ID);
                if (Utils.CheckPass(currentPass, user.Password))
                {
                    user.Password = Utils.GenPass(newPass);
                    await _dbContext.SaveChangesAsync();
                    return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, "");
                }

                return _responseService.ErrorResult(SystemParam.INVALID_PASSWORD_ERR_STR, SystemParam.SERVER_ERROR_CODE);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return _responseService.serverError();
            }
        }
        #endregion

        public async Task<JsonResultModel> GetDetails(string token)
        {
            UserService _userService = new UserService(_dbContext);
            try
            {
                var user = await _dbContext.Users
                   .Where(u => u.IsDeleted == SystemParam.IS_NOT_DELETED
                       && (u.Token == token))
                   .FirstOrDefaultAsync();

                if (user == null)
                    return _responseService.ErrorResult(SystemParam.ACCOUNT_HAD_BEEN_BLOCKED_ERR_STR, SystemParam.ACCOUNT_HAD_BEEN_BLOCKED_ERR);
                if (user.Status == Status.IN_ACTIVE)
                    return _responseService.ErrorResult(SystemParam.ACCOUNT_HAD_BEEN_BLOCKED_ERR_STR, SystemParam.ACCOUNT_HAD_BEEN_BLOCKED_ERR);
                var userDetail = await _userService.DetailUser(user.ID);
                return _responseService.SuccessResult(SystemParam.MESSAGE_SUCCESS, userDetail);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return _responseService.serverError();
            }
        }

        public async Task<int> GetUserLoginID(string token)
        {
            UserService _userService = new UserService(_dbContext);
            try
            {
                var user = await _dbContext.Users
                 .Where(u => u.IsDeleted == SystemParam.IS_NOT_DELETED
                     && (u.Token == token))
                 .FirstOrDefaultAsync();

                if (user == null)
                    return SystemParam.ACCOUNT_NOT_FOUND_ERR;
                if (user.Status == Status.IN_ACTIVE)
                    return SystemParam.ACCOUNT_HAD_BEEN_BLOCKED_ERR;

                return user.ID;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return SystemParam.RETURN_FALSE;
            }
        }
    }
}