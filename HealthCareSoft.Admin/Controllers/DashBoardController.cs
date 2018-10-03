using HealthCareSoft.Entity;
using HealthCareSoft.Repository.Interfaces;
using HealthCareSoft.Repository.Models;
using HealthCareSoft.Repository.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthCareSoft.Admin.Controllers
{
    [CustomAuthorize]
    public class DashBoardController : Controller
    {
        
        private readonly IUserServices _userservice;
        public DashBoardController()
        {
            _userservice = new UserService();
        }
        // GET: DashBoard
           public ActionResult Index()
        {
            return View();
        }
         
        public JsonResult GetAllCount()
        {
           
            try
            {
                var v = (from a in _userservice.GetAllUsers() select a);
                UserCountEntity obj = new UserCountEntity();
                obj.TotalDoctor = v.Where(x => x.RoleId == 1).Count();
                obj.TotalPatient = v.Where(x => x.RoleId == 3).Count();
                obj.TotalUsers = v.Count();           
                return Json(obj);
            }
            catch (Exception ex)
            {

            }
            return Json(null);
        }
    }
}