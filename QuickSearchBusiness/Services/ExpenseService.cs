using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickSearchData;
using System.Data.Entity;

namespace QuickSearchBusiness.Services
{
    public class ExpenseService
    {

        public bool CreateExpense(ExpenseMaster expense)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.ExpenseMasters.Add(expense);
                _context.SaveChanges();
                expense.ExpenseCode = "Ex" + (1100 + expense.ExpenseId).ToString();
                _context.ExpenseMasters.Attach(expense);
                _context.Entry(expense).State = EntityState.Modified;
                _context.SaveChanges();
                //_context.ExpenseSummaries.AddRange(expense.ExpenseSummaries);
                //_context.SaveChanges();
                return true;
            }

        }



        public bool CreateExpenseSummary(ExpenseSummary summary)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.ExpenseSummaries.Add(summary);
                _context.SaveChanges();
                return true;
            }

        }

        public ExpenseMaster GetExpenseForRepoManager(int id, string userId = null)
        {
            using (var _context = new AdminPortalEntities())
            {
                ExpenseMaster expense = new ExpenseMaster();

                if (userId != null)
                {
                    expense = _context.ExpenseMasters.Where(x => x.ExpenseId == id && x.AspNetUser.AspNetUser1.Id == userId && x.IsDeleted == false).Include(x => x.AspNetUser).Include(x => x.AspNetUser.AspNetUser1).Include(x => x.LookupCodeMaster).FirstOrDefault();

                }
                else
                {
                    expense = _context.ExpenseMasters.Where(x => x.ExpenseId == id && x.IsDeleted == false).Include(x => x.AspNetUser).Include(x => x.AspNetUser.AspNetUser1).Include(x => x.LookupCodeMaster).FirstOrDefault();

                }

                return expense;
            }

        }

        public ExpenseSummary GetExpenseSummary(int id)
        {
            using (var _context = new AdminPortalEntities())
            {
                
                var expenseSummary = _context.ExpenseSummaries.SingleOrDefault(x => x.Id == id);

                return expenseSummary;
            }

        }
       

        public List<ExpenseSummary> GetExpenseSummaries(int expenseId)
        {
            using (var _context = new AdminPortalEntities())
            {
                List<ExpenseSummary> expenseSummaries = new List<ExpenseSummary>();


                expenseSummaries = _context.ExpenseSummaries.Where(x => x.ExpenseId == expenseId).ToList();
                
                return expenseSummaries;
            }

        }

        public void UpdateExpenseSummary(ExpenseSummary es)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.ExpenseSummaries.Attach(es);
                _context.Entry(es).State = EntityState.Modified;
                _context.SaveChanges();                
            }

        }
        public void RemoveExpenseSummary(int id)
        {
            using (var _context = new AdminPortalEntities())
            {
                var expenseSummary = _context.ExpenseSummaries.Where(x => x.ExpenseId == id).FirstOrDefault();
                _context.ExpenseSummaries.Remove(expenseSummary);
                _context.SaveChanges();
            }

        }



        public ExpenseMaster GetMyExpense(int id, string userId = null)
        {
            using (var _context = new AdminPortalEntities())
            {
                ExpenseMaster expense = new ExpenseMaster();

                if (userId != null)
                {
                    expense = _context.ExpenseMasters.Where(x => x.ExpenseId == id && x.UserId == userId && x.IsDeleted == false).Include(x => x.AspNetUser).Include(x => x.AspNetUser.AspNetUser1).Include(x => x.LookupCodeMaster).FirstOrDefault();

                }
                else
                {
                    expense = _context.ExpenseMasters.Where(x => x.ExpenseId == id && x.IsDeleted == false).Include(x => x.AspNetUser).Include(x => x.AspNetUser.AspNetUser1).Include(x => x.LookupCodeMaster).FirstOrDefault();

                }

                return expense;
            }

        }

        public ExpenseMaster GetJustExpense(int id, string userId = null)
        {
            using (var _context = new AdminPortalEntities())
            {
                ExpenseMaster expense = new ExpenseMaster();

                if (userId != null)
                {
                    expense = _context.ExpenseMasters.Where(x => x.ExpenseId == id && x.IsDeleted == false && x.AspNetUser.ReportingManager == userId).FirstOrDefault();

                }
                else
                {
                    expense = _context.ExpenseMasters.Where(x => x.ExpenseId == id && x.IsDeleted == false).FirstOrDefault();
                }

                return expense;
            }

        }

        public ExpenseMaster GetJustMyExpense(int id, string userId = null)
        {
            using (var _context = new AdminPortalEntities())
            {
                ExpenseMaster expense = new ExpenseMaster();

                if (userId != null)
                {
                    expense = _context.ExpenseMasters.Where(x => x.ExpenseId == id && x.UserId == userId && x.IsDeleted == false).FirstOrDefault();

                }
                else
                {
                    expense = _context.ExpenseMasters.Where(x => x.ExpenseId == id && x.IsDeleted == false).FirstOrDefault();

                }

                return expense;
            }

        }


        public List<ExpenseMaster> GetExpenseListWithUserId(string userId)
        {
            using (var _context = new AdminPortalEntities())
            {
                var expenseList = _context.ExpenseMasters.Where(x => (x.UserId == userId) && (x.IsDeleted == false)).Include(x =>x.AspNetUser).Include(x =>x.LookupCodeMaster).ToList();
                return expenseList;
            }
        }

        public List<ExpenseMaster> GetEmployeePendingExpense()
        {
            using (var _context = new AdminPortalEntities())
            {
                var expenseList = _context.ExpenseMasters.Where(x => x.LookupCodeMaster.LookupCodeName == "Pending").Include(x => x.AspNetUser)/*.Include(x => x.AspNetUser.ReportingManager)*/.Include(x => x.LookupCodeMaster).ToList();
                return expenseList;
            }
        }

        public List<ExpenseMaster> GetEmployeeRemainingExpense()
        {
            using (var _context = new AdminPortalEntities())
            {
                var expenseList = _context.ExpenseMasters.Where(x => x.LookupCodeMaster.LookupCodeName == "Approved" || x.LookupCodeMaster.LookupCodeName == "Rejected").Include(x => x.AspNetUser)/*.Include(x => x.AspNetUser.ReportingManager)*/.Include(x => x.LookupCodeMaster).ToList();
                return expenseList;
            }
        }

        public List<ExpenseMaster> GetReportingManagerPendingExpense(string RMuserId)


        {
            using (var _context = new AdminPortalEntities())
            {
                var expenseList = _context.ExpenseMasters.Where(x => (x.LookupCodeMaster.LookupCodeName == "Pending") && (x.AspNetUser.ReportingManager == RMuserId)).Include(x => x.AspNetUser)/*.Include(x => x.AspNetUser.ReportingManager)*/.Include(x => x.LookupCodeMaster).ToList();
                return expenseList;
            }
        }

        public List<ExpenseMaster> GetReportingManagerRemainingExpense(string RMuserId)
        {
            using (var _context = new AdminPortalEntities())
            {
                var expenseList = _context.ExpenseMasters.Where(x => (x.LookupCodeMaster.LookupCodeName == "Approved" || x.LookupCodeMaster.LookupCodeName == "Rejected") && (x.AspNetUser.ReportingManager == RMuserId)).Include(x => x.AspNetUser)/*.Include(x => x.AspNetUser.ReportingManager)*/.Include(x => x.LookupCodeMaster).ToList();
                return expenseList;
            }
        }
        public bool UpdateExpense(ExpenseMaster expense)
        {

            using (var _context = new AdminPortalEntities())
            {
                _context.ExpenseMasters.Attach(expense);
                _context.Entry(expense).State = EntityState.Modified;
                foreach (ExpenseSummary s in expense.ExpenseSummaries)
                {
                    _context.ExpenseSummaries.Attach(s);
                    _context.Entry(s).State = EntityState.Modified;
                }
                _context.SaveChanges();
                return true;
            }

        }

        public bool CheckForDelete(int expenseId, string userId = null)
        {
            using (var _context = new AdminPortalEntities())
            {
                if (userId != null)
                {
                    var support = _context.ExpenseMasters.SingleOrDefault(s => s.ExpenseId == expenseId && s.UserId == userId);
                    if (support != null)
                    {

                        return true;
                    }
                    return false;
                }
                else
                {
                    var support = _context.ExpenseMasters.SingleOrDefault(s => s.ExpenseId == expenseId);
                    if (support != null)
                    {

                        return true;
                    }
                    return false;
                }


            }
        }

        public bool HardDeleteExpense(int id)
        {
            using (var _context = new AdminPortalEntities())
            {
                var summaries = _context.ExpenseSummaries.Where(s => s.ExpenseId == id);
                if (summaries != null)
                {
                    _context.ExpenseSummaries.RemoveRange(summaries);
                    _context.SaveChanges();
                }
                var expense = _context.ExpenseMasters.SingleOrDefault(s => s.ExpenseId == id);
                if (expense != null)
                {
                    _context.ExpenseMasters.Remove(expense);
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

        public bool DeleteExpenseSummary(int id)
        {
            using (var _context = new AdminPortalEntities())
            {
                
                var expenseSummary = _context.ExpenseSummaries.FirstOrDefault(s => s.Id == id);
                if (expenseSummary != null)
                {
                    _context.ExpenseSummaries.Remove(expenseSummary);
                    _context.SaveChanges();
                }
                return true;


            }
        }


        public bool SetExpenseAttachment(ExpenseAttachment expensedoc)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.ExpenseAttachments.Add(expensedoc);
                _context.SaveChanges();
                expensedoc.FileName = "(" + expensedoc.Id + ")" + expensedoc.FileName;
                _context.ExpenseAttachments.Attach(expensedoc);
                _context.Entry(expensedoc).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
        }

        public List<ExpenseAttachment> GetExpenseAttachments(int id)
        {
            using (var _context = new AdminPortalEntities())
            {
                var attachments = _context.ExpenseAttachments.Where(s => s.ExpenseId == id && s.IsDeleted == false).ToList();
                return attachments;
            }
        }

        public bool DeleteExpenseDocumentFile(int id)
        {
            using (var _context = new AdminPortalEntities())
            {
                var expenseAttachment = _context.ExpenseAttachments.SingleOrDefault(s => s.Id == id);
                if (expenseAttachment != null)
                {
                    _context.ExpenseAttachments.Remove(expenseAttachment);
                    _context.SaveChanges();
                }
                return true;
            }
        }

        public int GetPendingExpensesCount(string userId = null) // with user id search for reporting manager
        {
            using (var _context = new AdminPortalEntities())
            {
                if (userId != null)
                {
                    var no = _context.ExpenseMasters.Where(x => x.IsDeleted == false && x.AspNetUser.ReportingManager == userId && x.LookupCodeMaster.LookupCodeName == "Pending").ToList().Count;
                    return no;
                }
                else
                {
                    var no = _context.ExpenseMasters.Where(x => x.IsDeleted == false && x.LookupCodeMaster.LookupCodeName == "Pending").ToList().Count;
                    return no;
                }
            }
        }


    }
}
