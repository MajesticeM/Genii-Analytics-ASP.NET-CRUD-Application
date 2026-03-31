using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Genii_Assessment.Models;

namespace Genii_Assessment.Controllers
{
    /// Handles product management.
    /// Users with Admin, User, or Manager roles may access this controller.
    [Authorize(Roles = ApplicationRoles.Admin + "," + ApplicationRoles.User + "," + ApplicationRoles.Manager)]
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index()
        {
            return View();
        }
    }
}