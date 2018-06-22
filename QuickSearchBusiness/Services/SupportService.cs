using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickSearchData;
using System.Data.Entity;

namespace QuickSearchBusiness.Services
{
  public  class SupportService
    {
        public void CreateSupport(SupportMaster support)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.SupportMasters.Add(support);
                _context.SaveChanges();
                support.SupportCode = "ST-" + DateTime.Now.ToString("MM")+ DateTime.Now.ToString("yyyy")+ "-"+ (1100 + support.SupportId).ToString();
                _context.SupportMasters.Attach(support);
                _context.Entry(support).State = EntityState.Modified;
                _context.SaveChanges();

                //int Sid = support.SupportId;
                //return Sid;
            }
        }
        public bool UpdateSupport(SupportMaster support)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.SupportMasters.Attach(support);
                _context.Entry(support).State = EntityState.Modified;
                _context.SaveChanges();
                return true;

            }  
           
        }

        public int GetSupportOpenTicketsCount(string id)
        {
            using (var _context = new AdminPortalEntities())
            {
               var openTicketCount =  _context.SupportMasters.Where(x => x.UserId == id && x.LookupCodeMaster.LookupCodeName == "Open").Count();
                return openTicketCount;
            }
    
        }

        public int GetMyPendingRequestsCount(string userId = null) // with user id search for reporting manager
        {
            using (var _context = new AdminPortalEntities())
            {
                
                    var no = _context.SupportMasters.Where(x => x.UserId == userId && (x.LookupCodeMaster1.LookupCodeName =="Open"|| x.LookupCodeMaster1.LookupCodeName == "Assigned"|| x.LookupCodeMaster1.LookupCodeName == "InProgress")).ToList().Count();
                    return no;
                
                
            }
        }


        public int GetMyDoneRequestsCount(string userId = null) // with user id search for reporting manager
        {
            using (var _context = new AdminPortalEntities())
            {

                var no = _context.SupportMasters.Where(x => x.UserId == userId && x.LookupCodeMaster1.LookupCodeName == "Done" ).ToList().Count();
                return no;


            }
        }

        public int GetMyClosedRequestsCount(string userId = null) // with user id search for reporting manager
        {
            using (var _context = new AdminPortalEntities())
            {

                var no = _context.SupportMasters.Where(x => x.UserId == userId && x.LookupCodeMaster1.LookupCodeName == "Closed").ToList().Count();
                return no;


            }
        }

        #region Editing an existing support item 
        public SupportMaster GetSupportWithId(int id,string userId = null)
        {
            using (var _context = new AdminPortalEntities())
            {
                if(!string.IsNullOrEmpty(userId))
                {
                    var support = _context.SupportMasters.Where(s => s.SupportId == id && s.UserId == userId && s.IsDeleted == false).Include(x => x.AspNetUser).Include(x => x.AspNetUser1).Include(x => x.LookupCodeMaster).Include(x => x.LookupCodeMaster1).Include(x => x.LookupCodeMaster2).FirstOrDefault();
                    return support;
                }
                else
                {
                    var support = _context.SupportMasters.Where(s => s.SupportId == id && s.IsDeleted == false).Include(x => x.AspNetUser).Include(x => x.AspNetUser1).Include(x => x.LookupCodeMaster).Include(x => x.LookupCodeMaster1).Include(x => x.LookupCodeMaster2).FirstOrDefault();
                    return support;
                }
                
            }
        }
      

        public SupportMaster GetJustSupportWithId(int id, string userId = null)
        {
            using (var _context = new AdminPortalEntities())
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    var support = _context.SupportMasters.SingleOrDefault(s => s.SupportId == id && s.UserId == userId && s.IsDeleted == false);
                    return support;
                }
                else
                {
                    var support = _context.SupportMasters.SingleOrDefault(s => s.SupportId == id && s.IsDeleted == false);
                    return support;
                }

            }
        }

        public SupportMaster GetSupportForEngineerWithId(int id, string userId = null)
        {
            using (var _context = new AdminPortalEntities())
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    var support = _context.SupportMasters.Where(s => s.SupportId == id && s.AssignTo == userId && s.IsDeleted == false).Include(x => x.AspNetUser).Include(x => x.AspNetUser1).Include(x => x.LookupCodeMaster).Include(x => x.LookupCodeMaster1).Include(x => x.LookupCodeMaster2).FirstOrDefault();
                    return support;
                }
                else
                {
                    var support = _context.SupportMasters.Where(s => s.SupportId == id && s.IsDeleted == false).Include(x => x.AspNetUser).Include(x => x.AspNetUser1).Include(x => x.LookupCodeMaster).Include(x => x.LookupCodeMaster1).Include(x => x.LookupCodeMaster2).FirstOrDefault();
                    return support;
                }

            }
        }



        #endregion
        //public List<SupportMaster> GetMySupportListByUserId(string UserId)
        //{
        //    using (var _context = new AdminPortalEntities())
        //    {
        //        var supportList = _context.SupportMasters.Where(x => x.UserId == UserId).SingleOrDefault();
        //        return GetMySupportListByUserId(UserId);

        //    }

        //}
        #region Get all my support list and my support list by userid



        public List<SupportMaster> GetSupportListForUserId(string userId)
        {
            using (var _context = new AdminPortalEntities())
            {
                var list = _context.SupportMasters.Where(x => x.UserId.Equals(userId) && x.IsDeleted == false ).Include(x => x.AspNetUser).Include(x => x.AspNetUser1).Include(x => x.LookupCodeMaster).Include(x => x.LookupCodeMaster1).Include(x => x.LookupCodeMaster2).ToList();
                return list;
            }
        }
        public List<SupportMaster> GetAllSupportForEngineer(string userId)
        {
            using (var _context = new AdminPortalEntities())
            {
                var support = _context.SupportMasters.Where(s => s.AssignTo == userId && s.IsDeleted == false).Include(x => x.AspNetUser).Include(x => x.AspNetUser1).Include(x => x.LookupCodeMaster).Include(x => x.LookupCodeMaster1).Include(x => x.LookupCodeMaster2).ToList();
                return support;
            }
        }
        public List<SupportMaster> GetAllSupportList()
        {
            using (var _context = new AdminPortalEntities())
            {
                var list = _context.SupportMasters.Where(x => x.IsDeleted == false && x.LookupCodeMaster1.LookupCodeName != "Saved" ).Include(x => x.AspNetUser).Include(x => x.AspNetUser1).Include(x => x.LookupCodeMaster).Include(x => x.LookupCodeMaster1).Include(x => x.LookupCodeMaster2).ToList();
                return list;
            }
        } 
        #endregion
        public List<LookupCodeMaster> GetDropDownList(string lookupTypeName)
        {
            using (var _context = new AdminPortalEntities())
            {
                var list = _context.LookupCodeMasters.Where(l => l.LookupTypeMaster.LookupTypeName == lookupTypeName && l.IsActive == true).ToList();
                return list;
            }
        }
        public List<LookupCodeMaster> GetDropDownList(int lookupCodeId)
        {
            using (var _context = new AdminPortalEntities())
            {
                List<LookupCodeMaster> list = new List<LookupCodeMaster>();
                var lookupCode = _context.LookupCodeMasters.SingleOrDefault(l => l.LookupCodeId == lookupCodeId);
                if (lookupCode != null)
                {
                    list = _context.LookupCodeMasters.Where(l => l.LookupTypeMaster.LookupTypeName == lookupCode.LookupCodeName).ToList();

                }
                return list;

            }
        }
     
        #region Delete support
        //public bool SoftDeleteSupport(SupportMaster supportmaster)
        //{
        //    using (var _context = new AdminPortalEntities())
        //    {
        //        _context.SupportMasters.Attach(supportmaster);
        //        _context.Entry(supportmaster).State = EntityState.Modified;
        //        _context.SaveChanges();
        //    }
        //    return true;
        //}

            public bool CheckForHardDelete(int SupportId, string userId = null)
        {
            using (var _context = new AdminPortalEntities())
            {
                if (userId != null)
                {
                    var support = _context.SupportMasters.SingleOrDefault(s => s.SupportId == SupportId && s.UserId == userId);
                    if (support != null)
                    {
                        
                        return true;
                    }
                    return false;
                }
                else
                {
                    var support = _context.SupportMasters.SingleOrDefault(s => s.SupportId == SupportId);
                    if (support != null)
                    {
                        
                        return true;
                    }
                    return false;
                }


            }
        }

        public bool HardDeleteSupport(int SupportId, string userId = null)
        {
            using (var _context = new AdminPortalEntities())
            {
                if(userId != null)
                {
                    var support = _context.SupportMasters.SingleOrDefault(s => s.SupportId == SupportId && s.UserId == userId);
                    if (support != null)
                    {
                        _context.SupportMasters.Remove(support);
                        _context.SaveChanges();
                        return true;
                    }
                    return false;
                }
                else
                {
                    var support = _context.SupportMasters.SingleOrDefault(s => s.SupportId == SupportId);
                    if (support != null)
                    {
                        _context.SupportMasters.Remove(support);
                        _context.SaveChanges();
                        return true;
                    }
                    return false;
                }
                
               
            }
        }

        #endregion


        public LookupCodeMaster GetLookupIdForStatus(string code, string type)
        {
            using (var _context = new AdminPortalEntities())
            {
                var codeList = _context.LookupCodeMasters.Where(s => s.LookupTypeMaster.LookupTypeName == type).ToList();
                var val = codeList.FirstOrDefault(s => s.LookupCodeName == code);
                return val;
            }
        }

        public List<AspNetUser> UsersWithRoles(string roleName)
        {
            using (var _context = new AdminPortalEntities())
            {
                var users = (from u in _context.AspNetUsers
                            where u.AspNetRoles.Any(r => r.Name == roleName)
                            select u).ToList();

                return users;


            }

        }
        //public string GenerateSupportCode()
        //{
        //    using (var _context = new AdminPortalEntities())
        //    {
        //        var lastsupportid = _context.SupportMasters.OrderByDescending(o => o.SupportId).FirstOrDefault();
        //        int lastsupid = 0;
        //        if (lastsupportid != null)
        //        {
        //            lastsupid= lastsupportid.SupportId;
        //            //return lastsupportid.SupportId ;
        //        }

        //        return "100125405-" + (1100 + lastsupid + 1).ToString();

        //    }
        //}



        public bool SetSupportAttachment(SupportAttachment supportdoc)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.SupportAttachments.Add(supportdoc);
                _context.SaveChanges();
                supportdoc.FileName = "(" + supportdoc.Id + ")" +supportdoc.FileName ;
                _context.SupportAttachments.Attach(supportdoc);
                _context.Entry(supportdoc).State = EntityState.Modified;
                _context.SaveChanges();

                return true;
            }
        }

        public List<SupportAttachment> GetSupportAttachments(int id)
        {
            using (var _context = new AdminPortalEntities())
            {
                var attachments = _context.SupportAttachments.Where(s => s.SupportId == id && s.IsDeleted == false).ToList();
                return attachments;
            }
        }

        
        public bool DeleteSupportDocumentFile(int id)
        {
            using (var _context = new AdminPortalEntities())
            {
                var supportAttachment = _context.SupportAttachments.SingleOrDefault(s => s.Id == id);
                if (supportAttachment != null)
                {
                    _context.SupportAttachments.Remove(supportAttachment);
                    _context.SaveChanges();
                }
                return true;
            }
        }


        


    }
}
