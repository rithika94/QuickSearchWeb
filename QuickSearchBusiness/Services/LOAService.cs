using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickSearchData;
using System.Data.Entity;

namespace QuickSearchBusiness.Services
{
    public class LOAService
    {
        public bool CreateLoa(LOA loa)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.LOAs.Add(loa);
                _context.SaveChanges();
                loa.LoaCode = "LOA" + (1100 + loa.LoaId).ToString();
                _context.LOAs.Attach(loa);
                _context.Entry(loa).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }

        }

        public LOA GetMyLOA(int id, string userId)
        {
            using (var _context = new AdminPortalEntities())
            {
                var loa = _context.LOAs.Where(x => (x.LoaId == id) && (x.UserId == userId) && (x.IsDeleted == false)).Include(x => x.AspNetUser).Include(x => x.AspNetUser.AspNetUser1).Include(x => x.LookupCodeMaster).Include(x => x.LookupCodeMaster1).Include(x => x.LookupCodeMaster2).FirstOrDefault();
                return loa;
            }

        }
        public LOA GetJustMyLOA(int id, string userId)
        {
            using (var _context = new AdminPortalEntities())
            {
                var loa = _context.LOAs.Where(x => (x.LoaId == id) && (x.UserId == userId)).FirstOrDefault();
                return loa;
            }

        }

        public LOA GetLOA(int id , string userId = null)
        {
            using (var _context = new AdminPortalEntities())
            {
                var loa = new LOA();
                if(userId != null)
                {
                    loa = _context.LOAs.Where(x => x.LoaId == id && x.AspNetUser.AspNetUser1.Id == userId).Include(x => x.AspNetUser).Include(x => x.AspNetUser.AspNetUser1).Include(x => x.LookupCodeMaster).Include(x => x.LookupCodeMaster1).Include(x => x.LookupCodeMaster2).FirstOrDefault();

                }
                else
                {
                     loa = _context.LOAs.Where(x => x.LoaId == id).Include(x => x.AspNetUser).Include(x => x.AspNetUser.AspNetUser1).Include(x => x.LookupCodeMaster).Include(x => x.LookupCodeMaster1).Include(x => x.LookupCodeMaster2).FirstOrDefault();
                   
                }
                return loa;

            }

        }
        public LOA GetJustLOA(int id)
        {
            using (var _context = new AdminPortalEntities())
            {
                var loa = _context.LOAs.Where(x => x.LoaId == id).FirstOrDefault();
                return loa;
            }

        }

        public bool UpdateLOA(LOA loa)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.LOAs.Attach(loa);
                _context.Entry(loa).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
        }


        public List<LOA> GetLoaListWithUserId(string userId)
        {
            using (var _context = new AdminPortalEntities())
            {
                var loaList = _context.LOAs.Where(x => x.UserId == userId && x.IsDeleted == false).Include(x => x.AspNetUser).Include(x => x.LookupCodeMaster2).ToList();
                return loaList;
            }
        }




        public bool SoftDeleteLOA(int id)
        {
            using (var _context = new AdminPortalEntities())
            {
                var loa = _context.LOAs.SingleOrDefault(s => s.LoaId == id);
                if (loa != null)
                {
                    loa.IsDeleted = true;
                    loa.IsActive = false;
                    _context.LOAs.Attach(loa);
                    _context.Entry(loa).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                return true;
            }
        }

        public bool HardDeleteLOA(int id)
        {
            using (var _context = new AdminPortalEntities())
            {
                var loa = _context.LOAs.SingleOrDefault(s => s.LoaId == id);
                if (loa != null)
                {
                    _context.LOAs.Remove(loa);
                    _context.SaveChanges();
                }
                return true;
            }
        }

        public LookupCodeMaster GetLookupIdForCodeName(string code, string type)
        {
            using (var _context = new AdminPortalEntities())
            {
                var codeList = _context.LookupCodeMasters.Where(s => s.LookupTypeMaster.LookupTypeName == type).ToList();
                var val = codeList.FirstOrDefault(s => s.LookupCodeName == code);
                return val;
            }
        }

        public List<LOA> GetEmployeePendingLOAs()
        {
            using (var _context = new AdminPortalEntities())
            {
                //int pendingId = _context.LookupCodeMasters.Where(x => x.LookupCodeName == "Pending").FirstOrDefault().LookupCodeId;
                var loaList = _context.LOAs.Where(x => x.LookupCodeMaster2.LookupCodeName == "Pending").Include(x => x.AspNetUser).Include("AspNetUser.AspNetUser1").Include(x => x.LookupCodeMaster2).ToList();
                return loaList;
            }
        }

        public List<LOA> GetReportingManagerPendingLOAs(string RMuserId)
        {
            using (var _context = new AdminPortalEntities())
            {
                //int pendingId = _context.LookupCodeMasters.Where(x => x.LookupCodeName == "Pending").FirstOrDefault().LookupCodeId;
                var loaList = _context.LOAs.Where(x => x.AspNetUser.ReportingManager == RMuserId && x.LookupCodeMaster2.LookupCodeName == "Pending").Include(x => x.AspNetUser).Include("AspNetUser.AspNetUser1").Include(x => x.LookupCodeMaster2).ToList();
                return loaList;
            }
        }

        public List<LOA> GetReportingManagerRemianingLOAs( string RMuserId)
        {
            using (var _context = new AdminPortalEntities())
            {
                //int approvedId = _context.LookupCodeMasters.Where(x => x.LookupCodeName == "Approved" || x.LookupCodeName == "Rejected").FirstOrDefault().LookupCodeId;            
                var loaList = _context.LOAs.Where(x => (x.AspNetUser.ReportingManager == RMuserId)
                                                                   && (x.LookupCodeMaster2.LookupCodeName == "Approved" || x.LookupCodeMaster2.LookupCodeName == "Rejected"))
                                                                   .Include(x => x.AspNetUser).Include("AspNetUser.AspNetUser1").Include(x => x.LookupCodeMaster2).ToList();
                return loaList;
            }
        }

        public List<LOA> GetEmployeeRemianingLOAs()
        {
            using (var _context = new AdminPortalEntities())
            {
                var loaList = _context.LOAs.Where(x => (x.LookupCodeMaster2.LookupCodeName == "Approved" || x.LookupCodeMaster2.LookupCodeName == "Rejected"))
                                                               .Include(x => x.AspNetUser).Include("AspNetUser.AspNetUser1").Include(x => x.LookupCodeMaster2).ToList();
                return loaList;

            }
        }


        public int GetPendingLOAsCount(string userId = null) // with user id search for reporting manager
        {
            using (var _context = new AdminPortalEntities())
            {
                if (userId != null)
                {
                    var no = _context.LOAs.Where(x => x.IsDeleted == false && x.AspNetUser.ReportingManager == userId && x.LookupCodeMaster2.LookupCodeName == "Pending").ToList().Count;
                    return no;
                }
                else
                {
                    var no = _context.LOAs.Where(x => x.IsDeleted == false && x.LookupCodeMaster2.LookupCodeName == "Pending").ToList().Count;
                    return no;
                }
            }
        }

    }
}
