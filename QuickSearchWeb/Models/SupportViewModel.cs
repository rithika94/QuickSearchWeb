using QuickSearchData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QuickSearchWeb.Models
{
    public class SupportViewModel
    {
        public int SupportId { get; set; }

        [Required(ErrorMessage ="Summary is required")]
        [Display(Name = "Summary")]
        public string Summary { get; set; }

        [Display(Name = "Support Id")]
        public string SupportCode { get; set; }

        [Display(Name = "User Id")]
        public string UserId { get; set; }

       

        [Required(ErrorMessage ="Description is required")]
        [Display(Name = "Description")]
        public String Description { get; set; }

        [Required(ErrorMessage ="Issue Priority is required")]
        [Display(Name ="Issue Priority")]
        public int LookupIssuePriority { get; set; }
        public List<DropDownListViewModel> IssuePriorityList { get; set; }

        [Required(ErrorMessage = "Issue Type is required")]
        [Display(Name = "Issue Type")]
        public int LookupIssueType { get; set; }
        public List<DropDownListViewModel> IssueTypeList { get; set; }

        [Display(Name = "Issue Status")]
        public int LookupIssueStatus { get; set; }
        public List<DropDownListViewModel> IssueStatusList { get; set; }

        //[Display(Name = "Priority")]
        //public LookupCodeMaster LookupCodeMaster { get; set; }


        
        public string AssignTo { get; set; }

        [Display(Name = "Assigned To")]
        public string AssignToName { get; set; }

        [Display(Name = "Reported By")]
        public string ReportedByName { get; set; }

        [Display(Name ="Submitted Date")]
        public DateTime SubmittedDate { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "Comments")]
        [AllowHtml]
        public string Comments { get; set; }

        [Display(Name = "Comments")]
        public string TempComments { get; set; }

        [Display(Name = "User Comments")]
      
        public string UserComments { get; set; }

        public bool? IsFixed { get; set; }

        public bool showCloseButton { get; set; }

        public List<DropDownListViewModel> AssignToList { get; set; }

        public  LookupCodeMaster LookupCodeMaster { get; set; }
        public LookupCodeMaster LookupCodeMaster1 { get; set; }
        public LookupCodeMaster LookupCodeMaster2 { get; set; }

        //[Required]
        //[Display(Name = "Report By")]
        //public String ReportedBy { get; set; }

        public List<SupportAttachment> SupportFilesList { get; set; }

    }
        public class SupportListViewModel
        {
          
            public int SupportId { get; set; }

            [Display(Name = "Support Id")]
            public string SupportCode { get; set; }

            [Display(Name ="Summary")]
            public String Summary { get; set; }
        
        
            //[Display(Name ="Priority")]           
            //public int Priority{ get; set; }

           // [Display(Name = "Priority")]
           //public string Prioity_Name { get; set; }

        //[Display(Name = "Priority")]
        //  public LookupCodeMaster LookupCodeMaster { get; set; }
        
            [Display(Name ="Submitted")]
            public DateTime SubmittedDate { get; set; }

            [Display(Name = "Modified Date")]
            public DateTime ModifiedDate { get; set; }

        //[Display(Name = "AssignTo")]
        //public string AssignToName { get; set; }

        public bool? IsFixed { get; set; }

        public bool IsActive { get; set; }
        
            public bool IsDeleted { get; set; }

        public virtual AspNetUser AspNetUser { get; set; } // user
        public virtual AspNetUser AspNetUser1 { get; set; } //assigned to

        public LookupCodeMaster LookupCodeMaster { get; set; }// priority
        public LookupCodeMaster LookupCodeMaster1 { get; set; }// status

        public LookupCodeMaster LookupCodeMaster2 { get; set; } // issue type

    }


    public class SupportAttachments
    {
        public int Id { get; set; }
        public int SupportId { get; set; }
        public string FileServerPath { get; set; }
        public string FileName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

}