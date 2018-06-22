using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using QuickSearchWeb.Models;
using QuickSearchBusiness.Services;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace QuickSearchWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        
        {
            List<DashboardViewModel> dashboardItems = new List<DashboardViewModel>();
            List<DashboardViewModel> items = new List<DashboardViewModel>();
            var viewModel = new DashboardViewModels();

            SupportService supportService = new SupportService();
            TimeSheetService timesheetService = new TimeSheetService();
            ExpenseService expenseService = new ExpenseService();
            LOAService loaService = new LOAService();

            var roles = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);

            if (roles.Contains("TimeSheetAdmin"))
            {
                viewModel.AdminPendingExpensesCount = expenseService.GetPendingExpensesCount();
                viewModel.AdminPendingLOAsCount = loaService.GetPendingLOAsCount();
                viewModel.AdminPendingTimesheetsCount = timesheetService.GetPendingTimesheetsCount();

                //    viewModel.DasboardList.AddRange(ExpenseModuleAdmin());
            }
            else if (roles.Contains("TimeSheetManager"))
            {
                viewModel.ManagerPendingTimesheetsCount = timesheetService.GetPendingTimesheetsCount();
                viewModel.MangerPendingExpensesCount = expenseService.GetPendingExpensesCount();
                viewModel.MangerPendingLOAsCount = loaService.GetPendingLOAsCount();
                //    viewModel.DasboardList.AddRange(TimeSheetModuleManager());
                //    viewModel.DasboardList.AddRange(LOAModuleManager());
                //    items.AddRange(ExpenseModuleManager());
            }

            if (roles.Contains("TimeSheetReportingManager"))
            {

                viewModel.ReportingManagerPendingExpensesCount = expenseService.GetPendingExpensesCount(User.Identity.GetUserId());
                viewModel.ReportingManagerPendingLOAsCount = loaService.GetPendingLOAsCount(User.Identity.GetUserId());
                viewModel.ReportingManagerPendingTimesheetsCount = timesheetService.GetPendingTimesheetsCount(User.Identity.GetUserId());
                //    viewModel.DasboardList.AddRange(TimeSheetModuleReportingManager());
                //    viewModel.DasboardList.AddRange(LOAModuleReportingManager());
                //    viewModel.DasboardList.AddRange(ExpenseModuleReportingManager());
            }

            if (roles.Contains("SupportUser"))
            {
                //viewModel.DasboardList.AddRange(SupportModuleUserPendingRequest());
                //viewModel.DasboardList.AddRange(SupportModuleUserDoneRequest());
                //viewModel.DasboardList.AddRange(SupportModuleUserClosedRequest());
                viewModel.MyPendingRequestsCount = supportService.GetMyPendingRequestsCount(User.Identity.GetUserId());
                viewModel.MyClosedRequestsCount = supportService.GetMyClosedRequestsCount(User.Identity.GetUserId());
                viewModel.MyDoneRequestsCount = supportService.GetMyDoneRequestsCount(User.Identity.GetUserId());
            }

            

            //viewModel.DasboardList.AddRange(items);

            //dashboardItems.AddRange(items);

            return View(viewModel);
        }

//        #region oldmethod

//        private List<DashboardViewModel> TimeSheetModuleAdmin()
//        {
//            TimeSheetService timesheetService = new TimeSheetService();
//            List<DashboardViewModel> items = new List<DashboardViewModel>();

//            DashboardViewModel dvm = new DashboardViewModel();
//            dvm.title = "Pending TimeSheets";
//            dvm.value = timesheetService.GetPendingTimesheetsCount();
//            dvm.link  = "/TimeSheet/EmployeeAdminTimeSheetsList";
//            items.Add(dvm);
//            // pending timesheets
//            return items;
//        }
//        private List<DashboardViewModel> TimeSheetModuleManager()
//        {
//            TimeSheetService timesheetService = new TimeSheetService();
//            List<DashboardViewModel> items = new List<DashboardViewModel>();

//            DashboardViewModel dvm = new DashboardViewModel();
//            dvm.title = "Pending TimeSheets";
//            dvm.value = timesheetService.GetPendingTimesheetsCount();
//            dvm.link = "/TimeSheet/EmployeeManagerTimeSheetsList";
//            items.Add(dvm);
//            // pending timesheets
//            return items;
//        }
//        private List<DashboardViewModel> TimeSheetModuleReportingManager()
//        {
//            TimeSheetService timesheetService = new TimeSheetService();
//            List<DashboardViewModel> items = new List<DashboardViewModel>();

//            DashboardViewModel dvm = new DashboardViewModel();
//            dvm.title = "Pending TimeSheets(Reporting Manager)";
//            dvm.value = timesheetService.GetPendingTimesheetsCount(User.Identity.GetUserId());
//            dvm.link = "/TimeSheet/EmployeeRepoManTimeSheetsList";
//            items.Add(dvm);
//            // pending timesheets
//            return items;
//        }


//        private List<DashboardViewModel> LOAModuleAdmin()
//        {
//            LOAService loaService = new LOAService();
//            List<DashboardViewModel> items = new List<DashboardViewModel>();

//            DashboardViewModel dvm = new DashboardViewModel();
//            dvm.title = "Pending LOAs";
//            dvm.value = loaService.GetPendingLOAsCount();
//            dvm.link = "/LOA/EmployeeAdminLOAList";
//            items.Add(dvm);
//            // pending timesheets
//            return items;
//        }
//        private List<DashboardViewModel> LOAModuleManager()
//        {
//            LOAService loaService = new LOAService();
//            List<DashboardViewModel> items = new List<DashboardViewModel>();

//            DashboardViewModel dvm = new DashboardViewModel();
//            dvm.title = "Pending LOAs";
//            dvm.value = loaService.GetPendingLOAsCount();
//            dvm.link = "/LOA/EmployeeManagerLOAList";
//            items.Add(dvm);
//            // pending timesheets
//            return items;
//        }
//        private List<DashboardViewModel> LOAModuleReportingManager()
//        {
//            LOAService loaService = new LOAService();
//            List<DashboardViewModel> items = new List<DashboardViewModel>();

//            DashboardViewModel dvm = new DashboardViewModel();
//            dvm.title = "Pending LOAs(Reporting Manager)";
//            dvm.value = loaService.GetPendingLOAsCount(User.Identity.GetUserId());
//            dvm.link = "/LOA/EmployeeRepoManLOAList";
//            items.Add(dvm);
//            // pending timesheets
//            return items;
//        }


//        private List<DashboardViewModel> ExpenseModuleAdmin()
//        {
//            ExpenseService expenseService = new ExpenseService();
//            List<DashboardViewModel> items = new List<DashboardViewModel>();

//            DashboardViewModel dvm = new DashboardViewModel();
//            dvm.title = "Pending Expenses";
//            dvm.value = expenseService.GetPendingExpensesCount();
//            dvm.link = "/Expense/EmployeeAdminExpensesList";
//            items.Add(dvm);
//            // pending timesheets
//            return items;
//        }
//        private List<DashboardViewModel> ExpenseModuleManager()
//        {
//            ExpenseService expenseService = new ExpenseService();
//            List<DashboardViewModel> items = new List<DashboardViewModel>();

//            DashboardViewModel dvm = new DashboardViewModel();
//            dvm.title = "Pending Expenses";
//            dvm.value = expenseService.GetPendingExpensesCount();
//            dvm.link = "/Expense/EmployeeManagerExpensesList";
//            items.Add(dvm);
//            // pending timesheets
//            return items;
//        }
//        private List<DashboardViewModel> ExpenseModuleReportingManager()
//        {
//            ExpenseService expenseService = new ExpenseService();
//            List<DashboardViewModel> items = new List<DashboardViewModel>();

//            DashboardViewModel dvm = new DashboardViewModel();
//            dvm.title = "Pending Expenses(Reporting Manager)";
//            dvm.value = expenseService.GetPendingExpensesCount(User.Identity.GetUserId());
//            dvm.link = "/Expense/EmployeeRepoManExpensesList";
//            items.Add(dvm);
//            // pending timesheets
//            return items;
//        }

//        private List<DashboardViewModel> SupportModuleUserPendingRequest()
//        {
//            SupportService supportService = new SupportService();
//            List<DashboardViewModel> items = new List<DashboardViewModel>();

//            DashboardViewModel dvm = new DashboardViewModel();
//            dvm.title = "Pending Requests";
//            dvm.value = supportService.GetMyPendingRequestsCount(User.Identity.GetUserId());
//            dvm.link = "/Expense/EmployeeRepoManExpensesList";
//            items.Add(dvm);
//            // pending timesheets
//            return items;
//        }

//        private List<DashboardViewModel> SupportModuleUserDoneRequest()
//        {
//            SupportService supportService = new SupportService();
//            List<DashboardViewModel> items = new List<DashboardViewModel>();

//            DashboardViewModel dvm = new DashboardViewModel();
//            dvm.title = "Done Requests";
//            dvm.value = supportService.GetMyDoneRequestsCount(User.Identity.GetUserId());
//            dvm.link = "/Expense/EmployeeRepoManExpensesList";
//            items.Add(dvm);
//            // pending timesheets
//            return items;
//        }

//        private List<DashboardViewModel> SupportModuleUserClosedRequest()
//        {
//            SupportService supportService = new SupportService();
//            List<DashboardViewModel> items = new List<DashboardViewModel>();

//            DashboardViewModel dvm = new DashboardViewModel();
//            dvm.title = "Closed Requests";
//            dvm.value = supportService.GetMyClosedRequestsCount(User.Identity.GetUserId());
//            dvm.link = "/Expense/EmployeeRepoManExpensesList";
//            items.Add(dvm);
//            // pending timesheets
//            return items;
//        }

//#endregion

    }
}