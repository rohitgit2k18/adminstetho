using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HealthCareSoft.Admin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "CommonView", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            "UserEdit",
            "Doctors/EditUser/{id}",
            new { controller = "Doctors", action = "EditUser", id = UrlParameter.Optional }
            );
        }
    }
}
