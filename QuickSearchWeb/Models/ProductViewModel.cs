using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuickSearchWeb.Models
{
    public class ProductViewModel : IValidatableObject
    {
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Product Code*")]
        public string ProductCode { get; set; }

        [Required]
        [Display(Name = "Product Name*")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "Product Category*")]
        public int LookupCategory { get; set; }

        public List<DropDownListViewModel> CategoryList { get; set; }

        [Required]
        [Display(Name = "Product Type*")]
        public int LookupProductType { get; set; }

        public List<DropDownListViewModel> ProductTypeList { get; set; }

        [Display(Name = "Other Product Type")]
        public string OtherProductType { get; set; }

        [Display(Name = "Membership Id")]
        public string MembershipId { get; set; }

        [Display(Name = "GrantNumber")]
        public string GrantNumber { get; set; }

        [Display(Name = "Make Year")]
        public string MakeModel { get; set; }

        [DataType(DataType.Date)]
        //[Range(typeof(DateTime),"", DateTime.Now.ToString(), ErrorMessage = "Date must be after or equal to current date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Purchase Date")]
        public DateTime? PurchaseDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Assigned on")]
        public DateTime? AssignedDate { get; set; }


        public string Version { get; set; }

        [Display(Name = "UnAssign")]
        public bool DontAssign { get; set; }

        [Display(Name = "Assign to")]
        public string AssignToUserName { get; set; }

        [Display(Name = "Assign Other")]
        public bool OtherAssign { get; set; }

        public string AssignTo { get; set; }

        [Display(Name = "Other Assigned User")]
        public string OtherAssignedUser { get; set; }        

        public string Description { get; set; }

        [Display(Name = "Status")]
        public int? LookupStatus { get; set; }
        public List<DropDownListViewModel> StatusList { get; set; }

        
        [Display(Name = "Platform")]
        public int? LookupPlatform { get; set; }
        public List<DropDownListViewModel> PlatformList { get; set; }


        [Display(Name = "Product Manufacturer")]
        public string ProductCompany { get; set; }

        [Display(Name = "Product Location")]
        public string Location { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Expiration Date")]
        public DateTime? ExpirationDate { get; set; }

        [Display(Name = "Assign to Hardware")]
        public string AssignToHardware { get; set; }

        [Display(Name = "Website URL")]
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        [Display(Name = "Product Activation Key")]
        public string ActivationKey { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (PurchaseDate > DateTime.Now)
            {
                results.Add(new ValidationResult("Purchase date should be less than todays date", new[] { "PurchaseDate" }));
            }

            //if (EndDateTime <= StartDateTime)
            //{
            //    results.Add(new ValidationResult("EndDateTime must be greater that StartDateTime", new[] { "EndDateTime" }));
            //}

            return results;
        }

    }

    public class ProductListViewModel
    {
        [Display(Name = "Product Id")]
        public int ProductId { get; set; }

        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Product Category")]
        public string SelectedCategory { get; set; }

        [Display(Name = "Assigned To")]
        public string AssignTo { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Product Location")]
        public string Location { get; set; }

        public string Description { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //[Display(Name = "Product Type")]
        //public string SelectedProductType { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        //[Display(Name = "Assigned to")]
        //public string AssignTo { get; set; }




    }

   

}