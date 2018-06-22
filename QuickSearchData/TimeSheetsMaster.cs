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
    
    public partial class TimeSheetsMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TimeSheetsMaster()
        {
            this.TimeSheetSummaries = new HashSet<TimeSheetSummary>();
        }
    
        public int TimeSheetID { get; set; }
        public string TimeSheetCode { get; set; }
        public string UserID { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public decimal RegularHours { get; set; }
        public decimal OverTimeHours { get; set; }
        public decimal TotalHours { get; set; }
        public decimal Day1RegularHours { get; set; }
        public decimal Day2RegularHours { get; set; }
        public decimal Day3RegularHours { get; set; }
        public decimal Day4RegularHours { get; set; }
        public decimal Day5RegularHours { get; set; }
        public decimal Day6RegularHours { get; set; }
        public decimal Day7RegularHours { get; set; }
        public decimal Day1OverHours { get; set; }
        public decimal Day2OverHours { get; set; }
        public decimal Day3OverHours { get; set; }
        public decimal Day4OverHours { get; set; }
        public decimal Day5OverHours { get; set; }
        public decimal Day6OverHours { get; set; }
        public decimal Day7OverHours { get; set; }
        public int LookupAbsenceTypeDay1 { get; set; }
        public int LookupAbsenceTypeDay2 { get; set; }
        public int LookupAbsenceTypeDay3 { get; set; }
        public int LookupAbsenceTypeDay4 { get; set; }
        public int LookupAbsenceTypeDay5 { get; set; }
        public int LookupAbsenceTypeDay6 { get; set; }
        public int LookupAbsenceTypeDay7 { get; set; }
        public decimal AbsenceHoursDay1 { get; set; }
        public decimal AbsenceHoursDay2 { get; set; }
        public decimal AbsenceHoursDay3 { get; set; }
        public decimal AbsenceHoursDay4 { get; set; }
        public decimal AbsenceHoursDay5 { get; set; }
        public decimal AbsenceHoursDay6 { get; set; }
        public decimal AbsenceHoursDay7 { get; set; }
        public int LookupApprovedStatus { get; set; }
        public string Comments { get; set; }
        public string ApprovedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedUserId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual LookupCodeMaster LookupCodeMaster { get; set; }
        public virtual LookupCodeMaster LookupCodeMaster1 { get; set; }
        public virtual LookupCodeMaster LookupCodeMaster2 { get; set; }
        public virtual LookupCodeMaster LookupCodeMaster3 { get; set; }
        public virtual LookupCodeMaster LookupCodeMaster4 { get; set; }
        public virtual LookupCodeMaster LookupCodeMaster5 { get; set; }
        public virtual LookupCodeMaster LookupCodeMaster6 { get; set; }
        public virtual LookupCodeMaster LookupCodeMaster7 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeSheetSummary> TimeSheetSummaries { get; set; }
    }
}