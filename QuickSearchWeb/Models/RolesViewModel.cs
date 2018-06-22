using QuickSearchData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QuickSearchWeb.Models
{
    public class RolesViewModel
    {
        [Display(Name = "Last Modified")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Employee Id")]
        public string EmployeeCode { get; set; }
        
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Display(Name = "Module")]
        public string ModuleName { get; set; }
        [Display(Name = "Module")]
        public string ModuleAbbre { get; set; }

        [Display(Name = "Admin")]
        public bool Admin { get; set; }

        [Display(Name = "RM/Eng")]
        public bool ReportingManager { get; set; }

        [Display(Name = "Manager")]
        public bool Manager { get; set; }

        [Display(Name = "User")]
        public bool User { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

    }

}