using QuickSearchData;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSearchBusiness.Services
{

    public class UserService
    {       
        public bool CreateNewUser(AspNetUser user)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.AspNetUsers.Add(user);
                _context.SaveChanges();
                return true;
            }

        }
        public bool CreateNewUserInfo(UserInfo user)
        {
            using (var _context = new AdminPortalEntities())
            {               
                _context.UserInfoes.Add(user);
                _context.SaveChanges();
                return true;
            }

        }

        public List<AspNetUser> GetUserList()
        {
            using (var _context = new AdminPortalEntities())
            {
              var userlist =  _context.AspNetUsers.Where(x => x.IsDeleted == false).Include(x => x.AspNetUser1).ToList();
              return userlist;
            }
        }
        public List<AspNetUser> GetUsersWithRolesList()
        {
            using (var _context = new AdminPortalEntities())
            {
                var userlist = _context.AspNetUsers.Where(x => x.IsDeleted == false).Include(x => x.AspNetRoles).ToList();
                return userlist;
            }
        }

        public   AspNetUser GetUserWithId(string userId)
        {
            using (var _context = new AdminPortalEntities())
            {
                var user = _context.AspNetUsers.Where(u => u.Id == userId).Include(u => u.AspNetUser1).FirstOrDefault();
                return user;
            }
        }

        public int GetUserCount()
        {
            using (var _context = new AdminPortalEntities())
            {
                var user = _context.AspNetUsers.Count();
                return user;
            }
        }
        //public string GetUserIdWithUserName(string userName)
        //{
        //    using (var _context = new AdminPortalEntities())
        //    {
        //        var user = _context.AspNetUsers.SingleOrDefault(s => s.UserName == userName).Id;
        //        return user;
        //    }
        //}

        //public string GetUserNameWithUserId(string Id)
        //{
        //    using (var _context = new AdminPortalEntities())
        //    {
        //        var user = _context.AspNetUsers.SingleOrDefault(s => s.Id == Id).UserName;
        //        return user;
        //    }
        //}

        public List<LookupCodeMaster> GetGenderDropDownList(string lookupTypeName)
        {
            using (var _context = new AdminPortalEntities())
            {
                var list = _context.LookupCodeMasters.Where(l => l.LookupTypeMaster.LookupTypeName == lookupTypeName).ToList();
                return list;
            }
        }
        public UserInfo GetUserInfo(string userId)
        {
            using (var _context = new AdminPortalEntities())
            {
               var userInfo =  _context.UserInfoes.SingleOrDefault(s => s.UserId == userId);
                return userInfo;
            }
        }


        public bool SoftDeleteUserInfo(UserInfo userInfo)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.UserInfoes.Attach(userInfo);
                _context.Entry(userInfo).State = EntityState.Modified;
                _context.SaveChanges();

                return true;
            }
        }


        public bool HardDeleteUserInfo(string userId)
        {
            using (var _context = new AdminPortalEntities())
            {
                var userInfo = _context.UserInfoes.SingleOrDefault(s => s.UserId == userId);
                if(userInfo != null)
                {
                    _context.UserInfoes.Remove(userInfo);
                    _context.SaveChanges();
                }
                return true;
            }
        }
       


        public bool HasEmployeesAssociated(string userId)// Reporting manager for any employees
        {
            using (var _context = new AdminPortalEntities())
            {
                var user = _context.AspNetUsers.Where(s => s.Id == userId).Include(x => x.AspNetUsers1).FirstOrDefault();
                if (user != null)
                {
                    if(user.AspNetUsers1.Count > 0)
                    {
                        return true;
                    }
                }
                return false;

            }
        }

        public List<AspNetUser> GetListOfUserStartsWith(string term, string userId)
        {
            using (var _context = new AdminPortalEntities())
            {
   var model = _context.AspNetUsers.Where(a => a.Firstname.Contains(term) && a.Id != userId && a.IsActive==true && a.IsDeleted == false).ToList()/*.Select(a => new { label = a.UserName })*/;
                return model;
            }

        }

        //public bool IsUserNameExists(string userId, string userName)
        //{

        //    using (var _context = new AdminPortalEntities())
        //    {
        //        var userExist = _context.AspNetUsers.Where(x => x.UserName.Equals(userName) && !x.Id.Equals(userId)).FirstOrDefault();
        //        if (userExist != null)
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //}
    }

}
