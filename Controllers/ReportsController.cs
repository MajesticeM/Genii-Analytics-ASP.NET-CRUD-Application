using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Genii_Assessment.Controllers
{
    /// Provides management reporting features.
    /// Only managers may access this controller.
    [Authorize(Roles = "Manager")]
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
    }
}