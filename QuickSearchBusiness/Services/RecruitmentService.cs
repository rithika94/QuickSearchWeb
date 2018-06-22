using QuickSearchData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace QuickSearchBusiness.Services
{
    public class RecruitmentService
    {
        public int SetRecruitment(Recruitment recruitment)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.Recruitments.Add(recruitment);
                _context.SaveChanges();
                int Rid = recruitment.RecruitmentId;
                return Rid;
            }
        }
        public bool UpdateRecruitment(Recruitment recruitment)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.Recruitments.Attach(recruitment);
                _context.Entry(recruitment).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
        }

        public List<RecruitmentClient> UpdateClient(List<RecruitmentClient> recruitmentClient)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.RecruitmentClients.AddRange(recruitmentClient);
                _context.SaveChanges();

                return recruitmentClient;
            }
        }

        public bool UpdateClientDocument(List<RecruitmentClientDocument> recruitmentClientDocument)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.RecruitmentClientDocuments.AddRange(recruitmentClientDocument);
                _context.SaveChanges();
                return true;
            }
        }

        public bool DeleteClients(int clientId)
        {
            using (var _context = new AdminPortalEntities())
            {
                List<RecruitmentClient> recruitmentClient = _context.RecruitmentClients.Where(x => x.Id == clientId).ToList();
                foreach (var item in recruitmentClient)
                {
                    _context.RecruitmentClientDocuments.RemoveRange(_context.RecruitmentClientDocuments.Where(x => x.RecruitmentClientId == item.Id).ToList());
                }

                _context.RecruitmentClients.RemoveRange(recruitmentClient);
                _context.SaveChanges();
                return true;
            }
        }

        public bool DeleteClientsByRecruitment(int recruitmentId)
        {
            using (var _context = new AdminPortalEntities())
            {
                List<RecruitmentClient> recruitmentClient = _context.RecruitmentClients.Where(x => x.RecruitmentId == recruitmentId).ToList();
                foreach (var item in recruitmentClient)
                {
                    _context.RecruitmentClientDocuments.RemoveRange(_context.RecruitmentClientDocuments.Where(x => x.RecruitmentClientId == item.Id).ToList());
                }

                _context.RecruitmentClients.RemoveRange(recruitmentClient);
                _context.SaveChanges();
                return true;
            }
        }

        public bool DeleteClientDocumentById(string UserId, int FileId, int ClientId)
        {
            using (var _context = new AdminPortalEntities())
            {
                ////RecruitmentClientDocument recruitmentClientDoc = _context.RecruitmentClientDocuments.Where(x => x.Id == FileId).FirstOrDefault();
                ////_context.RecruitmentClientDocuments.Remove(recruitmentClientDoc);
                RecruitmentClient recruitmentClientDoc = _context.RecruitmentClients.Where(x => x.Id == ClientId).FirstOrDefault();
                recruitmentClientDoc.ModifiedUserId = UserId;
                recruitmentClientDoc.ModifiedDate = System.DateTime.Now;
                switch (FileId)
                {
                    case 1:
                        recruitmentClientDoc.ClientDocument1 = null;
                        recruitmentClientDoc.DocumentName1 = null;
                        break;
                    case 2:
                        recruitmentClientDoc.ClientDocument2 = null;
                        recruitmentClientDoc.DocumentName2 = null;
                        break;
                }
                _context.Entry(recruitmentClientDoc).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
        }


        public List<LookupCodeMaster> GetLookUpDropDownList(string lookupTypeName)
        {
            using (var _context = new AdminPortalEntities())
            {
                var list = _context.LookupCodeMasters.Where(l => l.LookupTypeMaster.LookupTypeName == lookupTypeName && l.IsActive == true).ToList();
                return list;
            }
        }

        public List<Recruitment> GetRecruiterList()
        {
            using (var _context = new AdminPortalEntities())
            {
                var recruiterlist = _context.Recruitments.ToList();
                return recruiterlist;
            }
        }

        public Recruitment GetRecruiterWithId(int id)
        {
            using (var _context = new AdminPortalEntities())
            {

                var recruiter = _context.Recruitments.SingleOrDefault(u => u.RecruitmentId == id);
                return recruiter;
            }
        }

        public List<LookupCodeMaster> GetLookUpDropDownList(int lookupCodeId)
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


        public bool SoftDeleteRecruitment(Recruitment employeerecord)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.Recruitments.Attach(employeerecord);
                _context.Entry(employeerecord).State = EntityState.Modified;
                _context.SaveChanges();

                return true;
            }
        }


        public bool HardDeleteRecruitment(int RecruitmentId)
        {
            using (var _context = new AdminPortalEntities())
            {
                var employeerecord = _context.Recruitments.SingleOrDefault(s => s.RecruitmentId == RecruitmentId);
                if (employeerecord != null)
                {
                    _context.Recruitments.Remove(employeerecord);
                    _context.SaveChanges();
                }
                return true;
            }
        }
        public bool SetRecruitmentDocument(RecruitmentDocument recruitmentdoc)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.RecruitmentDocuments.Add(recruitmentdoc);
                _context.SaveChanges();
                //int Rid = recruitment.RecruitmentId;
                return true;
            }
        }
        public List<RecruitmentDocument> GetRecruitFilesWithId(int id)
        {
            using (var _context = new AdminPortalEntities())
            {
                var recruiterfiles = _context.RecruitmentDocuments.Where(x => x.RecruitmentId == id).ToList();
                return recruiterfiles;
            }
        }

        public List<RecruitmentClient> GetRecruitmentClientWithId(int id)
        {
            using (var _context = new AdminPortalEntities())
            {
                var recruiterfiles = _context.RecruitmentClients.Where(x => x.RecruitmentId == id).Include(x => x.RecruitmentClientDocuments).ToList();
                return recruiterfiles;
            }
        }
        public bool DeleteDocumentFile(int Id)
        {
            using (var _context = new AdminPortalEntities())
            {
                var Recruitdocrecord = _context.RecruitmentDocuments.SingleOrDefault(s => s.Id == Id);
                if (Recruitdocrecord != null)
                {
                    _context.RecruitmentDocuments.Remove(Recruitdocrecord);
                    _context.SaveChanges();
                }
                return true;
            }
        }
    }
}
