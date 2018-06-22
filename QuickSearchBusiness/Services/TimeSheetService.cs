using QuickSearchData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;


namespace QuickSearchBusiness.Services
{
    public class TimeSheetService
    {

        public bool CreateTimeSheet(TimeSheetsMaster timeSheet)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.TimeSheetsMasters.Add(timeSheet);
                _context.SaveChanges();
                timeSheet.TimeSheetCode = "TS" + (1100 + timeSheet.TimeSheetID).ToString();
                _context.TimeSheetsMasters.Attach(timeSheet);
                _context.Entry(timeSheet).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }

        }

        public bool CreateTimeSheetSummary(List<TimeSheetSummary> summaries)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.TimeSheetSummaries.AddRange(summaries);
                _context.SaveChanges();
                return true;
            }

        }

        public TimeSheetsMaster GetTimeSheetMaster(int id, string userId = null)
        {
            using (var _context = new AdminPortalEntities())
            {
                TimeSheetsMaster timeSheet = new TimeSheetsMaster();

                if(userId != null)
                {
                    timeSheet = _context.TimeSheetsMasters.Where(x => x.TimeSheetID == id && x.AspNetUser.AspNetUser1.Id == userId && x.IsDeleted == false).Include(x => x.TimeSheetSummaries).Include(x => x.AspNetUser).Include("AspNetUser.AspNetUser1").Include(x => x.LookupCodeMaster).Include(x => x.LookupCodeMaster1).Include(x => x.LookupCodeMaster2).Include(x => x.LookupCodeMaster3).Include(x => x.LookupCodeMaster4).Include(x => x.LookupCodeMaster5).Include(x => x.LookupCodeMaster6).Include(x => x.LookupCodeMaster7).FirstOrDefault();

                }
                else
                {
                    timeSheet = _context.TimeSheetsMasters.Where(x => x.TimeSheetID == id && x.IsDeleted == false).Include(x => x.TimeSheetSummaries).Include(x => x.AspNetUser).Include("AspNetUser.AspNetUser1").Include(x => x.LookupCodeMaster).Include(x => x.LookupCodeMaster1).Include(x => x.LookupCodeMaster2).Include(x => x.LookupCodeMaster3).Include(x => x.LookupCodeMaster4).Include(x => x.LookupCodeMaster5).Include(x => x.LookupCodeMaster6).Include(x => x.LookupCodeMaster7).FirstOrDefault();

                }

                return timeSheet;
            }

        }
        public TimeSheetsMaster GetMyTimeSheetMaster(int id, string userId = null)
        {
            using (var _context = new AdminPortalEntities())
            {
                TimeSheetsMaster timeSheet = new TimeSheetsMaster();

                if (userId != null)
                {
                    timeSheet = _context.TimeSheetsMasters.Where(x => (x.TimeSheetID == id) && (x.UserID == userId) && (x.IsDeleted == false)).Include(x => x.TimeSheetSummaries).Include(x => x.AspNetUser).Include("AspNetUser.AspNetUser1").Include(x => x.LookupCodeMaster).Include(x => x.LookupCodeMaster1).Include(x => x.LookupCodeMaster2).Include(x => x.LookupCodeMaster3).Include(x => x.LookupCodeMaster4).Include(x => x.LookupCodeMaster5).Include(x => x.LookupCodeMaster6).Include(x => x.LookupCodeMaster7).FirstOrDefault();

                }
                else
                {
                    timeSheet = _context.TimeSheetsMasters.Where(x => x.TimeSheetID == id && x.IsDeleted == false).Include(x => x.TimeSheetSummaries).Include(x => x.AspNetUser).Include("AspNetUser.AspNetUser1").Include(x => x.LookupCodeMaster).Include(x => x.LookupCodeMaster1).Include(x => x.LookupCodeMaster2).Include(x => x.LookupCodeMaster3).Include(x => x.LookupCodeMaster4).Include(x => x.LookupCodeMaster5).Include(x => x.LookupCodeMaster6).Include(x => x.LookupCodeMaster7).FirstOrDefault();

                }

                return timeSheet;
            }

        }

        public TimeSheetsMaster GetJustTimeSheetMaster(int id, string userId = null)
        {
            using (var _context = new AdminPortalEntities())
            {
                TimeSheetsMaster timeSheet = new TimeSheetsMaster();

                if (userId != null)
                {
                    timeSheet = _context.TimeSheetsMasters.Where(x => x.TimeSheetID == id && x.IsDeleted == false && x.UserID == userId).FirstOrDefault();
                   
                }
                else
                {
                    timeSheet = _context.TimeSheetsMasters.Where(x => x.TimeSheetID == id && x.IsDeleted == false).FirstOrDefault();
                }

                return timeSheet;
            }

        }
       
        
        public List<TimeSheetSummary> GetTimeSheetSummary(int id)
        {
            using (var _context = new AdminPortalEntities())
            {
                var timeSheet = _context.TimeSheetSummaries.Where(x => x.TimeSheetID == id).ToList();

                return timeSheet;
            }

        }

     


        
        public List<TimeSheetsMaster> GetTimeSheetListWithUserId(string userId)
        {
            using (var _context = new AdminPortalEntities())
            {
                var timesheetList = _context.TimeSheetsMasters.Where(x => x.UserID == userId).ToList();
                return timesheetList;
            }
        }

        public List<TimeSheetsMaster> GetTimeSheetsBetweenDates(DateTime FromDate, DateTime Todate)
        {
            using (var _context = new AdminPortalEntities())
            {
                var timesheetList = _context.TimeSheetsMasters.Where(x => (x.StartDate >= FromDate && x.EndDate <= Todate) || (x.StartDate <= FromDate && x.EndDate >= FromDate) || (x.StartDate <= Todate && x.EndDate >= Todate)).ToList();
                return timesheetList;
            }
        }

        public List<TimeSheetsMaster> GetTimeSheetsBetweenDatesAndUserId(DateTime FromDate, DateTime Todate, string userId)
        {
            using (var _context = new AdminPortalEntities())
            {
                var timesheetList = _context.TimeSheetsMasters.Where(x => (x.UserID == userId)).Where(x =>(x.StartDate >= FromDate && x.EndDate <= Todate) || (x.StartDate <= FromDate && x.EndDate >= FromDate) || (x.StartDate <= Todate && x.EndDate >= Todate)).Include(x => x.AspNetUser).Include(x => x.LookupCodeMaster7).ToList();
                return timesheetList;
            }
        }

        public List<TimeSheetsMaster> GetEmployeePendingTimeSheets()
        {
            using (var _context = new AdminPortalEntities())
            {
               
                var timesheetList = _context.TimeSheetsMasters.Where(x => x.LookupCodeMaster7.LookupCodeName == "Pending").Include(x => x.AspNetUser).Include("AspNetUser.AspNetUser1").Include(x => x.LookupCodeMaster7).ToList();
                return timesheetList;
            }
        }

        public List<TimeSheetsMaster> GetReportingManagerPendingTimeSheets(string RMuserId)
        {
            using (var _context = new AdminPortalEntities())
            {
                
                var timesheetList = _context.TimeSheetsMasters.Where(x => x.AspNetUser.ReportingManager == RMuserId && x.LookupCodeMaster7.LookupCodeName == "Pending").Include(x => x.AspNetUser).Include("AspNetUser.AspNetUser1").Include(x => x.LookupCodeMaster7).ToList();
                return timesheetList;
            }
        }

        public List<TimeSheetsMaster> GetReportingManagerRemianingTimeSheets(DateTime FromDate, DateTime Todate,string RMuserId)
        {
            using (var _context = new AdminPortalEntities())
            {
                        
                var timesheetList = _context.TimeSheetsMasters.Where(x => (x.AspNetUser.ReportingManager == RMuserId) 
                                                                   && (x.LookupCodeMaster7.LookupCodeName == "Approved" || x.LookupCodeMaster7.LookupCodeName == "Rejected") 
                                                                   && ((x.StartDate >= FromDate && x.EndDate <= Todate)|| (x.StartDate <= FromDate && x.EndDate >= FromDate) || (x.StartDate <= Todate && x.EndDate >= Todate))).Include(x => x.AspNetUser).Include("AspNetUser.AspNetUser1").Include(x => x.LookupCodeMaster7).ToList();
                return timesheetList;
            }
        }

        public List<TimeSheetsMaster> GetEmployeeRemianingTimeSheets(DateTime FromDate, DateTime Todate)
        {
            using (var _context = new AdminPortalEntities())
            {
                         
                var timesheetList = _context.TimeSheetsMasters.Where(x => (x.LookupCodeMaster7.LookupCodeName == "Approved" || x.LookupCodeMaster7.LookupCodeName == "Rejected") 
                && ((x.StartDate >= FromDate && x.EndDate <= Todate) || (x.StartDate <= FromDate && x.EndDate >= FromDate) || (x.StartDate <= Todate && x.EndDate >= Todate))).Include(x => x.AspNetUser).Include("AspNetUser.AspNetUser1").Include(x => x.LookupCodeMaster7).ToList();
                return timesheetList;
            }
        }

        public bool UpdateTimeSheet(TimeSheetsMaster timeSheetsMaster)
        {
           
                using (var _context = new AdminPortalEntities())
                {
                    _context.TimeSheetsMasters.Attach(timeSheetsMaster);
                    _context.Entry(timeSheetsMaster).State = EntityState.Modified;
                    _context.SaveChanges();
                    return true;
                }
                
        }

        public bool UpdateTimeSheetSummaries(List<TimeSheetSummary> summaries)
        {

            using (var _context = new AdminPortalEntities())
            {
                foreach (TimeSheetSummary s in summaries)
                {
                    _context.TimeSheetSummaries.Attach(s);
                    _context.Entry(s).State = EntityState.Modified;
                }
                _context.SaveChanges();
                return true;
            }

        }


        


        public bool HardDeleteTimeSheet(int timeSheetId)
        {
            using (var _context = new AdminPortalEntities())
            {
                var timeSheet = _context.TimeSheetsMasters.SingleOrDefault(s => s.TimeSheetID == timeSheetId);
                if (timeSheet != null)
                {
                    _context.TimeSheetsMasters.Remove(timeSheet);
                    _context.SaveChanges();
                }
                return true;
            }
        }
        public bool HardDeleteSummaries(int timeSheetId)
        {
            using (var _context = new AdminPortalEntities())
            {
                var summaries = _context.TimeSheetSummaries.Where(s => s.TimeSheetID == timeSheetId);
                if (summaries != null)
                {
                    _context.TimeSheetSummaries.RemoveRange(summaries);
                    _context.SaveChanges();
                }
                return true;
            }
        }

        public LookupCodeMaster GetLookupIdForStatus(string code, string type)
        {
            using (var _context = new AdminPortalEntities())
            {
                var codeList = _context.LookupCodeMasters.Where(s => s.LookupTypeMaster.LookupTypeName == type).ToList();
                var val = codeList.FirstOrDefault(s => s.LookupCodeName == code);               
                return val;
            }
        }


        public int GetLastTimeSheetId()
        {
            using (var _context = new AdminPortalEntities())
            {
                var lastTS = _context.TimeSheetsMasters.OrderByDescending(o => o.TimeSheetID).FirstOrDefault();
                if (lastTS != null)
                {
                    return lastTS.TimeSheetID;
                }
                else
                {
                    return 0;
                }

            }
        }


        public TimeSheetsMaster CheckTSStartDateExists(string userId,DateTime startDate,DateTime endDate, int? timeSheetId = null)
        {
            using (var _context = new AdminPortalEntities())
            {
                if(timeSheetId != null)
                {
                    var lastTS = _context.TimeSheetsMasters.FirstOrDefault(x => (x.TimeSheetID != timeSheetId) && (x.UserID == userId) && (x.IsDeleted == false) && ((x.StartDate <= startDate && x.EndDate >= endDate) || (x.StartDate >= startDate && x.StartDate <= endDate) || (x.EndDate >= startDate && x.EndDate <= endDate)));
                    return lastTS;
                }
                else
                {
                    var lastTS = _context.TimeSheetsMasters.FirstOrDefault(x => (x.UserID == userId) && (x.IsDeleted == false) && ((x.StartDate <= startDate && x.EndDate >= endDate) || (x.StartDate >= startDate && x.StartDate <= endDate) || (x.EndDate >= startDate && x.EndDate <= endDate)));
                    return lastTS;
                }
                
                

            }
        }

        public LOA GetLOABetweenDates(DateTime fromDate, DateTime toDate, string userId)
        {
            using (var _context = new AdminPortalEntities())
            {
                
                    var lastTS = _context.LOAs.FirstOrDefault(x =>( ((x.StartDate >= fromDate &&x.EndDate <= toDate) || (x.StartDate <= fromDate && x.EndDate >= fromDate) || (x.StartDate <= toDate && x.EndDate >= toDate)) && (x.UserId == userId) && (x.IsDeleted == false)) );
                    return lastTS;
            }
        }

        public List<LOA> GetLOABetweenDatesRepoMan(DateTime fromDate, DateTime toDate, string userId)
        {
            using (var _context = new AdminPortalEntities())
            {

                var loas = _context.LOAs.Where(x => (((x.StartDate >= fromDate && x.EndDate <= toDate) || (x.StartDate <= fromDate && x.EndDate >= fromDate) || (x.StartDate <= toDate && x.EndDate >= toDate)) && (x.UserId == userId) && (x.IsDeleted == false) && (x.LookupCodeMaster2.LookupCodeName != "Saved"))).Include(x => x.AspNetUser).Include(x => x.LookupCodeMaster2).ToList();
                return loas;
            }
        }


        public int GetPendingTimesheetsCount(string userId = null) // with user id search for reporting manager
        {
            using (var _context = new AdminPortalEntities())
            {
                if(userId != null)
                {
                    var no = _context.TimeSheetsMasters.Where(x => x.IsDeleted == false && x.AspNetUser.ReportingManager == userId && x.LookupCodeMaster7.LookupCodeName == "Pending").ToList().Count;
                    return no;
                }
                else
                {
                    var no = _context.TimeSheetsMasters.Where(x => x.IsDeleted == false && x.LookupCodeMaster7.LookupCodeName == "Pending").ToList().Count;
                    return no;
                }
            }
        }



    }
}


