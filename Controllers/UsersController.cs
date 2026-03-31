using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Genii_Assessment.Models;

namespace Genii_Assessment.Controllers
{
    [Authorize(Roles = ApplicationRoles.Admin)]
    public class UsersController : Controller
    {
        /// Displays the create user form.
        public ActionResult Create()
        {
            return View();
        }

        // GET: Users
        public ActionResult Index()
        {
            return View();
        }
    }
}