using AutoMapper;
using Microsoft.AspNet.Identity;
using QuickSearchBusiness.Services;
using QuickSearchData;
using QuickSearchWeb.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


using System.IO;
using System.Text;

namespace QuickSearchWeb.Controllers
{

    [Authorize]
    public class LOAController : Controller
    {
        [Authorize(Roles = "TimeSheetUser")]
        public ActionResult MyLOAList()
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }


            LOAService loaService = new LOAService();
            var loas = loaService.GetLoaListWithUserId(User.Identity.GetUserId());
            List<LoaListViewModel>loaList = new List<LoaListViewModel>();

            loaList = Mapper.Map<List<LOA>, List<LoaListViewModel>>(loas);

            return View(loaList);
        }

        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult EmployeeAdminLOAList()
        {

            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }
            ViewBag.TSRole = "TimeSheetAdmin";
            ViewBag.RoleName = "(Admin)";

            return View("EmployeeLOAList");
        }

        [Authorize(Roles = "TimeSheetReportingManager")]
        public ActionResult EmployeeRepoManLOAList()
        {

            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }
            ViewBag.TSRole = "TimeSheetReportingManager";
            ViewBag.RoleName = "(Approver)";

            return View("EmployeeLOAList");
        }

        [Authorize(Roles = "TimeSheetManager")]
        public ActionResult EmployeeManagerLOAList()
        {

            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }
            ViewBag.TSRole = "TimeSheetManager";
            ViewBag.RoleName = "(Viewer)";
            return View("EmployeeLOAList");
        }



        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult GetEmployeeAdminPendingLOAList()
        {
            LOAService loaService = new LOAService();

            List<LOA> loas = new List<LOA> ();
           
                loas = loaService.GetEmployeePendingLOAs();
            
            var loaList = Mapper.Map<List<LOA>, List<LoaListViewModel>>(loas);

            ViewBag.TSRole = "TimeSheetAdmin";

            return PartialView("_EmployeePendingLOAList", loaList);
        }
        [Authorize(Roles = "TimeSheetReportingManager")]
        public ActionResult GetEmployeeRepoManPendingLOAList()
        {
            LOAService loaService = new LOAService();

            List<LOA> loas = new List<LOA>();
           
             loas = loaService.GetReportingManagerPendingLOAs(User.Identity.GetUserId());
            
            var loaList = Mapper.Map<List<LOA>, List<LoaListViewModel>>(loas);

            ViewBag.TSRole = "TimeSheetReportingManager";

            return PartialView("_EmployeePendingLOAList", loaList);
        }

        [Authorize(Roles = "TimeSheetManager")]
        public ActionResult GetEmployeeManagerPendingLOAList()
        {
            LOAService loaService = new LOAService();

            List<LOA> loas = new List<LOA>();
           
                loas = loaService.GetEmployeePendingLOAs();
           
            var loaList = Mapper.Map<List<LOA>, List<LoaListViewModel>>(loas);

            ViewBag.TSRole = "TimeSheetManager";

            return PartialView("_EmployeePendingLOAList", loaList);
        }



        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult GetEmployeeAdminRemainingLOAList()
        {
            LOAService loaService = new LOAService();

            List<LOA> loas = new List<LOA>();

           loas = loaService.GetEmployeeRemianingLOAs();
           
            var loaList = Mapper.Map<List<LOA>, List<LoaListViewModel>>(loas);

            ViewBag.TSRole = "TimeSheetAdmin";

            return PartialView("_EmployeeRemainingLOAList", loaList);
        }


        [Authorize(Roles = "TimeSheetReportingManager")]
        public ActionResult GetEmployeeRepoManRemainingLOAList()
        {
            LOAService loaService = new LOAService();

            List<LOA> loas = new List<LOA>();

           
                loas = loaService.GetReportingManagerRemianingLOAs(User.Identity.GetUserId());
           

            var loaList = Mapper.Map<List<LOA>, List<LoaListViewModel>>(loas);

            ViewBag.TSRole = "TimeSheetReportingManager";

            return PartialView("_EmployeeRemainingLOAList", loaList);
        }



        [Authorize(Roles = "TimeSheetManager")]
        public ActionResult GetEmployeeManagerRemainingLOAList()
        {
            LOAService loaService = new LOAService();

            List<LOA> loas = new List<LOA>();

            loas = loaService.GetEmployeeRemianingLOAs();
            
            var loaList = Mapper.Map<List<LOA>, List<LoaListViewModel>>(loas);

            ViewBag.TSRole = "TimeSheetManager";

            return PartialView("_EmployeeRemainingLOAList", loaList);
        }


        [Authorize(Roles = "TimeSheetUser")]
        public ActionResult MyLOACreate()
        {

            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }


            //string Employeename = HttpContext.User.Identity.Name;
            LoaViewModel lvm = new LoaViewModel();
            LOAService loaService = new LOAService();


            UserService userService = new UserService();
            lvm.UserId = User.Identity.GetUserId();
            lvm.AspNetUser = userService.GetUserWithId(lvm.UserId);
            lvm.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
            lvm.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);

            lvm.LookupOtherTimeOfDay = loaService.GetLookupIdForCodeName("Other", "LoaTimeOfDay").LookupCodeId;
            lvm.LookupOtherTypeOfLeave = loaService.GetLookupIdForCodeName("Other", "LoaTypeOfLeave").LookupCodeId;
            
            lvm.StartDate = DateTime.Now;
            lvm.EndDate = DateTime.Now;
            if (lvm.AspNetUser.AspNetUser1 != null)
            {
                lvm.ReportingManager = lvm.AspNetUser.AspNetUser1.Firstname + " " + lvm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                lvm.ReportingManager = "None";
            }


            return View(lvm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TimeSheetUser")]
        public async Task<ActionResult> MyLOACreate([Bind(Exclude = "LookupLoaStatus,LoaCode")]LoaViewModel lvm, string action)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    lvm.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
                    lvm.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);                    
                    return View(lvm);
                }
                //else if(tsvm.TotalHours <= 0 || tsvm.TotalHours == null)
                //{
                //    ModelState.AddModelError("error", "Total Hours cannot be less than or equal to 0");
                //    tsvm.AbsenceTypeList = GetDropDownList("Absence", null);
                //    return View(tsvm);
                //}

                LOA loa = new LOA();
                LOAService loaService = new LOAService();

                loa = Mapper.Map<LoaViewModel,LOA>(lvm);


                //loa.UserId = lvm.UserId;
                //loa.StartDate = lvm.StartDate;
                //loa.EndDate = lvm.EndDate;
                loa.AspNetUser = null;
                UserService userService = new UserService();
                var user = userService.GetUserWithId(lvm.UserId);                
                var repoMan = user.AspNetUser1;


                loa.IsActive = true;
                loa.IsDeleted = false;
                loa.CreatedUserId = User.Identity.GetUserId();
                loa.CreatedDate = DateTime.Now;
                loa.ModifiedUserId = User.Identity.GetUserId();
                loa.ModifiedDate = DateTime.Now;
                loa.LookupLoaStatus = loaService.GetLookupIdForCodeName("Saved", "LoaStatus").LookupCodeId;

                
                loaService.CreateLoa(loa);

                
                if (string.Equals(action.ToString(), "Submit"))
                {

                    if (repoMan != null)
                    {
                        ChangeStatusOfLOA(loa.LoaId, "Pending");
                        await SendEmailLOASubmitted(loa.LoaId);
                        TempData["status"] = "LOA " + loa.LoaCode + " successfully saved and submitted to your reporting manager";
                    }

                    else
                    {
                        TempData["status"] = "Your LOA is saved but not submitted. You do not have a reporting manager assigned. Please contact your admin.";

                    }
                    
                }


                return RedirectToAction("MyLOAList");


            }


            catch (Exception ex)
            {
                LOAService loaService = new LOAService();
                lvm.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
                lvm.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);
                TempData["error"] = "Something went wrong while saving LOA";
                lvm.LookupOtherTimeOfDay = loaService.GetLookupIdForCodeName("Other", "LoaTimeOfDay").LookupCodeId;
                lvm.LookupOtherTypeOfLeave = loaService.GetLookupIdForCodeName("Other", "LoaTypeOfLeave").LookupCodeId;
                return RedirectToAction("MyLOAList");
            }

        }

        [Authorize(Roles = "TimeSheetUser")]
        public ActionResult MyLOAEdit(int id)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }


            // string Employeename = HttpContext.User.Identity.Name;
            LoaViewModel lvm = new LoaViewModel();
            LOAService loaService = new LOAService();
            var loa = loaService.GetMyLOA(id,User.Identity.GetUserId());
            if (loa == null)
            {
                TempData["error"] = "LOA does not exist or you dont have permissions";
                return RedirectToAction("MyLOAList");
            }
            lvm = Mapper.Map<LOA, LoaViewModel>(loa);
            if (lvm.AspNetUser.AspNetUser1 != null)
            {
                lvm.ReportingManager = lvm.AspNetUser.AspNetUser1.Firstname + " " + lvm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                lvm.ReportingManager = "None";
            }
            lvm.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
            lvm.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);
            lvm.LookupOtherTimeOfDay = loaService.GetLookupIdForCodeName("Other", "LoaTimeOfDay").LookupCodeId;
            lvm.LookupOtherTypeOfLeave = loaService.GetLookupIdForCodeName("Other", "LoaTypeOfLeave").LookupCodeId;
            return View(lvm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TimeSheetUser")]
        public async Task<ActionResult> MyLOAEdit([Bind(Exclude = "LookupLoaStatus,LoaCode")]LoaViewModel lvm, string action)
        {
            try
            {
                LOAService loaService = new LOAService();

                if (!ModelState.IsValid)
                {
                    lvm.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
                    lvm.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);  
                    return View(lvm);
                }
                
                //var loa = Mapper.Map<LoaViewModel, LOA>(lvm);
                var loa = loaService.GetJustMyLOA(lvm.LoaId, User.Identity.GetUserId());
                if (loa == null)
                {
                    TempData["error"] = "LOA does not exist";
                    return RedirectToAction("MyLOAList");
                }
                loa.StartDate = lvm.StartDate;
                loa.EndDate = lvm.EndDate;
                loa.LookupTimeOfDay = lvm.LookupTimeOfDay;
                loa.OtherTimeOfDay = lvm.OtherTimeOfDay;
                loa.LookupTypeOfLeave = lvm.LookupTypeOfLeave;
                loa.OtherTypeOfLeave = lvm.OtherTypeOfLeave;
                loa.ReasonForLeave = lvm.ReasonForLeave;


                loa.ModifiedUserId = User.Identity.GetUserId();
                loa.ModifiedDate = DateTime.Now;

                loaService.UpdateLOA(loa);


                UserService userService = new UserService();
                var user = userService.GetUserWithId(lvm.UserId);
                var repoMan = user.AspNetUser1;

                TempData["status"] = "LOA successfully updated";

                if (string.Equals(action.ToString(), "Submit"))
                {
                    if (repoMan != null)
                    {

                        ChangeStatusOfLOA(loa.LoaId, "Pending");
                        await SendEmailLOASubmitted(loa.LoaId);
                        TempData["status"] = "LOA " + loa.LoaCode + " successfully saved and submitted to your reporting manager";

                    }

                    else
                    {
                        TempData["status"] = "Your LOA is saved but not submitted. You do not have a reporting manager assigned. Please contact your admin.";

                    }
                    
                }
                
                return RedirectToAction("MyLOAList");
            }
            catch (Exception ex)
            {
                LOAService loaService = new LOAService();
                TempData["error"] = "Something went wrong while editing LOA";
                lvm.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
                lvm.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);
                lvm.LookupOtherTimeOfDay = loaService.GetLookupIdForCodeName("Other", "LoaTimeOfDay").LookupCodeId;
                lvm.LookupOtherTypeOfLeave = loaService.GetLookupIdForCodeName("Other", "LoaTypeOfLeave").LookupCodeId;
                return RedirectToAction("MyLOAList");
            }
        }


        [Authorize(Roles = "TimeSheetUser")]
        public ActionResult MyLOADetails(int id)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }
            
            LoaViewModel lvm = new LoaViewModel();
            LOAService loaService = new LOAService();
            var loa = loaService.GetMyLOA(id, User.Identity.GetUserId());
            if (loa == null)
            {
                TempData["error"] = "LOA does not exist or you dont have permissions";
                return RedirectToAction("MyLOAList");
            }

            lvm = Mapper.Map<LOA, LoaViewModel>(loa);

            lvm.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
            lvm.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);
            lvm.LookupOtherTimeOfDay = loaService.GetLookupIdForCodeName("Other", "LoaTimeOfDay").LookupCodeId;
            lvm.LookupOtherTypeOfLeave = loaService.GetLookupIdForCodeName("Other", "LoaTypeOfLeave").LookupCodeId;

            if (lvm.AspNetUser.AspNetUser1 != null)
            {
                lvm.ReportingManager = lvm.AspNetUser.AspNetUser1.Firstname + " " + lvm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                lvm.ReportingManager = "None";
            }
            
            return View(lvm);
            
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


        private void ChangeStatusOfLOA(int id, string status)
        {
            LOAService loaService = new LOAService();
            var loa = loaService.GetJustLOA(id);
            loa.LookupLoaStatus = loaService.GetLookupIdForCodeName(status, "LoaStatus").LookupCodeId;
            loa.ModifiedDate = DateTime.Now;
            loa.ModifiedUserId = User.Identity.GetUserId();
            loaService.UpdateLOA(loa);
        }



        [Authorize(Roles = "TimeSheetUser")]
        public ActionResult MyLOADelete(int id)
        {
            try
            {
                LOAService loaService = new LOAService();

                var loa = loaService.GetJustMyLOA(id, User.Identity.GetUserId());
                if (loa == null)
                {
                    TempData["error"] = "LOA does not exist or you dont have permissions";
                    return RedirectToAction("MyLOAList");
                }

                loa.IsDeleted = true;
                loa.IsActive = false;
                loa.ModifiedUserId = User.Identity.GetUserId();
                loa.ModifiedDate = DateTime.Now;

                loaService.UpdateLOA(loa);

                TempData["status"] = "LOA " + loa.LoaCode + " successfully deleted";
                return RedirectToAction("MyLOAList");
            }
            catch
            {
                TempData["error"] = "Something went wrong while deleting LOA";
                return RedirectToAction("MyLOAList");
            }
        }


        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult EmployeeAdminLOAEdit(int id)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            ViewBag.TSRole = "TimeSheetAdmin";
            // string Employeename = HttpContext.User.Identity.Name;
            LoaViewModel lvm = new LoaViewModel();
            LOAService loaService = new LOAService();
            var loa = loaService.GetLOA(id);
            if (loa == null)
            {
                TempData["error"] = "LOA does not exist";
                return RedirectToAction("MyLOAList");
            }
            lvm = Mapper.Map<LOA, LoaViewModel>(loa);
            lvm.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
            lvm.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);
            lvm.LookupOtherTimeOfDay = loaService.GetLookupIdForCodeName("Other", "LoaTimeOfDay").LookupCodeId;
            lvm.LookupOtherTypeOfLeave = loaService.GetLookupIdForCodeName("Other", "LoaTypeOfLeave").LookupCodeId;
            if (lvm.AspNetUser.AspNetUser1 != null)
            {
                lvm.ReportingManager = lvm.AspNetUser.AspNetUser1.Firstname + " " + lvm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                lvm.ReportingManager = "None";
            }

            return View("EmployeeLOAEdit",lvm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "TimeSheetAdmin")]
        public async Task<ActionResult> EmployeeAdminLOAEdit([Bind(Exclude = "LookupLoaStatus,LoaCode")]LoaViewModel lvm, string action)
        {

            try
            {
                LOAService loaService = new LOAService();

                if (!ModelState.IsValid)
                {
                    lvm.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
                    lvm.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);
                    return View(lvm);
                }

                
                var loa = loaService.GetJustLOA(lvm.LoaId);
                if (loa == null)
                {
                    TempData["error"] = "LOA does not exist";
                    return RedirectToAction("EmployeeAdminLOAList");
                }
                loa.StartDate = lvm.StartDate;
                loa.EndDate = lvm.EndDate;
                loa.LookupTimeOfDay = lvm.LookupTimeOfDay;
                loa.OtherTimeOfDay = lvm.OtherTimeOfDay;
                loa.LookupTypeOfLeave = lvm.LookupTypeOfLeave;
                loa.OtherTypeOfLeave = lvm.OtherTypeOfLeave;
                loa.ReasonForLeave = lvm.ReasonForLeave;
                
                loa.ModifiedUserId = User.Identity.GetUserId();
                loa.ModifiedDate = DateTime.Now;

                loaService.UpdateLOA(loa);
                ChangeStatusOfLOA(loa.LoaId, "Pending");

                TempData["status"] = "LOA " + loa.LoaCode + " successfully updated and status changed to pending ";               

                return RedirectToAction("EmployeeAdminLOAList");
            }
            catch (Exception ex)
            {
                LOAService loaService = new LOAService();
                TempData["error"] = "Something went wrong while editing LOA";
                lvm.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
                lvm.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);
                lvm.LookupOtherTimeOfDay = loaService.GetLookupIdForCodeName("Other", "LoaTimeOfDay").LookupCodeId;
                lvm.LookupOtherTypeOfLeave = loaService.GetLookupIdForCodeName("Other", "LoaTypeOfLeave").LookupCodeId;
                return RedirectToAction("EmployeeAdminLOAList");
            }
        }


        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult EmployeeAdminLOADetails(int id)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }
            ViewBag.TSRole = "TimeSheetAdmin";

            // string Employeename = HttpContext.User.Identity.Name;
            LoaViewModel lvm = new LoaViewModel();
            LOAService loaService = new LOAService();
            var loa = loaService.GetLOA(id);
            if (loa == null)
            {
                TempData["error"] = "LOA does not exist or you dont have permissions";
                //return RedirectToAction("Index");
                return RedirectToAction("EmployeeAdminLOAList");
            }
            lvm = Mapper.Map<LOA, LoaViewModel>(loa);
            lvm.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
            lvm.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);
            lvm.LookupOtherTimeOfDay = loaService.GetLookupIdForCodeName("Other", "LoaTimeOfDay").LookupCodeId;
            lvm.LookupOtherTypeOfLeave = loaService.GetLookupIdForCodeName("Other", "LoaTypeOfLeave").LookupCodeId;
            if (lvm.AspNetUser.AspNetUser1 != null)
            {
                lvm.ReportingManager = lvm.AspNetUser.AspNetUser1.Firstname + " " + lvm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                lvm.ReportingManager = "None";
            }

            return View("EmployeeLOADetails", lvm);
        }


        [Authorize(Roles = "TimeSheetReportingManager")]
        public ActionResult EmployeeRepoManLOADetails(int id)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            ViewBag.TSRole = "TimeSheetReportingManager";
            // string Employeename = HttpContext.User.Identity.Name;
            LoaViewModel lvm = new LoaViewModel();
            LOAService loaService = new LOAService();
            var loa = loaService.GetLOA(id, User.Identity.GetUserId());
            if(loa == null)
            {
                TempData["error"] = "LOA does not exist or you dont have permissions";
                //return RedirectToAction("Index");
                return RedirectToAction("EmployeeRepoManLOAList");
            }
            lvm = Mapper.Map<LOA, LoaViewModel>(loa);
            lvm.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
            lvm.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);
            lvm.LookupOtherTimeOfDay = loaService.GetLookupIdForCodeName("Other", "LoaTimeOfDay").LookupCodeId;
            lvm.LookupOtherTypeOfLeave = loaService.GetLookupIdForCodeName("Other", "LoaTypeOfLeave").LookupCodeId;
            if (lvm.AspNetUser.AspNetUser1 != null)
            {
                lvm.ReportingManager = lvm.AspNetUser.AspNetUser1.Firstname + " " + lvm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                lvm.ReportingManager = "None";
            }

            return View("EmployeeLOADetails", lvm);
        }

        [Authorize(Roles = "TimeSheetManager")]
        public ActionResult EmployeeManagerLOADetails(int id)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            ViewBag.TSRole = "TimeSheetManager";
            // string Employeename = HttpContext.User.Identity.Name;
            LoaViewModel lvm = new LoaViewModel();
            LOAService loaService = new LOAService();
            var loa = loaService.GetLOA(id);
            if (loa == null)
            {
                TempData["error"] = "LOA does not exist";
                //return RedirectToAction("Index");
                return RedirectToAction("EmployeeManagerLOAList");
            }
            lvm = Mapper.Map<LOA, LoaViewModel>(loa);
            lvm.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
            lvm.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);
            lvm.LookupOtherTimeOfDay = loaService.GetLookupIdForCodeName("Other", "LoaTimeOfDay").LookupCodeId;
            lvm.LookupOtherTypeOfLeave = loaService.GetLookupIdForCodeName("Other", "LoaTypeOfLeave").LookupCodeId;

            if (lvm.AspNetUser.AspNetUser1 != null)
            {
                lvm.ReportingManager = lvm.AspNetUser.AspNetUser1.Firstname + " " + lvm.AspNetUser.AspNetUser1.LastName;
            }
            else
            {
                lvm.ReportingManager = "None";
            }

            return View("EmployeeLOADetails", lvm);
        }


        [Authorize(Roles = "TimeSheetManager")]
        public ActionResult EmployeeAdminLOADelete(int id)
        {
            try
            {
                LOAService loaService = new LOAService();

                var loa = loaService.GetJustLOA(id);
                if(loa == null)
                {
                    TempData["error"] = "LOA does not exist";
                    return RedirectToAction("EmployeeAdminLOAList");
                }
                loa.IsDeleted = true;
                loa.IsActive = false;
                loa.ModifiedUserId = User.Identity.GetUserId();
                loa.ModifiedDate = DateTime.Now;

                loaService.UpdateLOA(loa);

                TempData["status"] = "LOA "+ loa.LoaCode +" successfully deleted";
                return RedirectToAction("EmployeeAdminLOAList");
            }
            catch
            {
                TempData["error"] = "Something went wrong while deleting LOA";
                return RedirectToAction("EmployeeAdminLOAList");
            }
        }

        [Authorize(Roles = "TimeSheetReportingManager")]
        public ActionResult Approve(int id)
        {
            try
            {
                ChangeStatusOfLOA(id, "Approved");
                LOAService loaService = new LOAService();
                var loa = loaService.GetJustLOA(id);
                if (loa == null)
                {
                    TempData["error"] = "LOA does not exist or you dont have permissions";
                    return RedirectToAction("EmployeeRepomanLOAList");
                }
                var userId = User.Identity.GetUserId();
                UserService userService = new UserService();
                var user = userService.GetUserWithId(userId);
                loa.ApprovedBy = user.Firstname + " " + user.LastName + " (" + user.EmployeeCode + ")";
                loa.ModifiedDate = DateTime.Now;
                loa.ModifiedUserId = User.Identity.GetUserId();
                loaService.UpdateLOA(loa);
                TempData["status"] = "LOA " + loa.LoaCode + " has been approved";
                return RedirectToAction("EmployeeRepomanLOAList");


            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while Approving";
                return RedirectToAction("EmployeeRepoManLOAList");
            }
        }

        [Authorize(Roles = "TimeSheetReportingManager")]
        public async Task<ActionResult> Reject(LoaViewModel lvm)
        {
            try
            {
                //if(string.IsNullOrEmpty(tsvm.TempComments))
                //{
                //    return Json(new { HasErrors= true, Errors = "Comments are Mandatory"});
                //}

                LOAService loaService = new LOAService();

                var loa = loaService.GetJustLOA(lvm.LoaId);
                if (loa == null)
                {
                    TempData["error"] = "LOA does not exist or you dont have permissions";
                    return RedirectToAction("EmployeeRepomanLOAList");
                }
                loa.LookupLoaStatus = loaService.GetLookupIdForCodeName("Rejected", "LoaStatus").LookupCodeId;
                loa.Comments += " " + lvm.TempComments + " (" + DateTime.Now.ToString() + "), <br> ";
                loa.ModifiedDate = DateTime.Now;
                loa.ModifiedUserId = User.Identity.GetUserId();
                loaService.UpdateLOA(loa);
                await SendEmailLOARejected(lvm.LoaId);

                TempData["status"] = "LOA " + loa.LoaCode + " has been rejected and mail has been sent to employee";
                return null;


            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while Rejecting";
                return RedirectToAction("EmployeeRepoManLOAList");
            }
        }


        public ActionResult HardDeleteMyLOA(int id)
        {
            try
            {
                LOAService loaService = new LOAService();

                loaService.HardDeleteLOA(id);
               
                TempData["status"] = "LOA successfully deleted";
                return RedirectToAction("MyLOAList");
            }
            catch
            {
                TempData["error"] = "Something went wrong while deleting LOA";
                return RedirectToAction("MyLOAList");
            }
        }

        public async Task SendEmailLOASubmitted(int LoaId, bool includeAttachment = false)
        {
            try
            {
                LOAService loaService = new LOAService();               
                var loa = loaService.GetLOA(LoaId);

                var subject = "LOA from " + loa.AspNetUser.Firstname +" "+ loa.AspNetUser.LastName;
                //var fileName = GetExcelFileName(timeSheet);
                var callbackUrl = Url.Action("EmployeeRepoManLOADetails", "LOA", new { id = LoaId }, protocol: Request.Url.Scheme);
                var body = "Your Employee " + loa.AspNetUser.Firstname + " " + loa.AspNetUser.LastName + " submitted LOA with Id" +loa.LoaCode +
                    " from " + loa.StartDate.ToString("MM/dd/yyyy") + "-" + loa.EndDate.ToString("MM/dd/yyyy") + "" +
                    ", You can Approve/Reject it by clicking <a href=\"" + callbackUrl + "\">here</a>";

                //var destinationEmail = WebConfigurationManager.AppSettings["HREmail"];
                var destinationEmail = loa.AspNetUser.AspNetUser1.Email;
                IdentityMessage im = CreateMessage(destinationEmail, subject, body);

                if (includeAttachment)
                {
                    //byte[] bytes = GetExcelByteDate(loa);
                    //await SendEmailWithAttachment(im, bytes, subject);
                    await SendEmailWithAttachment(im, null, null);
                }
                else
                {
                    await SendEmailWithAttachment(im, null, null);
                }


                TempData["status"] = "LOA submitted to your Reporting Manager";
               

            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while sending email";
                
            }
        }

        public async Task SendEmailLOARejected(int LoaId, bool includeAttachment = false)
        {
            try
            {
                LOAService loaService = new LOAService();
                var loa = loaService.GetLOA(LoaId);

                var subject = "LOA Rejected";
                var callbackUrl = Url.Action("MyLOAEdit", "LOA", new { id = LoaId }, protocol: Request.Url.Scheme);
                var body = "Your LOA with Id "+ loa.LoaCode +" from " + loa.StartDate.ToString("MM/dd/yyyy") + "-" + loa.EndDate.ToString("MM/dd/yyyy") + "" +
                    "was rejected with comments '" + loa.Comments + "' please click <a href=\"" + callbackUrl + "\">here</a> to review LOA and submit agian";

                //var destinationEmail = WebConfigurationManager.AppSettings["HREmail"];
                var destinationEmail = loa.AspNetUser.Email;
                IdentityMessage im = CreateMessage(destinationEmail, subject, body);

                if (includeAttachment)
                {
                    //byte[] bytes = GetExcelByteDate(timeSheet);
                    //await SendEmailWithAttachment(im, bytes, subject);
                    await SendEmailWithAttachment(im, null, null);

                }
                else
                {
                    await SendEmailWithAttachment(im, null, null);
                }


                TempData["status"] = "LOA rejection mail sent to employee";
                //return RedirectToAction("EmployeeTimeSheetsList");

            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while sending rejection mail";
                
            }
        }

        private IdentityMessage CreateMessage(string destinationEmail, string subject, string body)
        {
            IdentityMessage im = new IdentityMessage();
            im.Destination = destinationEmail;
            im.Subject = subject;
            im.Body = body;            
            return im;

        }

        private async Task SendEmailWithAttachment(IdentityMessage message, byte[] bytes, string attName)
        {
            // Plug in your email service here to send an email.
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.office365.com";
            client.EnableSsl = true;
            //client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("noreply@epathusa.in", "Suman@123");

            MailMessage mailMessage = new MailMessage("noreply@epathusa.in", message.Destination, message.Subject, message.Body);
            mailMessage.IsBodyHtml = true;
            if (attName != null)
            {
                Attachment at = new Attachment(new MemoryStream(bytes), attName);
                mailMessage.Attachments.Add(at);
            }



            await client.SendMailAsync(mailMessage);
        }


        //todo
        //public string RenderRazorViewToString(string viewName, object model)
        //{
        //    ViewData.Model = model;
        //    using (var sw = new StringWriter())
        //    {
        //        var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
        //                                                                 viewName);
        //        var viewContext = new ViewContext(ControllerContext, viewResult.View,
        //                                     ViewData, TempData, sw);
        //        viewResult.View.Render(viewContext, sw);
        //        viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
        //        return sw.GetStringBuilder().ToString();
        //    }
        //}


        [Authorize(Roles = "TimeSheetUser")]
        public ActionResult MyLOADownload(int id)
        {
            try
            {
                LoaViewModel lvm = new LoaViewModel();
                LOAService loaService = new LOAService();
                var loa = loaService.GetMyLOA(id, User.Identity.GetUserId());
                if(loa == null)
                {
                    TempData["error"] = "Either this file does not exists or you dont have permissions";
                    //return RedirectToAction("Index");
                    return View("MyLOAList");
                }

                lvm = Mapper.Map<LOA, LoaViewModel>(loa);

                lvm.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
                lvm.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);
                lvm.LookupOtherTimeOfDay = loaService.GetLookupIdForCodeName("Other", "LoaTimeOfDay").LookupCodeId;
                lvm.LookupOtherTypeOfLeave = loaService.GetLookupIdForCodeName("Other", "LoaTypeOfLeave").LookupCodeId;

                //var typeOfleaveDDL = new SelectList(lvm.TypeOfLeaveList, "Id", "Name");
                if(lvm.LookupTypeOfLeave != lvm.LookupOtherTypeOfLeave)
                {
                    var leaveType = lvm.TypeOfLeaveList.Where(x => x.Id == lvm.LookupTypeOfLeave).FirstOrDefault().Name;
                    lvm.OtherTypeOfLeave = leaveType;
                }
                if (lvm.LookupTimeOfDay != lvm.LookupOtherTimeOfDay)
                {
                    var timeOdDay = lvm.TimeOfDayList.Where(x => x.Id == lvm.LookupTimeOfDay).FirstOrDefault().Name;
                    lvm.OtherTimeOfDay = timeOdDay;
                }

                if (lvm.AspNetUser.AspNetUser1 != null)
                {
                    lvm.ReportingManager = lvm.AspNetUser.AspNetUser1.Firstname + " " + lvm.AspNetUser.AspNetUser1.LastName;
                }
                else
                {
                    lvm.ReportingManager = "None";
                }


                return PartialView("_DownloadLOAView", lvm);


            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while";
                return RedirectToAction("MyLOAList");
            }
        }

        [Authorize(Roles = "TimeSheetAdmin")]
        public ActionResult EmployeeAdminLOADownload(int id)
        {
            try
            {
                LoaViewModel lvm = new LoaViewModel();
                LOAService loaService = new LOAService();

                var loa = new LOA();
                    loa = loaService.GetLOA(id);
                if (loa == null)
                {
                    TempData["error"] = "Either this LOA does not exist or you dont have permissions";
                    return RedirectToAction("EmployeeAdminLOAList");
                }
                
                lvm = Mapper.Map<LOA, LoaViewModel>(loa);

                lvm.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
                lvm.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);
                lvm.LookupOtherTimeOfDay = loaService.GetLookupIdForCodeName("Other", "LoaTimeOfDay").LookupCodeId;
                lvm.LookupOtherTypeOfLeave = loaService.GetLookupIdForCodeName("Other", "LoaTypeOfLeave").LookupCodeId;

                //var typeOfleaveDDL = new SelectList(lvm.TypeOfLeaveList, "Id", "Name");
                if (lvm.LookupTypeOfLeave != lvm.LookupOtherTypeOfLeave)
                {
                    var leaveType = lvm.TypeOfLeaveList.Where(x => x.Id == lvm.LookupTypeOfLeave).FirstOrDefault().Name;
                    lvm.OtherTypeOfLeave = leaveType;
                }
                if (lvm.LookupTimeOfDay != lvm.LookupOtherTimeOfDay)
                {
                    var timeOdDay = lvm.TimeOfDayList.Where(x => x.Id == lvm.LookupTimeOfDay).FirstOrDefault().Name;
                    lvm.OtherTimeOfDay = timeOdDay;
                }

                if (lvm.AspNetUser.AspNetUser1 != null)
                {
                    lvm.ReportingManager = lvm.AspNetUser.AspNetUser1.Firstname + " " + lvm.AspNetUser.AspNetUser1.LastName;
                }
                else
                {
                    lvm.ReportingManager = "None";
                }
               
                return PartialView("_DownloadLOAView", lvm);
                

            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while downloading";
                return RedirectToAction("EmployeeAdminLOAList");
            }
        }


        [Authorize(Roles = "TimeSheetReportingManager")]
        public ActionResult EmployeeRepoManLOADownload(int id)
        {
            try
            {
                LoaViewModel lvm = new LoaViewModel();
                LOAService loaService = new LOAService();

                var loa = new LOA();

               
                    loa = loaService.GetLOA(id, User.Identity.GetUserId());

                if (loa == null)
                {
                    TempData["error"] = "Either this file does not exist or you dont have permissions";
                    return RedirectToAction("EmployeeRepoManLOAList");
                }

                lvm = Mapper.Map<LOA, LoaViewModel>(loa);

                lvm.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
                lvm.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);
                lvm.LookupOtherTimeOfDay = loaService.GetLookupIdForCodeName("Other", "LoaTimeOfDay").LookupCodeId;
                lvm.LookupOtherTypeOfLeave = loaService.GetLookupIdForCodeName("Other", "LoaTypeOfLeave").LookupCodeId;

                //var typeOfleaveDDL = new SelectList(lvm.TypeOfLeaveList, "Id", "Name");
                if (lvm.LookupTypeOfLeave != lvm.LookupOtherTypeOfLeave)
                {
                    var leaveType = lvm.TypeOfLeaveList.Where(x => x.Id == lvm.LookupTypeOfLeave).FirstOrDefault().Name;
                    lvm.OtherTypeOfLeave = leaveType;
                }
                if (lvm.LookupTimeOfDay != lvm.LookupOtherTimeOfDay)
                {
                    var timeOdDay = lvm.TimeOfDayList.Where(x => x.Id == lvm.LookupTimeOfDay).FirstOrDefault().Name;
                    lvm.OtherTimeOfDay = timeOdDay;
                }

                if (lvm.AspNetUser.AspNetUser1 != null)
                {
                    lvm.ReportingManager = lvm.AspNetUser.AspNetUser1.Firstname + " " + lvm.AspNetUser.AspNetUser1.LastName;
                }
                else
                {
                    lvm.ReportingManager = "None";
                }
                
                return PartialView("_DownloadLOAView", lvm);
                

            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while downloading";
                return RedirectToAction("EmployeeRepoManLOAList");
            }
        }


        [Authorize(Roles = "TimeSheetManager")]
        public ActionResult EmployeeManagerLOADownload(int id)
        {
            try
            {
                LoaViewModel lvm = new LoaViewModel();
                LOAService loaService = new LOAService();

                var loa = new LOA();

                
                    loa = loaService.GetLOA(id);

               
                if (loa == null)
                {
                    TempData["error"] = "Either this file does not exist or you dont have permissions";
                    return RedirectToAction("EmployeeManagerLOAList");
                }

                lvm = Mapper.Map<LOA, LoaViewModel>(loa);

                lvm.TimeOfDayList = GetDropDownList("LoaTimeOfDay", null);
                lvm.TypeOfLeaveList = GetDropDownList("LoaTypeOfLeave", null);
                lvm.LookupOtherTimeOfDay = loaService.GetLookupIdForCodeName("Other", "LoaTimeOfDay").LookupCodeId;
                lvm.LookupOtherTypeOfLeave = loaService.GetLookupIdForCodeName("Other", "LoaTypeOfLeave").LookupCodeId;

                //var typeOfleaveDDL = new SelectList(lvm.TypeOfLeaveList, "Id", "Name");
                if (lvm.LookupTypeOfLeave != lvm.LookupOtherTypeOfLeave)
                {
                    var leaveType = lvm.TypeOfLeaveList.Where(x => x.Id == lvm.LookupTypeOfLeave).First().Name;
                    lvm.OtherTypeOfLeave = leaveType;
                }
                if (lvm.LookupTimeOfDay != lvm.LookupOtherTimeOfDay)
                {
                    var timeOfDay = lvm.TimeOfDayList.Where(x => x.Id == lvm.LookupTimeOfDay).First().Name;
                    lvm.OtherTimeOfDay = timeOfDay;
                }

                if (lvm.AspNetUser.AspNetUser1 != null)
                {
                    lvm.ReportingManager = lvm.AspNetUser.AspNetUser1.Firstname + " " + lvm.AspNetUser.AspNetUser1.LastName;
                }
                else
                {
                    lvm.ReportingManager = "None";
                }
                
                return PartialView("_DownloadLOAView", lvm);
                
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while downloading";
                return RedirectToAction("EmployeeManagerLOAList");
            }
        }




        private string GetDocFileName(LOA loa)
        {
            string fileName = loa.AspNetUser.Firstname + loa.AspNetUser.LastName + "-LOA" + loa.EndDate.ToString("MMddyy") + ".pdf";
            return fileName;

        }
        
    }
}