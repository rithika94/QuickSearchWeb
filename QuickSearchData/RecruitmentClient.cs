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
    
    public partial class RecruitmentClient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RecruitmentClient()
        {
            this.RecruitmentClientDocuments = new HashSet<RecruitmentClientDocument>();
        }
    
        public int Id { get; set; }
        public int RecruitmentId { get; set; }
        public string ClientName { get; set; }
        public Nullable<System.DateTime> SubmissionDate { get; set; }
        public string JobLocation { get; set; }
        public string JobTitle { get; set; }
        public int LookupCandidateStatus { get; set; }
        public string Comments { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedUserId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ClientDocument1 { get; set; }
        public string ClientDocument2 { get; set; }
        public string DocumentName1 { get; set; }
        public string DocumentName2 { get; set; }
    
        public virtual LookupCodeMaster LookupCodeMaster { get; set; }
        public virtual Recruitment Recruitment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecruitmentClientDocument> RecruitmentClientDocuments { get; set; }
    }
}