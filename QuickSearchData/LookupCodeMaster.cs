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
    
    public partial class LookupCodeMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LookupCodeMaster()
        {
            this.Products = new HashSet<Product>();
            this.Products1 = new HashSet<Product>();
            this.Products2 = new HashSet<Product>();
            this.Products3 = new HashSet<Product>();
            this.RecruitmentClients = new HashSet<RecruitmentClient>();
            this.AspNetUsers = new HashSet<AspNetUser>();
            this.LOAs = new HashSet<LOA>();
            this.LOAs1 = new HashSet<LOA>();
            this.LOAs2 = new HashSet<LOA>();
            this.Recruitments = new HashSet<Recruitment>();
            this.Recruitments1 = new HashSet<Recruitment>();
            this.SupportMasters = new HashSet<SupportMaster>();
            this.SupportMasters1 = new HashSet<SupportMaster>();
            this.SupportMasters2 = new HashSet<SupportMaster>();
            this.ExpenseMasters = new HashSet<ExpenseMaster>();
            this.TimeSheetsMasters = new HashSet<TimeSheetsMaster>();
            this.TimeSheetsMasters1 = new HashSet<TimeSheetsMaster>();
            this.TimeSheetsMasters2 = new HashSet<TimeSheetsMaster>();
            this.TimeSheetsMasters3 = new HashSet<TimeSheetsMaster>();
            this.TimeSheetsMasters4 = new HashSet<TimeSheetsMaster>();
            this.TimeSheetsMasters5 = new HashSet<TimeSheetsMaster>();
            this.TimeSheetsMasters6 = new HashSet<TimeSheetsMaster>();
            this.TimeSheetsMasters7 = new HashSet<TimeSheetsMaster>();
        }
    
        public int LookupCodeId { get; set; }
        public int LookupTypeId { get; set; }
        public string Code { get; set; }
        public string LookupCodeName { get; set; }
        public bool IsActive { get; set; }
    
        public virtual LookupTypeMaster LookupTypeMaster { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecruitmentClient> RecruitmentClients { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOA> LOAs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOA> LOAs1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOA> LOAs2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recruitment> Recruitments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recruitment> Recruitments1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupportMaster> SupportMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupportMaster> SupportMasters1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupportMaster> SupportMasters2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExpenseMaster> ExpenseMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeSheetsMaster> TimeSheetsMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeSheetsMaster> TimeSheetsMasters1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeSheetsMaster> TimeSheetsMasters2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeSheetsMaster> TimeSheetsMasters3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeSheetsMaster> TimeSheetsMasters4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeSheetsMaster> TimeSheetsMasters5 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeSheetsMaster> TimeSheetsMasters6 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeSheetsMaster> TimeSheetsMasters7 { get; set; }
    }
}
