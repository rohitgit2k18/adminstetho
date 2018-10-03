using HealthCareSoft.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace HealthCareSoft.Admin.Controllers
{
    [CustomAuthorize]
    public class StaticInformationController : Controller
    {
        // GET: StaticInformation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetEditor()
        {
            StaticInformationEntity Obj = new StaticInformationEntity();
            return View();
        }
        [HttpPost]
        public ActionResult GetEditor(StaticInformationEntity Obj)
        {
           
            return View(Obj);
        }
    }
}