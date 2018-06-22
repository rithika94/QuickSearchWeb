using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using QuickSearchBusiness.Services;
using QuickSearchData;
using QuickSearchWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuickSearchWeb.Controllers
{
    [Authorize]
    public class MyProfileController : Controller
    {


        private ApplicationUserManager _userManager;
        // GET: MyProfile

        
        public ActionResult Index()
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            var uId = User.Identity.GetUserId();

            UserService userService = new UserService();
           // UserViewModel roles = await GetUserRoles(uId);
            var user = userService.GetUserWithId(uId);

            MyProfileViewModel usvm = new MyProfileViewModel();


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


            //usvm.HrRole = roles.HrRole;
            //usvm.ContractRole = roles.ContractRole;
            //usvm.InventoryRole = roles.InventoryRole;
            //usvm.RecruitmentRole = roles.RecruitmentRole;
            //usvm.TimeSheetRole = roles.TimeSheetRole;
            //usvm.EmployeeRole = roles.EmployeeRole;
            usvm.JoiningDate = user.JoiningDate;
            usvm.EmployeeCode = user.EmployeeCode;

            usvm.GenderList = GetDropDownList("Gender", null);

            return View(usvm);
        }



        //[Authorize(Roles = "UserAdmin")]
        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(MyProfileViewModel myProfileViewModel)
        {
            try
            {

                if (TempData["error"] != null)
                {
                    ViewBag.error = TempData["error"];

                }
                if (TempData["status"] != null)
                {
                    ViewBag.status = TempData["status"];
                }

                myProfileViewModel.GenderList = GetDropDownList("Gender", null);

                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
                if (!ModelState.IsValid)
                {
                    return View(myProfileViewModel);
                }

                
                ApplicationUser user = UserManager.FindById(myProfileViewModel.Id);

                

                // TODO: Add update logic here
                // AspNetUser aspuser = new AspNetUser();
                UserService userService = new UserService();
                UserViewModel usvm = new UserViewModel();



                user.Email = myProfileViewModel.Email;
                user.UserName = myProfileViewModel.UserName;
                user.FirstName = myProfileViewModel.FirstName;
                user.LastName = myProfileViewModel.LastName;
                user.PhoneNumber = myProfileViewModel.PhoneNumber;
                user.LookupGender = myProfileViewModel.LookupGender;
                user.EmployeeCode = myProfileViewModel.EmployeeCode;
                user.JoiningDate = myProfileViewModel.JoiningDate;
                user.ReportingManager = myProfileViewModel.ReportingManagerId;
                user.ModifiedDate = DateTime.Now;
                user.ModifiedUserId = User.Identity.GetUserId();
                



                var result = await UserManager.UpdateAsync(user);


                //if (result.Succeeded)
                //{
                //    if (emailChanged)
                //    {
                //        // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                //        // Send an email with this link
                //        var c =  UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                //        var callbackUrl = Url.Action(
                //           "ConfirmEmail", "Account",
                //           new { userId = user.Id, code = c },
                //           protocol: Request.Url.Scheme);

                //         UserManager.SendEmailAsync(user.Id,
                //           "Confirm your email",
                //           "Please confirm your email by clicking this link: <a href=\""
                //                                           + callbackUrl + "\">link</a>");
                //    }

                //    if (userViewModel.ChangePassword)
                //    {
                //        var token = UserManager.GeneratePasswordResetTokenAsync(user.Id);
                //        var result2 = UserManager.ResetPasswordAsync(user.Id, token, userViewModel.NewPassword);
                //    }


                //    var createdUser = UserManager.FindByName(user.UserName);
                //    //SaveUserRoles(createdUser, userViewModel.HrRole, userViewModel.InventoryRole, userViewModel.RecruitmentRole, userViewModel.TimeSheetRole, userViewModel.EmployeeRole, userViewModel.ContractRole);



                //    //if (userViewModel.ChangePassword || userNameChanged)
                //    //{

                //    //    EmployeeDetailsViewModel edvm = new EmployeeDetailsViewModel();

                //    //    edvm.Id = user.Id;
                //    //    edvm.FirstName = user.FirstName;
                //    //    edvm.LastName = user.LastName;
                //    //    edvm.Email = user.Email;
                //    //    edvm.UserName = user.UserName;
                //    //    edvm.Password = userViewModel.NewPassword;
                //    //    edvm.EmployeeCode = user.EmployeeCode;
                //    //    edvm.ReportingManager = userViewModel.ReportingManagerId;




                //    //    var c = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                //    //    var callbackUrl = Url.Action(
                //    //       "ConfirmEmail", "Account",
                //    //       new { userId = user.Id, c = c },
                //    //       protocol: Request.Url.Scheme);

                //    //    await UserManager.SendEmailAsync(user.Id,
                //    //       "Confirm your email",
                //    //       "Your new login credentials for QuickSearch   Username: " + edvm.UserName + ", Password :" + edvm.Password);
                //    //    TempData["status"] = "Employee successfully Edited";
                //    //    return RedirectToAction("Details", "User", edvm);


                //    //}
                //}
                TempData["status"] = "Employee successfully Edited";
                return View(myProfileViewModel);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while adding employee";
                myProfileViewModel.GenderList = GetDropDownList("Gender", null);
                return View(myProfileViewModel);
            }
        }

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
    }
}