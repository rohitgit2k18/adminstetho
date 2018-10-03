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
    public class EnqurieController : Controller
    {
        private readonly IEnquiryServices _enquiryservice;
        public EnqurieController()
        {
            _enquiryservice = new EnquiryService();
        }
        // GET: Enqurie
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EnquiryList()
        {
            return View();
        }
        public ActionResult LoadEnquiry()
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
            var v = (from a in _enquiryservice.GetAllEnquiry() select a);
            if (!string.IsNullOrEmpty(search))
            {
                v = v.Where(a => a.Title.ToLower().StartsWith(search.ToLower()) || a.Title.ToLower().StartsWith(search.ToLower()));
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

        public ActionResult EnquiryStatus(string id)
        {
            bool result = _enquiryservice.EnquiryStatus(Convert.ToInt32(id));
            if (result)
            {
                return RedirectToAction("EnquiryList", "Enqurie");
            }
            else
            {
                TempData["Delete"] = string.Format("Error Coming While Deleting Data");
                return RedirectToAction("EnquiryList", "Enqurie");
            }
        }

        public ActionResult ViewEnqDetails(string id )
        {
            EnquiryEntity data = _enquiryservice.GetEnquiryById(Convert.ToInt32(id));
            return PartialView("_UserDetails", data);
        }
    }
}