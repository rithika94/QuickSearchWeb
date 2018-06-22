using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuickSearchData;
using QuickSearchBusiness;
using QuickSearchBusiness.Services;
using Microsoft.AspNet.Identity;

namespace QuickSearchWeb.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        [Authorize(Roles = "SupportUser")]
        public ActionResult Index()
        {
            SupportService supportservice = new SupportService();
            var userId = User.Identity.GetUserId();
            
            ViewBag.openCount = supportservice.GetLookupIdForStatusCount(userId);
            ViewBag.closeCount = supportservice.GetLookupIdForStatusCountClose(userId);
            //ViewBag.inProgressCount = supportservice.GetLookupIdForStatusInprogress(userId);
            return View();
        }
    }
}