using AutoMapper;
using Microsoft.AspNet.Identity;
using QuickSearchBusiness.Services;
using QuickSearchData;
using QuickSearchWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace QuickSearchWeb.Controllers
{
    [Authorize]
    public class TimeSheetController : Controller
    {

        // GET: TimeSheet
        [Authorize(Roles = "TimeSheetUser")]
        public ActionResult MyTimeSheetsList()
        {

            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "TimeSheetAdmin,TimeSheetReportingManager,TimeSheetManager,TimeSheetUser")]
        public ActionResult GetMyTimeSheetListUserId(DateTime FromDate, DateTime Todate)
        {
            TimeSheetService timeSheetService = new TimeSheetService();

            var timeSheets = timeSheetService.GetTimeSheetsBetweenDatesAndUserId(FromDate,Todate,User.Identity.GetUserId());

            var timeSheetList = Mapper.Map<List<TimeSheetsMaster>, List<TimeSheetListViewModel>>(timeSheets);
            
            return PartialView("_MyTimeSheetsListView", timeSheetList);
        }


        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult EmployeeAdminTimeSheetsList()
        {

            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }
            ViewBag.TSRole = "TimeSheetAdmin";
            ViewBag.RoleName = "(Admin)";
            
            return View("EmployeeTimeSheetsList");
        }
        
        [Authorize(Roles = "TimeSheetReportingManager")]
        public ActionResult EmployeeRepoManTimeSheetsList()
        {

            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }
            ViewBag.TSRole = "TimeSheetReportingManager";
            ViewBag.RoleName = "(Approver)";

            return View("EmployeeTimeSheetsList");
        }

        [Authorize(Roles = "TimeSheetManager")]
        public ActionResult EmployeeManagerTimeSheetsList()
        {

            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }
            ViewBag.TSRole = "TimeSheetManager";
            ViewBag.RoleName = "(Viewer)";

            return View("EmployeeTimeSheetsList");
        }


        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult GetEmployeeAdminPendingTimeSheetList()
        {
            TimeSheetService timeSheetService = new TimeSheetService();

            List<TimeSheetsMaster> timeSheets = new List<TimeSheetsMaster>();
            
                timeSheets = timeSheetService.GetEmployeePendingTimeSheets();
            

            var timeSheetList = Mapper.Map<List<TimeSheetsMaster>, List<TimeSheetListViewModel>>(timeSheets);
            ViewBag.TSRole = "TimeSheetAdmin";
            return PartialView("_EmployeePendingTimeSheetsList", timeSheetList);
        }

        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult GetEmployeeAdminRemainingTimeSheetList(DateTime FromDate, DateTime Todate)
        {
            TimeSheetService timeSheetService = new TimeSheetService();

            List<TimeSheetsMaster> timeSheets = new List<TimeSheetsMaster>();

           timeSheets = timeSheetService.GetEmployeeRemianingTimeSheets(FromDate, Todate);
            
            var timeSheetList = Mapper.Map<List<TimeSheetsMaster>, List<TimeSheetListViewModel>>(timeSheets);
            ViewBag.TSRole = "TimeSheetAdmin";
            return PartialView("_EmployeeRemainingTimeSheetsList", timeSheetList);
        }


        [Authorize(Roles = "TimeSheetReportingManager")]
        public ActionResult GetEmployeeRepoManPendingTimeSheetList()
        {
            TimeSheetService timeSheetService = new TimeSheetService();

            List<TimeSheetsMaster> timeSheets = new List<TimeSheetsMaster>();
           
                timeSheets = timeSheetService.GetReportingManagerPendingTimeSheets(User.Identity.GetUserId());
            
            var timeSheetList = Mapper.Map<List<TimeSheetsMaster>, List<TimeSheetListViewModel>>(timeSheets);
            ViewBag.TSRole = "TimeSheetReportingManager";
            return PartialView("_EmployeePendingTimeSheetsList", timeSheetList);
        }

        [Authorize(Roles = "TimeSheetReportingManager")]
        public ActionResult GetEmployeeRepoManRemainingTimeSheetList(DateTime FromDate, DateTime Todate)
        {
            TimeSheetService timeSheetService = new TimeSheetService();

            List<TimeSheetsMaster> timeSheets = new List<TimeSheetsMaster>();

            
                timeSheets = timeSheetService.GetReportingManagerRemianingTimeSheets(FromDate, Todate, User.Identity.GetUserId());
            

            var timeSheetList = Mapper.Map<List<TimeSheetsMaster>, List<TimeSheetListViewModel>>(timeSheets);
            ViewBag.TSRole = "TimeSheetReportingManager";
            return PartialView("_EmployeeRemainingTimeSheetsList", timeSheetList);
        }

        
        [Authorize(Roles = "TimeSheetManager")]
        public ActionResult GetEmployeeManagerPendingTimeSheetList()
        {
            TimeSheetService timeSheetService = new TimeSheetService();

            List<TimeSheetsMaster> timeSheets = new List<TimeSheetsMaster>();
           
                timeSheets = timeSheetService.GetEmployeePendingTimeSheets();
           

            var timeSheetList = Mapper.Map<List<TimeSheetsMaster>, List<TimeSheetListViewModel>>(timeSheets);
            ViewBag.TSRole = "TimeSheetManager";
            return PartialView("_EmployeePendingTimeSheetsList", timeSheetList);
        }

        [Authorize(Roles = "TimeSheetManager")]
        public ActionResult GetEmployeeManagerRemainingTimeSheetList(DateTime FromDate, DateTime Todate)
        {
            TimeSheetService timeSheetService = new TimeSheetService();

            List<TimeSheetsMaster> timeSheets = new List<TimeSheetsMaster>();

           
                timeSheets = timeSheetService.GetEmployeeRemianingTimeSheets(FromDate, Todate);
            
            var timeSheetList = Mapper.Map<List<TimeSheetsMaster>, List<TimeSheetListViewModel>>(timeSheets);
            ViewBag.TSRole = "TimeSheetManager";
            return PartialView("_EmployeeRemainingTimeSheetsList", timeSheetList);
        }
        


        [Authorize(Roles = "TimeSheetUser")]
        public ActionResult MyTimeSheetCreate()
        {

            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            ViewBag.TSStartDate = WebConfigurationManager.AppSettings["TimeSheetStartDay"];
            

            string Employeename = HttpContext.User.Identity.Name;
            TimeSheetViewModel tvm = new TimeSheetViewModel();
            UserService userService = new UserService();
            tvm.UserID = User.Identity.GetUserId();
            tvm.AspNetUser = userService.GetUserWithId(tvm.UserID);
            if (tvm.AspNetUser.AspNetUser1 != null)
            {
                tvm.ReportingManager = tvm.AspNetUser.AspNetUser1.Firstname + " " + tvm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                tvm.ReportingManager = "None";
            }
            tvm.FullName = User.Identity.Name;
            tvm.AbsenceTypeList = GetDropDownList("Absence", null); 
            var ddlValue = GetDDLID("None", tvm.AbsenceTypeList);
            tvm.LookupAbsenceTypeDay1 = ddlValue;
            tvm.LookupAbsenceTypeDay2 = ddlValue;
            tvm.LookupAbsenceTypeDay3 = ddlValue;
            tvm.LookupAbsenceTypeDay4 = ddlValue;
            tvm.LookupAbsenceTypeDay5 = ddlValue;
            tvm.LookupAbsenceTypeDay6 = ddlValue;
            tvm.LookupAbsenceTypeDay7 = ddlValue;
            int dayOfWeek = (int)DateTime.Now.DayOfWeek;
            tvm.StartDate = GetTSStartingDate();
            tvm.EndDate = tvm.StartDate.AddDays(6);
            tvm.TimeSheetSummaryList = new List<TimeSheetSummaryViewModel>();
            for (int i = 0; i<5; i++)
            {
                TimeSheetSummaryViewModel tsvm = new TimeSheetSummaryViewModel();
                tvm.TimeSheetSummaryList.Add(tsvm);
            }
            
            tvm.TSStartDay = WebConfigurationManager.AppSettings["TimeSheetStartDay"];
            return View(tvm);

        }

        private DateTime GetTSStartingDate()
        {
            DateTime now = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Now.StartOfWeek(DayOfWeek.Monday).Day);
            
            int TSStartDay = 0;
            Int32.TryParse(WebConfigurationManager.AppSettings["TimeSheetStartDay"], out TSStartDay);
            now = now.AddDays(-7);
            while ((int)now.DayOfWeek != TSStartDay){
                now = now.AddDays(-1);
            }


            return now;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TimeSheetUser")]
        public async Task<ActionResult> MyTimeSheetCreate([Bind(Exclude = "LookupApprovedStatus, Comments")]TimeSheetViewModel  tsvm, string action)
        {

            try
            {
                ViewBag.TSStartDate = WebConfigurationManager.AppSettings["TimeSheetStartDay"];

                TimeSheetService timeSheetService = new TimeSheetService();
                
                if (!ModelState.IsValid)
                {
                    tsvm.AbsenceTypeList = GetDropDownList("Absence", null);
                    return View(tsvm);
                }
                var lastTS = timeSheetService.CheckTSStartDateExists(tsvm.UserID, tsvm.StartDate, tsvm.EndDate);
                if (lastTS != null)
                {
                    ModelState.AddModelError("error", $"*A time sheet with Id {lastTS.TimeSheetCode} with this date range already exists");
                    tsvm.AbsenceTypeList = GetDropDownList("Absence", null);
                    return View(tsvm);
                }
                
                
                else if (tsvm.TotalHours <= 0 || tsvm.TotalHours == null)
                {
                    tsvm.AbsenceTypeList = GetDropDownList("Absence", null);
                    var ddlValue = GetDDLID("None", tsvm.AbsenceTypeList);
                    if ((tsvm.Day1RegularHours <= 0 || tsvm.Day1RegularHours == null) && tsvm.LookupAbsenceTypeDay1 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 1 ");
                        return View(tsvm);
                    }
                    else if ((tsvm.Day2RegularHours <= 0 || tsvm.Day2RegularHours == null) && tsvm.LookupAbsenceTypeDay2 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 2 ");
                        return View(tsvm);
                    }
                    else if ((tsvm.Day3RegularHours <= 0 || tsvm.Day3RegularHours == null) && tsvm.LookupAbsenceTypeDay3 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 3 ");
                        return View(tsvm);
                    }
                    else if ((tsvm.Day4RegularHours <= 0 || tsvm.Day4RegularHours == null) && tsvm.LookupAbsenceTypeDay4 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 4 ");
                        return View(tsvm);
                    }
                    else if ((tsvm.Day5RegularHours <= 0 || tsvm.Day5RegularHours == null) && tsvm.LookupAbsenceTypeDay5 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 5 ");
                        return View(tsvm);
                    }
                    else if ((tsvm.Day6RegularHours <= 0 || tsvm.Day6RegularHours == null) && tsvm.LookupAbsenceTypeDay6 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 6 ");
                        return View(tsvm);
                    }
                    else if ((tsvm.Day7RegularHours <= 0 || tsvm.Day7RegularHours == null) && tsvm.LookupAbsenceTypeDay7 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 7 ");
                        return View(tsvm);
                    }
                }
                

                TimeSheetsMaster timesheet = new TimeSheetsMaster();
                


                timesheet.UserID = tsvm.UserID;
                timesheet.TimeSheetCode = tsvm.TimeSheetCode;
                timesheet.StartDate = tsvm.StartDate;
                timesheet.EndDate = tsvm.EndDate;
                timesheet.RegularHours = tsvm.RegularHours ?? 0;
                timesheet.OverTimeHours = tsvm.OverTimeHours ?? 0;
                timesheet.TotalHours = tsvm.TotalHours ?? 0;
                timesheet.Day1OverHours = tsvm.Day1OverHours ?? 0;
                timesheet.Day2OverHours = tsvm.Day2OverHours ?? 0;
                timesheet.Day3OverHours = tsvm.Day3OverHours ?? 0;
                timesheet.Day4OverHours = tsvm.Day4OverHours ?? 0;
                timesheet.Day5OverHours = tsvm.Day5OverHours ?? 0;
                timesheet.Day6OverHours = tsvm.Day6OverHours ?? 0;
                timesheet.Day7OverHours = tsvm.Day7OverHours ?? 0;
                timesheet.Day1RegularHours = tsvm.Day1RegularHours ?? 0;
                timesheet.Day2RegularHours = tsvm.Day2RegularHours ?? 0;
                timesheet.Day3RegularHours = tsvm.Day3RegularHours ?? 0;
                timesheet.Day4RegularHours = tsvm.Day4RegularHours ?? 0;
                timesheet.Day5RegularHours = tsvm.Day5RegularHours ?? 0;
                timesheet.Day6RegularHours = tsvm.Day6RegularHours ?? 0;
                timesheet.Day7RegularHours = tsvm.Day7RegularHours ?? 0;
                timesheet.LookupAbsenceTypeDay1 = tsvm.LookupAbsenceTypeDay1;
                timesheet.LookupAbsenceTypeDay2 = tsvm.LookupAbsenceTypeDay2;
                timesheet.LookupAbsenceTypeDay3 = tsvm.LookupAbsenceTypeDay3 ;
                timesheet.LookupAbsenceTypeDay4 = tsvm.LookupAbsenceTypeDay4;
                timesheet.LookupAbsenceTypeDay5 = tsvm.LookupAbsenceTypeDay5;
                timesheet.LookupAbsenceTypeDay6 = tsvm.LookupAbsenceTypeDay6;
                timesheet.LookupAbsenceTypeDay7 = tsvm.LookupAbsenceTypeDay7;
                timesheet.AbsenceHoursDay1 = tsvm.AbsenceHoursDay1;
                timesheet.AbsenceHoursDay2 = tsvm.AbsenceHoursDay2;
                timesheet.AbsenceHoursDay3 = tsvm.AbsenceHoursDay3;
                timesheet.AbsenceHoursDay4 = tsvm.AbsenceHoursDay4;
                timesheet.AbsenceHoursDay5 = tsvm.AbsenceHoursDay5;
                timesheet.AbsenceHoursDay6 = tsvm.AbsenceHoursDay6;
                timesheet.AbsenceHoursDay7 = tsvm.AbsenceHoursDay7;
                timesheet.IsActive = true;
                timesheet.IsDeleted = false;
                timesheet.CreatedUserId = User.Identity.GetUserId();
                timesheet.CreatedDate = DateTime.Now;
                timesheet.ModifiedUserId = User.Identity.GetUserId();
                timesheet.ModifiedDate = DateTime.Now;
                timesheet.LookupApprovedStatus = timeSheetService.GetLookupIdForStatus("Saved", "TimeSheetStatus").LookupCodeId;

                UserService userService = new UserService();
                var user = userService.GetUserWithId(tsvm.UserID);
                var repoMan = user.AspNetUser1;

                List<TimeSheetSummary> timeSheetSummaries = new List<TimeSheetSummary>();

                timeSheetService.CreateTimeSheet(timesheet);

                timeSheetSummaries = Mapper.Map<List<TimeSheetSummaryViewModel>, List<TimeSheetSummary>>(tsvm.TimeSheetSummaryList);
                foreach (TimeSheetSummary sum in timeSheetSummaries)
                {
                    sum.TimeSheetID = timesheet.TimeSheetID;
                    
                }
                timeSheetService.CreateTimeSheetSummary(timeSheetSummaries);

                TempData["status"] = "Timesheet successfully saved";

                if (string.Equals(action.ToString(), "Submit"))
                {
                    if (repoMan != null)
                    {

                        ChangeStatusOfTimeSheet(timesheet.TimeSheetID, "Pending");
                        await SendEmailTimeSheetSubmitted(timesheet.TimeSheetID);
                        TempData["status"] = "Timesheet successfully saved and submitted";

                    }

                    else
                    {
                        TempData["status"] = "Your Time sheet is not submitted. You donot have a reporting manager. Please contact your admin.";

                    }
                }
                
                return RedirectToAction("MyTimeSheetsList");


            }


            catch (Exception ex)
            {
                tsvm.AbsenceTypeList = GetDropDownList("Absence", null);
                TempData["error"] = "Something went wrong while adding timesheet";
                return RedirectToAction("MyTimeSheetsList");
            }

        }

        [Authorize(Roles = "TimeSheetUser")]
        public ActionResult MyTimesheetEdit(int id)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            ViewBag.TSStartDate = WebConfigurationManager.AppSettings["TimeSheetStartDay"];

            
            TimeSheetViewModel tvm = new TimeSheetViewModel();            
            TimeSheetService timeSheetService = new TimeSheetService();
            var timeSheet = timeSheetService.GetMyTimeSheetMaster(id,User.Identity.GetUserId());
            if(timeSheet == null)
            {
                TempData["error"] = "Either this file does not exist or you dont have permission to access this";
                return RedirectToAction("MyTimeSheetsList");
            }

            var timesheetSummaries = timeSheetService.GetTimeSheetSummary(timeSheet.TimeSheetID);
            tvm.TimeSheetSummaryList = new List<TimeSheetSummaryViewModel>();
            

            tvm = Mapper.Map<TimeSheetsMaster,TimeSheetViewModel>(timeSheet);
            tvm.TimeSheetSummaryList = Mapper.Map<List<TimeSheetSummary>, List<TimeSheetSummaryViewModel>>(timesheetSummaries);
            tvm.AbsenceTypeList = GetDropDownList("Absence", null);
            if (tvm.AspNetUser.AspNetUser1 != null)
            {
                tvm.ReportingManager = tvm.AspNetUser.AspNetUser1.Firstname + " " + tvm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                tvm.ReportingManager = "None";
            }
            

            return View(tvm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TimeSheetUser")]
        public async Task<ActionResult> MyTimesheetEdit([Bind(Exclude = "LookupApprovedStatus,Comments,TimeSheetCode")]TimeSheetViewModel tsvm , string action)
        {
            try
            {
                ViewBag.TSStartDate = WebConfigurationManager.AppSettings["TimeSheetStartDay"];

                TimeSheetService timeSheetService = new TimeSheetService();

                if (!ModelState.IsValid)
                {
                    tsvm.AbsenceTypeList = GetDropDownList("Absence", null);
                    return View(tsvm);
                }
                var lastTS = timeSheetService.CheckTSStartDateExists(tsvm.UserID, tsvm.StartDate, tsvm.EndDate, tsvm.TimeSheetID);
                if (lastTS != null)
                {
                    ModelState.AddModelError("error", $"*A time sheet with Id {lastTS.TimeSheetCode} with this date range already exists");
                    tsvm.AbsenceTypeList = GetDropDownList("Absence", null);
                    return View(tsvm);
                }
                else if (tsvm.TotalHours <= 0 || tsvm.TotalHours == null)
                {
                    tsvm.AbsenceTypeList = GetDropDownList("Absence", null);
                    var ddlValue = GetDDLID("None", tsvm.AbsenceTypeList);
                    if ((tsvm.Day1RegularHours <= 0 || tsvm.Day1RegularHours == null) && tsvm.LookupAbsenceTypeDay1 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 1 ");
                        return View(tsvm);
                    }
                    else if ((tsvm.Day2RegularHours <= 0 || tsvm.Day2RegularHours == null) && tsvm.LookupAbsenceTypeDay2 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 2 ");
                        return View(tsvm);
                    }
                    else if ((tsvm.Day3RegularHours <= 0 || tsvm.Day3RegularHours == null) && tsvm.LookupAbsenceTypeDay3 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 3 ");
                        return View(tsvm);
                    }
                    else if ((tsvm.Day4RegularHours <= 0 || tsvm.Day4RegularHours == null) && tsvm.LookupAbsenceTypeDay4 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 4 ");
                        return View(tsvm);
                    }
                    else if ((tsvm.Day5RegularHours <= 0 || tsvm.Day5RegularHours == null) && tsvm.LookupAbsenceTypeDay5 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 5 ");
                        return View(tsvm);
                    }
                    else if ((tsvm.Day6RegularHours <= 0 || tsvm.Day6RegularHours == null) && tsvm.LookupAbsenceTypeDay6 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 6 ");
                        return View(tsvm);
                    }
                    else if ((tsvm.Day7RegularHours <= 0 || tsvm.Day7RegularHours == null) && tsvm.LookupAbsenceTypeDay7 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 7 ");
                        return View(tsvm);
                    }
                }
                TimeSheetsMaster timeSheetsMaster = timeSheetService.GetMyTimeSheetMaster(tsvm.TimeSheetID,User.Identity.GetUserId());
                if (timeSheetsMaster == null)
                {
                    TempData["error"] = "Either this file does not exist or you dont have permission to access this";
                    return RedirectToAction("MyTimeSheetsList");
                }
                List<TimeSheetSummary> summaries = timeSheetService.GetTimeSheetSummary(tsvm.TimeSheetID);

                timeSheetsMaster.StartDate = tsvm.StartDate;
                timeSheetsMaster.EndDate = tsvm.EndDate;
                timeSheetsMaster.RegularHours = tsvm.RegularHours??0;
                timeSheetsMaster.OverTimeHours = tsvm.OverTimeHours??0;
                timeSheetsMaster.TotalHours = tsvm.TotalHours ?? 0;
                timeSheetsMaster.Day1RegularHours = tsvm.Day1RegularHours ?? 0;
                timeSheetsMaster.Day2RegularHours = tsvm.Day2RegularHours ?? 0;
                timeSheetsMaster.Day3RegularHours = tsvm.Day3RegularHours ?? 0;
                timeSheetsMaster.Day4RegularHours = tsvm.Day4RegularHours ?? 0;
                timeSheetsMaster.Day5RegularHours = tsvm.Day5RegularHours ?? 0;
                timeSheetsMaster.Day6RegularHours = tsvm.Day6RegularHours ?? 0;
                timeSheetsMaster.Day7RegularHours = tsvm.Day7RegularHours ?? 0;
                timeSheetsMaster.Day1OverHours = tsvm.Day1OverHours ?? 0;
                timeSheetsMaster.Day2OverHours = tsvm.Day2OverHours ?? 0;
                timeSheetsMaster.Day3OverHours = tsvm.Day3OverHours ?? 0;
                timeSheetsMaster.Day4OverHours = tsvm.Day4OverHours ?? 0;
                timeSheetsMaster.Day5OverHours = tsvm.Day5OverHours ?? 0;
                timeSheetsMaster.Day6OverHours = tsvm.Day6OverHours ?? 0;
                timeSheetsMaster.Day7OverHours = tsvm.Day7OverHours ?? 0;
                timeSheetsMaster.LookupAbsenceTypeDay1 = tsvm.LookupAbsenceTypeDay1;
                timeSheetsMaster.LookupAbsenceTypeDay2 = tsvm.LookupAbsenceTypeDay2;
                timeSheetsMaster.LookupAbsenceTypeDay3 = tsvm.LookupAbsenceTypeDay3;
                timeSheetsMaster.LookupAbsenceTypeDay4 = tsvm.LookupAbsenceTypeDay4;
                timeSheetsMaster.LookupAbsenceTypeDay5 = tsvm.LookupAbsenceTypeDay5;
                timeSheetsMaster.LookupAbsenceTypeDay6 = tsvm.LookupAbsenceTypeDay6;
                timeSheetsMaster.LookupAbsenceTypeDay7 = tsvm.LookupAbsenceTypeDay7;
                timeSheetsMaster.LookupCodeMaster  = null;
                timeSheetsMaster.LookupCodeMaster1 = null;
                timeSheetsMaster.LookupCodeMaster2 = null;
                timeSheetsMaster.LookupCodeMaster3 = null;
                timeSheetsMaster.LookupCodeMaster4 = null;
                timeSheetsMaster.LookupCodeMaster5 = null;
                timeSheetsMaster.LookupCodeMaster6 = null;

                timeSheetsMaster.AbsenceHoursDay1 = tsvm.AbsenceHoursDay1;
                timeSheetsMaster.AbsenceHoursDay2 = tsvm.AbsenceHoursDay2;
                timeSheetsMaster.AbsenceHoursDay3 = tsvm.AbsenceHoursDay3;
                timeSheetsMaster.AbsenceHoursDay4 = tsvm.AbsenceHoursDay4;
                timeSheetsMaster.AbsenceHoursDay5 = tsvm.AbsenceHoursDay5;
                timeSheetsMaster.AbsenceHoursDay6 = tsvm.AbsenceHoursDay6;
                timeSheetsMaster.AbsenceHoursDay7 = tsvm.AbsenceHoursDay7;

                timeSheetsMaster.ModifiedUserId = User.Identity.GetUserId();
                timeSheetsMaster.ModifiedDate = DateTime.Now;
                
                summaries = Mapper.Map<List<TimeSheetSummaryViewModel>, List<TimeSheetSummary>>(tsvm.TimeSheetSummaryList);

                timeSheetService.UpdateTimeSheet(timeSheetsMaster);
                timeSheetService.UpdateTimeSheetSummaries(summaries);
                TempData["status"] = "Timesheet successfully updated";



                if (string.Equals(action.ToString(), "Submit"))
                {                    
                    


                    if (timeSheetsMaster.AspNetUser.AspNetUser1 != null)
                    {

                        ChangeStatusOfTimeSheet(tsvm.TimeSheetID, "Pending");
                        await SendEmailTimeSheetSubmitted(timeSheetsMaster.TimeSheetID);
                        TempData["status"] = "Timesheet successfully saved and submitted";

                    }

                    else
                    {
                        TempData["status"] = "Your Time sheet is not submitted. You donot have a reporting manager. Please contact your admin.";

                    }
                }
                

                return RedirectToAction("MyTimeSheetsList");
            }
            catch(Exception ex)
            {
                TempData["error"] = "Something went wrong while editing timesheet";
                tsvm.AbsenceTypeList = GetDropDownList("Absence", null);
                return RedirectToAction("MyTimeSheetsList");
            }
        }


        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult EmployeeAdminTimeSheetEdit(int id)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            ViewBag.TSRole = "TimeSheetAdmin";

            ViewBag.TSStartDate = WebConfigurationManager.AppSettings["TimeSheetStartDay"];

            
            TimeSheetViewModel tvm = new TimeSheetViewModel();
            TimeSheetService timeSheetService = new TimeSheetService();
            var timeSheet = timeSheetService.GetTimeSheetMaster(id);
            var timesheetSummaries = timeSheetService.GetTimeSheetSummary(timeSheet.TimeSheetID);
            tvm.TimeSheetSummaryList = new List<TimeSheetSummaryViewModel>();
            

            tvm = Mapper.Map<TimeSheetsMaster, TimeSheetViewModel>(timeSheet);
            tvm.TimeSheetSummaryList = Mapper.Map<List<TimeSheetSummary>, List<TimeSheetSummaryViewModel>>(timesheetSummaries);
            tvm.AbsenceTypeList = GetDropDownList("Absence", null);
            if (tvm.AspNetUser.AspNetUser1 != null)
            {
                tvm.ReportingManager = tvm.AspNetUser.AspNetUser1.Firstname + " " + tvm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                tvm.ReportingManager = "None";
            }
            
            

            return View("EmployeeTimeSheetEdit",tvm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TimeSheetAdmin")]
        public async Task<ActionResult> EmployeeAdminTimeSheetEdit([Bind(Exclude = "LookupApprovedStatus,TimeSheetCode")]TimeSheetViewModel tsvm, string action)
        {
            try
            {
                ViewBag.TSStartDate = WebConfigurationManager.AppSettings["TimeSheetStartDay"];
                ViewBag.TSRole = "TimeSheetAdmin";

                TimeSheetService timeSheetService = new TimeSheetService();

                if (!ModelState.IsValid)
                {
                    tsvm.AbsenceTypeList = GetDropDownList("Absence", null);
                    return View(tsvm);
                }
                var lastTS = timeSheetService.CheckTSStartDateExists(tsvm.UserID, tsvm.StartDate, tsvm.EndDate, tsvm.TimeSheetID);
                if (lastTS != null)
                {
                    ModelState.AddModelError("error", $"*A time sheet with Id {lastTS.TimeSheetCode} with this date range already exists");
                    tsvm.AbsenceTypeList = GetDropDownList("Absence", null);
                    return View(tsvm);
                }
                else if (tsvm.TotalHours <= 0 || tsvm.TotalHours == null)
                {
                    tsvm.AbsenceTypeList = GetDropDownList("Absence", null);
                    var ddlValue = GetDDLID("None", tsvm.AbsenceTypeList);
                    if ((tsvm.Day1RegularHours <= 0 || tsvm.Day1RegularHours == null) && tsvm.LookupAbsenceTypeDay1 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 1 ");
                        return View(tsvm);
                    }
                    else if ((tsvm.Day2RegularHours <= 0 || tsvm.Day2RegularHours == null) && tsvm.LookupAbsenceTypeDay2 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 2 ");
                        return View(tsvm);
                    }
                    else if ((tsvm.Day3RegularHours <= 0 || tsvm.Day3RegularHours == null) && tsvm.LookupAbsenceTypeDay3 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 3 ");
                        return View(tsvm);
                    }
                    else if ((tsvm.Day4RegularHours <= 0 || tsvm.Day4RegularHours == null) && tsvm.LookupAbsenceTypeDay4 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 4 ");
                        return View(tsvm);
                    }
                    else if ((tsvm.Day5RegularHours <= 0 || tsvm.Day5RegularHours == null) && tsvm.LookupAbsenceTypeDay5 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 5 ");
                        return View(tsvm);
                    }
                    else if ((tsvm.Day6RegularHours <= 0 || tsvm.Day6RegularHours == null) && tsvm.LookupAbsenceTypeDay6 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 6 ");
                        return View(tsvm);
                    }
                    else if ((tsvm.Day7RegularHours <= 0 || tsvm.Day7RegularHours == null) && tsvm.LookupAbsenceTypeDay7 == ddlValue)
                    {
                        ModelState.AddModelError("error", "Leave Type should be selected if hours are zero for Day 7 ");
                        return View(tsvm);
                    }
                }
                
                TimeSheetsMaster timeSheetsMaster = timeSheetService.GetJustTimeSheetMaster(tsvm.TimeSheetID);
                List<TimeSheetSummary> summaries = timeSheetService.GetTimeSheetSummary(tsvm.TimeSheetID);

                timeSheetsMaster.StartDate = tsvm.StartDate;
                timeSheetsMaster.EndDate = tsvm.EndDate;
                timeSheetsMaster.RegularHours = tsvm.RegularHours ?? 0;
                timeSheetsMaster.OverTimeHours = tsvm.OverTimeHours ?? 0;
                timeSheetsMaster.TotalHours = tsvm.TotalHours ?? 0;
                timeSheetsMaster.Day1RegularHours = tsvm.Day1RegularHours ?? 0;
                timeSheetsMaster.Day2RegularHours = tsvm.Day2RegularHours ?? 0;
                timeSheetsMaster.Day3RegularHours = tsvm.Day3RegularHours ?? 0;
                timeSheetsMaster.Day4RegularHours = tsvm.Day4RegularHours ?? 0;
                timeSheetsMaster.Day5RegularHours = tsvm.Day5RegularHours ?? 0;
                timeSheetsMaster.Day6RegularHours = tsvm.Day6RegularHours ?? 0;
                timeSheetsMaster.Day7RegularHours = tsvm.Day7RegularHours ?? 0;
                timeSheetsMaster.Day1OverHours = tsvm.Day1OverHours ?? 0;
                timeSheetsMaster.Day2OverHours = tsvm.Day2OverHours ?? 0;
                timeSheetsMaster.Day3OverHours = tsvm.Day3OverHours ?? 0;
                timeSheetsMaster.Day4OverHours = tsvm.Day4OverHours ?? 0;
                timeSheetsMaster.Day5OverHours = tsvm.Day5OverHours ?? 0;
                timeSheetsMaster.Day6OverHours = tsvm.Day6OverHours ?? 0;
                timeSheetsMaster.Day7OverHours = tsvm.Day7OverHours ?? 0;
                timeSheetsMaster.LookupAbsenceTypeDay1 = tsvm.LookupAbsenceTypeDay1;
                timeSheetsMaster.LookupAbsenceTypeDay2 = tsvm.LookupAbsenceTypeDay2;
                timeSheetsMaster.LookupAbsenceTypeDay3 = tsvm.LookupAbsenceTypeDay3;
                timeSheetsMaster.LookupAbsenceTypeDay4 = tsvm.LookupAbsenceTypeDay4;
                timeSheetsMaster.LookupAbsenceTypeDay5 = tsvm.LookupAbsenceTypeDay5;
                timeSheetsMaster.LookupAbsenceTypeDay6 = tsvm.LookupAbsenceTypeDay6;
                timeSheetsMaster.LookupAbsenceTypeDay7 = tsvm.LookupAbsenceTypeDay7;

                timeSheetsMaster.AbsenceHoursDay1 = tsvm.AbsenceHoursDay1;
                timeSheetsMaster.AbsenceHoursDay2 = tsvm.AbsenceHoursDay2;
                timeSheetsMaster.AbsenceHoursDay3 = tsvm.AbsenceHoursDay3;
                timeSheetsMaster.AbsenceHoursDay4 = tsvm.AbsenceHoursDay4;
                timeSheetsMaster.AbsenceHoursDay5 = tsvm.AbsenceHoursDay5;
                timeSheetsMaster.AbsenceHoursDay6 = tsvm.AbsenceHoursDay6;
                timeSheetsMaster.AbsenceHoursDay7 = tsvm.AbsenceHoursDay7;

                timeSheetsMaster.ModifiedUserId = User.Identity.GetUserId();
                timeSheetsMaster.ModifiedDate = DateTime.Now;
                timeSheetsMaster.LookupApprovedStatus = timeSheetService.GetLookupIdForStatus("Pending", "TimeSheetStatus").LookupCodeId;
               
                summaries = Mapper.Map<List<TimeSheetSummaryViewModel>, List<TimeSheetSummary>>(tsvm.TimeSheetSummaryList);

                timeSheetService.UpdateTimeSheet(timeSheetsMaster);
                timeSheetService.UpdateTimeSheetSummaries(summaries);
                TempData["status"] = "Timesheet successfully edited";



                if (string.Equals(action.ToString(), "Save And Submit"))
                {                   
                    await SendEmailTimeSheetSubmitted(timeSheetsMaster.TimeSheetID);
                }


                return RedirectToAction("EmployeeAdminTimeSheetsList");
            }
            catch (Exception ex)
            {
                ViewBag.TSRole = "TimeSheetAdmin";
                TempData["error"] = "Something went wrong while editing timesheet";
                tsvm.AbsenceTypeList = GetDropDownList("Absence", null);
                return View("EmployeeTimeSheetEdit",tsvm);
            }
        }

        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult EmployeeAdminTimeSheetDetails(int id )
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            ViewBag.TSRole = "TimeSheetAdmin";

          
            TimeSheetViewModel tvm = new TimeSheetViewModel();
            TimeSheetService timeSheetService = new TimeSheetService();
            TimeSheetsMaster timeSheet = new TimeSheetsMaster();
            List<TimeSheetSummary> timesheetSummaries = new List<TimeSheetSummary>();
            
                timeSheet = timeSheetService.GetTimeSheetMaster(id);
               
            
            if(timeSheet == null)
            {
                TempData["error"] = "Either this file does not exist or you dont have permission to access this";
                return RedirectToAction("EmployeeAdminTimeSheetsList");
            }
            timesheetSummaries = timeSheetService.GetTimeSheetSummary(timeSheet.TimeSheetID);

            tvm.TimeSheetSummaryList = new List<TimeSheetSummaryViewModel>();
            for (int i = 0; i < 5; i++)
            {
                TimeSheetSummaryViewModel tsvm = new TimeSheetSummaryViewModel();
                tvm.TimeSheetSummaryList.Add(tsvm);
            }

            tvm = Mapper.Map<TimeSheetsMaster, TimeSheetViewModel>(timeSheet);
            tvm.TimeSheetSummaryList = Mapper.Map<List<TimeSheetSummary>, List<TimeSheetSummaryViewModel>>(timesheetSummaries);
            tvm.AbsenceTypeList = GetDropDownList("Absence", null);
            if (tvm.AspNetUser.AspNetUser1 != null)
            {
                tvm.ReportingManager = tvm.AspNetUser.AspNetUser1.Firstname + " " + tvm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                tvm.ReportingManager = "NA";
            }

            List<LOA> loas = new List<LOA>();
            loas = timeSheetService.GetLOABetweenDatesRepoMan(timeSheet.StartDate, timeSheet.EndDate, timeSheet.UserID);
            var loasAssociated = Mapper.Map<List<LOA>, List<LoaViewModel>>(loas);
            foreach (var l in loasAssociated)
            {

                l.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
                l.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);
            }
            tvm.LoasAssociated = loasAssociated;


            return View("EmployeeTimeSheetDetails", tvm);
        }

        [Authorize(Roles = "TimeSheetReportingManager")]
        public ActionResult EmployeeRepoManTimeSheetDetails(int id)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            ViewBag.TSRole = "TimeSheetReportingManager";

          
            TimeSheetViewModel tvm = new TimeSheetViewModel();
            TimeSheetService timeSheetService = new TimeSheetService();
            TimeSheetsMaster timeSheet = new TimeSheetsMaster();
            List<TimeSheetSummary> timesheetSummaries = new List<TimeSheetSummary>();
            
            timeSheet = timeSheetService.GetTimeSheetMaster(id, User.Identity.GetUserId());
            
            if (timeSheet == null)
            {
                TempData["error"] = "Either this file does not exist or you dont have permission to access this";
                return RedirectToAction("EmployeeRepoManTimeSheetsList");
            }


            timesheetSummaries = timeSheetService.GetTimeSheetSummary(timeSheet.TimeSheetID);

            tvm.TimeSheetSummaryList = new List<TimeSheetSummaryViewModel>();
            for (int i = 0; i < 5; i++)
            {
                TimeSheetSummaryViewModel tsvm = new TimeSheetSummaryViewModel();
                tvm.TimeSheetSummaryList.Add(tsvm);
            }

            tvm = Mapper.Map<TimeSheetsMaster, TimeSheetViewModel>(timeSheet);
            tvm.TimeSheetSummaryList = Mapper.Map<List<TimeSheetSummary>, List<TimeSheetSummaryViewModel>>(timesheetSummaries);
            tvm.AbsenceTypeList = GetDropDownList("Absence", null);
            if (tvm.AspNetUser.AspNetUser1 != null)
            {
                tvm.ReportingManager = tvm.AspNetUser.AspNetUser1.Firstname + " " + tvm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                tvm.ReportingManager = "NA";
            }

            
            List<LOA> loas = new List<LOA>();
            loas = timeSheetService.GetLOABetweenDatesRepoMan(timeSheet.StartDate, timeSheet.EndDate, timeSheet.UserID);
            var loasAssociated = Mapper.Map<List<LOA>, List<LoaViewModel>>(loas);
            foreach (var l in loasAssociated)
            {

                l.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
                l.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);
            }
            tvm.LoasAssociated = loasAssociated;

            return View("EmployeeTimeSheetDetails", tvm);
        }

        [Authorize(Roles = "TimeSheetManager")]
        public ActionResult EmployeeManagerTimeSheetDetails(int id)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            ViewBag.TSRole = "TimeSheetManager";

            
            TimeSheetViewModel tvm = new TimeSheetViewModel();
            TimeSheetService timeSheetService = new TimeSheetService();
            TimeSheetsMaster timeSheet = new TimeSheetsMaster();
            List<TimeSheetSummary> timesheetSummaries = new List<TimeSheetSummary>();
            
                timeSheet = timeSheetService.GetTimeSheetMaster(id);

            
            if (timeSheet == null)
            {
                TempData["error"] = "Either this file does not exist or you dont have permission to access this";
                return RedirectToAction("EmployeeManagerTimeSheetsList");
            }
            timesheetSummaries = timeSheetService.GetTimeSheetSummary(timeSheet.TimeSheetID);

            tvm.TimeSheetSummaryList = new List<TimeSheetSummaryViewModel>();
            for (int i = 0; i < 5; i++)
            {
                TimeSheetSummaryViewModel tsvm = new TimeSheetSummaryViewModel();
                tvm.TimeSheetSummaryList.Add(tsvm);
            }

            tvm = Mapper.Map<TimeSheetsMaster, TimeSheetViewModel>(timeSheet);
            tvm.TimeSheetSummaryList = Mapper.Map<List<TimeSheetSummary>, List<TimeSheetSummaryViewModel>>(timesheetSummaries);
            tvm.AbsenceTypeList = GetDropDownList("Absence", null);
            if (tvm.AspNetUser.AspNetUser1 != null)
            {
                tvm.ReportingManager = tvm.AspNetUser.AspNetUser1.Firstname + " " + tvm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                tvm.ReportingManager = "NA";
            }

            List<LOA> loas = new List<LOA>();
            loas = timeSheetService.GetLOABetweenDatesRepoMan(timeSheet.StartDate, timeSheet.EndDate, timeSheet.UserID);
            var loasAssociated = Mapper.Map<List<LOA>, List<LoaViewModel>>(loas);
            foreach (var l in loasAssociated)
            {

                l.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
                l.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);
            }
            tvm.LoasAssociated = loasAssociated;

            return View("EmployeeTimeSheetDetails", tvm);
        }

        [Authorize(Roles = "TimeSheetUser")]
        public ActionResult MyTimesheetDetails(int id)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }


            
            TimeSheetViewModel tvm = new TimeSheetViewModel();
            TimeSheetService timeSheetService = new TimeSheetService();
            var timeSheet = timeSheetService.GetMyTimeSheetMaster(id, User.Identity.GetUserId());
            if(timeSheet == null)
            {
                TempData["error"] = "Either this file does not exists or you dont have permission";
               
                return RedirectToAction("MyTimeSheetsList");
            }
            var timesheetSummaries = timeSheetService.GetTimeSheetSummary(timeSheet.TimeSheetID);
            tvm.TimeSheetSummaryList = new List<TimeSheetSummaryViewModel>();
            

            tvm = Mapper.Map<TimeSheetsMaster, TimeSheetViewModel>(timeSheet);
            tvm.TimeSheetSummaryList = Mapper.Map<List<TimeSheetSummary>, List<TimeSheetSummaryViewModel>>(timesheetSummaries);
            tvm.AbsenceTypeList = GetDropDownList("Absence", null);
            if (tvm.AspNetUser.AspNetUser1 != null)
            {
                tvm.ReportingManager = tvm.AspNetUser.AspNetUser1.Firstname + " " + tvm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                tvm.ReportingManager = "None";
            }
            
            return View(tvm);
        }


        [Authorize(Roles = "TimeSheetUser")]
        public ActionResult MyTSDelete(int id)
        {
            try
            {
                TimeSheetService timeSheetService = new TimeSheetService();

                var timeSheet = timeSheetService.GetJustTimeSheetMaster(id,User.Identity.GetUserId());
                if (timeSheet == null)
                {
                    TempData["error"] = "Either this file does not exists or you dont have permission";
                    
                    return View("MyTimeSheetsList");
                }
                timeSheet.IsDeleted = true;
                timeSheet.IsActive  = false;
                timeSheet.ModifiedUserId = User.Identity.GetUserId();
                timeSheet.ModifiedDate = DateTime.Now;

                timeSheetService.UpdateTimeSheet(timeSheet);

                TempData["status"] = "Timesheet successfully deleted";
                return RedirectToAction("MyTimeSheetsList");
            }
            catch
            {
                TempData["error"] = "Something went wrong while deleting timesheet";
                return RedirectToAction("MyTimeSheetsList");
            }
        }

        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult EmployeeAdminTSDelete(int id)
        {
            try
            {
                ViewBag.TSRole = "TimeSheetAdmin";
                TimeSheetService timeSheetService = new TimeSheetService();
                ViewBag.TSRole = "TimeSheetAdmin";
                var timeSheet = timeSheetService.GetJustTimeSheetMaster(id);
                if (timeSheet == null)
                {
                    TempData["error"] = "Either this file does not exists or you dont have permission";
                    
                    return RedirectToAction("EmployeeAdminTimeSheetsList");
                }
                timeSheet.IsDeleted = true;
                timeSheet.IsActive = false;
                timeSheet.ModifiedUserId = User.Identity.GetUserId();
                timeSheet.ModifiedDate = DateTime.Now;

                timeSheetService.UpdateTimeSheet(timeSheet);

                TempData["status"] = "Timesheet successfully deleted";
                return RedirectToAction("EmployeeAdminTimeSheetsList");
            }
            catch
            {
                TempData["error"] = "Something went wrong while deleting timesheet";
                return RedirectToAction("EmployeeAdminTimeSheetsList");
            }
        }

        //[Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult HardDelete(int id)
        {
            try
            {
                TimeSheetService timeSheetService = new TimeSheetService();

                timeSheetService.HardDeleteSummaries(id);
                timeSheetService.HardDeleteTimeSheet(id);

                TempData["status"] = "Timesheet successfully deleted";
                return RedirectToAction("MyTimeSheetsList");
            }
            catch
            {
                TempData["error"] = "Something went wrong while deleting timesheet";
                return RedirectToAction("MyTimeSheetsList");
            }
        }


        private List<DropDownListViewModel> GetDropDownList(string lookupTypeName, int? lookupCodeId)
        {

            var _list = new List<DropDownListViewModel>();

            ProductService productService = new ProductService();
            List<LookupCodeMaster> lookUpCategories = new List<LookupCodeMaster>();
            if (lookupTypeName != null)
            {
                lookUpCategories = productService.GetProductDropDownList(lookupTypeName);
            }
            else if (lookupCodeId != null)
            {
                lookUpCategories = productService.GetProductDropDownList(lookupCodeId ?? 0);
            }


            foreach (LookupCodeMaster cat in lookUpCategories)
            {
                DropDownListViewModel ddl = new DropDownListViewModel();

                ddl.Id = cat.LookupCodeId;
                ddl.Name = cat.LookupCodeName;
                _list.Add(ddl);
            }


            return _list;
        }

        private int GetDDLID(string Name, List<DropDownListViewModel> ddl)
        {
            foreach(DropDownListViewModel d in ddl)
            {
                if(Name == d.Name)
                {
                    return d.Id;
                }
            }
            return 1;

        }



        


        [Authorize(Roles = "TimeSheetReportingManager")]
        public ActionResult Approve(int timeSheetId)
        {
            try
            {

               
                TimeSheetService timeSheetService = new TimeSheetService();
                var tsheet = timeSheetService.GetTimeSheetMaster(timeSheetId, User.Identity.GetUserId());
                if(tsheet == null)
                {
                    TempData["error"] = "Timesheet does not exist or you dont have permissions";
                    return RedirectToAction("EmployeeRepoManTimeSheetsList");
                }

                var userId = User.Identity.GetUserId();
                UserService userService = new UserService();
                var user = userService.GetUserWithId(userId);
                tsheet.ApprovedBy = user.Firstname + " " + user.LastName + " (" + user.EmployeeCode + ")";
                timeSheetService.UpdateTimeSheet(tsheet);

                


                ViewBag.TSRole = "TimeSheetReportingManager";
                ChangeStatusOfTimeSheet(timeSheetId, "Approved");

                

                TempData["status"] = "TimeSheet has been approved";
                return RedirectToAction("EmployeeRepoManTimeSheetsList");
                

            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while Approving";              
                return RedirectToAction("EmployeeRepoManTimeSheetsList");
            }
        }

        [Authorize(Roles = "TimeSheetReportingManager")]
        public async Task<ActionResult> Reject(TimeSheetViewModel tsvm)
        {
            try
            {
                
                ViewBag.TSRole = "TimeSheetReportingManager";
                TimeSheetService timeSheetService = new TimeSheetService();

                var timeSheet = timeSheetService.GetJustTimeSheetMaster(tsvm.TimeSheetID);
                timeSheet.LookupApprovedStatus = timeSheetService.GetLookupIdForStatus("Rejected", "TimeSheetStatus").LookupCodeId;
                timeSheet.Comments += " " + tsvm.TempComments + " (" + DateTime.Now.ToString() + "), <br> ";
                timeSheet.ModifiedDate = DateTime.Now;
                timeSheet.ModifiedUserId = User.Identity.GetUserId();
                timeSheetService.UpdateTimeSheet(timeSheet);
                await SendEmailTimeSheetRejected(tsvm.TimeSheetID);

                TempData["status"] = "TimeSheet has been rejected and mail has been sent to employee";
                return null;


            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while Rejecting";
                return RedirectToAction("EmployeeRepoManTimeSheetsList");
            }
        }

        private void ChangeStatusOfTimeSheet(int id, string status)
        {
            TimeSheetService timeSheetService = new TimeSheetService();
            var timeSheet = timeSheetService.GetJustTimeSheetMaster(id);
            timeSheet.LookupApprovedStatus = timeSheetService.GetLookupIdForStatus(status, "TimeSheetStatus").LookupCodeId;            
            timeSheet.ModifiedDate = DateTime.Now;
            timeSheet.ModifiedUserId = User.Identity.GetUserId();
            timeSheetService.UpdateTimeSheet(timeSheet);
        }


        [Authorize(Roles = "TimeSheetUser")]
        public ActionResult MyTimeSheetDownload(int id)
        {
            try
            {
                TimeSheetService timeSheetService = new TimeSheetService();
                var timeSheet = timeSheetService.GetMyTimeSheetMaster(id,User.Identity.GetUserId());
                if(timeSheet == null)
                {
                    TempData["error"] = "Either this file does not exists or you dont have permission";
                    
                    return RedirectToAction("MyTimeSheetsList");
                }
                
                var wb = GenerateTimeSheetExcelFile(timeSheet);
                var fileName = GetExcelFileName(timeSheet);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                   
                }
                
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while";
               
                return RedirectToAction("MyTimeSheetsList");
            }
        }

        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult EmployeeAdminTimeSheetDownload(int id)
        {
            try
            {
                TimeSheetService timeSheetService = new TimeSheetService();
                var timeSheet = new TimeSheetsMaster();

               
                    timeSheet = timeSheetService.GetTimeSheetMaster(id);

                if (timeSheet == null)
                {
                    TempData["error"] = "Either this file does not exist or you dont have permission";
                    return RedirectToAction("EmployeeAdminTimeSheetsList");
                }

               

                var wb = GenerateTimeSheetExcelFile(timeSheet);
                var fileName = GetExcelFileName(timeSheet);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while";
                
                return RedirectToAction("EmployeeAdminTimeSheetsList");
            }
        }

        [Authorize(Roles = "TimeSheetReportingManager")]
        public ActionResult EmployeeRepoManTimeSheetDownload(int id)
        {
            try
            {
                TimeSheetService timeSheetService = new TimeSheetService();
                var timeSheet = new TimeSheetsMaster();

               
                    timeSheet = timeSheetService.GetTimeSheetMaster(id, User.Identity.GetUserId());

                
                if (timeSheet == null)
                {
                    TempData["error"] = "Either this file does not exist or you dont have permission";
                    return RedirectToAction("EmployeeRepoManTimeSheetsList");
                }



                var wb = GenerateTimeSheetExcelFile(timeSheet);
                var fileName = GetExcelFileName(timeSheet);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                   
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while";
               
                return RedirectToAction("EmployeeRepoManTimeSheetsList");
            }
        }


        [Authorize(Roles = "TimeSheetManager")]
        public ActionResult EmployeeManagerTimeSheetDownload(int id)
        {
            try
            {
                TimeSheetService timeSheetService = new TimeSheetService();
                var timeSheet = new TimeSheetsMaster();

               
                    timeSheet = timeSheetService.GetTimeSheetMaster(id);

                
                if (timeSheet == null)
                {
                    TempData["error"] = "Either this file does not exist or you dont have permission";
                    return RedirectToAction("EmployeeManagerTimeSheetsList");
                }



                var wb = GenerateTimeSheetExcelFile(timeSheet);
                var fileName = GetExcelFileName(timeSheet);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                   
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while";
               
                return RedirectToAction("EmployeeManagerTimeSheetsList");
            }
        }


        
        public async Task SendEmailTimeSheetSubmitted(int timeSheetId, bool includeAttachment = false)
        {
            try
            {
                TimeSheetService timeSheetService = new TimeSheetService();
                
                var timeSheet = timeSheetService.GetTimeSheetMaster(timeSheetId);

                var subject = "Timesheet Submitted " + GetExcelFileName(timeSheet);

               


               
                var callbackUrl = Url.Action("EmployeeRepoManTimeSheetDetails", "TimeSheet", new { id = timeSheetId }, protocol: Request.Url.Scheme);
                var body = "Your Employee " + timeSheet.AspNetUser.Firstname + " " + timeSheet.AspNetUser.LastName + " submitted time " +
                    "sheet for week " + timeSheet.StartDate.ToString("MM/dd/yyyy") + "-" + timeSheet.EndDate.ToString("MM/dd/yyyy") + "" +
                    ", You can check it by clicking <a href=\"" + callbackUrl + "\">here</a>";

               
                var destinationEmail = timeSheet.AspNetUser.AspNetUser1.Email;
                IdentityMessage im = CreateMessage(destinationEmail, subject, body);
                
                if (includeAttachment)
                {
                    byte[] bytes = GetExcelByteDate(timeSheet);
                    await SendEmailWithAttachment(im, bytes, subject);
                }
                else
                {
                    await SendEmailWithAttachment(im, null, null);
                }

                
                TempData["status"] = "TimeSheet submitted to your Reporting Manager";
               

            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while submiting email";
                
            }
        }

        public async Task SendEmailTimeSheetRejected(int timeSheetId, bool includeAttachment = false)
        {
            try
            {
                TimeSheetService timeSheetService = new TimeSheetService();
                
                var timeSheet = timeSheetService.GetTimeSheetMaster(timeSheetId);

                var subject = "TimeSheet Not Accepted";
                var callbackUrl = Url.Action("MyTimeSheetEdit", "TimeSheet", new { id = timeSheetId }, protocol: Request.Url.Scheme);
                var body = "Your TimeSheet for week " + timeSheet.StartDate.ToString("MM/dd/yyyy") + "-" + timeSheet.EndDate.ToString("MM/dd/yyyy") + "" +
                    "was Not Accepted with comments '"+ timeSheet.Comments +"' please click <a href=\"" + callbackUrl + "\">here</a> to review timesheet and send again.";

                
                var destinationEmail = timeSheet.AspNetUser.Email;
                IdentityMessage im = CreateMessage(destinationEmail, subject, body);

                if (includeAttachment)
                {
                    byte[] bytes = GetExcelByteDate(timeSheet);
                    await SendEmailWithAttachment(im, bytes, subject);
                }
                else
                {
                    await SendEmailWithAttachment(im, null, null);
                }


                TempData["status"] = "TimeSheet rejection mail sent to employee";
                
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong";
                
            }
        }

        private IdentityMessage CreateMessage(string destinationEmail, string subject, string body)
        {
            IdentityMessage im = new IdentityMessage();
            im.Destination = destinationEmail;
            im.Subject = subject;
            im.Body = body;


            return im;

        }


        private byte[] GetExcelByteDate(TimeSheetsMaster timeSheet)
        {
            try
            {
                var wb = GenerateTimeSheetExcelFile(timeSheet);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    byte[] bytes = stream.ToArray();
                    return bytes;
                    
                }
                
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while sending email";
               
                return null;
            }
        }

        private XLWorkbook GenerateTimeSheetExcelFile(TimeSheetsMaster timeSheet)
        {
            try
            {
                

                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("TimeSheet");

                    ws.Style.Fill.BackgroundColor = XLColor.White;

                    ws.Cell("B2").Value = "Employee code";
                    ws.Cell("B2").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    ws.Cell("C2").Value = timeSheet.AspNetUser.EmployeeCode;
                    ws.Cell("C2").Style.Fill.BackgroundColor = XLColor.Beige;

                    ws.Cell(3, 2).Value = "Employee Name";
                    ws.Cell(3, 2).Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    ws.Cell(3, 3).Value = timeSheet.AspNetUser.Firstname + ' ' + timeSheet.AspNetUser.LastName;
                    ws.Cell(3, 3).Style.Fill.BackgroundColor = XLColor.Beige;

                    ws.Cell("E2").Value = "Start Date";
                    ws.Cell("E2").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    ws.Cell("F2").Value = timeSheet.StartDate.ToString("MM/dd/yyyy");
                    ws.Cell("F2").Style.Fill.BackgroundColor = XLColor.Beige;

                    ws.Cell("E3").Value = "End Date";
                    ws.Cell("E3").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    ws.Cell("F3").Value = timeSheet.EndDate.ToString("MM/dd/yyyy");
                    ws.Cell("F3").Style.Fill.BackgroundColor = XLColor.Beige;

                    ws.Cell("H2").Value = "Time Sheet Id";
                    ws.Cell("H2").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    ws.Cell("I2").Value = timeSheet.TimeSheetCode;
                    ws.Cell("I2").Style.Fill.BackgroundColor = XLColor.Beige;

                    ws.Cell("H3").Value = "Reporting Manger";
                    ws.Cell("H3").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    if (timeSheet.AspNetUser.AspNetUser1 != null)
                    {
                        ws.Cell("I3").Value = timeSheet.AspNetUser.AspNetUser1.Firstname + ' ' + timeSheet.AspNetUser.AspNetUser1.LastName;
                    }
                    else
                    {
                        ws.Cell("I3").Value = "None";
                    }
                    
                    ws.Cell("I3").Style.Fill.BackgroundColor = XLColor.Beige;

                    ws.Cell("K2").Value = "Time Sheet Status";
                    ws.Cell("K2").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    ws.Cell("L2").Value = timeSheet.LookupCodeMaster7.LookupCodeName;
                    ws.Cell("L2").Style.Fill.BackgroundColor = XLColor.Beige;




                    ws.Cell("B17").Value = "Comments :";
                    ws.Cell("B17").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    if (timeSheet.Comments != null)
                    {
                        string str = timeSheet.Comments;
                        List<string> cmts = str.Split(new[] { "<br>" },StringSplitOptions.None).ToList();
                        
                        var rangeWithStrings = ws.Cell("B18").InsertData(cmts).Style.Alignment.SetWrapText(false);



                        
                        
                    }
                var dates = new List<DateTime>();

                    for (var dt = timeSheet.StartDate; dt <= timeSheet.EndDate; dt = dt.AddDays(1))
                    {
                        dates.Add(dt);
                    }

                    DataTable summaries = new DataTable();

                    
                    summaries.Columns.AddRange(new DataColumn[11] { new DataColumn("No"),
                                            new DataColumn("Project Name"),
                                            new DataColumn("Description"),
                                            new DataColumn(dates[0].ToString("ddd MM/dd/yyyy")),
                                            new DataColumn(dates[1].ToString("ddd MM/dd/yyyy")),
                                            new DataColumn(dates[2].ToString("ddd MM/dd/yyyy")),
                                            new DataColumn(dates[3].ToString("ddd MM/dd/yyyy")),
                                            new DataColumn(dates[4].ToString("ddd MM/dd/yyyy")),
                                            new DataColumn(dates[5].ToString("ddd MM/dd/yyyy")),
                                            new DataColumn(dates[6].ToString("ddd MM/dd/yyyy")),
                                            new DataColumn("Total") });
                    
                    int i = 1;
                    foreach (var summary in timeSheet.TimeSheetSummaries)
                    {
                        
                        decimal projectTotal = summary.Day1Hours + summary.Day2Hours + summary.Day3Hours + summary.Day4Hours + summary.Day5Hours + summary.Day6Hours + summary.Day7Hours;
                        summaries.Rows.Add(i, summary.ProjectName, summary.Description, summary.Day1Hours, summary.Day2Hours, summary.Day3Hours, summary.Day4Hours, summary.Day5Hours, summary.Day6Hours, summary.Day7Hours, projectTotal);
                        i++;
                    }

                        summaries.Rows.Add("", "","LeaveType", timeSheet.LookupCodeMaster.LookupCodeName, timeSheet.LookupCodeMaster1.LookupCodeName, timeSheet.LookupCodeMaster2.LookupCodeName, timeSheet.LookupCodeMaster3.LookupCodeName, timeSheet.LookupCodeMaster4.LookupCodeName, timeSheet.LookupCodeMaster5.LookupCodeName, timeSheet.LookupCodeMaster6.LookupCodeName,"");

                    summaries.Rows.Add("", "", "RegularHours", timeSheet.Day1RegularHours, timeSheet.Day2RegularHours, timeSheet.Day3RegularHours, timeSheet.Day4RegularHours, timeSheet.Day5RegularHours, timeSheet.Day6RegularHours, timeSheet.Day7RegularHours, timeSheet.RegularHours);
                    summaries.Rows.Add("", "", "Over Time Hours", timeSheet.Day1OverHours, timeSheet.Day2OverHours, timeSheet.Day3OverHours, timeSheet.Day4OverHours, timeSheet.Day5OverHours, timeSheet.Day6OverHours, timeSheet.Day7OverHours, timeSheet.OverTimeHours);
                    summaries.Rows.Add("", "", "Total Hours", timeSheet.Day1RegularHours+timeSheet.Day1OverHours, 
                        timeSheet.Day2RegularHours+timeSheet.Day2OverHours,
                        timeSheet.Day3RegularHours + timeSheet.Day3OverHours,
                        timeSheet.Day4RegularHours + timeSheet.Day4OverHours,
                        timeSheet.Day5RegularHours + timeSheet.Day5OverHours,
                        timeSheet.Day6RegularHours + timeSheet.Day6OverHours,
                        timeSheet.Day7RegularHours + timeSheet.Day7OverHours, timeSheet.TotalHours);


                    ws.Cell(5, 2).InsertTable(summaries.AsEnumerable(), false);                    
                    ws.Range("D5","L5").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    ws.Range("B5", "D5").Style.Fill.BackgroundColor = XLColor.Beige;
                    ws.Range("B11", "K11").Style.Fill.BackgroundColor = XLColor.Beige;
                    ws.Range("B12", "K12").Style.Fill.BackgroundColor = XLColor.Beige;
                    ws.Range("B13", "K13").Style.Fill.BackgroundColor = XLColor.Beige;
                    ws.Range("B14", "K14").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    ws.Range("L5", "L14").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    ws.Range("L5", "L14").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("B5", "L14").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                    ws.Range("L5", "L14").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("B5", "L14").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("B5", "B12").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range("C5", "C12").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range("D5", "D14").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range("E5", "E14").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range("F5", "F14").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range("G5", "G14").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range("H5", "H14").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range("I5", "I14").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range("J5", "J14").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range("I5", "I14").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range("B14", "L14").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range("B5", "L5").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.Range("B10", "L10").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                    ws.Range("B5", "L5").Style.Font.Bold = true;
                    ws.Range("B14", "K14").Style.Font.Bold = true;
                    ws.Range("D13", "D15").Style.Font.Bold = true;
                    ws.Range("B5", "K5").Style.Font.Bold = true;
                    ws.Range("B11", "D11").Merge();
                    ws.Range("B11", "D11").Value = "Leave Type";
                    ws.Range("B12", "D12").Merge();
                    ws.Range("B12", "D12").Value = "Regular Hours";
                    ws.Range("B13", "D13").Merge();
                    ws.Range("B13", "D13").Value = "Over Time Hours";
                    ws.Range("B14", "D14").Merge();
                    ws.Range("B14", "D14").Value="Total";
                    ws.Columns().AdjustToContents();

                    return wb;
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while";                
                return null;
            }
        }


        private string GetExcelFileName(TimeSheetsMaster timeSheet)
        {
            string fileName = timeSheet.AspNetUser.Firstname+ timeSheet.AspNetUser.LastName+"- WE:" +timeSheet.EndDate.ToString("MMddyy")+ ".xlsx";
            return fileName;

        }

      
        private async Task SendEmailWithAttachment(IdentityMessage message, byte[] bytes, string attName)
        {
            // Plug in your email service here to send an email.
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.office365.com";
            client.EnableSsl = true;
            //client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("noreply@epathusa.in", "Suman@123");

            MailMessage mailMessage = new MailMessage("noreply@epathusa.in", message.Destination, message.Subject, message.Body);
            mailMessage.IsBodyHtml = true;
            if(attName != null)
            {
                Attachment at = new Attachment(new MemoryStream(bytes), attName);
                mailMessage.Attachments.Add(at);
            }
            

           
            await client.SendMailAsync(mailMessage);
        }


        public JsonResult GetLOABetweenDates(DateTime fromDate, DateTime toDate)
        {
            TimeSheetService tsService = new TimeSheetService();
            var obj = tsService.GetLOABetweenDates(fromDate, toDate, User.Identity.GetUserId());
            var result = new object();
            if (obj != null)
            {
                result = new { id = obj.LoaCode, dates = obj.StartDate.ToString("MM/dd/yyyy") + "-" + obj.EndDate.ToString("MM/dd/yyyy") };

            }
            else
            {
                result = null;
            }
            
          
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetLOABetweenDatesRepoMan(DateTime fromDate, DateTime toDate, string UserId)
        //{
        //    TimeSheetService tsService = new TimeSheetService();
        //    var obj = tsService.GetLOABetweenDatesRepoMan(fromDate, toDate, UserId);
        //    var result = new object();
        //    if (obj != null)
        //    {
        //        result = new { id = obj.LoaCode, dates = obj.StartDate.ToString("MM/dd/yyyy") + "-" + obj.EndDate.ToString("MM/dd/yyyy") };

        //    }
        //    else
        //    {
        //        result = null;
        //    }

           
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
    }

   
}

