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
using HealthCareSoft.Entity.UnitOfWork;

namespace HealthCareSoft.Admin.Controllers
{
   [CustomAuthorize]
    public class HospitalController : Controller
    {
        private UnitOfWork _unitOfWork;
        private readonly IHospitalServices _hospitalservice;
        private readonly IHospitalDoctorMapServices _hdocMapService;
        private readonly IUserServices _userService;
        private readonly IDepartmentServices _departmentService;
        private readonly IHospitalDeptMapServices _hospitalDeptmapService;
        public HospitalController()
        {
            _unitOfWork = new UnitOfWork();
            _hospitalservice = new HospitalService();
            _hdocMapService = new HospitalDoctormap();
            _userService = new UserService();
            _departmentService = new DepartmentService();
            _hospitalDeptmapService = new HospitalDeptmap();
        }
        // GET: Hospital
        public ActionResult HospitalList()
        {
            return View();
        }
        public ActionResult HospitalDeptList()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoadHospital()
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
            var v = (from a in _hospitalservice.GetAllHospital() select a);
            if (!string.IsNullOrEmpty(search))
            {
                v = v.Where(a => a.HospitalName.ToLower().StartsWith(search.ToLower()) || a.HospitalName.ToLower().StartsWith(search.ToLower()));
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
        public ActionResult LoadHospitalDepartment()
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
            var v = (from a in _hospitalDeptmapService.GetAllHospitalDeptmapping() select a);
            if (!string.IsNullOrEmpty(search))
            {
                v = v.Where(a => a.HospitalName.ToLower().StartsWith(search.ToLower()) || a.HospitalName.ToLower().StartsWith(search.ToLower()));
            }
            // SORT
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    v = v.OrderBy(sortColumn + " " + sortColumnDir);
            //}
            recordsTotal = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);

        }
        //CRUD operations for Users
        public ActionResult HospitalStatus(string id)
        {
            bool result = _hospitalservice.HospitalStatus(Convert.ToInt32(id));
            if (result)
            {
                return RedirectToAction("HospitalList", "Hospital");
            }
            else
            {
                TempData["Delete"] = string.Format("Error Coming While Deleting Data");
                return RedirectToAction("HospitalList", "Hospital");
            }
        }

        [HttpGet]
        public ActionResult EditHospital(string id)
        {
            HospitalEntity data = _hospitalservice.GetHospitalById(Convert.ToInt64(id));
            return PartialView("_UserEdit", data);
        }
        [HttpPost]
        public ActionResult EditHospital(HospitalEntity data)
        {
            data.CreatedBy= ApplicationSession.CurrentUser.Id; 
            try
            {
                if (ModelState.IsValid)
                {
                    bool result = _hospitalservice.UpdateHospital(data.Id, data);
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
        public ActionResult CreateHospital()
        {
            return PartialView("_UserCreate");
        }
        [HttpPost]
        public ActionResult CreateHospital(HospitalEntity u)
        {
            u.CreatedBy= ApplicationSession.CurrentUser.Id; 
            if (ModelState.IsValid)
            {
                bool uname = _hospitalservice.GetHospitalByLocation(u.Latitude,u.Longitude);
                if (!uname)
                {
                    ModelState.AddModelError("", "User Name Already Exist");
                    return PartialView("_UserCreate", u);
                }
                //bool email = _hospitalservice.GetUserByEmail(u.Email);
                //if (!email)
                //{
                //    ModelState.AddModelError("", "Email Already Exist");
                //    return PartialView("_UserCreate", u);
                //}
                long result = _hospitalservice.CreateHospital(u);
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

        public JsonResult DeleteHospital(string id)
        {
            bool result = _hospitalservice.DeleteHospital(Convert.ToInt32(id));
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

        [HttpGet]
        public ActionResult AddDoctortoHospital(string id)
        {
            var doctor = _userService.GetAllDoctors().Select(D => new 
            {
                DocName = D.FirstName+D.LastName,
                DocId = D.Id.ToString()
            }).ToList();
            ViewBag.DoctorList = new MultiSelectList(doctor, "DocId", "DocName");
            var dept = _hospitalDeptmapService.GetAllDeptByHospitalId(Convert.ToInt32(id)).Select(DE => new SelectListItem
            {
                Text = DE.DeptName,
                Value = DE.DeptId.ToString()
            }).ToList();
            ViewBag.DeptList = dept;
            return PartialView("_AddDoctor");
        }
        public ActionResult AddDoctortoHospital(long[] DocId, HospitalDoctorEntity ObjHDEntity)
        {
            ObjHDEntity.HospitalId = ObjHDEntity.Id;
            var dept = false;
            var ExistngHospitalList = _unitOfWork.HospitalDoctorMappingRepository.GetDocListByHospitalID(ObjHDEntity.HospitalId);
            if (ExistngHospitalList != null)
            {
                _unitOfWork.HospitalDoctorMappingRepository.DeleteHospitalDoctorMapping(ExistngHospitalList);
            }
            for (int i = 0; i < DocId.Count(); i++)
            {
                dept = _hdocMapService.CreateHDocMapping(DocId[i], ObjHDEntity);
            }
            if (dept == true)
            {
                ModelState.AddModelError("", "Dept Assigned Sucessfully!");
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong please try Again!!");
            }
            return View();
        }

        [HttpGet]
        public ActionResult AddDepttoHospital(string id)
          {          
            var dept = _departmentService.GetAllDepartment().Select(DE => new 
            {
                DepName = DE.Name,
                DepId = DE.Id.ToString()
               // Selected = true
            }).ToList();
            ViewBag.DeptList = new MultiSelectList(dept, "DepId", "DepName");
            return PartialView("_AddDept");
        }
        [HttpPost]
        public ActionResult AddDepttoHospital(long[] DepId, HospitalDeptEntity ObjHDeptEntity)
        {
            ObjHDeptEntity.HospitalId = ObjHDeptEntity.Id;
            var dept=false;
            var ExistngHospitalList = _unitOfWork.HospitalDepartmentMappingRepository.GetByHospitalID(ObjHDeptEntity.HospitalId);
            if (ExistngHospitalList != null)
            {
                _unitOfWork.HospitalDepartmentMappingRepository.DeleteHospitalDepartmentMapping(ExistngHospitalList);
            }
            for (int i=0; i<DepId.Count();i++)
            {
                 dept = _hospitalDeptmapService.CreateHDeptMapping(DepId[i], ObjHDeptEntity);
            }
            if (dept==true)
            {
                ModelState.AddModelError("", "Dept Assigned Sucessfully!");
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong please try Again!!");
            }
            return PartialView("_AddDept");
        }
    }
}