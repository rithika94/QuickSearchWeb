using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuickSearchWeb.Models
{
    public class MyProfileViewModel
    {


        public string UserID { get; set; }

        public string User { get; set; }


        [Required]
        [Display(Name = "First Name*")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name*")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "User Name*")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Phone Number*")]
        [RegularExpression(@"^\(?\d{3}\)?[- ]?\d{3}[- ]?\d{4}$", ErrorMessage = "Entered phone format is not valid")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Gender*")]
        public int LookupGender { get; set; }

        public List<DropDownListViewModel> GenderList { get; set; }


        [Required]
        [Display(Name = "Date of Joining*")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime JoiningDate { get; set; }


        [Required]
        [EmailAddress]
        [Display(Name = "Email*")]
        public string Email { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password*")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must have an upper case, lower case, special character and numeric combined.")]
        public string Password { get; set; }
        
        [Display(Name = "Employee Code")]
        [DisplayFormat(DataFormatString = "{0:##-####-####}", ApplyFormatInEditMode = true)]
        public string EmployeeCode { get; set; }

        [Display(Name = "Is Account Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Dont Assign")]
        public bool DontAssign { get; set; }

        [Display(Name = "Reporting Manager")]
        public string ReportingManagerUsername { get; set; }

        public string ReportingManagerId { get; set; }

        public string Id { get; set; }

        [Display(Name = "Human Resources")]
        public string HrRole { get; set; }

        [Display(Name = "Inventory")]
        public string InventoryRole { get; set; }

        [Display(Name = "Recruitment")]
        public string RecruitmentRole { get; set; }

        [Display(Name = "Timesheet")]
        public string TimeSheetRole { get; set; }

        [Display(Name = "Employee")]
        public string EmployeeRole { get; set; }

        [Display(Name = "Contract")]
        public string ContractRole { get; set; }

        // public List<DropDownListViewModel> GenderList { get; set; }
    }
}