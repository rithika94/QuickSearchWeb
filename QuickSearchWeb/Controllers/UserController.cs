using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuickSearchWeb.Models;
using QuickSearchBusiness.Services;
using QuickSearchData;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using QuickSearchWeb;
using Microsoft.AspNet.Identity.Owin;
using AutoMapper;
using System.Web.Configuration;

namespace QuickSearch.Controllers
{
    [Authorize(Roles = "EmployeeAdmin")]
    public class UserController : Controller
    {
       

        private ApplicationUserManager _userManager;
        private readonly AdminPortalEntities _context;
       


        public UserController()
        {

        }
        public UserController(ApplicationUserManager userManager, AdminPortalEntities context  )
        {
            UserManager = userManager;
            _context = context;
        }
        
        
     
        // GET: User
        public ActionResult Index()
        {
            UserService userService = new UserService();
            var users = userService.GetUserList();
            List<UserListViewModel> userList = new List<UserListViewModel>();

            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            foreach (AspNetUser u in users)
            {
                if(u.Id.Equals(User.Identity.GetUserId()))
                {
                    continue;
                }
                UserListViewModel ulvm = new UserListViewModel();
                ulvm.Email = u.Email;
                ulvm.EmployeeCode = u.EmployeeCode;
                ulvm.FirstName = u.Firstname;
                ulvm.LastName = u.LastName;
                ulvm.UserName = u.UserName;
                ulvm.PhoneNumber = u.PhoneNumber;
                ulvm.IsActive = u.IsActive;
                ulvm.IsDeleted = u.IsDeleted;
                if (u.AspNetUser1 != null)
                {
                    ulvm.ReportingManager = u.AspNetUser1.Firstname + " " + u.AspNetUser1.LastName;
                }
                else
                {
                    ulvm.ReportingManager = "N/A";
                }
                
                ulvm.Id = u.Id;
                ulvm.ModifiedDate = u.ModifiedDate;
                userList.Add(ulvm);
            }

            return View(userList);
        }


        public ActionResult RolesList()
        {
            UserService userService = new UserService();
            var users = userService.GetUsersWithRolesList();
            List<RolesViewModel> rolesList = new List<RolesViewModel>();

            foreach(var user in users)
            {
                var role = new RolesViewModel();
                role.EmployeeCode = user.EmployeeCode;
                role.FirstName = user.Firstname;
                role.LastName = user.LastName;
                role.UserName = user.UserName;
                role.IsActive = user.IsActive;
                role.ModifiedDate = user.ModifiedDate;                
                role.ModuleName = "Inventory Management";
                role.ModuleAbbre= "IM";
                role.Admin = false; role.Manager = false; role.ReportingManager = false;  role.User = false;
                if (user.AspNetRoles.SingleOrDefault(x => x.Name == "InventoryAdmin") != null)
                {
                    role.Admin = true;
                }              
                if (user.AspNetRoles.SingleOrDefault(x => x.Name == "InventoryManager") != null)
                {
                    role.Manager = true;
                }
                rolesList.Add(role);

                role = new RolesViewModel();
                role.EmployeeCode = user.EmployeeCode;
                role.FirstName = user.Firstname;
                role.LastName = user.LastName;
                role.UserName = user.UserName;
                role.IsActive = user.IsActive;
                role.ModifiedDate = user.ModifiedDate;
                role.ModuleName = "TS Management";
                role.ModuleAbbre = "TS";
                role.Admin = false; role.Manager = false; role.ReportingManager = false; role.User = false;
                if (user.AspNetRoles.SingleOrDefault(x => x.Name == "TimeSheetAdmin") != null)
                {
                    role.Admin = true;
                }
                if (user.AspNetRoles.SingleOrDefault(x => x.Name == "TimeSheetManager") != null)
                {
                    role.Manager = true;
                }
                if (user.AspNetRoles.SingleOrDefault(x => x.Name == "TimeSheetReportingManager") != null)
                {
                    role.ReportingManager = true;
                }
                if (user.AspNetRoles.SingleOrDefault(x => x.Name == "TimeSheetUser") != null)
                {
                    role.User = true;
                }
                rolesList.Add(role);

                role = new RolesViewModel();
                role.EmployeeCode = user.EmployeeCode;
                role.FirstName = user.Firstname;
                role.LastName = user.LastName;
                role.UserName = user.UserName;
                role.IsActive = user.IsActive;
                role.ModifiedDate = user.ModifiedDate;
                role.ModuleName = "Support";
                role.ModuleAbbre = "SUP";
                role.Admin = false; role.Manager = false; role.ReportingManager = false; role.User = false;
                if (user.AspNetRoles.SingleOrDefault(x => x.Name == "SupportAdmin") != null)
                {
                    role.Admin = true;
                }
                if (user.AspNetRoles.SingleOrDefault(x => x.Name == "SupportManager") != null)
                {
                    role.Manager = true;
                }
                if (user.AspNetRoles.SingleOrDefault(x => x.Name == "SupportEngineer") != null)
                {
                    role.ReportingManager = true;
                }
                if (user.AspNetRoles.SingleOrDefault(x => x.Name == "SupportUser") != null)
                {
                    role.User = true;
                }
                rolesList.Add(role);

                role = new RolesViewModel();
                role.EmployeeCode = user.EmployeeCode;
                role.FirstName = user.Firstname;
                role.LastName = user.LastName;
                role.UserName = user.UserName;
                role.IsActive = user.IsActive;
                role.ModifiedDate = user.ModifiedDate;
                role.ModuleName = "EPAL";
                role.ModuleAbbre = "EPAl";
                role.Admin = false; role.Manager = false; role.ReportingManager = false; role.User = false;
                if (user.AspNetRoles.SingleOrDefault(x => x.Name == "EPALUser") != null)
                {
                    role.User = true;
                }
                rolesList.Add(role);

                role = new RolesViewModel();
                role.EmployeeCode = user.EmployeeCode;
                role.FirstName = user.Firstname;
                role.LastName = user.LastName;
                role.UserName = user.UserName;
                role.IsActive = user.IsActive;
                role.ModifiedDate = user.ModifiedDate;
                role.ModuleName = "Employee";
                role.ModuleAbbre = "EMP";
                role.Admin = false; role.Manager = false; role.ReportingManager = false; role.User = false;
                if (user.AspNetRoles.SingleOrDefault(x => x.Name == "EmployeeAdmin") != null)
                {
                    role.Admin = true;
                }
                if (user.AspNetRoles.SingleOrDefault(x => x.Name == "EmployeeManager") != null)
                {
                    role.Manager = true;
                }
                rolesList.Add(role);
            }
            return PartialView("_EmployeeRolesList", rolesList);
        }
        

        // GET: User/Details/5
        public ActionResult Details(EmployeeDetailsViewModel e)
        {

            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }
            return View(e);
        }

        //[Authorize(Roles = "UserAdmin")]
        // GET: User/Create
        public ActionResult Create()
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            UserViewModel uvm = new UserViewModel();
            uvm.IsActive = true;
            uvm.JoiningDate = DateTime.Now;
            uvm.GenderList = GetDropDownList("Gender", null);
            return View(uvm);
        }

        //[Authorize(Roles = "UserAdmin")]
        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserViewModel uvm)
        {
            try
            {
                uvm.GenderList = GetDropDownList("Gender", null);

                if (!ModelState.IsValid)
                {
                    return View(uvm);
                }
                var userNameExists = IsUserNameExists(uvm.UserName, null);
                if (userNameExists)
                {
                    ViewBag.UniqueUserName = "User Name already Exists. Please change.";
                    return View(uvm);
                }

                var emailExists = IsEmailExists(uvm.Email, null);
                if (emailExists)
                {
                    ViewBag.UniqueEmail = "Email already Exists. Please change.";
                    return View(uvm);
                }
                UserService userService = new UserService();
                
                int random = 1000 + userService.GetUserCount();

                if(uvm.ReportingManagerId == null)
                {
                    if(WebConfigurationManager.AppSettings["HREmail"] != null)
                    {
                       var hr =  UserManager.FindByEmail(WebConfigurationManager.AppSettings["HREmail"]);
                        if(hr != null)
                        {
                            uvm.ReportingManagerId = hr.Id;
                        }
                    }
                }

               
                var appuser = new ApplicationUser()
                {

                    UserName = uvm.UserName,
                    Email = uvm.Email,
                    PhoneNumber = uvm.PhoneNumber,
                    FirstName = uvm.FirstName,

                    JoiningDate = uvm.JoiningDate,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = User.Identity.GetUserId(),

                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,


                    
                    LastName = uvm.LastName,
                    LookupGender = uvm.LookupGender,
                    EmployeeCode = uvm.FirstName.Substring(0, 1).ToUpper() + uvm.LastName.Substring(0, 1).ToUpper() +"-"+ uvm.GenderList.Single(x => x.Id == uvm.LookupGender).Name.ToString().Substring(0, 1) + uvm.JoiningDate.Month.ToString("00") + (uvm.JoiningDate.Year).ToString().Substring(2, 2) + "-" + random,                    
                    IsActive = uvm.IsActive,
                    ReportingManager = uvm.ReportingManagerId,
                    IsDeleted = false,
                    ModifiedUserId = User.Identity.GetUserId(),
                    ModifiedDate = DateTime.Now,
                    
                };


                var result = await UserManager.CreateAsync(appuser, uvm.Password);

                if(result.Succeeded)
                {
                    

                    
                    await SaveUserRoles(appuser,uvm);
                    UserInfo userInfo = new UserInfo();
                    userInfo.UserId = appuser.Id;
                    userInfo.IsActive = true;
                    userInfo.IsDeleted = false;
                    userInfo.CreatedDate = DateTime.Now;
                    userInfo.CreatedUserId = User.Identity.GetUserId();
                    userInfo.ModifiedDate = DateTime.Now;
                    userInfo.ModifiedUserId = User.Identity.GetUserId();
                    userService.CreateNewUserInfo(userInfo);

                    EmployeeDetailsViewModel edvm = new EmployeeDetailsViewModel();
                    edvm.Id = appuser.Id;
                    edvm.FirstName = appuser.FirstName;
                    edvm.LastName = appuser.LastName;
                    edvm.Email = appuser.Email;
                    edvm.ReportingManager = appuser.ReportingManager;
                    edvm.UserName = appuser.UserName;
                    edvm.Password = uvm.Password;
                    edvm.EmployeeCode = appuser.EmployeeCode;


                    var c = await UserManager.GenerateEmailConfirmationTokenAsync(appuser.Id);
                    var callbackUrl = Url.Action(
                       "ConfirmEmail", "Account",
                       new { userId = appuser.Id, code = c },
                       protocol: Request.Url.Scheme);

                    string reportingManName = "Not Assigned";
                   
                    if (edvm.ReportingManager != null)
                    {
                       var reportM = UserManager.FindById(edvm.ReportingManager);
                        if(reportM != null)
                        {
                            reportingManName = reportM.FirstName + " " + reportM.LastName;
                        }
                    }

                    await UserManager.SendEmailAsync(appuser.Id,
                       "Confirm your email",
                       "Login credentials for QuickSearch Username: " + edvm.UserName + ", Password :" + edvm.Password + ", EmployeeCode :" + edvm.EmployeeCode + ", ReportingManager :" + reportingManName + " Please confirm your email by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");


                    return RedirectToAction("Details","User", edvm);
                }

                
                TempData["status"] = "Employee successfully added";

                return RedirectToAction("Index", "User");
            }
            catch (Exception ex)
            {
                uvm.GenderList = GetDropDownList("Gender", null);
                TempData["error"] = "Something went wrong while adding user";
                return View(uvm);
            }
        }

       // [Authorize(Roles = "UserAdmin")]
        // GET: User/Edit/5
        public async Task<ActionResult> Edit(string uId)
        {

            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            if(uId.Equals(User.Identity.GetUserId()))
            {
                TempData["error"] = "You cant edit your details";
                return RedirectToAction("Index");
            }

            //var uId = id;
          
            UserService userService = new UserService();
              UserViewModel roles = await GetUserRoles(uId);
              var user = userService.GetUserWithId(uId);

            UserViewModel usvm = new UserViewModel();

           
            usvm.ReportingManagerId = user.ReportingManager;
         
            
            usvm.FirstName = user.Firstname;
            usvm.LookupGender = user.LookupGender;
            usvm.LastName = user.LastName;
            usvm.Email = user.Email;
            usvm.UserName = user.UserName;
            usvm.Id = user.Id;
            usvm.PhoneNumber = user.PhoneNumber;
            usvm.IsActive = user.IsActive;

            if (usvm.ReportingManagerId != null)
            {
                var rm = userService.GetUserWithId(usvm.ReportingManagerId);
                usvm.ReportingManagerUsername = rm.Firstname + " " + rm.LastName + " (" + rm.UserName + ")";
            }

            usvm.EPALRole = roles.EPALRole;
            usvm.HrRole = roles.HrRole;
            usvm.ContractRole = roles.ContractRole;
            usvm.InventoryRole = roles.InventoryRole;
            usvm.RecruitmentRole = roles.RecruitmentRole;
            usvm.TimeSheetAdmin = roles.TimeSheetAdmin;
            usvm.TimeSheetManager = roles.TimeSheetManager;
            usvm.TimeSheetRepoMan = roles.TimeSheetRepoMan;
            usvm.TimeSheetUser = roles.TimeSheetUser;
            usvm.SupportAdmin = roles.SupportAdmin;
            usvm.SupportManager = roles.SupportManager;
            usvm.SupportEngineer = roles.SupportEngineer;
            usvm.SupportUser = roles.SupportUser;           
            usvm.EmployeeRole = roles.EmployeeRole;
            usvm.JoiningDate = user.JoiningDate;
            usvm.EmployeeCode = user.EmployeeCode;           
            
            usvm.GenderList = GetDropDownList("Gender", null);

            return View(usvm);
        }

        //[Authorize(Roles = "UserAdmin")]
        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserViewModel userViewModel)
        {
            try
            {
                userViewModel.GenderList = GetDropDownList("Gender", null);
                
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
                if (!ModelState.IsValid)
                {
                    return View(userViewModel);
                }

                var userNameExists = IsUserNameExists(userViewModel.UserName, userViewModel.Id);
                if (userNameExists)
                {
                    ViewBag.UniqueUserName = "User Name already Exists. Please change.";
                    return View(userViewModel);
                }
                var emailExists = IsEmailExists(userViewModel.Email, userViewModel.Id);
                if (emailExists)
                {
                    ViewBag.UniqueEmail = "Email already Exists. Please change.";
                    return View(userViewModel);
                }

                ApplicationUser user = UserManager.FindById(userViewModel.Id);

                bool emailChanged = false;
                bool userNameChanged = false;
                
                if (userViewModel.Email != user.Email) // if email changed
                {
                    emailChanged = true;
                }
                if (userViewModel.UserName != user.UserName) // if email changed
                {
                    userNameChanged = true;
                }

                // TODO: Add update logic here
                // AspNetUser aspuser = new AspNetUser();
                UserService userService = new UserService();
                UserViewModel usvm = new UserViewModel();


                user.IsActive = userViewModel.IsActive;               
                user.Email = userViewModel.Email;
                user.UserName = userViewModel.UserName;
                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;
                string phone;
                if (userViewModel.PhoneNumber != null)
                {
                    phone = new string(userViewModel.PhoneNumber.Where(char.IsDigit).ToArray());
                }
                else
                {
                    phone = userViewModel.PhoneNumber;
                }

                user.PhoneNumber = phone;
                user.LookupGender = userViewModel.LookupGender;              
                user.EmployeeCode = userViewModel.EmployeeCode;
                user.JoiningDate = userViewModel.JoiningDate;
                user.ReportingManager = userViewModel.ReportingManagerId;
                user.ModifiedDate = DateTime.Now;
                user.ModifiedUserId = User.Identity.GetUserId();
                if (emailChanged)
                {
                    user.EmailConfirmed = false;
                }



                    var result = await UserManager.UpdateAsync(user);
                

                if (result.Succeeded)
                {
                    if(emailChanged)
                    {
                        // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        var c = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var callbackUrl = Url.Action(
                           "ConfirmEmail", "Account",
                           new { userId = user.Id, code = c },
                           protocol: Request.Url.Scheme);

                        await UserManager.SendEmailAsync(user.Id,
                           "Confirm your email",
                           "Please confirm your email by clicking this link: <a href=\""
                                                           + callbackUrl + "\">link</a>");
                    }

                    if (userViewModel.ChangePassword)
                    {
                        var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                        var result2 = await UserManager.ResetPasswordAsync(user.Id, token, userViewModel.NewPassword);
                    }
                    

                    var createdUser = UserManager.FindByName(user.UserName);
                     await SaveUserRoles(createdUser, userViewModel);

                    

                    if (userViewModel.ChangePassword || userNameChanged )
                    {

                        EmployeeDetailsViewModel edvm = new EmployeeDetailsViewModel();

                        edvm.Id = user.Id;
                        edvm.FirstName = user.FirstName;
                        edvm.LastName = user.LastName;
                        edvm.Email = user.Email;
                        edvm.UserName = user.UserName;
                        edvm.Password = userViewModel.NewPassword;
                        edvm.EmployeeCode = user.EmployeeCode;
                        edvm.ReportingManager = userViewModel.ReportingManagerId;




                        var c = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var callbackUrl = Url.Action(
                           "ConfirmEmail", "Account",
                           new { userId = user.Id, c = c },
                           protocol: Request.Url.Scheme);

                        await UserManager.SendEmailAsync(user.Id,
                           "Confirm your email",
                           "Your new login credentials for QuickSearch   Username: " + edvm.UserName + ", Password :" + edvm.Password );
                        TempData["status"] = "Employee successfully Edited";
                        return RedirectToAction("Details", "User", edvm);


                    }
                }
                TempData["status"] = "Successfully updated employee details.";
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["error"] = "Something went wrong while updating employee details.";
                userViewModel.GenderList = GetDropDownList("Gender", null);
                return View(userViewModel);
            }
        }

       
        
   

        public ActionResult SoftDelete(string id)
        {
            try
            {
                UserService userService = new UserService();


                if (userService.HasEmployeesAssociated(id))
                {
                    TempData["error"] = "This employee is reporting manager to other employees. please change that and delete again";
                    return RedirectToAction("Index");
                }

                ApplicationUser user = UserManager.FindById(id);
                user.IsActive = false;
                user.IsDeleted = true;
                var result = UserManager.Update(user);

                if (result.Succeeded)
                {
                    var userInfo = userService.GetUserInfo(id);
                    if (userInfo != null)
                    {
                        userInfo.IsActive = false;
                        userInfo.IsDeleted = true;
                        userInfo.ModifiedUserId = User.Identity.GetUserId();
                        userInfo.ModifiedDate = DateTime.Now;
                        userService.SoftDeleteUserInfo(userInfo);
                    }
                }
                TempData["status"] = "Employee successfully deleted";
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["error"] = "Something went wrong while deleting employee";
                return RedirectToAction("Index");
            }
        }

        //To Do
      
        //public ActionResult HardDelete(string id)
        //{

        //    try
        //    {
                
        //        UserService userService = new UserService();

        //        if (userService.HasEmployeesAssociated(id))
        //        {
        //            TempData["error"] = "This employee is reporting manager to other employees. please change that and delete again";
        //            return RedirectToAction("Index");
        //        }


        //        userService.HardDeleteUserInfo(id);
               

        //        var user = UserManager.FindById(id);
        //        var result = UserManager.Delete(user);
               

               
        //        TempData["status"] = "Employee successfully hard deleted";
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["error"] = "Something went wrong while deleting employee";
        //        return RedirectToAction("Index");
        //    }

        //}




        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        private async Task SaveUserRoles(ApplicationUser user, UserViewModel uvm)
        {
            List<string> roles = new List<string>();
            if (uvm.SupportAdmin == true)
            {
                roles.Add("SupportAdmin");
            }
            if (uvm.SupportEngineer == true)
            {
                roles.Add("SupportEngineer");
            }
            if (uvm.SupportManager == true)
            {
                roles.Add("SupportManager");
            }
            if (uvm.SupportUser == true)
            {
                roles.Add("SupportUser");
            }

          

            if (uvm.HrRole == "Admin")
            {
                roles.Add("HRAdmin");
            }
            else if(uvm.HrRole == "Manager")
            {
                roles.Add("HRManager");
            }

            if (uvm.RecruitmentRole == "Admin")
            {
                roles.Add("RecruitmentAdmin");
            }
            else if (uvm.RecruitmentRole == "Manager")
            {
                roles.Add("RecruitmentManager");
            }

            if (uvm.InventoryRole == "Admin")
            {
                roles.Add("InventoryAdmin");
            }
            else if (uvm.InventoryRole == "Manager")
            {
                roles.Add("InventoryManager");
            }

            if (uvm.TimeSheetAdmin == true)
            {
                roles.Add("TimeSheetAdmin");
            }
            if (uvm.TimeSheetManager == true)
            {
                roles.Add("TimeSheetManager");
            }
            if (uvm.TimeSheetRepoMan == true)
            {
                roles.Add("TimeSheetReportingManager");
            }
            if (uvm.TimeSheetUser == true)
            {
                roles.Add("TimeSheetUser");
            }

            if (uvm.ContractRole == "Admin")
            {
                roles.Add("ContractAdmin");
            }
            else if (uvm.ContractRole == "Manager")
            {
                roles.Add("ContractManager");
            }

            if (uvm.EmployeeRole == "Admin")
            {
                roles.Add("EmployeeAdmin");
            }

            if (uvm.EPALRole == "User")
            {
                roles.Add("EPALUser");
            }

            await UserManager.RemoveFromRolesAsync(user.Id, UserManager.GetRoles(user.Id).ToArray());
            await UserManager.AddToRolesAsync(user.Id, roles.ToArray());
            await UserManager.UpdateAsync(user);
        }

        private async Task<UserViewModel> GetUserRoles(string userId)
        {
            UserViewModel uvm = new UserViewModel();
            
            var allRoles = await UserManager.GetRolesAsync(userId);
            var roles = allRoles.ToArray();

            if (roles.Contains("SupportAdmin"))
            {
                uvm.SupportAdmin = true;
            }
            if (roles.Contains("SupportEngineer"))
            {
                uvm.SupportEngineer = true;
            }
            if(roles.Contains("SupportManager"))
            {
                uvm.SupportManager = true;
            }
            if(roles.Contains("SupportUser"))
            {
                uvm.SupportUser = true;
            }

            if (roles.Contains("HRAdmin"))
            {
                uvm.HrRole = "Admin";
            }
            else if(roles.Contains("HRManager"))
            {
                uvm.HrRole = "Manager";
            }
            else
            {
                uvm.HrRole = "None";
            }

            if (roles.Contains("ContractAdmin"))
            {
                uvm.ContractRole = "Admin";
            }
            else if (roles.Contains("ContractManager"))
            {
                uvm.ContractRole = "Manager";
            }
            else
            {
                uvm.ContractRole = "None";
            }

            if (roles.Contains("InventoryAdmin"))
            {
                uvm.InventoryRole = "Admin";
            }
            else if (roles.Contains("InventoryManager"))
            {
                uvm.InventoryRole = "Manager";
            }
            else
            {
                uvm.InventoryRole = "None";
            }

            if (roles.Contains("RecruitmentAdmin"))
            {
                uvm.RecruitmentRole = "Admin";
            }
            else if (roles.Contains("RecruitmentManager"))
            {
                uvm.RecruitmentRole = "Manager";
            }
            else
            {
                uvm.RecruitmentRole = "None";
            }

            if (roles.Contains("TimeSheetAdmin"))
            {
                uvm.TimeSheetAdmin = true;
            }
            if (roles.Contains("TimeSheetManager"))
            {
                uvm.TimeSheetManager = true;
            }
            if (roles.Contains("TimeSheetReportingManager"))
            {
                uvm.TimeSheetRepoMan = true;
            }
            if (roles.Contains("TimeSheetUser"))
            {
                uvm.TimeSheetUser = true;
            }
            

            if (roles.Contains("EmployeeAdmin"))
            {
                uvm.EmployeeRole = "Admin";
            }
            else
            {
                uvm.EmployeeRole = "None";
            }

            if (roles.Contains("EPALUser"))
            {
                uvm.EPALRole = "User";
            }
            else
            {
                uvm.EPALRole = "None";
            }

            return uvm;
        }

        private bool IsUserNameExists(string userName, string userId)
        {
            var existingAccount = UserManager.FindByName(userName);
            if (existingAccount != null && existingAccount.Id != userId)
            {
                return true;
            }
            return false;
        }
        private bool IsEmailExists(string email, string userId)
        {
            var existingAccount = UserManager.FindByEmail(email);
            if (existingAccount != null && existingAccount.Id != userId)
            {
                return true;
            }
            return false;
        }
        private List<DropDownListViewModel> GetDropDownList(string lookupTypeName, int? lookupCodeId)
        {

            var _list = new List<DropDownListViewModel>();

            ProductService productService = new ProductService();
            List<LookupCodeMaster> lookUpCategories = new List<LookupCodeMaster>();
            if (lookupTypeName != null)
            {
                lookUpCategories = productService.GetProductDropDownList(lookupTypeName);
            }
            else if (lookupCodeId != null)
            {
                lookUpCategories = productService.GetProductDropDownList(lookupCodeId ?? 0);
            }


            foreach (LookupCodeMaster cat in lookUpCategories)
            {
                DropDownListViewModel ddl = new DropDownListViewModel();

                ddl.Id = cat.LookupCodeId;
                ddl.Name = cat.LookupCodeName;
                _list.Add(ddl);
            }

            return _list;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

             
            }

            base.Dispose(disposing);
        }


        public JsonResult GetSearchValue(string search,string userId)
        {
           UserService userservice = new UserService();
            var obj = userservice.GetListOfUserStartsWith(search, userId);
            var result = obj.Select(a => new { Name = a.Firstname + " " + a.LastName + " (" + a.UserName + ")", Value = a.Id });           
            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}
