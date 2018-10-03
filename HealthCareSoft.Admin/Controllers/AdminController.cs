using HealthCareSoft.Admin.Models;
using HealthCareSoft.Repository.Interfaces;
using HealthCareSoft.Repository.Models;
using HealthCareSoft.Repository.Services;
using System;
using System.Linq.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using HealthCareSoft.Entity;
using System.Collections.Generic;

namespace HealthCareSoft.Admin.Controllers
{
    [CustomAuthorize]
    public class AdminController : Controller
    {
        private string _profileImagesPath = WebConfigurationManager.AppSettings["ProfileImages"];
        private readonly IUserServices _userservice;
        private readonly IRoleServices _roleservice;
      //  List<DocWorkingShiftEntity> list;
       // DocWorkingShiftEntity _ObjDocWorkingShiftEntity;
        public AdminController()
        {
            _userservice = new UserService();
            _roleservice = new RoleService();
           // list=   new List<DocWorkingShiftEntity>();
           // list = _ObjDocWorkingShiftEntity.GetAllWorkingShift;
        }
        // GET: All Users
        public ActionResult UserList()
        {
            return View();
        }
        // GET: All Doctors
        public ActionResult DoctorList()
        {
            return View();
        }
        // GET: All Patients
        public ActionResult PatientList()
        {
            return View();

        }
        // GET: All SubAdmin
        public ActionResult SubAdminList()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoadUser()
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
            var v = (from a in _userservice.GetAllUsers() select a);  
            if (!string.IsNullOrEmpty(search))
            {
                v = v.Where(a => a.Email.ToLower().StartsWith(search.ToLower()) ||a.Id.ToString().StartsWith(search) || a.FirstName.ToLower().StartsWith(search.ToLower()));
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
        [HttpPost]
        public ActionResult LoadDoctors()
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
            var v = (from a in _userservice.GetAllDoctors() select a);
            if (!string.IsNullOrEmpty(search))
            {
                v = v.Where(a => a.Email.ToLower().StartsWith(search.ToLower()) || a.Id.ToString().StartsWith(search) || a.FirstName.ToLower().StartsWith(search.ToLower()));
            }
            //SORT
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                v = v.OrderBy(sortColumn + " " + sortColumnDir);
            }
            recordsTotal = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult LoadPatient()
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
            var v = (from a in _userservice.GetAllPatients() select a);
            if (!string.IsNullOrEmpty(search))
            {
                v = v.Where(a => a.Email.ToLower().StartsWith(search.ToLower()) || a.Id.ToString().StartsWith(search) || a.FirstName.ToLower().StartsWith(search.ToLower()));
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
        [HttpPost]
        public ActionResult LoadSubAdmin()
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
            var v = (from a in _userservice.GetAllSubAdmin() select a);
            if (!string.IsNullOrEmpty(search))
            {
                v = v.Where(a => a.Email.ToLower().StartsWith(search.ToLower()) || a.Id.ToString().StartsWith(search) || a.FirstName.ToLower().StartsWith(search.ToLower()));
            }
            //SORT
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                v = v.OrderBy(sortColumn + " " + sortColumnDir);
            }
            recordsTotal = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        }



               //CRUD operations for Users
        public ActionResult UserStatus(string id)
        {
            bool result = _userservice.UserStatus(Convert.ToInt32(id));
            if (result)
            {
                return RedirectToAction("UserList", "Admin");
            }
            else
            {
                TempData["Delete"] = string.Format("Error Coming While Deleting Data");
                return RedirectToAction("UserList", "Admin");
            }
        }

        [HttpGet]
        public ActionResult EditUser(string id)
        {
           // var rolelist = new List<SelectListItem>();
            var role = _roleservice.GetAllRole().Select(c=>new SelectListItem
            {
                Text=c.Name,
                Value=c.Id.ToString()
            }).ToList();
            
            //list sto           
            ViewBag.RoleList = role;
            UserEntity data = _userservice.GetUserById(Convert.ToInt32(id));
            return PartialView("_UserEdit",data);
        }
        [HttpPost]
        public ActionResult EditUser(UserEntity data, HttpPostedFileBase file)
        {
            if(file!=null && file.ContentLength<= 3145728)
            {          
            var allowedExtensions = new[] {
            ".Jpg", ".png", ".jpg", "jpeg"
        };
            var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
            var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
            if (allowedExtensions.Contains(ext)) //check what type of extension  
            {
                string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                string myfile = name + "_" + data.FirstName + ext; //appending the name with id  
                                                         // store the file inside ~/project folder(Img)  
                var path = Path.Combine(Server.MapPath("~/Content/ProfileImages"), myfile);
                data.ProfilePicture = _profileImagesPath + myfile;
                data.CreatedBy = ApplicationSession.CurrentUser.Id;
                file.SaveAs(path);
            }
            else
            {
                ModelState.AddModelError("", "Please selet the iamge in .Jpg .png .jpg .jpeg Extensions Only or check your Image Size image size must be less than 3 MB!!!!");
            }
            }
            try
            {
                if (ModelState.IsValid)
                {
                    data.CreatedBy = ApplicationSession.CurrentUser.Id;
                    bool result = _userservice.UpdateUser(data.Id, data);
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

        //[HttpGet]
        public ActionResult CreateUser()
        {
            var role = _roleservice.GetAllRole().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();
            ViewBag.RoleList = role;
            return PartialView("_UserCreate");
        }
        [HttpPost]
        public ActionResult CreateUser(UserEntity u, HttpPostedFileBase file)
        {
            if(file!=null && file.ContentLength <= 3145728)
            {

         
            var allowedExtensions = new[] {
            ".Jpg", ".png", ".jpg", "jpeg"
        };
            var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
            var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
            if (allowedExtensions.Contains(ext)) //check what type of extension  
            {
                string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                string myfile = name + "_" + u.FirstName + ext; //appending the name with id  
                                                         // store the file inside ~/project folder(Img)  
                var path = Path.Combine(Server.MapPath("~/Content/ProfileImages"), myfile);
                u.ProfilePicture = _profileImagesPath + myfile;
                u.CreatedBy = ApplicationSession.CurrentUser.Id;
                file.SaveAs(path);
            }
            else
            {
                ModelState.AddModelError("", "Please selet the iamge in .Jpg .png .jpg .jpeg Extensions Only and image size must be less than 3 MB!!");
            }
            }
            if (ModelState.IsValid)
            {
                u.CreatedBy = ApplicationSession.CurrentUser.Id;
                bool uname = _userservice.GetUserByUserName(u.FirstName);
                if (!uname)
                {
                    ModelState.AddModelError("", "User Name Already Exist");
                    return PartialView("_UserCreate", u);
                }
                bool email = _userservice.GetUserByEmail(u.Email);
                if (!email)
                {
                    ModelState.AddModelError("", "Email Already Exist");
                    return PartialView("_UserCreate", u);
                }
                long result = _userservice.CreateUser(u);
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

        public JsonResult DeleteUser(string id)
        {
            bool result = _userservice.DeleteUser(Convert.ToInt32(id));
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

        public ActionResult AddUsers()
        {
            var role = _roleservice.GetAllRole().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();
            ViewBag.RoleList = role;
            return View();
        }
        [HttpPost]
        public ActionResult AddUsers(UserEntity u, HttpPostedFileBase file)
        {
            if (file != null)
            {
                var allowedExtensions = new[] {
            ".Jpg", ".png", ".jpg", "jpeg"
        };
                var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                if (allowedExtensions.Contains(ext)) //check what type of extension  
                {
                    string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                    string myfile = name + "_" + u.FirstName + ext; //appending the name with id  
                                                                    // store the file inside ~/project folder(Img)  
                    var path = Path.Combine(Server.MapPath("~/Content/ProfileImages"), myfile);
                    u.ProfilePicture = _profileImagesPath + myfile;
                   
                    //u.CreatedBy = ApplicationSession.CurrentUser.Id;
                    file.SaveAs(path);
                }
                else
                {
                    ModelState.AddModelError("", "Please selet the iamge in .Jpg .png .jpg .jpeg Extensions Only!!");
                }
            }
            if (ModelState.IsValid)
            {
                u.CreatedBy = ApplicationSession.CurrentUser.Id;
                bool uname = _userservice.GetUserByUserName(u.FirstName);
                if (!uname)
                {
                    ModelState.AddModelError("", "User Name Already Exist");
                    return View( u);
                }
                bool email = _userservice.GetUserByEmail(u.Email);
                if (!email)
                {
                    ModelState.AddModelError("", "Email Already Exist");
                    return View( u);
                }
                long result = _userservice.CreateUser(u);
                if (result > 0)
                {
                   // return Json(new { success = true });
                   return RedirectToAction("UserList","Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Network Error Try Again ");
                }
            }
            return PartialView(u);
        }

        public ActionResult AddDoctorTimings()
        {
            return PartialView("_DocTimings");
        }
         
        [HttpPost]
        public ActionResult AddDoctorTimings(DocWorkingShiftEntity ObjDocWorkingShiftEntity)
        {
            List<DocWorkingShiftEntity> listValues = new List<DocWorkingShiftEntity>();
            
            
            for (var i = 1; i < 8; i++)
            {
                string rnd = (Request.Form["Weakdayname" + i] == null) ? "" : Request.Form["Weakdayname" + i].ToString();
                string sttime = (Request.Form["StartTime" + i] == null) ? "" : Request.Form["StartTime" + i].ToString();
                string ftime = (Request.Form["EndTime" + i] == null) ? "" : Request.Form["EndTime" + i].ToString();

                DocWorkingShiftEntity obj = new DocWorkingShiftEntity();
                obj.StartTime = sttime;
                obj.EndTime = ftime;
                obj.Weakdayname = rnd;
                listValues.Add(obj);
            }
            return PartialView("_DocTimings");
        }

    }
}