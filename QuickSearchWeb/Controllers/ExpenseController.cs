using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNet.Identity;
using QuickSearchBusiness.Services;
using QuickSearchData;
using QuickSearchWeb.Models;

namespace QuickSearchWeb.Controllers
{

    [Authorize]
    public class ExpenseController : Controller
    {
        // GET: Expenses
        [Authorize(Roles = "TimeSheetUser")]
        public ActionResult MyExpenseList()
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            ExpenseService expenseService = new ExpenseService();

            var expenses = expenseService.GetExpenseListWithUserId(User.Identity.GetUserId());

            var expensesList = Mapper.Map<List<ExpenseMaster>, List<ExpenseListViewModel>>(expenses);

            return View(expensesList);
        }


        [Authorize(Roles = "TimeSheetUser")]
        public ActionResult MyExpenseCreate()
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
            //TempData["TSStartDate"] = WebConfigurationManager.AppSettings["TimeSheetStartDay"];


            ExpenseViewModel evm = new ExpenseViewModel();
            UserService userService = new UserService();
            evm.UserId = User.Identity.GetUserId();
            evm.AspNetUser = userService.GetUserWithId(evm.UserId);
            if (evm.AspNetUser.AspNetUser1 != null)
            {
                evm.ReportingManager = evm.AspNetUser.AspNetUser1.Firstname + " " + evm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                evm.ReportingManager = "None";
            }
            //evm.EmployeeName = AspNetUser.Firstname +" "+ AspNetUser.LastName;
            
            //int dayOfWeek = (int)DateTime.Now.DayOfWeek;
            evm.StartDate = GetTSStartingDate();
            evm.EndDate = evm.StartDate.AddDays(6);

            return View(evm);

        }


        private DateTime GetTSStartingDate()
        {
            DateTime now = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Now.StartOfWeek(DayOfWeek.Monday).Day);

            int TSStartDay = 0;
            Int32.TryParse(WebConfigurationManager.AppSettings["TimeSheetStartDay"], out TSStartDay);
            now = now.AddDays(-7);
            while ((int)now.DayOfWeek != TSStartDay)
            {
                now = now.AddDays(-1);
            }


            return now;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TimeSheetUser")]
        public async Task<ActionResult> MyExpenseCreate([Bind(Exclude = "LookupStatus, Comments")]ExpenseViewModel evm, string action)
        {

            try
            {
                

                ExpenseService expenseService = new ExpenseService();
               
                if (!ModelState.IsValid)
                {
                    ViewBag.TSStartDate = WebConfigurationManager.AppSettings["TimeSheetStartDay"];
                    return View(evm);
                }
                
                ExpenseMaster expenseMaster = new ExpenseMaster();

                //expenseMaster = Mapper.Map<ExpenseViewModel,ExpenseMaster>(evm);

                expenseMaster.StartDate = evm.StartDate;
                expenseMaster.EndDate   = evm.EndDate;
                expenseMaster.IsActive = true;
                expenseMaster.IsDeleted = false;
                expenseMaster.CreatedUserId = User.Identity.GetUserId();
                expenseMaster.CreatedDate = DateTime.Now;
                expenseMaster.ModifiedUserId = User.Identity.GetUserId();
                expenseMaster.ModifiedDate = DateTime.Now;
                expenseMaster.LookupStatus = expenseService.GetLookupIdForStatus("Saved", "ExpenseStatus").LookupCodeId;
                expenseMaster.Total = 0;
                expenseMaster.UserId = evm.UserId;
                

                //List<ExpenseSummary> expenseSummaries = new List<ExpenseSummary>();


                ////expenseSummaries = Mapper.Map<List<ExpenseSummaryViewModel>, List<ExpenseSummary>>(evm.ExpensesSummaries);
                //foreach (ExpenseSummary sum in expenseMaster.ExpenseSummaries)
                //{
                //    sum.ExpenseId = expenseMaster.ExpenseId;

                //}
                expenseService.CreateExpense(expenseMaster);
                


                TempData["status"] = "Expense Sheet successfully saved";

                if (string.Equals(action.ToString(), "Save And Submit"))
                {
                    UserService userService = new UserService();
                    var user = userService.GetUserWithId(evm.UserId);
                    var repoMan = user.AspNetUser1;
                    if (repoMan != null)
                    {

                        //ChangeStatusOfTimeSheet(expenseMaster.TimeSheetID, "Pending");
                        //await SendEmailTimeSheetSubmitted(expenseMaster.TimeSheetID);
                        TempData["status"] = "Expense Sheet successfully saved and submitted";

                    }

                    else
                    {
                        TempData["status"] = "Your Expense Sheet is not submitted. You do not have a reporting manager. Please contact your admin.";

                    }
                }

                return RedirectToAction("MyExpenseEdit",new { id = expenseMaster.ExpenseId});


            }


            catch (Exception ex)
            {                
                TempData["error"] = "Something went wrong while adding Expense Sheet";
                
                return RedirectToAction("MyExpenseList");
            }

        }


        [Authorize(Roles = "TimeSheetUser")]
        public ActionResult MyExpenseEdit(int id)
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
            
            //TempData["TSStartDate"] = WebConfigurationManager.AppSettings["TimeSheetStartDay"];


            ExpenseViewModel evm = new ExpenseViewModel();
            //UserService userService = new UserService();
            ExpenseService expenseService = new ExpenseService();

            var expenseMaster = expenseService.GetMyExpense(id,User.Identity.GetUserId());

            if (expenseMaster == null)
            {
                TempData["error"] = "Either this file does not exists or you dont have permission";
                //return RedirectToAction("Index");
                return RedirectToAction("MyExpenseList");
            }

            List<ExpenseSummary> expenseSummaries = new List<ExpenseSummary>();

            expenseSummaries = expenseService.GetExpenseSummaries(id);

            var esList = Mapper.Map<List<ExpenseSummary>, List<ExpenseSummaryViewModel>>(expenseSummaries);

            evm = Mapper.Map<ExpenseMaster, ExpenseViewModel>(expenseMaster);

            evm.ExpensesSummaryList = esList;

            //evm.UserId = User.Identity.GetUserId();
            evm.AspNetUser = expenseMaster.AspNetUser;
            if (expenseMaster.AspNetUser.AspNetUser1 != null)
            {
                evm.ReportingManager = evm.AspNetUser.AspNetUser1.Firstname + " " + evm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                evm.ReportingManager = "N/A";
            }

            evm.ExpenseFilesList = expenseService.GetExpenseAttachments(expenseMaster.ExpenseId);


            return View(evm);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TimeSheetUser")]
        public async Task<ActionResult> MyExpenseEdit([Bind(Exclude = "LookupStatus, Comments")]ExpenseViewModel evm, string action)
        {

            try
            {
                ExpenseService expenseService = new ExpenseService();

                if (!ModelState.IsValid)
                {
                    ViewBag.TSStartDate = WebConfigurationManager.AppSettings["TimeSheetStartDay"];
                    return View(evm);
                }

                ExpenseMaster expenseMaster = expenseService.GetMyExpense(evm.ExpenseId);

                if (evm.ExpensesSummary.Date != null || evm.ExpensesSummary.Total > 0 )
                {
                    ExpenseSummary expenseSummary = new ExpenseSummary();
                    expenseSummary = Mapper.Map<ExpenseSummaryViewModel, ExpenseSummary>(evm.ExpensesSummary);
                    expenseSummary.Total = expenseSummary.MilesDriven + expenseSummary.Transport + expenseSummary.Lodging + expenseSummary.Meals + expenseSummary.Phone + expenseSummary.Other;
                    expenseSummary.ExpenseId = expenseMaster.ExpenseId;
                    expenseService.CreateExpenseSummary(expenseSummary);

                    //expenseMaster.Total += expenseSummary.Total;

                    TempData["status"] = "Expense added successfully";

                }

                expenseMaster.StartDate = evm.StartDate;
                expenseMaster.EndDate = evm.EndDate;
                expenseMaster.Advance = Math.Abs(evm.Advance??0);
                expenseMaster.ModifiedUserId = User.Identity.GetUserId();
                expenseMaster.ModifiedDate = DateTime.Now;
                expenseMaster.LookupCodeMaster = null;
                if(!string.Equals(evm.LookupCodeMaster.LookupCodeName, "Rejected"))
                {
                    expenseMaster.LookupStatus = expenseService.GetLookupIdForStatus("Saved", "ExpenseStatus").LookupCodeId;

                }

                expenseService.UpdateExpense(expenseMaster);

                UpdateExpenseTotal(expenseMaster.ExpenseId);


                TempData["status"] = "Expense sheet saved successfully";


                if (string.Equals(action.ToString(), "Submit"))
                {
                    UserService userService = new UserService();
                    var user = userService.GetUserWithId(evm.UserId);
                    var repoMan = user.AspNetUser1;
                    if (repoMan != null)
                    {
                        ChangeStatusOfExpenseSheet(expenseMaster.ExpenseId, "Pending");
                        await SendEmailExpenseSubmitted(expenseMaster.ExpenseId);
                        TempData["status"] = "Expense Sheet successfully saved and submitted";
                    }

                    else
                    {
                        TempData["status"] = "Your Expense Sheet is not submitted. You do not have a reporting manager. Please contact your admin.";
                    }
                    return RedirectToAction("MyExpenseList");
                }
                else if(string.Equals(action.ToString(), "RejectedSubmit"))
                {
                    ChangeStatusOfExpenseSheet(expenseMaster.ExpenseId, "Pending");
                    TempData["status"] = "Expense successfully saved and submitted ";                   
                }

                return RedirectToAction("MyExpenseEdit", new { id = expenseMaster.ExpenseId });


            }


            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while adding Expense Sheet";
                return RedirectToAction("MyExpenseList");
            }

        }

        public ActionResult GetExpenseSummaryPartialView(int id)
        {
            ExpenseService expenseService = new ExpenseService();
            var expenseSummary = expenseService.GetExpenseSummary(id);
            var esvm = Mapper.Map<ExpenseSummary, ExpenseSummaryViewModel>(expenseSummary);
            return PartialView("_EditExpenseSummaryView", esvm);

        }
        public ActionResult UpdateExpenseSummary(ExpenseSummaryViewModel esvm)
        {
            if(ModelState.IsValid)
            {
                ExpenseService expenseService = new ExpenseService();
                var expenseSummary = Mapper.Map<ExpenseSummaryViewModel, ExpenseSummary>(esvm);
                expenseSummary.Total = expenseSummary.MilesDriven + expenseSummary.Transport + expenseSummary.Lodging + expenseSummary.Meals + expenseSummary.Phone + expenseSummary.Other;
                expenseService.UpdateExpenseSummary(expenseSummary);
                UpdateExpenseTotal(esvm.ExpenseId);
                return null;
            }
            else
            {
                return null;
            }
            
        }

        public void UpdateExpenseTotal(int expenseId)
        {
            ExpenseService expenseService = new ExpenseService();
            var expSummaries = expenseService.GetExpenseSummaries(expenseId);
            decimal total = 0;
            foreach(ExpenseSummary expSum in expSummaries)
            {
                total += expSum.Total;
            }
            var expenseMaster = expenseService.GetJustExpense(expenseId);
            expenseMaster.SubTotal = total;
            total -= expenseMaster.Advance;
            expenseMaster.Total = total;
            expenseService.UpdateExpense(expenseMaster);
        }


        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult EmployeeAdminExpenseEdit(int id)
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
            ViewBag.TSStartDate = WebConfigurationManager.AppSettings["TimeSheetStartDay"];

            //TempData["TSStartDate"] = WebConfigurationManager.AppSettings["TimeSheetStartDay"];


            ExpenseViewModel evm = new ExpenseViewModel();
            //UserService userService = new UserService();
            ExpenseService expenseService = new ExpenseService();

            var expenseMaster = expenseService.GetMyExpense(id,null);

            List<ExpenseSummary> expenseSummaries = new List<ExpenseSummary>();

            expenseSummaries = expenseService.GetExpenseSummaries(id);

            var esList = Mapper.Map<List<ExpenseSummary>, List<ExpenseSummaryViewModel>>(expenseSummaries);

            evm = Mapper.Map<ExpenseMaster, ExpenseViewModel>(expenseMaster);

            evm.ExpensesSummaryList = esList;

            //evm.UserId = User.Identity.GetUserId();
            evm.AspNetUser = expenseMaster.AspNetUser;
            if (expenseMaster.AspNetUser.AspNetUser1 != null)
            {
                evm.ReportingManager = evm.AspNetUser.AspNetUser1.Firstname + " " + evm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                evm.ReportingManager = "N/A";
            }

            evm.ExpenseFilesList = expenseService.GetExpenseAttachments(expenseMaster.ExpenseId);


            return View("EmployeeExpenseEdit", evm);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TimeSheetAdmin")]
        public async Task<ActionResult> EmployeeAdminExpenseEdit([Bind(Exclude = "LookupStatus, Comments")]ExpenseViewModel evm, string action)
        {

            try
            {
                ExpenseService expenseService = new ExpenseService();

                if (!ModelState.IsValid)
                {
                    ViewBag.TSRole = "TimeSheetAdmin";
                    ViewBag.RoleName = "(Admin)";
                    return View("EmployeeExpenseEdit",evm);
                }

                ExpenseMaster expenseMaster = expenseService.GetMyExpense(evm.ExpenseId);

                if (evm.ExpensesSummary.Date != null || evm.ExpensesSummary.Total > 0)
                {
                    ExpenseSummary expenseSummary = new ExpenseSummary();
                    expenseSummary = Mapper.Map<ExpenseSummaryViewModel, ExpenseSummary>(evm.ExpensesSummary);
                    expenseSummary.Total = expenseSummary.MilesDriven + expenseSummary.Transport + expenseSummary.Lodging + expenseSummary.Meals + expenseSummary.Phone + expenseSummary.Other;
                    expenseSummary.ExpenseId = expenseMaster.ExpenseId;
                    expenseService.CreateExpenseSummary(expenseSummary);

                    //expenseMaster.Total += expenseSummary.Total;

                    TempData["status"] = "Expense added successfully";

                }
                expenseMaster.Advance = Math.Abs(evm.Advance??0);
                expenseMaster.StartDate = evm.StartDate;
                expenseMaster.EndDate = evm.EndDate;
                expenseMaster.ModifiedUserId = User.Identity.GetUserId();
                expenseMaster.ModifiedDate = DateTime.Now;
                expenseMaster.LookupCodeMaster = null;
                expenseMaster.LookupStatus = expenseService.GetLookupIdForStatus("Pending", "ExpenseStatus").LookupCodeId;

                expenseService.UpdateExpense(expenseMaster);
                UpdateExpenseTotal(expenseMaster.ExpenseId);


                TempData["status"] = "Expense sheet saved successfully";
                

                return RedirectToAction("EmployeeAdminExpenseEdit", new { id = expenseMaster.ExpenseId });


            }


            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while adding Expense Sheet";
                return RedirectToAction("EmployeeAdminExpensesList");
            }

        }




        [Authorize(Roles = "TimeSheetUser")]
        public ActionResult MyExpenseDetails(int id)
        {

            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }


            //TempData["TSStartDate"] = WebConfigurationManager.AppSettings["TimeSheetStartDay"];


            ExpenseViewModel evm = new ExpenseViewModel();
            //UserService userService = new UserService();
            ExpenseService expenseService = new ExpenseService();

            var expenseMaster = expenseService.GetMyExpense(id, User.Identity.GetUserId());

            if (expenseMaster == null)
            {
                TempData["error"] = "Either this file does not exists or you dont have permission";
                return RedirectToAction("MyExpenseList");
            }

            List<ExpenseSummary> expenseSummaries = new List<ExpenseSummary>();

            expenseSummaries = expenseService.GetExpenseSummaries(id);

            var esList = Mapper.Map<List<ExpenseSummary>, List<ExpenseSummaryViewModel>>(expenseSummaries);

            evm = Mapper.Map<ExpenseMaster, ExpenseViewModel>(expenseMaster);

            evm.ExpensesSummaryList = esList;


            //evm.UserId = User.Identity.GetUserId();
            evm.AspNetUser = expenseMaster.AspNetUser;
            if (expenseMaster.AspNetUser.AspNetUser1 != null)
            {
                evm.ReportingManager = evm.AspNetUser.AspNetUser1.Firstname + " " + evm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                evm.ReportingManager = "N/A";
            }
            evm.ExpenseFilesList = expenseService.GetExpenseAttachments(expenseMaster.ExpenseId);

            return View(evm);

        }

        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult EmployeeAdminExpenseDetails(int id)
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


            //TempData["TSStartDate"] = WebConfigurationManager.AppSettings["TimeSheetStartDay"];


            ExpenseViewModel evm = new ExpenseViewModel();
            //UserService userService = new UserService();
            ExpenseService expenseService = new ExpenseService();

            var expenseMaster = expenseService.GetMyExpense(id, null);

            List<ExpenseSummary> expenseSummaries = new List<ExpenseSummary>();

            expenseSummaries = expenseService.GetExpenseSummaries(id);

            var esList = Mapper.Map<List<ExpenseSummary>, List<ExpenseSummaryViewModel>>(expenseSummaries);

            evm = Mapper.Map<ExpenseMaster, ExpenseViewModel>(expenseMaster);

            evm.ExpensesSummaryList = esList;

            //evm.UserId = User.Identity.GetUserId();
            evm.AspNetUser = expenseMaster.AspNetUser;
            if (expenseMaster.AspNetUser.AspNetUser1 != null)
            {
                evm.ReportingManager = evm.AspNetUser.AspNetUser1.Firstname + " " + evm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                evm.ReportingManager = "N/A";
            }
            evm.ExpenseFilesList = expenseService.GetExpenseAttachments(expenseMaster.ExpenseId);

            return View("EmployeeExpenseDetails",evm);

        }

        [Authorize(Roles = "TimeSheetManager")]
        public ActionResult EmployeeManagerExpenseDetails(int id)
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
            

            ExpenseViewModel evm = new ExpenseViewModel();
            //UserService userService = new UserService();
            ExpenseService expenseService = new ExpenseService();

            var expenseMaster = expenseService.GetMyExpense(id, null);

            List<ExpenseSummary> expenseSummaries = new List<ExpenseSummary>();

            expenseSummaries = expenseService.GetExpenseSummaries(id);

            var esList = Mapper.Map<List<ExpenseSummary>, List<ExpenseSummaryViewModel>>(expenseSummaries);

            evm = Mapper.Map<ExpenseMaster, ExpenseViewModel>(expenseMaster);

            evm.ExpensesSummaryList = esList;

            //evm.UserId = User.Identity.GetUserId();
            evm.AspNetUser = expenseMaster.AspNetUser;
            if (expenseMaster.AspNetUser.AspNetUser1 != null)
            {
                evm.ReportingManager = evm.AspNetUser.AspNetUser1.Firstname + " " + evm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                evm.ReportingManager = "N/A";
            }
            evm.ExpenseFilesList = expenseService.GetExpenseAttachments(expenseMaster.ExpenseId);

            return View("EmployeeExpenseDetails",evm);

        }

        [Authorize(Roles = "TimeSheetReportingManager")]
        public ActionResult EmployeeRepoManExpenseDetails(int id)
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
            // string Employeename = HttpContext.User.Identity.Name;
            ExpenseViewModel evm = new ExpenseViewModel();
            //UserService userService = new UserService();
            ExpenseService expenseService = new ExpenseService();
            //var loa = loaService.GetLOA(id, User.Identity.GetUserId());
            var expenseMaster = expenseService.GetExpenseForRepoManager(id, User.Identity.GetUserId());
            if (expenseMaster == null)
            {
                TempData["error"] = "Either this file does not exists or you dont have permission";
                //return RedirectToAction("Index");
                return RedirectToAction("EmployeeRepoManExpensesList");
            }


            List<ExpenseSummary> expenseSummaries = new List<ExpenseSummary>();

            expenseSummaries = expenseService.GetExpenseSummaries(id);

            var esList = Mapper.Map<List<ExpenseSummary>, List<ExpenseSummaryViewModel>>(expenseSummaries);

            evm = Mapper.Map<ExpenseMaster, ExpenseViewModel>(expenseMaster);

            evm.ExpensesSummaryList = esList;
            
            evm.AspNetUser = expenseMaster.AspNetUser;

            if (expenseMaster.AspNetUser.AspNetUser1 != null)
            {
                evm.ReportingManager = evm.AspNetUser.AspNetUser1.Firstname + " " + evm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                evm.ReportingManager = "N/A";
            }
            evm.ExpenseFilesList = expenseService.GetExpenseAttachments(expenseMaster.ExpenseId);

            return View("EmployeeExpenseDetails", evm);
        }


        //[Authorize(Roles = "TimeSheetReportingManager")]
        //public ActionResult EmployeeRepoManExpenseDetails(int id)
        //{

        //    if (TempData["error"] != null)
        //    {
        //        ViewBag.error = TempData["error"];

        //    }
        //    if (TempData["status"] != null)
        //    {
        //        ViewBag.status = TempData["status"];
        //    }


        //    ExpenseViewModel evm = new ExpenseViewModel();
        //    //UserService userService = new UserService();
        //    ExpenseService expenseService = new ExpenseService();

        //    var expenseMaster = expenseService.GetExpenseForRepoManager(id, User.Identity.GetUserId());

        //    evm = Mapper.Map<ExpenseMaster, ExpenseViewModel>(expenseMaster);


        //    //evm.UserId = User.Identity.GetUserId();
        //    evm.AspNetUser = expenseMaster.AspNetUser;
        //    if (expenseMaster.AspNetUser.AspNetUser1 != null)
        //    {
        //        evm.ReportingManager = evm.AspNetUser.AspNetUser1.Firstname + " " + evm.AspNetUser.AspNetUser1.LastName;
        //    }
        //    else
        //    {
        //        evm.ReportingManager = "N/A";
        //    }
        //    evm.ExpenseFilesList = expenseService.GetExpenseAttachments(expenseMaster.ExpenseId);

        //    return View("EmployeeExpenseDetails", evm);

        //}



        public ActionResult GetExpensesSummaries(int expenseId)
        {
            ExpenseService expenseService = new ExpenseService();

            List<ExpenseSummary> expenseSummaries = new List<ExpenseSummary>();

            expenseSummaries = expenseService.GetExpenseSummaries(expenseId);

            var esList = Mapper.Map<List<ExpenseSummary>, List<ExpenseSummaryViewModel>>(expenseSummaries);
           
            return PartialView("_ExpenseSummaryListView", esList);
        }

        public ActionResult GetExpensesSummariesForDetails(int expenseId)
        {
            ExpenseService expenseService = new ExpenseService();

            List<ExpenseSummary> expenseSummaries = new List<ExpenseSummary>();

            expenseSummaries = expenseService.GetExpenseSummaries(expenseId);

            var esList = Mapper.Map<List<ExpenseSummary>, List<ExpenseSummaryViewModel>>(expenseSummaries);

            return PartialView("_ExpenseSummaryListViewReadOnly", esList);
        }


        private void ChangeStatusOfExpenseSheet(int id, string status)
        {
            ExpenseService expenseService = new ExpenseService();
            var expense = expenseService.GetJustExpense(id);
            expense.LookupStatus = expenseService.GetLookupIdForStatus(status, "ExpenseStatus").LookupCodeId;
            expense.ModifiedDate = DateTime.Now;
            expense.ModifiedUserId = User.Identity.GetUserId();
            expenseService.UpdateExpense(expense);
        }



        [Authorize(Roles = "TimeSheetReportingManager")]
        public ActionResult EmployeeRepoManExpensesList()
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

            return View("EmployeeExpenseSheetList");
        }

        [Authorize(Roles = "TimeSheetReportingManager")]
        public ActionResult GetEmployeeRepoManPendingExpensesList()
        {
            ExpenseService expenseService = new ExpenseService();

            //List<Expense> expenses = new List<Expense>();

           var expenses = expenseService.GetReportingManagerPendingExpense(User.Identity.GetUserId());

            
            var expensesList = Mapper.Map<List<ExpenseMaster>, List<ExpenseListViewModel>>(expenses);

            ViewBag.TSRole = "TimeSheetReportingManager";

            return PartialView("_EmployeePendingExpensesList", expensesList);
        }

        [Authorize(Roles = "TimeSheetReportingManager")]
        public ActionResult GetEmployeeRepoManRemainingExpensesList()
        {
            ExpenseService expenseService = new ExpenseService();

            //List<LOA> loas = new List<LOA>();


            var expenses = expenseService.GetReportingManagerRemainingExpense(User.Identity.GetUserId());


            var expensesList = Mapper.Map<List<ExpenseMaster>, List<ExpenseListViewModel>>(expenses);

            ViewBag.TSRole = "TimeSheetReportingManager";

            return PartialView("_EmployeeRemainingExpensesList", expensesList);
        }

        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult EmployeeAdminExpensesList()
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

            return View("EmployeeExpenseSheetList");
        }


        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult GetEmployeeAdminPendingExpensesList()
        {
            ExpenseService expenseService = new ExpenseService();

            //List<LOA> loas = new List<LOA>();

            var expenses = expenseService.GetEmployeePendingExpense();


            var expensesList = Mapper.Map<List<ExpenseMaster>, List<ExpenseListViewModel>>(expenses);


            ViewBag.TSRole = "TimeSheetAdmin";

            return PartialView("_EmployeePendingExpensesList", expensesList);
        }


        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult GetEmployeeAdminRemainingExpensesList()
        {
            ExpenseService expenseService = new ExpenseService();

            //List<LOA> loas = new List<LOA>();

            var expenses = expenseService.GetEmployeeRemainingExpense();


            var expensesList = Mapper.Map<List<ExpenseMaster>, List<ExpenseListViewModel>>(expenses);

            ViewBag.TSRole = "TimeSheetAdmin";

            return PartialView("_EmployeeRemainingExpensesList", expensesList);
        }

        [Authorize(Roles = "TimeSheetManager")]
        public ActionResult EmployeeManagerExpensesList()
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
            return View("EmployeeExpenseSheetList");
        }

        [Authorize(Roles = "TimeSheetManager")]
        public ActionResult GetEmployeeManagerPendingExpensesList()
        {
            ExpenseService expenseService = new ExpenseService();

            //List<LOA> loas = new List<LOA>();

            var expenses = expenseService.GetEmployeePendingExpense();


            var expensesList = Mapper.Map<List<ExpenseMaster>, List<ExpenseListViewModel>>(expenses);


            ViewBag.TSRole = "TimeSheetManager";

            return PartialView("_EmployeePendingExpensesList", expensesList);
        }

        [Authorize(Roles = "TimeSheetManager")]
        public ActionResult GetEmployeeManagerRemainingExpensesList()
        {
            ExpenseService expenseService = new ExpenseService();

            //List<LOA> loas = new List<LOA>();

            var expenses = expenseService.GetEmployeeRemainingExpense();


            var expensesList = Mapper.Map<List<ExpenseMaster>, List<ExpenseListViewModel>>(expenses);

            ViewBag.TSRole = "TimeSheetManager";

            return PartialView("_EmployeeRemainingExpensesList", expensesList);
        }

        [Authorize(Roles = "TimeSheetReportingManager")]
        public ActionResult Approve(int id)
        {
            try
            {
               
                ExpenseService expenseService = new ExpenseService();
                var expense = expenseService.GetExpenseForRepoManager(id,User.Identity.GetUserId());
                if (expense == null)
                {
                    TempData["error"] = "Expense Sheet does not exist or you dont have permissions";
                    return RedirectToAction("EmployeeRepoManExpensesList");
                }
                var userId = User.Identity.GetUserId();
                UserService userService = new UserService();
                var user = userService.GetUserWithId(userId);
                expense.ApprovedBy = user.Firstname + " " + user.LastName + " (" + user.EmployeeCode + ")";
                expense.ApprovedDate = DateTime.Now;
                expenseService.UpdateExpense(expense);

                ChangeStatusOfExpenseSheet(id, "Approved");

                TempData["status"] = "Expense Sheet has been approved";
                return RedirectToAction("EmployeeRepoManExpensesList");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while Approving";
                return RedirectToAction("EmployeeRepoManExpensesList");
            }
        }

        [Authorize(Roles = "TimeSheetReportingManager")]
        public async Task<ActionResult> Reject(ExpenseViewModel evm)
        {
            try
            {
                //if(string.IsNullOrEmpty(tsvm.TempComments))
                //{
                //    return Json(new { HasErrors= true, Errors = "Comments are Mandatory"});
                //}

                ExpenseService expenseService = new ExpenseService();
                var expense = expenseService.GetExpenseForRepoManager(evm.ExpenseId, User.Identity.GetUserId());
                if (expense == null)
                {
                    TempData["error"] = "Expense Sheet does not exist or you dont have permissions";
                    return RedirectToAction("EmployeeRepoManExpensesList");
                }
                //expense.LookupStatus = expenseService.GetLookupIdForStatus("Rejected", "LoaStatus").LookupCodeId;
                expense.Comments += " " + evm.TempComments + " (" + DateTime.Now.ToString() + "), <br> ";
                expense.ModifiedDate = DateTime.Now;
                expense.ModifiedUserId = User.Identity.GetUserId();
                expenseService.UpdateExpense(expense);
                ChangeStatusOfExpenseSheet(evm.ExpenseId, "Rejected");

                await SendEmailExpenseRejected(evm.ExpenseId);

                TempData["status"] = "Expense Sheet has been rejected and mail has been sent to employee";
                return null;


            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while Rejecting";
                return RedirectToAction("EmployeeRepoManExpensesList");
            }
        }

        public async Task SendEmailExpenseRejected(int ExpenseId, bool includeAttachment = false)
        {
            try
            {
                ExpenseService expenseService = new ExpenseService();
                var expense = expenseService.GetMyExpense(ExpenseId);

                var subject = "Expense Sheet Rejected";
                var callbackUrl = Url.Action("MyExpenseEdit", "Expense", new { id = ExpenseId }, protocol: Request.Url.Scheme);
                var body = "Your expense sheet from " + expense.StartDate.ToString("MM/dd/yyyy") + "-" + expense.EndDate.ToString("MM/dd/yyyy") + "" +
                    "was Not Accepted with comments '" + expense.Comments + "' please click <a href=\"" + callbackUrl + "\">here</a> to review Expense Sheet";

                //var destinationEmail = WebConfigurationManager.AppSettings["HREmail"];
                var destinationEmail = expense.AspNetUser.Email;
                IdentityMessage im = CreateMessage(destinationEmail, subject, body);

                if (includeAttachment)
                {
                    //byte[] bytes = GetExcelByteDate(timeSheet);
                    //await SendEmailWithAttachment(im, bytes, subject);
                    await SendEmailWithAttachment(im, null, null);

                }
                else
                {
                    await SendEmailWithAttachment(im, null, null);
                }


                TempData["status"] = "Expense Sheet rejection mail sent to employee";
                //return RedirectToAction("EmployeeTimeSheetsList");

            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while sending rejection mail";

            }
        }


        [Authorize(Roles = "TimeSheetUser")]
        public ActionResult MyExpenseDownload(int id)
        {
            try
            {
                ExpenseService expenseService = new ExpenseService();
                var expense = expenseService.GetMyExpense(id, User.Identity.GetUserId());
                if (expense == null)
                {
                    TempData["error"] = "Either this file does not exists or you dont have permission";
                    return RedirectToAction("MyExpenseList");
                }
                var expSummaries = expenseService.GetExpenseSummaries(expense.ExpenseId);
                var wb = GenerateExpenseExcelFile(expense, expSummaries);
                var fileName = GetExcelFileName(expense);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    //return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while downloading";
                //return RedirectToAction("Index");
                return RedirectToAction("MyExpenseList");
            }
        }

        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult EmployeeAdminExpenseDownload(int id)
        {
            try
            {
                ExpenseService expenseService = new ExpenseService();
                var expense = expenseService.GetMyExpense(id,null);
                if (expense == null)
                {
                    TempData["error"] = "Either this file does not exists or you dont have permission";
                    return RedirectToAction("EmployeeAdminExpensesList");
                }
                var expSummaries = expenseService.GetExpenseSummaries(expense.ExpenseId);
                var wb = GenerateExpenseExcelFile(expense, expSummaries);
                var fileName = GetExcelFileName(expense);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    //return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while downloading";
                //return RedirectToAction("Index");
                return RedirectToAction("EmployeeAdminExpensesList");
            }
        }

        [Authorize(Roles = "TimeSheetManager")]
        public ActionResult EmployeeManagerExpenseDownload(int id)
        {
            try
            {
                ExpenseService expenseService = new ExpenseService();
                var expense = expenseService.GetMyExpense(id, null);
                if (expense == null)
                {
                    TempData["error"] = "Either this file does not exists or you dont have permission";
                    return RedirectToAction("EmployeeManagerExpensesList");
                }
                var expSummaries = expenseService.GetExpenseSummaries(expense.ExpenseId);
                var wb = GenerateExpenseExcelFile(expense, expSummaries);
                var fileName = GetExcelFileName(expense);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    //return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while downloading";
                //return RedirectToAction("Index");
                return RedirectToAction("EmployeeManagerExpensesList");
            }
        }

        [Authorize(Roles = "TimeSheetReportingManager")]
        public ActionResult EmployeeRepoManExpenseDownload(int id)
        {
            try
            {
                ExpenseService expenseService = new ExpenseService();
                var expense = expenseService.GetExpenseForRepoManager(id, User.Identity.GetUserId());
                if (expense == null)
                {
                    TempData["error"] = "Either this file does not exists or you dont have permission";
                    return RedirectToAction("EmployeeRepoManExpensesList");
                }
                var expSummaries = expenseService.GetExpenseSummaries(expense.ExpenseId);
                var wb = GenerateExpenseExcelFile(expense, expSummaries);
                var fileName = GetExcelFileName(expense);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    //return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while downloading";
                //return RedirectToAction("Index");
                return RedirectToAction("EmployeeRepoManExpensesList");
            }
        }

        private XLWorkbook GenerateExpenseExcelFile(ExpenseMaster expense, List<ExpenseSummary> expenseSummaries)
        {
            try
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("Expenses");

                    //ws.Style.Fill.BackgroundColor = XLColor.White;

                    ws.Cell("B2").Value = "Employee code";
                    ws.Cell("B2").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    ws.Cell("C2").Value = expense.AspNetUser.EmployeeCode;
                    ws.Cell("C2").Style.Fill.BackgroundColor = XLColor.Beige;

                    ws.Cell(3, 2).Value = "Employee Name";
                    ws.Cell(3, 2).Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    ws.Cell(3, 3).Value = expense.AspNetUser.Firstname + ' ' + expense.AspNetUser.LastName;
                    ws.Cell(3, 3).Style.Fill.BackgroundColor = XLColor.Beige;

                    ws.Cell("E2").Value = "Start Date";
                    ws.Cell("E2").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    ws.Cell("F2").Value = expense.StartDate.ToString("MM/dd/yyyy");
                    ws.Cell("F2").Style.Fill.BackgroundColor = XLColor.Beige;

                    ws.Cell("E3").Value = "End Date";
                    ws.Cell("E3").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    ws.Cell("F3").Value = expense.EndDate.ToString("MM/dd/yyyy");
                    ws.Cell("F3").Style.Fill.BackgroundColor = XLColor.Beige;

                    ws.Cell("H2").Value = "Expense Sheet Code";
                    ws.Cell("H2").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    ws.Cell("I2").Value = expense.ExpenseCode;
                    ws.Cell("I2").Style.Fill.BackgroundColor = XLColor.Beige;

                    ws.Cell("H3").Value = "Reporting Manger";
                    ws.Cell("H3").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    if (expense.AspNetUser.AspNetUser1 != null)
                    {
                        ws.Cell("I3").Value = expense.AspNetUser.AspNetUser1.Firstname + ' ' + expense.AspNetUser.AspNetUser1.LastName;
                    }
                    else
                    {
                        ws.Cell("I3").Value = "N/A";
                    }

                    ws.Cell("I3").Style.Fill.BackgroundColor = XLColor.Beige;

                    ws.Cell("K2").Value = "Expense Sheet Status";
                    ws.Cell("K2").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    ws.Cell("L2").Value = expense.LookupCodeMaster.LookupCodeName;
                    ws.Cell("L2").Style.Fill.BackgroundColor = XLColor.Beige;

                    ws.Cell("K3").Value = "Total Expenses";
                    ws.Cell("K3").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    ws.Cell("L3").Value = string.Format("{0:c}", expense.Total) ;
                    ws.Cell("L3").Style.Fill.BackgroundColor = XLColor.Beige;


                    //ws.Cell("B17").Value = "Comments :";

                    //if (expense.Comments != null)
                    //{
                    //    string str = expense.Comments;
                    //    List<string> cmts = str.Split(new[] { "<br>" }, StringSplitOptions.None).ToList();

                    //    var rangeWithStrings = ws.Cell("B18").InsertData(cmts).Style.Alignment.SetWrapText(false);
                    //}
                    //var dates = new List<DateTime>();

                    //for (var dt = timeSheet.StartDate; dt <= timeSheet.EndDate; dt = dt.AddDays(1))
                    //{
                    //    dates.Add(dt);
                    //}

                    DataTable expenses = new DataTable();

                    expenses.Columns.AddRange(new DataColumn[11]);

                    expenses.Columns.AddRange(new DataColumn[11] { new DataColumn("Date"),
                                            new DataColumn("Description"),
                                            new DataColumn("Miles Driven"),
                                            new DataColumn(),
                                            new DataColumn(),
                                            new DataColumn("Transport"),
                                            new DataColumn("Lodging"),
                                            new DataColumn("Meals"),
                                            new DataColumn("Phone/Fax"),
                                            new DataColumn("Other"),
                                            new DataColumn("Total") });
                    //expenses.Columns.AddRange(new DataColumn[11] { new DataColumn(""),
                    //                        new DataColumn(""),
                    //                        new DataColumn("Miles"),
                    //                        new DataColumn("Multiply"),
                    //                        new DataColumn("Total"),
                    //                        new DataColumn(""),
                    //                        new DataColumn(""),
                    //                        new DataColumn(""),
                    //                        new DataColumn(""),
                    //                        new DataColumn(""),
                    //                        new DataColumn("") });
                    //expenses.Rows.Add("Date", "Description", "Miles Driven", "", "", "Transport", "Lodging", "Meals", "Phone/Fax", "Other", "Total");
                    expenses.Rows.Add("", "", "Miles", "Multiply", "Total", "", "", "", "", "", "");


                    foreach (var exp in expenseSummaries)
                    {
                        string date = exp.Date.HasValue ? exp.Date.Value.ToString("MM/dd/yyyy") : string.Empty; 
                        //decimal projectTotal = exp.Day1Hours + summary.Day2Hours + summary.Day3Hours + summary.Day4Hours + summary.Day5Hours + summary.Day6Hours + summary.Day7Hours;
                        expenses.Rows.Add(date, exp.Description, exp.Miles, exp.MilesMultiplicationFactor, string.Format("{0:c}", exp.Total), string.Format("{0:c}", exp.Transport) , string.Format("{0:c}", exp.Lodging), string.Format("{0:c}", exp.Meals), string.Format("{0:c}", exp.Phone) , string.Format("{0:c}", exp.Other) , string.Format("{0:c}", exp.Total));                       
                    }

                    //summaries.Rows.Add("", "", "LeaveType", timeSheet.LookupCodeMaster.LookupCodeName, timeSheet.LookupCodeMaster1.LookupCodeName, timeSheet.LookupCodeMaster2.LookupCodeName, timeSheet.LookupCodeMaster3.LookupCodeName, timeSheet.LookupCodeMaster4.LookupCodeName, timeSheet.LookupCodeMaster5.LookupCodeName, timeSheet.LookupCodeMaster6.LookupCodeName, "");

                    //summaries.Rows.Add("", "", "RegularHours", timeSheet.Day1RegularHours, timeSheet.Day2RegularHours, timeSheet.Day3RegularHours, timeSheet.Day4RegularHours, timeSheet.Day5RegularHours, timeSheet.Day6RegularHours, timeSheet.Day7RegularHours, timeSheet.RegularHours);
                    //summaries.Rows.Add("", "", "Over Time Hours", timeSheet.Day1OverHours, timeSheet.Day2OverHours, timeSheet.Day3OverHours, timeSheet.Day4OverHours, timeSheet.Day5OverHours, timeSheet.Day6OverHours, timeSheet.Day7OverHours, timeSheet.OverTimeHours);
                    //summaries.Rows.Add("", "", "Total Hours", timeSheet.Day1RegularHours + timeSheet.Day1OverHours,
                    //    timeSheet.Day2RegularHours + timeSheet.Day2OverHours,
                    //    timeSheet.Day3RegularHours + timeSheet.Day3OverHours,
                    //    timeSheet.Day4RegularHours + timeSheet.Day4OverHours,
                    //    timeSheet.Day5RegularHours + timeSheet.Day5OverHours,
                    //    timeSheet.Day6RegularHours + timeSheet.Day6OverHours,
                    //    timeSheet.Day7RegularHours + timeSheet.Day7OverHours, timeSheet.TotalHours);


                    ws.Cell(5, 2).InsertTable(expenses.AsEnumerable(), false);

                    ws.Range("D5", "F5").Merge();

                    //ws.Range("D5", "L5").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    //ws.Range("B5", "D5").Style.Fill.BackgroundColor = XLColor.Beige;
                    //ws.Range("B11", "K11").Style.Fill.BackgroundColor = XLColor.Beige;
                    //ws.Range("B12", "K12").Style.Fill.BackgroundColor = XLColor.Beige;
                    //ws.Range("B13", "K13").Style.Fill.BackgroundColor = XLColor.Beige;
                    //ws.Range("B14", "K14").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    //ws.Range("L5", "L14").Style.Fill.BackgroundColor = XLColor.BabyBlue;
                    //ws.Range("L5", "L14").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    //ws.Range("B5", "L14").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                    //ws.Range("L5", "L14").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    //ws.Range("B5", "L14").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    //ws.Range("B5", "B12").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //ws.Range("C5", "C12").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //ws.Range("D5", "D14").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //ws.Range("E5", "E14").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //ws.Range("F5", "F14").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //ws.Range("G5", "G14").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //ws.Range("H5", "H14").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //ws.Range("I5", "I14").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //ws.Range("J5", "J14").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //ws.Range("I5", "I14").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    //ws.Range("B14", "L14").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    //ws.Range("B5", "L5").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    //ws.Range("B10", "L10").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                    //ws.Range("B5", "L5").Style.Font.Bold = true;
                    //ws.Range("B14", "K14").Style.Font.Bold = true;
                    //ws.Range("D13", "D15").Style.Font.Bold = true;
                    //ws.Range("B5", "K5").Style.Font.Bold = true;
                    //ws.Range("B11", "D11").Merge();
                    //ws.Range("B11", "D11").Value = "Leave Type";
                    //ws.Range("B12", "D12").Merge();
                    //ws.Range("B12", "D12").Value = "Regular Hours";
                    //ws.Range("B13", "D13").Merge();
                    //ws.Range("B13", "D13").Value = "Over Time Hours";
                    //ws.Range("B14", "D14").Merge();
                    //ws.Range("B14", "D14").Value = "Total";
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


        private string GetExcelFileName(ExpenseMaster expense)
        {
            string fileName = expense.AspNetUser.Firstname + expense.AspNetUser.LastName + "-" + expense.ExpenseCode+ ".xlsx";
            return fileName;

        }


        public async Task SendEmailExpenseSubmitted(int expenseId, bool includeAttachment = false)
        {
            try
            {
                ExpenseService expenseService = new ExpenseService();
                //timeSheetService.ChangeTimeSheetStatus(timeSheetId, "Pending");
                var expense = expenseService.GetMyExpense(expenseId);

                var subject = "Expense Submitted " + expense.ExpenseCode;




                //var fileName = GetExcelFileName(timeSheet);
                var callbackUrl = Url.Action("EmployeeRepoManExpenseDetails", "Expense", new { id = expenseId }, protocol: Request.Url.Scheme);
                var body = "Your Employee " + expense.AspNetUser.Firstname + " " + expense.AspNetUser.LastName + " submitted expense " +
                    "sheet between dates " + expense.StartDate.ToString("MM/dd/yyyy") + "-" + expense.EndDate.ToString("MM/dd/yyyy") + "" +
                    ", You can check it by clicking <a href=\"" + callbackUrl + "\">here</a>";

                //var destinationEmail = WebConfigurationManager.AppSettings["HREmail"];
                var destinationEmail = expense.AspNetUser.AspNetUser1.Email;
                IdentityMessage im = CreateMessage(destinationEmail, subject, body);

                if (includeAttachment)
                {
                    //byte[] bytes = GetExcelByteDate(timeSheet);
                    //await SendEmailWithAttachment(im, bytes, subject);
                    await SendEmailWithAttachment(im, null, null);
                }
                else
                {
                    await SendEmailWithAttachment(im, null, null);
                }


                TempData["status"] = "Expense Sheet submitted to your Reporting Manager";
                //return RedirectToAction("MyTimeSheetsList");

            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while sending email to your reporting manager";
                //return RedirectToAction("Index");
                //return View("MyTimeSheetsList");
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
            if (attName != null)
            {
                Attachment at = new Attachment(new MemoryStream(bytes), attName);
                mailMessage.Attachments.Add(at);
            }
            
            await client.SendMailAsync(mailMessage);
        }


        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult HardDeleteAdmin(int id)
        {
            try
            {
                ExpenseService expenseService = new ExpenseService();
                var expense = expenseService.GetJustExpense(id);
                if (expense == null)
                {
                    TempData["status"] = "Either this file does not exists or you dont have permissions";
                    return RedirectToAction("EmployeeAdminExpensesList");
                }
                var expenseAttachments = expenseService.GetExpenseAttachments(id);
                foreach (ExpenseAttachment s in expenseAttachments)
                {
                    RemoveImg(s.Id, s.FileName, s.ExpenseId);
                }
               
                expenseService.HardDeleteExpense(id);

                TempData["status"] = "Expense " + expense.ExpenseCode + " is deleted";
                return RedirectToAction("EmployeeAdminExpensesList");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while deleting";
                return RedirectToAction("EmployeeAdminExpensesList");
            }

        }

        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult SoftDeleteAdmin(int id)
        {
            try
            {
                ExpenseService expenseService = new ExpenseService();
                var expense = expenseService.GetJustExpense(id);
                if(expense == null)
                {
                    TempData["status"] = "Either this file does not exists or you dont have permissions";
                    return RedirectToAction("EmployeeAdminExpensesList");
                }
                expense.IsDeleted = true;
                expense.IsActive = false;
                expenseService.UpdateExpense(expense);

                TempData["status"] = "Expense "+ expense .ExpenseCode + " is deleted";
                return RedirectToAction("EmployeeAdminExpensesList");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while deleting";
                return RedirectToAction("EmployeeAdminExpensesList");
            }

        }

        [Authorize(Roles = "TimeSheetUser")]
        public ActionResult HardDelete(int id)
        {
            try
            {
                ExpenseService expenseService = new ExpenseService();
                var isDeleted = expenseService.CheckForDelete(id, User.Identity.GetUserId());
                if(isDeleted)
                {
                    var expenseAttachments = expenseService.GetExpenseAttachments(id);
                    foreach (ExpenseAttachment s in expenseAttachments)
                    {
                        RemoveImg(s.Id, s.FileName, s.ExpenseId);
                    }

                    expenseService.HardDeleteExpense(id);
                }
                else
                {
                    TempData["status"] = "Either this file does not exists or you dont have permissions";
                    return RedirectToAction("MyExpenseList");
                }

                

                return RedirectToAction("MyExpenseList");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while deleting";
                return RedirectToAction("MyExpenseList");
            }

        }

        [Authorize(Roles = "TimeSheetUser")]
        public ActionResult SoftDelete(int id)
        {
            try
            {
                ExpenseService expenseService = new ExpenseService();
                var isDeleted = expenseService.CheckForDelete(id, User.Identity.GetUserId());
                if (isDeleted)
                {
                    var expense = expenseService.GetJustMyExpense(id, User.Identity.GetUserId());
                    expense.IsDeleted = true;
                    expense.IsActive = false;
                    expenseService.UpdateExpense(expense);
                }
                else
                {
                    TempData["status"] = "Either this file does not exists or you dont have permissions";
                    return RedirectToAction("MyExpenseList");
                }
                

                TempData["status"] = "Expense is deleted";
                return RedirectToAction("MyExpenseList");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while deleting";
                return RedirectToAction("MyExpenseList");
            }

        }


        public ActionResult EditDocumentUpload(ExpenseViewModel evm, HttpPostedFileBase[] FILEPhoto)
        {
            string Name = string.Empty;
            string strImageLink = string.Empty;
            string ext = string.Empty;
            int sid = default(int);
            sid = evm.ExpenseId;
            ExpenseService expenseService = new ExpenseService();
            //Upload Files
            foreach (HttpPostedFileBase varfile in FILEPhoto)
            {
                if (varfile != null && varfile.ContentLength > 0)
                {
                    ExpenseAttachment expenseDoc = new ExpenseAttachment()
                    {
                        ExpenseId = evm.ExpenseId,
                        FileServerPath = ConfigurationManager.AppSettings["ExpenseAttachments"] + evm.ExpenseId.ToString() + "/",
                        FileName = varfile.FileName,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedUserId = User.Identity.GetUserId(),
                        CreatedDate = DateTime.Now,
                        ModifiedUserId = User.Identity.GetUserId(),
                        ModifiedDate = DateTime.Now
                    };
                    expenseService.SetExpenseAttachment(expenseDoc);
                    string subDirectory = Server.MapPath(expenseDoc.FileServerPath);
                    System.IO.Directory.CreateDirectory(subDirectory);
                    var newFileName = Path.Combine(subDirectory, expenseDoc.FileName);
                    varfile.SaveAs(newFileName);

                }
            }
            
            return RedirectToAction("MyExpenseEdit", "Expense", new { id = sid });
        }

        public ActionResult FileExists(string imgname, string expenseId)
        {
            if (!string.IsNullOrEmpty(imgname))
            {
                try
                {
                    // var fileId = Guid.Parse(id);
                    byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~" + ConfigurationManager.AppSettings["ExpenseAttachments"] + expenseId.ToString() + "/" + imgname));
                    //var myFile = Server.MapPath("~" + ConfigurationManager.AppSettings["SupportAttachments"] + supportId.ToString() + imgname);

                    if (fileBytes != null)
                    {
                        //byte[] fileBytes = myFile.FileData;
                        // return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, myFile.FileName);
                        return Json(
             new { isFileExists = true },
             JsonRequestBehavior.AllowGet
         );
                    }
                }
                catch (Exception ex)
                {
                    return Json(
         new { isFileExists = false },
         JsonRequestBehavior.AllowGet
     );
                }
            }
            return Json(
          new { isFileExists = false },
          JsonRequestBehavior.AllowGet
      );

        }

        [HttpGet]
        public FileResult GetImage(string imgname, string expenseId)
        {
            if (!string.IsNullOrEmpty(imgname))
            {
                try
                {
                    // var fileId = Guid.Parse(id);
                    byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~" + ConfigurationManager.AppSettings["ExpenseAttachments"] + expenseId.ToString() + "/" + imgname));
                    //var myFile = Server.MapPath("~" + ConfigurationManager.AppSettings["SupportAttachments"] + supportId.ToString() + imgname);

                    if (fileBytes != null)
                    {
                        //byte[] fileBytes = myFile.FileData;
                        // return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, myFile.FileName);
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, imgname);
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return null;
        }


        [HttpPost]
        public ActionResult RemoveImg(int imgid, string imgname, int expenseId)
        {
            string imgresult = string.Empty;
            bool isDeleted = false;
            ExpenseService expenseService = new ExpenseService();
            isDeleted = expenseService.DeleteExpenseDocumentFile(imgid);
            if (isDeleted)
            {
                //Remove file from Phyical Location
                string newFileName = Path.Combine(Server.MapPath("~" + ConfigurationManager.AppSettings["ExpenseAttachments"] + expenseId.ToString()), imgname);
                if (System.IO.File.Exists(newFileName))
                {
                    System.IO.File.Delete(newFileName);
                }
                imgresult = "File Removed Successfully!";
            }
            else
            {
                imgresult = "File Not Removed";
            }

            return Json(imgresult, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult RemoveExpenseSummary(int id)
        {
            string result = "";
            ExpenseService expenseService = new ExpenseService();            
            var isDeleted = expenseService.DeleteExpenseSummary(id);
            if (isDeleted)
            {                
                result = "Expense removed successfully!";
            }
            else
            {
                result = "Expense not removed";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}