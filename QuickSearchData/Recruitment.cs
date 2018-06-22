//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuickSearchData
{
    using System;
    using System.Collections.Generic;
    
    public partial class Recruitment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Recruitment()
        {
            this.RecruitmentClients = new HashSet<RecruitmentClient>();
            this.RecruitmentDocuments = new HashSet<RecruitmentDocument>();
        }
    
        public int RecruitmentId { get; set; }
        public string RecruiterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PrimarySkillSet { get; set; }
        public string SecondarySkillSet { get; set; }
        public int LookupVisaStatus { get; set; }
        public Nullable<System.DateTime> AvailableDate { get; set; }
        public string CurrentLocation { get; set; }
        public string Notes { get; set; }
        public int LookupEmployementType { get; set; }
        public string C2CContactPerson { get; set; }
        public string C2CEmail { get; set; }
        public string C2CCompanyName { get; set; }
        public string C2CContactNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedUserId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual LookupCodeMaster LookupCodeMaster { get; set; }
        public virtual LookupCodeMaster LookupCodeMaster1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecruitmentClient> RecruitmentClients { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecruitmentDocument> RecruitmentDocuments { get; set; }
    }
}
