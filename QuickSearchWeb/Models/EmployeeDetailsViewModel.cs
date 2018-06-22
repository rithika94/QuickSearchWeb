using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickSearchWeb.Models
{
    public class EmployeeDetailsViewModel
    {
        public string Id { get; set; }
        public string EmployeeCode { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string ReportingManager { get; set; }
    }
}