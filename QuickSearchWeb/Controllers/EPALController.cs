using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuickSearchWeb.Controllers
{
    [Authorize(Roles = "EPALUser")]
    public class EPALController : Controller
    {
        // GET: EPAL
        public ActionResult Index()
        {
            return View();
        }
    }
    
}