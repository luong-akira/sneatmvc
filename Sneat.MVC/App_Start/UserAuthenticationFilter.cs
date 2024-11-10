using Sneat.MVC.Common;
using Sneat.MVC.Models.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Filters;
using System.Web.Mvc;
using System.Web.Routing;
using Sneat.MVC.DAL;
using Sneat.MVC.Models.Entity;

namespace Sneat.MVC.App_Start
{
    public class UserAuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        public SneatContext cnn = new SneatContext();
        readonly int Role1 = 0;
        readonly int Role2 = 0;
        public UserAuthenticationFilter()
        {

        }
        public UserAuthenticationFilter(int role1, int role2)
        {
            this.Role1 = role1;
            this.Role2 = role2;
        }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            UserDetailOutputModel ss = (UserDetailOutputModel)HttpContext.Current.Session[SystemParam.SESSION_LOGIN];
            if (ss != null)
            {
                int currentUserID = ss.ID;
                var us = cnn.Users.Where(u => u.ID == currentUserID && u.IsDeleted == SystemParam.IS_NOT_DELETED).FirstOrDefault();
              
                if (us != null)
                {
                    var roleIds = us.UserRoles.Select(u => u.RoleID).ToList();
                    var permissionIds = cnn.RolePermissions
                        .Where(rp => roleIds.Contains(rp.RoleID))
                        .Select(rp => rp.PermissionID)
                        .Distinct()
                        .ToList();
                    var listPermissionTabs = cnn.Permissions
                        .Where(p => permissionIds.Contains(p.ID))
                        .Select(p => p.TabID)
                        .ToList();
                    var projectIDs = us.UserProjects
                         .Where(x => x.Project.IsDeleted == SystemParam.IS_NOT_DELETED)
                         .Select(x => x.ProjectID)
                         .ToList();
                    var userProjects = cnn.Projects
                            .Where(x => projectIDs.Contains(x.ID))
                            .Select(x => new ProjectUserOutputModel
                            {
                                ProjectID = x.ID,
                                ProjectName = x.Name,
                            })
                            .ToList();
                    // Update the session with the latest user details
                    UserDetailOutputModel data = new UserDetailOutputModel
                    {
                        UserName = us.UserName,
                        ID = us.ID,
                        Phone = us.Phone,
                        Email = us.Email,
                        Avatar = us.Avatar,
                        Status = (int?)us.Status,
                        PermissionTabs = listPermissionTabs,
                        ListProjects = userProjects,
                        TotalProjects = userProjects != null ? userProjects.Count : 0,
                    };

                    HttpContext.Current.Session[SystemParam.SESSION_LOGIN] = data;
                }
            }

            else
            {
                //Chuyen ve trang dang nhap
                var routeValues = new RouteValueDictionary();
                routeValues["controller"] = "Home";
                routeValues["action"] = "Login";
                //routeValues["action"] = "LoginExpired";
                filterContext.Result = new RedirectToRouteResult(routeValues);
            }
        }

        //Runs after the OnAuthentication method  
        //------------//
        //OnAuthenticationChallenge:- if Method gets called when Authentication or Authorization is 
        //failed and this method is called after
        //Execution of Action Method but before rendering of View
        //------------//
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            //We are checking Result is null or Result is HttpUnauthorizedResult 
            // if yes then we are Redirect to Error View
            /* if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
             {
                 filterContext.Result = new ViewResult
                 {
                     ViewName = "Error"
                 };
             }*/
        }
    }
}