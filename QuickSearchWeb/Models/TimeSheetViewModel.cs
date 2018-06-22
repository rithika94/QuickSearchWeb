using QuickSearchData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickSearchWeb.Models
{
    public class TimeSheetViewModel
    {
        public int TimeSheetID { get; set; }

        [Display(Name = "TimeSheet ID")]
        public string TimeSheetCode { get; set; }

        public string UserID { get; set; }

        public string FullName { get; set; }

        public AspNetUser AspNetUser { get; set; }

        [Display(Name = "Reporting Manager")]
        public string ReportingManager { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

       
        
        public Decimal? RegularHours { get; set; }

       
        [Display(Name = "OverTime Hours")]
        public Decimal? OverTimeHours { get; set; }

        
        [Display(Name = "Total Hours")]
        public Decimal? TotalHours { get; set; }

        


      
        public Decimal? Day1RegularHours { get; set; }

        public Decimal? Day2RegularHours { get; set; }

        public Decimal? Day3RegularHours { get; set; }

        public Decimal? Day4RegularHours { get; set; }

        public Decimal? Day5RegularHours { get; set; }

        public Decimal? Day6RegularHours { get; set; }

        public Decimal? Day7RegularHours { get; set; }

        public Decimal? Day1OverHours { get; set; }

        public Decimal? Day2OverHours { get; set; }

        public Decimal? Day3OverHours { get; set; }

        public Decimal? Day4OverHours { get; set; }

        public Decimal? Day5OverHours { get; set; }

        public Decimal? Day6OverHours { get; set; }

        public Decimal? Day7OverHours { get; set; }

        [Display(Name = "LookupAbsenceTypeDay1")]
        public int LookupAbsenceTypeDay1 { get; set; }

        [Display(Name = "LookupAbsenceTypeDay2")]
        public int LookupAbsenceTypeDay2 { get; set; }

        [Display(Name = "LookupAbsenceTypeDay3")]
        public int LookupAbsenceTypeDay3 { get; set; }

        [Display(Name = "LookupAbsenceTypeDay4")]
        public int LookupAbsenceTypeDay4 { get; set; }

        [Display(Name = "LookupAbsenceTypeDay5")]
        public int LookupAbsenceTypeDay5 { get; set; }

        [Display(Name = "LookupAbsenceTypeDay6")]
        public int LookupAbsenceTypeDay6 { get; set; }

        [Display(Name = "LookupAbsenceTypeDay7")]
        public int LookupAbsenceTypeDay7 { get; set; }

        public Decimal AbsenceHoursDay1 { get; set; }

        public Decimal AbsenceHoursDay2 { get; set; }

        public Decimal AbsenceHoursDay3 { get; set; }

        public Decimal AbsenceHoursDay4 { get; set; }

        public Decimal AbsenceHoursDay5 { get; set; }

        public Decimal AbsenceHoursDay6 { get; set; }

        public Decimal AbsenceHoursDay7 { get; set; }


        public List<DropDownListViewModel> AbsenceTypeList { get; set; }

        public List<TimeSheetSummaryViewModel> TimeSheetSummaryList { get; set; }

        

        [Display(Name = "Status")]
        public LookupCodeMaster LookupCodeMaster7 { get; set; } // lookup approval status

        [Display(Name = "Comments: ")]
        public string Comments { get; set; }


       
        [Display(Name = "*Comments: ")]
        public string TempComments { get; set; }

        public string TSStartDay { get; set; }

        public List<LoaViewModel> LoasAssociated { get; set; }

    }

    public class TimeSheetListViewModel
    {
        public int TimeSheetID { get; set; }

        [Display(Name = "TimeSheet Id")]
        public string TimeSheetCode { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}"/*, ApplyFormatInEditMode = true*/)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}"/*, ApplyFormatInEditMode = true*/)]
        public DateTime EndDate { get; set; }

       

        public decimal TotalHours { get; set; }

        [Display(Name = "Approval Status")]
        public LookupCodeMaster LookupCodeMaster7 { get; set; } // lookup approval status

        public string UserID { get; set; }
        public AspNetUser AspNetUser { get; set; }
        

        public DateTime? ModifiedDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        [Display(Name = "Approved By")]
        public string ApprovedBy { get; set; }

    }
}
