using QuickSearchData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuickSearchWeb.Models
{
    public class RecruitmentViewModel
    {
        public int RecruitmentId { get; set; }
        
        public string RecruiterId { get; set; }

        public AspNetUser AspNetUser { get; set; }

        public string RecruiterName { get; set; }

        [Required]
        [Display(Name = "First Name*")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name*")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone Number*")]
        [RegularExpression(@"^\(?\d{3}\)?[- ]?\d{3}[- ]?\d{4}$", ErrorMessage = "Entered phone format is not valid")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email*")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Primary Skill Set*")]
        public string PrimarySkillSet { get; set; }

        [Required]
        [Display(Name = "Secondary Skill Set")]
        public string SecondarySkillSet { get; set; }

        [Required]
        [Display(Name = "Visa Status*")]
        public int LookupVisaStatus { get; set; }


        public List<DropDownListViewModel> VisaStatusList { get; set; }

        [Display(Name = "Available Date*")]
        public DateTime? AvailableDate { get; set; }

        [Display(Name = "Current Location")]
        public string CurrentLocation { get; set; }

        public string Notes { get; set; }

       
        [Display(Name = "Employement Type")]
        public int LookupEmployementType { get; set; }

        public List<DropDownListViewModel> EmployementTypeList { get; set; }


        [Display(Name = "Contact Person")]
        public string C2CContactPerson { get; set; }


        [Display(Name = "Email")]
        public string C2CEmail { get; set; }


        [Display(Name = "Company Name")]
        public string C2CCompanyName { get; set; }


        [Display(Name = "Contact Number")]
        public string C2CContactNumber { get; set; }

        
        public List<RecruitmentClient> RecruitmentClientList { get; set; }

        //public List<RecruitmentListViewModel> RecruitmentList { get; set; }

        [Display(Name = "Copy as a Submission")]
        public int Submission { get; set; }
        public DateTime? SubmissionDate { get; set; }

        public string ClientName { get; set; }

        public string JobLocation { get; set; }
        public string JobTitle { get; set; }
        public int? LookupCandidateStatus { get; set; }
        public List<DropDownListViewModel> CandidateStatusList { get; set; }
        public string Comments { get; set; }


        public DateTime? ModifiedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedUserId { get; set; }
        public List<RecruitmentDocument> RFileList { get; set; }
    }



    public class RecruitmentClientListViewModel
    {
        [Display(Name = "Copy as a Submission")]
        public int Submission { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public string JobLocation { get; set; }
        public string JobTitle { get; set; }
        public string ClientName { get; set; }
        public int? LookupCandidateStatus { get; set; }
        public List<DropDownListViewModel> CandidateStatusList { get; set; }
        public string Comments { get; set; }
    }

    public class RecruitmentListViewModel
    {

        public int RecruitmentId { get; set; }

        public string FirstName { get; set; }
   
        public string LastName { get; set; }
       
        public string PhoneNumber { get; set; }

        public string RecruiterName { get; set; }

        public string PrimarySkillSet { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? AvailableDate { get; set; }

        public string CreatedUser { get; set; }

        public string ModifiedUser { get; set; }
        
    }
    public class RecruitmentDocuments
    {
        public int Id { get; set; }
        public int RecruitmentId { get; set; }
        public string FileServerPath { get; set; }
        public string RecruitmentDocumentName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}