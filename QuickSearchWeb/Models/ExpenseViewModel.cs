using QuickSearchData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuickSearchWeb.Models
{
    public class ExpenseViewModel
    {

        public string UserId { get; set; }

        [Display(Name = "Expense code")]
        public int ExpenseId { get; set; }

        [Display(Name = "Expense Code")]
        public string ExpenseCode { get; set; }
                

        public AspNetUser AspNetUser { get; set; }

        [Display(Name = "Reporting Manager")]
        public string ReportingManager { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime EndDate { get; set; }

        [Display(Name = "SubTotal($)")]
        public decimal SubTotal { get; set; }

        [Display(Name = "Total Expenses($)")]
        public decimal Total { get; set; }

        [Display(Name = "Advances($)")]
        public decimal? Advance { get; set; }

        [Display(Name = "Approved By")]
        public string ApprovedBy { get; set; }

        [Display(Name = "Comments")]
        public string Comments { get; set; }

        [Display(Name = "*Comments")]
        public string TempComments { get; set; }

        [Display(Name = "Approved On")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ApprovedDate { get; set; }

        public ExpenseSummaryViewModel ExpensesSummary { get; set; }

        public List<ExpenseAttachment> ExpenseFilesList { get; set; }

        [Display(Name = "Status")]
        public LookupCodeMaster LookupCodeMaster { get; set; }
        public List<ExpenseSummaryViewModel> ExpensesSummaryList = new List<ExpenseSummaryViewModel>();
       

    }


    public class ExpenseListViewModel
    {

        public int ExpenseId { get; set; }
        public string ExpenseCode { get; set; }

        public string UserId { get; set; }

        public AspNetUser AspNetUser { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime EndDate { get; set; }
       
        public decimal Total { get; set; }
        public string ApprovedBy { get; set; }

        [Display(Name = "Approved On")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ApprovedDate { get; set; }

        public LookupCodeMaster LookupCodeMaster { get; set; }


        public DateTime ModifiedDate { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }


    }
}