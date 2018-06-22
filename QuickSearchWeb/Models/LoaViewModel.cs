using QuickSearchData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuickSearchWeb.Models
{
    public class LoaViewModel
    {
        public int LoaId { get; set; }

        [Display(Name = "LOA Id")]
        public string LoaCode { get; set; }
        public string UserId { get; set; }

        public string ApprovedBy { get; set; }

        public AspNetUser AspNetUser { get; set; }

        [Display(Name = "Reporting Manager")]
        public String ReportingManager { get; set; }

        [Required]
        [Display(Name = "Time off Start Date*")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}"/*, ApplyFormatInEditMode = true*/)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Time off End Date*")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}"/*, ApplyFormatInEditMode = true*/)]
        public DateTime EndDate { get; set; }
        public string TimeOfDay { get; set; }
        [Required]
        [Display(Name = "*Time of Day")]
        public int LookupTimeOfDay { get; set; }
        public List<DropDownListViewModel> TimeOfDayList { get; set; }

        [Display(Name = "*Appointment must be less than 2 hours")]
        public string OtherTimeOfDay { get; set; }

        [Required]
        [Display(Name = "*Type of Leave")]
        public int LookupTypeOfLeave { get; set; }
        public List<DropDownListViewModel> TypeOfLeaveList { get; set; }

        [Display(Name = "*Other Type of Leave")]
        public string OtherTypeOfLeave { get; set; }
        [Display(Name = "Reason for Leave")]
        public string ReasonForLeave { get; set; }

        [Display(Name = "Comments: ")]
        public string Comments { get; set; }

        [Display(Name = "*Comments: ")]
        public string TempComments { get; set; }

        [Display(Name = "Status ")]
        public LookupCodeMaster LookupCodeMaster2 { get; set; }

        public int LookupOtherTimeOfDay { get; set; }
        public int LookupOtherTypeOfLeave { get; set; }
        

    }

    public class LoaListViewModel
    {
        public int LoaId { get; set; }
        public string LoaCode { get; set; }
        public AspNetUser AspNetUser { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LookupCodeMaster LookupCodeMaster2 { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }

}