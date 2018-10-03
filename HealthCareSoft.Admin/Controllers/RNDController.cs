using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthCareSoft.Admin.Controllers
{
    [CustomAuthorize]
    public class RNDController : Controller
    {
        // GET: RND
        public ActionResult Index()
        {
            return View();
        }
    }
}