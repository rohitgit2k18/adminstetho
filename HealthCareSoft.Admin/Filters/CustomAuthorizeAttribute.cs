using HealthCareSoft.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HealthCareSoft.Admin
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) &&
                !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                if (ApplicationSession.CurrentUser == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new
                            RouteValueDictionary(new { controller = "CommonView", action = "Login" }));
                    return;
                }

                if (Roles == null || Roles == "" || Roles.Contains(ApplicationSession.CurrentUser.RoleId.ToString()))
                {

                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new
                            RouteValueDictionary(new { controller = "CommonView", action = "Login" }));
                }
            }


        }
    }
}