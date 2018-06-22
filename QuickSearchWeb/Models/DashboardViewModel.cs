using QuickSearchData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuickSearchWeb.Models
{
    public class DashboardViewModel
    {
        public string title { get; set; }
        public int value { get; set; }
        public string link { get; set; }


    }
    public class DashboardViewModels
    {
     public List <DashboardViewModel> DasboardList { get; set; }

        public int MyPendingRequestsCount { get; set; }
        public int MyDoneRequestsCount { get; set; }
        public int MyClosedRequestsCount { get; set; }

        public int AdminPendingTimesheetsCount { get; set; }

        public int ManagerPendingTimesheetsCount { get; set; }
        public int ReportingManagerPendingTimesheetsCount { get; set; }
        public int AdminPendingLOAsCount { get; set; }
        public int MangerPendingLOAsCount { get; set; }
        public int ReportingManagerPendingLOAsCount { get; set; }
        public int AdminPendingExpensesCount { get; set; }
        public int MangerPendingExpensesCount { get; set; }
        public int ReportingManagerPendingExpensesCount { get; set; }



    }
    

}