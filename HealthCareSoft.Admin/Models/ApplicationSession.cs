using HealthCareSoft.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCareSoft.Admin.Models
{
    public class ApplicationSession
    {
        private static string key = "Login";
        public static long UserId { get; set; }
        public static void Login(UserEntity objUser)
        {
            System.Web.HttpContext.Current.Session[key] = objUser;
            System.Web.HttpContext.Current.Session.Timeout = 60;
            UserId = objUser.Id;

        }

        public static UserEntity CurrentUser
        {
            get
            {
                if ((System.Web.HttpContext.Current.Session != null))
                {
                    return (UserEntity)System.Web.HttpContext.Current.Session[key];
                }
                else
                {
                    return null;
                }
            }
        }

        public static void Logout()
        {
            System.Web.HttpContext.Current.Session.Clear();
            System.Web.HttpContext.Current.Session.Abandon();
        }
    }
}