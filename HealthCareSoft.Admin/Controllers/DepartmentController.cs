using HealthCareSoft.Admin.Models;
using HealthCareSoft.Repository.Interfaces;
using HealthCareSoft.Repository.Models;
using HealthCareSoft.Repository.Services;
using System;
using System.Linq.Dynamic;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthCareSoft.Admin.Controllers
{
    [CustomAuthorize]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentServices _departmentservice;
        public DepartmentController()
        {
            _departmentservice = new DepartmentService();
        }
        // GET: Department
        public ActionResult Index()
        {
            return View();
        }
         public ActionResult DeptList()
        {
            return View();
        }
        public ActionResult LoadDept()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var search = Request.Form.GetValues("search[value]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            // dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            var v = (from a in _departmentservice.GetAllDepartment() select a);
            if (!string.IsNullOrEmpty(search))
            {
                v = v.Where(a => a.Name.ToLower().StartsWith(search.ToLower()) || a.Name.ToLower().StartsWith(search.ToLower()));
            }
            // SORT
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                v = v.OrderBy(sortColumn + " " + sortColumnDir);
            }
            recordsTotal = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        }

        //CRUD operations for Users
        public ActionResult DepartmentStatus(string id)
        {
            bool result = _departmentservice.DepartmentStatus(Convert.ToInt32(id));
            if (result)
            {
                return RedirectToAction("DeptList", "Department");
            }
            else
            {
                TempData["Delete"] = string.Format("Error Coming While Deleting Data");
                return RedirectToAction("DeptList", "Department");
            }
        }

        [HttpGet]
        public ActionResult EditDepartment(string id)
        {
            DepartmentEntity data = _departmentservice.GetDepartmentById(Convert.ToInt64(id));
            return PartialView("_UserEdit", data);
        }
        [HttpPost]
        public ActionResult EditDepartment(DepartmentEntity data)
        {
            data.CreatedBy= ApplicationSession.CurrentUser.Id;
            try
            {
                if (ModelState.IsValid)
                {
                    bool result = _departmentservice.UpdateDepartment(data.Id, data);
                    if (result)
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Network Error Try Again ");
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("", "Unable to Save. Try again, and if the problem persists see your system administrator.");// handle for database troble
            }
            return PartialView("_UserEdit", data);
        }

        [HttpGet]
        public ActionResult CreateDepartment()
        {
            return PartialView("_UserCreate");
        }
        [HttpPost]
        public ActionResult CreateDepartment(DepartmentEntity u)
        {
            if (ModelState.IsValid)
            {
                u.CreatedBy= ApplicationSession.CurrentUser.Id;
                //bool uname = _departmentservice.GetHospitalByLocation(u.Latitude, u.Longitude);
                //if (!uname)
                //{
                //    ModelState.AddModelError("", "User Name Already Exist");
                //    return PartialView("_UserCreate", u);
                //}
                //bool email = _hospitalservice.GetUserByEmail(u.Email);
                //if (!email)
                //{
                //    ModelState.AddModelError("", "Email Already Exist");
                //    return PartialView("_UserCreate", u);
                //}
                long result = _departmentservice.CreateDepartment(u);
                if (result > 0)
                {
                    return Json(new { success = true });
                }
                else
                {
                    ModelState.AddModelError("", "Network Error Try Again ");
                }
            }
            return PartialView("_UserCreate", u);
        }

        public JsonResult DeleteDepartment(string id)
        {
            bool result = _departmentservice.DeleteDepartment(Convert.ToInt64(id));
            if (result)
            {
                return Json("Successfully deleted Record !");
            }
            else
            {
                TempData["Delete"] = string.Format("Error Coming While Deleting Data");
                return Json("Error Coming While Deleting Data");
            }
        }
    }
}