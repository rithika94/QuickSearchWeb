using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuickSearchWeb.Models;
using QuickSearchBusiness.Services;
using QuickSearchData;
using AutoMapper;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Configuration;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Web.Configuration;

namespace QuickSearchWeb.Controllers
{
    [Authorize]
    public class SupportController : Controller
    {
        // GET: Support
        [Authorize(Roles = "SupportUser")]
        public ActionResult MySupportList()
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            SupportService supportservice = new SupportService();
            var supports = supportservice.GetSupportListForUserId(User.Identity.GetUserId());
            //List<SupportListViewModel> supportlist = new List<SupportListViewModel>();
            var supportlist = Mapper.Map<List<SupportMaster>, List<SupportListViewModel>>(supports);
          
            return View(supportlist);
        }
     

        [Authorize(Roles = "SupportEngineer")]       
        public ActionResult SupportEngineerList()
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }


            SupportService supportservice = new SupportService();
            var supports = supportservice.GetAllSupportForEngineer(User.Identity.GetUserId());
            var supportlist = Mapper.Map<List<SupportMaster>, List<SupportListViewModel>>(supports);

            
            return View(supportlist);
        }

        [Authorize(Roles = "SupportManager")]       
        public ActionResult SupportManagerList()
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            SupportService supportservice = new SupportService();
            var supports = supportservice.GetAllSupportList();           
            var supportlist = Mapper.Map<List<SupportMaster>, List<SupportListViewModel>>(supports);

            return View(supportlist);
        }

        [Authorize(Roles = "SupportAdmin")]
        public ActionResult SupportAdminList()
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            SupportService supportservice = new SupportService();
            var supports = supportservice.GetAllSupportList();
            var supportlist = Mapper.Map<List<SupportMaster>, List<SupportListViewModel>>(supports);

            return View(supportlist);
        }

        [Authorize(Roles ="SupportUser")]
        [HttpGet]     
        public ActionResult MySupportCreate()
        {
            SupportViewModel svm = new  SupportViewModel();            
                svm.IssueTypeList = GetSupportDropDownList("IssueType", null);               
                svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);
             return View(svm);
              

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SupportUser")]
        public async Task<ActionResult> MySupportCreate(SupportViewModel svm,string action, IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
               

                if (!ModelState.IsValid)
                {
                    svm.IssueTypeList = GetSupportDropDownList("IssueType", null);
                    svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);
                }

                SupportService supportService = new SupportService();
                var support = Mapper.Map<SupportViewModel,SupportMaster>(svm);
                support.UserId = User.Identity.GetUserId();
                support.LookupIssueStatus = supportService.GetLookupIdForStatus("Saved", "IssueStatus").LookupCodeId;
                support.CreatedDate = DateTime.Now;
                support.ModifiedDate = DateTime.Now;
                support.SubmittedDate = DateTime.Now;
                support.CreatedUserId = User.Identity.GetUserId();
                support.ModifiedUserId = User.Identity.GetUserId();
                support.IsActive = true;
                support.IsDeleted = false;

                supportService.CreateSupport(support);
                TempData["status"] = "Service request " + support.SupportCode + " successfully saved";

                
                //Insert data into SupportAttachments
                foreach (HttpPostedFileBase varfile in files)
                {
                    if (varfile != null && varfile.ContentLength > 0)
                    {
                        SupportAttachment supportDoc = new SupportAttachment()
                        {
                            SupportId = support.SupportId,
                            FileServerPath = ConfigurationManager.AppSettings["SupportAttachments"] + support.SupportId.ToString() + "/",
                            FileName = varfile.FileName,
                            IsActive = true,
                            IsDeleted = false,
                            CreatedUserId = User.Identity.GetUserId(),
                            CreatedDate = DateTime.Now,
                            ModifiedUserId = User.Identity.GetUserId(),
                            ModifiedDate = DateTime.Now
                        };
                        supportService.SetSupportAttachment(supportDoc);
                        string subDirectory = Server.MapPath(supportDoc.FileServerPath);
                        System.IO.Directory.CreateDirectory(subDirectory);
                        var newFileName = Path.Combine(subDirectory, supportDoc.FileName);
                        varfile.SaveAs(newFileName);

                    }
                }

                if (string.Equals(action.ToString(), "Submit"))
                {                  
                    ChangeStatusOfSupport(support.SupportId, "Open");
                    await SendEmailSupportTicketSubmitted(support.SupportId);
                    await SendEmailSupportTicketSubmittedUser(support.SupportId);
                    TempData["status"] = "Service request "+ support.SupportCode + " successfully saved and submitted";
                }

                return RedirectToAction("MySupportList");

            }
            catch (Exception ex)
            {
                svm.IssueTypeList = GetSupportDropDownList("IssueType", null);
                svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);
                TempData["error"] = "Something went wrong while creating a support request";
                return View(svm);
            }
            
        }
            
          
      


        [Authorize(Roles = "SupportUser")]
        public ActionResult MySupportDetails(int id)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];
            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            SupportService supportservice = new SupportService();
            var support = supportservice.GetSupportWithId(id, User.Identity.GetUserId());
            if (support == null)
            {
                TempData["error"] = "Service request does not exist or you dont have permission";
                return RedirectToAction("MySupportList");
            }
           
            var svm = Mapper.Map<SupportMaster, SupportViewModel>(support);
            if (support.AspNetUser1 != null)
            {
                svm.AssignToName = support.AspNetUser1.Firstname + support.AspNetUser1.LastName + "(" + support.AspNetUser1.EmployeeCode + ")";
            }
            else
            {
                svm.AssignToName = "NA";
            }
            svm.IssueTypeList = GetSupportDropDownList("IssueType", null);
            svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);
            svm.SupportFilesList = supportservice.GetSupportAttachments(support.SupportId);
            return View(svm);
        }

        [Authorize(Roles = "SupportEngineer")]
        public ActionResult SupportEngineerDetails(int id)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];
            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            SupportService supportservice = new SupportService();
            var support = supportservice.GetSupportForEngineerWithId(id, User.Identity.GetUserId());
            if (support == null)
            {
                TempData["error"] = "Service request does not exist or you dont have permission";
                return RedirectToAction("SupportEngineerDetails");
            }

            var svm = Mapper.Map<SupportMaster, SupportViewModel>(support);
            svm.ReportedByName = support.AspNetUser.Firstname + " " + support.AspNetUser.LastName + "(" + support.AspNetUser.EmployeeCode + ")";
            svm.IssueTypeList = GetSupportDropDownList("IssueType", null);
            svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);
            svm.SupportFilesList = supportservice.GetSupportAttachments(support.SupportId);
            return View(svm);
        }

        [Authorize(Roles = "SupportManager")]
        public ActionResult SupportManagerDetails(int id)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];
            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            SupportService supportservice = new SupportService();
            var support = supportservice.GetSupportWithId(id,null);
            if (support == null)
            {
                TempData["error"] = "Service request does not exist or you dont have permission";
                return RedirectToAction("SupportManagerDetails");
            }

            var svm = Mapper.Map<SupportMaster, SupportViewModel>(support);

            if (support.AspNetUser1 != null)
            {
                svm.AssignToName = support.AspNetUser1.Firstname + support.AspNetUser1.LastName + "(" + support.AspNetUser1.EmployeeCode + ")";
            }
            else
            {
                svm.AssignToName = "NA";
            }
            svm.ReportedByName = support.AspNetUser.Firstname + " " + support.AspNetUser.LastName + "(" + support.AspNetUser.EmployeeCode + ")";
            svm.IssueTypeList = GetSupportDropDownList("IssueType", null);
            svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);
            svm.SupportFilesList = supportservice.GetSupportAttachments(support.SupportId);
            return View(svm);
        }


        [Authorize(Roles = "SupportUser")]
        public ActionResult MySupportEdit(int id)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];
            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            SupportService supportservice = new SupportService();
            var support = supportservice.GetSupportWithId(id, User.Identity.GetUserId());
            if (support == null)
            {
                TempData["error"] = "Service request does not exist or you dont have permission";
                return RedirectToAction("MySupportList");
            }
            var svm = Mapper.Map<SupportMaster, SupportViewModel>(support);
            if (support.AspNetUser1 != null)
            {
                svm.AssignToName = support.AspNetUser1.Firstname + support.AspNetUser1.LastName + "(" + support.AspNetUser1.EmployeeCode + ")";
            }
            else
            {
                svm.AssignToName = "NA";
            }

            svm.IssueTypeList = GetSupportDropDownList("IssueType", null);
            svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);
            svm.SupportFilesList = supportservice.GetSupportAttachments(support.SupportId);

            return View(svm);
        }
       


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SupportUser")]
        public async Task<ActionResult> MySupportEdit([Bind(Exclude = "LookupIssueStatus,SupportCode")]SupportViewModel svm, string action)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    svm.IssueTypeList = GetSupportDropDownList("IssueType", null);
                    svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);
                    return View(svm);

                }
                SupportService supportservice = new SupportService();
                var support = supportservice.GetSupportWithId(svm.SupportId);
                //var support = Mapper.Map<SupportViewModel, SupportMaster>(svm);               
                support.Summary = svm.Summary;
                support.Description = svm.Description;
                support.LookupCodeMaster = svm.LookupCodeMaster;
                support.LookupCodeMaster2 = svm.LookupCodeMaster2;
                support.ModifiedUserId = User.Identity.GetUserId();
                support.ModifiedDate = DateTime.Now;
                supportservice.UpdateSupport(support);
                TempData["status"] = "Service request " + support.SupportCode + " successfully updated";


                if (string.Equals(action.ToString(), "Submit"))
                {
                    ChangeStatusOfSupport(support.SupportId, "Open");
                    await SendEmailSupportTicketSubmitted(support.SupportId);
                    await SendEmailSupportTicketSubmittedUser(support.SupportId);
                    TempData["status"] = "Service request " + support.SupportCode + " successfully updated and submitted";
                }

                return RedirectToAction("MySupportList");
            }
            catch (Exception ex)
            {
                svm.IssueTypeList = GetSupportDropDownList("IssueType", null);
                svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);
                TempData["error"] = "something went wrong while updating";
                return View(svm);
            }

        }


        [Authorize(Roles = "SupportEngineer")]
        public ActionResult SupportEngineerEdit(int id)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];
            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }
            SupportService supportservice = new SupportService();
            var support = supportservice.GetSupportForEngineerWithId(id, User.Identity.GetUserId());
            if (support == null)
            {
                TempData["error"] = "Service request does not exist or you dont have permission";
                return RedirectToAction("SupportEngineerList");
            }
            var svm = Mapper.Map<SupportViewModel>(support);
            if(support.FixedTime != null && support.IsFixed == null)
            {
                var diffHours = (DateTime.Now - support.FixedTime).Value.TotalHours;
                if(diffHours > Convert.ToInt32(WebConfigurationManager.AppSettings["SupportIsFixedTimeHours"]))
                {
                    support.IsFixed = true;
                    svm.showCloseButton = true;
                    supportservice.UpdateSupport(support);
                }
            }

            svm.ReportedByName = support.AspNetUser.Firstname + " " + support.AspNetUser.LastName + "(" + support.AspNetUser.EmployeeCode + ")";
            svm.IssueTypeList = GetSupportDropDownList("IssueType", null);
            svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);
            svm.SupportFilesList = supportservice.GetSupportAttachments(support.SupportId);
            return View(svm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SupportEngineer")]
        public ActionResult SupportEngineerEdit(SupportViewModel svm, string action)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    svm.IssueTypeList = GetSupportDropDownList("IssueType", null);
                    svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);

                    return View(svm);

                }
                SupportService supportservice = new SupportService();
                var support = supportservice.GetJustSupportWithId(svm.SupportId, null);
                support.ModifiedUserId = User.Identity.GetUserId();
                support.ModifiedDate = DateTime.Now;
                if(support.Notes == null )
                {
                    support.Notes = svm.Notes;
                    support.Notes += " (" + DateTime.Now.ToString() + "-" + User.Identity.GetUserName() + ")";
                }
                else if(support.Notes.Equals(svm.Notes))
                {
                    support.Notes = svm.Notes;
                }
                else
                {
                    support.Notes = svm.Notes;
                    support.Notes += " (" + DateTime.Now.ToString()+"-"+ User.Identity.GetUserName() + ")";
                }
                
                //support.Comments = svm.Comments;
                supportservice.UpdateSupport(support);
                TempData["status"] = "Service request changes are made successfully";

                if (string.Equals(action.ToString(), "InProgress"))
                {
                    ChangeStatusOfSupport(support.SupportId, "InProgress");
                    //await SendEmailTimeSheetSubmitted(timesheet.TimeSheetID);
                    TempData["status"] = "Service request status changed to InProgress";
                }

                if (string.Equals(action.ToString(), "Close the ticket"))
                {
                    ChangeStatusOfSupport(support.SupportId, "Closed");
                    //await SendEmailTimeSheetSubmitted(timesheet.TimeSheetID);
                    TempData["status"] = "Service request status changed to closed";
                }
                //else
                //{
                //    return RedirectToAction("SupportEngineerEdit",new {id = svm.SupportId});
                    
                //}

                return RedirectToAction("SupportEngineerList");
            }
            catch (Exception ex)
            {
                //svm.IssueTypeList = GetSupportDropDownList("IssueType", null);
                //svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);
                TempData["error"] = "something went wrong while editing.";
                return RedirectToAction("SupportEngineerList");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DoneTicket(SupportViewModel svm)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    svm.IssueTypeList = GetSupportDropDownList("IssueType", null);
                //    svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);

                //    return View(svm);
                //}
                SupportService supportService = new SupportService();
                var support = supportService.GetJustSupportWithId(svm.SupportId, null);
                support.ModifiedUserId = User.Identity.GetUserId();
                support.ModifiedDate = DateTime.Now;
                support.FixedTime = DateTime.Now;
                //support.Notes = svm.Notes;

                if (support.Notes == null)
                {
                    support.Notes = svm.Notes;
                    support.Notes += " (" + DateTime.Now.ToString() + "-" + User.Identity.GetUserName() + ")";
                }
                else if (support.Notes.Equals(svm.Notes))
                {
                    support.Notes = svm.Notes;
                }
                else
                {
                    support.Notes = svm.Notes;
                    support.Notes += " (" + DateTime.Now.ToString() + "-" + User.Identity.GetUserName() + ")    ";
                }

                support.Comments += "   " + svm.TempComments + " (" + DateTime.Now.ToString() + "-" + User.Identity.GetUserName() + "),  ";

                support.LookupIssueStatus = supportService.GetLookupIdForStatus("Done", "IssueStatus").LookupCodeId;
                supportService.UpdateSupport(support);
                await SendEmailSupportTicketDone(support.SupportId);
                TempData["status"] = "Service request status succesfully changed to Done ";
                return null;
                //return RedirectToAction("SupportEngineerList");
            }
            catch (Exception ex)
            {
                //svm.IssueTypeList = GetSupportDropDownList("IssueType", null);
                //svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);
                TempData["error"] = "something went wrong";
                return RedirectToAction("SupportEngineerList");
            }

        }

        [Authorize(Roles = "SupportUser")]
        public ActionResult IssueFixedYes(int supportId)
        {
            try
            {
                SupportService supportService = new SupportService();
                var support = supportService.GetJustSupportWithId(supportId);
                support.IsFixed = true;
                supportService.UpdateSupport(support);

                TempData["status"] = "Support request is marked as fixed";
                return RedirectToAction("MySupportList");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while marking as fixed";
                return RedirectToAction("MySupportList");
            }
        }

        [Authorize(Roles = "SupportUser")]
        public async Task<ActionResult> IssueFixedNo(SupportViewModel svm)
        {
            try
            {
                SupportService supportService = new SupportService();
                var support = supportService.GetJustSupportWithId(svm.SupportId);
                support.ModifiedUserId = User.Identity.GetUserId();
                support.ModifiedDate = DateTime.Now;


                support.UserComments += "   " + svm.TempComments + " (" + DateTime.Now.ToString() + "-" + User.Identity.GetUserName() + "),  ";
                support.IsFixed = false;
                supportService.UpdateSupport(support);
                await SendEmailSupportTicketNotFixed(svm.SupportId);
                TempData["status"] = "Support request is marked as fixed";
                return RedirectToAction("MySupportList");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while marking as fixed";
                return RedirectToAction("MySupportList");
            }
        }

        


        [Authorize(Roles = "SupportManager")]
        public ActionResult SupportManagerEdit(int id)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];
            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            SupportService supportservice = new SupportService();
            var support = supportservice.GetSupportWithId(id, null);
            var svm = Mapper.Map<SupportViewModel>(support);
            svm.ReportedByName = support.AspNetUser.Firstname + " " + support.AspNetUser.LastName +"("+support.AspNetUser.EmployeeCode+")";
            svm.IssueTypeList = GetSupportDropDownList("IssueType", null);
            svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);
            svm.SupportFilesList = supportservice.GetSupportAttachments(support.SupportId);
            svm.AssignToList = GetSupportEngineersList();

            return View(svm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SupportManager")]
        public async Task<ActionResult> SupportManagerEdit(SupportViewModel svm, string action)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    svm.IssueTypeList = GetSupportDropDownList("IssueType", null);
                    svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);
                    svm.AssignToList = GetSupportEngineersList();
                   
                    return View(svm);

                }
                SupportService supportservice = new SupportService();
               
                var support = supportservice.GetJustSupportWithId(svm.SupportId,null);  
                if(support == null)
                {
                    TempData["error"] = "Support request does not exist or you dont have permission";
                    return RedirectToAction("SupportManagerList");
                }
                
                support.ModifiedUserId = User.Identity.GetUserId();
                support.ModifiedDate = DateTime.Now;                
                support.AssignTo = svm.AssignTo;

                supportservice.UpdateSupport(support);

                if (string.Equals(action.ToString(), "Open this ticket"))
                {
                    ChangeStatusOfSupport(support.SupportId, "Open");
                    //await SendEmailTimeSheetSubmitted(timesheet.TimeSheetID);
                    TempData["status"] = "Support ticket successfully opened again";
                }
            
                if (svm.AssignTo == null)
                {
                    ChangeStatusOfSupport(svm.SupportId, "Open");
                }
                else
                {
                    ChangeStatusOfSupport(svm.SupportId, "Assigned");
                    await SendEmailSupportTicketAssigned(support.SupportId);
                }
                TempData["status"] = "Changes are saved successfully";
                return RedirectToAction("SupportManagerList");
            }
            catch (Exception ex)
            {
           
                //svm.IssueTypeList = GetSupportDropDownList("IssueType", null);
                //svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);
                //svm.AssignToList = GetSupportEngineersList();
                TempData["error"] = "something went wrong while editing";
                return RedirectToAction("SupportManagerList");
            }


        }

        [Authorize(Roles = "SupportAdmin")]
        public ActionResult SupportAdminEdit(int id)
        {
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];
            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            SupportService supportservice = new SupportService();
            var support = supportservice.GetSupportWithId(id, null);
            var svm = Mapper.Map<SupportViewModel>(support);
            svm.ReportedByName = support.AspNetUser.Firstname + " " + support.AspNetUser.LastName + "(" + support.AspNetUser.EmployeeCode + ")";
            svm.IssueTypeList = GetSupportDropDownList("IssueType", null);
            svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);
            svm.IssueStatusList = GetSupportDropDownList("IssueStatus", null);
            var itemToRemove = svm.IssueStatusList.SingleOrDefault(x => x.Name == "Saved");
            svm.IssueStatusList.Remove(itemToRemove);
            //itemToRemove = svm.IssueStatusList.SingleOrDefault(x => x.Name == "Assigned");
            //svm.IssueStatusList.Remove(itemToRemove);
            //itemToRemove = svm.IssueStatusList.SingleOrDefault(x => x.Name == "InProgress");
            //svm.IssueStatusList.Remove(itemToRemove);
            //itemToRemove = svm.IssueStatusList.SingleOrDefault(x => x.Name == "Done");
            //svm.IssueStatusList.Remove(itemToRemove);

            svm.SupportFilesList = supportservice.GetSupportAttachments(support.SupportId);
            svm.AssignToList = GetSupportEngineersList();

            return View(svm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SupportAdmin")]
        public ActionResult SupportAdminEdit(SupportViewModel svm, string action)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    svm.IssueTypeList = GetSupportDropDownList("IssueType", null);
                    svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);
                    svm.IssueStatusList = GetSupportDropDownList("IssueStatus", null);
                    var itemToRemove = svm.IssueStatusList.SingleOrDefault(x => x.Name == "Saved");
                    svm.IssueStatusList.Remove(itemToRemove);
                    svm.AssignToList = GetSupportEngineersList();

                    return View(svm);

                }
                SupportService supportservice = new SupportService();

                var support = supportservice.GetJustSupportWithId(svm.SupportId, null);
                if (support == null)
                {
                    TempData["error"] = "Support request does not exist or you dont have permission";
                    return RedirectToAction("SupportAdminList");
                }

                support.ModifiedUserId = User.Identity.GetUserId();
                support.ModifiedDate = DateTime.Now;                
                support.AssignTo = svm.AssignTo;
                if(svm.AssignTo != null)
                {
                    if (svm.LookupIssueStatus == GetLookupCodeIdForStatus("Open"))
                    {
                        support.LookupIssueStatus = GetLookupCodeIdForStatus("Assigned");

                    }
                    else
                    {
                        support.LookupIssueStatus = svm.LookupIssueStatus;
                    }

                }
                else
                {
                     if (svm.LookupIssueStatus == GetLookupCodeIdForStatus("InProgress") || svm.LookupIssueStatus == GetLookupCodeIdForStatus("Done") || svm.LookupIssueStatus == GetLookupCodeIdForStatus("Assigned"))
                    {
                        support.LookupIssueStatus = GetLookupCodeIdForStatus("Open");
                    }
                    else
                    {
                        support.LookupIssueStatus = svm.LookupIssueStatus;
                    }
                }
                support.IsFixed = null;
                supportservice.UpdateSupport(support);
              
                //if (svm.AssignTo == null)
                //{
                //    ChangeStatusOfSupport(svm.SupportId, "Open");
                //}
                //else
                //{
                //    ChangeStatusOfSupport(svm.SupportId, "InProgress");
                //}
                TempData["status"] = "Changes are saved successfully";
                return RedirectToAction("SupportAdminList");
            }
            catch (Exception ex)
            {

                //svm.IssueTypeList = GetSupportDropDownList("IssueType", null);
                //svm.IssuePriorityList = GetSupportDropDownList("IssuePriority", null);
                //svm.AssignToList = GetSupportEngineersList();
                TempData["error"] = "something went wrong while editing";
                return RedirectToAction("SupportAdminList");
            }


        }

      


        [Authorize(Roles = "SupportUser")]
        public ActionResult MySupportDelete(int id)
        {
            try
            {
                SupportService supportService = new SupportService();
                var deleted = supportService.CheckForHardDelete(id, User.Identity.GetUserId());
                if (deleted == false)
                {
                    TempData["status"] = "Support request does not exists or you dont have permissions";
                    return RedirectToAction("MySupportList");
                }
                else
                {
                    var supportAttachments = supportService.GetSupportAttachments(id);
                    foreach (SupportAttachment s in supportAttachments)
                    {
                        RemoveImg(s.Id, s.FileName, s.SupportId);
                    }
                    supportService.HardDeleteSupport(id, User.Identity.GetUserId());
                }
                
                TempData["status"] = "Support request is deleted successfully";
                return RedirectToAction("MySupportList");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while deleting";
                return RedirectToAction("MySupportList");
            }

           
        }

        [Authorize(Roles = "SupportManager")]
        public ActionResult HardDeleteManager(int id)
        {
            try
            {
                SupportService supportService = new SupportService();
                var supportAttachments = supportService.GetSupportAttachments(id);
                foreach (SupportAttachment s in supportAttachments)
                {
                    RemoveImg(s.Id, s.FileName, s.SupportId);
                }
                supportService.HardDeleteSupport(id);
                
                TempData["status"] = "Support request is deleted";
                return RedirectToAction("SupportManagerList");
            }
            catch(Exception ex)
            {
                TempData["error"] = "Something went wrong while deleting";
                return RedirectToAction("SupportManagerList");
            }
            
        }

        [Authorize(Roles = "SupportManager")]
        public ActionResult SoftDeleteManager(int id)
        {
            try
            {
                SupportService supportService = new SupportService();
                var support = supportService.GetJustSupportWithId(id);
                support.IsDeleted = true;
                support.IsActive = false;
                supportService.UpdateSupport(support);
               
                TempData["status"] = "Support request is deleted successfully";
                return RedirectToAction("SupportManagerList");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while deleting";
                return RedirectToAction("SupportManagerList");
            }

        }

        [Authorize(Roles = "SupportAdmin")]
        public ActionResult HardDeleteAdmin(int id)
        {
            try
            {
                SupportService supportService = new SupportService();
                var supportAttachments = supportService.GetSupportAttachments(id);
                foreach (SupportAttachment s in supportAttachments)
                {
                    RemoveImg(s.Id, s.FileName, s.SupportId);
                }
                supportService.HardDeleteSupport(id);
                
                TempData["status"] = "Service Request is deleted successfully";
                return RedirectToAction("SupportAdminList");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while deleting";
                return RedirectToAction("SupportAdminList");
            }

        }

        [Authorize(Roles = "SupportAdmin")]
        public ActionResult SoftDeleteAdmin(int id)
        {
            try
            {
                SupportService supportService = new SupportService();
                var support = supportService.GetJustSupportWithId(id);
                support.IsDeleted = true;
                support.IsActive = false;
                supportService.UpdateSupport(support);

                TempData["status"] = "Service Request "+support.SupportCode +" is deleted";
                return RedirectToAction("SupportAdminList");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while deleting";
                return RedirectToAction("SupportAdminList");
            }

        }
      


        private void ChangeStatusOfSupport(int id, string status)
        {
            SupportService supportService = new SupportService();
            var support = supportService.GetJustSupportWithId(id);
            support.LookupIssueStatus = supportService.GetLookupIdForStatus(status, "IssueStatus").LookupCodeId;
            support.ModifiedDate = DateTime.Now;
            support.ModifiedUserId = User.Identity.GetUserId();
            supportService.UpdateSupport(support);
        }

        private int GetLookupCodeIdForStatus(string status)
        {
            SupportService supportService = new SupportService();           
            var lookupid = supportService.GetLookupIdForStatus(status, "IssueStatus").LookupCodeId;
            return lookupid;
        }


        


        private List<DropDownListViewModel> GetSupportEngineersList()
        {

            var _list = new List<DropDownListViewModel>();

            SupportService supportservice = new SupportService();
            var usersWithRoles = supportservice.UsersWithRoles("SupportEngineer");


            foreach (var user in usersWithRoles)
            {
                DropDownListViewModel ddl = new DropDownListViewModel();

                ddl.IdString = user.Id;
                ddl.Name = user.Firstname+" "+user.LastName+ " ("+user.EmployeeCode.ToString()+")";
                _list.Add(ddl);
            }

            return _list;
        }

        private List<DropDownListViewModel> GetSupportDropDownList(string lookupTypeName, int? lookupCodeId)
        {

            var _list = new List<DropDownListViewModel>();

            SupportService supportservice = new SupportService();

            List<LookupCodeMaster> lookupcategories = new List<LookupCodeMaster>();
            if (lookupTypeName != null)
            {
                lookupcategories = supportservice.GetDropDownList(lookupTypeName);
            }
            else if (lookupCodeId != null)
            {
                lookupcategories = supportservice.GetDropDownList(lookupCodeId ?? 0);
            }


            foreach (LookupCodeMaster cat in lookupcategories)
            {
                DropDownListViewModel ddl = new DropDownListViewModel();

                ddl.Id = cat.LookupCodeId;
                ddl.Name = cat.LookupCodeName;
                _list.Add(ddl);
            }

            return _list;
        }

        public async Task SendEmailSupportTicketSubmitted(int supportId, bool includeAttachment = false)
        {
            try
            {
                SupportService supportService = new SupportService();
                //timeSheetService.ChangeTimeSheetStatus(timeSheetId, "Pending");
                var support = supportService.GetSupportWithId(supportId);

                var subject = "Service request #  " + support.SupportCode + "-" + support.AspNetUser.Firstname + " " + support.AspNetUser.LastName;




                //var fileName = GetExcelFileName(timeSheet);
                var callbackUrl = Url.Action("SupportManagerEdit", "Support", new { id = support.SupportId }, protocol: Request.Url.Scheme);

                var body = "Dear Manager,<br /> A New Service Ticket " + support.SupportCode + " has been raised by " + support.AspNetUser.Firstname + " " + support.AspNetUser.LastName + "." + " " +
                    "<br />The Priority of the ticket is " + support.LookupCodeMaster.LookupCodeName + ". Please click <a href=\"" + callbackUrl + "\">here</a>" +" to assign support engineer."+
                   
                    "<br />We thank you in advance for your prompt attention to this matter.  This assists us in ensuring our clients/customers receive the highest form of customer service at an elevated efficiency rate. " +
                    "<br />Thank you  ";
                //var body = "Employee " + support.AspNetUser.Firstname + " " + support.AspNetUser.LastName + " submitted service request " +                    
                //    ", You can check it by clicking <a href=\"" + callbackUrl + "\">here</a>";

                var destinationEmail = WebConfigurationManager.AppSettings["SupportEmail"];
                //var destinationEmail = support.AspNetUser.Email;
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


                //TempData["status"] = "Expense Sheet submitted to your Reporting Manager";
                //return RedirectToAction("MyTimeSheetsList");

            }
            catch (Exception ex)
            {
                //TempData["error"] = "Something went wrong while sending email to your reporting manager";
                //return RedirectToAction("Index");
                //return View("MyTimeSheetsList");
            }
        }
        public async Task SendEmailSupportTicketSubmittedUser(int supportId, bool includeAttachment = false)
        {
            try
            {
                SupportService supportService = new SupportService();
                //timeSheetService.ChangeTimeSheetStatus(timeSheetId, "Pending");
                var support = supportService.GetSupportWithId(supportId);

                var subject = "Service request  " + support.SupportCode;




                //var fileName = GetExcelFileName(timeSheet);
                var callbackUrl = Url.Action("MySupportDetails", "Support", new { id = support.SupportId }, protocol: Request.Url.Scheme);
                var body = "Thank you for contacting the ePATHUSA Inc Client Support team. This message is to confirm that we have received your request and have opened a case for your issue.The new case number is: " + support.SupportCode + " . If you have any questions, Please feel free to contact at support@epathusa.net " +
                    "<br />Thank you";

                //var destinationEmail = WebConfigurationManager.AppSettings["SupportEmail"];
                var destinationEmail = support.AspNetUser.Email;
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


                //TempData["status"] = "Expense Sheet submitted to your Reporting Manager";
                //return RedirectToAction("MyTimeSheetsList");

            }
            catch (Exception ex)
            {
                //TempData["error"] = "Something went wrong while sending email to your reporting manager";
                //return RedirectToAction("Index");
                //return View("MyTimeSheetsList");
            }
        }
        public async Task SendEmailSupportTicketAssigned(int supportId, bool includeAttachment = false)
        {
            try
            {
                SupportService supportService = new SupportService();
                //timeSheetService.ChangeTimeSheetStatus(timeSheetId, "Pending");
                var support = supportService.GetSupportWithId(supportId);

                var subject = "Service request Assigned  " + support.SupportCode;


                //var fileName = GetExcelFileName(timeSheet);
                var callbackUrl = Url.Action("SupportEngineerEdit", "Support", new { id = support.SupportId }, protocol: Request.Url.Scheme);
                var body = "Employee " + support.AspNetUser.Firstname + " " + support.AspNetUser.LastName + " submitted service request " +
                    ", You can check it by clicking <a href=\"" + callbackUrl + "\">here</a>";

                //var destinationEmail = WebConfigurationManager.AppSettings["HREmail"];
                var destinationEmail = support.AspNetUser1.Email;
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


                //TempData["status"] = "Expense Sheet submitted to your Reporting Manager";
                //return RedirectToAction("MyTimeSheetsList");

            }
            catch (Exception ex)
            {
                //TempData["error"] = "Something went wrong while sending email to your reporting manager";
                //return RedirectToAction("Index");
                //return View("MyTimeSheetsList");
            }
        }

        public async Task SendEmailSupportTicketDone(int supportId, bool includeAttachment = false)
        {
            try
            {
                SupportService supportService = new SupportService();
                //timeSheetService.ChangeTimeSheetStatus(timeSheetId, "Pending");
                var support = supportService.GetSupportWithId(supportId);

                var subject = "Service request Done  " + support.SupportCode;




                //var fileName = GetExcelFileName(timeSheet);
                var callbackUrl = Url.Action("MySupportDetails", "Support", new { id = support.SupportId }, protocol: Request.Url.Scheme);
                var body = "Your issue is marked as done by support engineer" +
                    ", please confirm it by clicking <a href=\"" + callbackUrl + "\">here</a>";

                //var destinationEmail = WebConfigurationManager.AppSettings["HREmail"];
                var destinationEmail = support.AspNetUser.Email;
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


                //TempData["status"] = "Expense Sheet submitted to your Reporting Manager";
                //return RedirectToAction("MyTimeSheetsList");

            }
            catch (Exception ex)
            {
                //TempData["error"] = "Something went wrong while sending email to your reporting manager";
                //return RedirectToAction("Index");
                //return View("MyTimeSheetsList");
            }
        }

        public async Task SendEmailSupportTicketNotFixed(int supportId, bool includeAttachment = false)
        {
            try
            {
                SupportService supportService = new SupportService();
                //timeSheetService.ChangeTimeSheetStatus(timeSheetId, "Pending");
                var support = supportService.GetSupportWithId(supportId);

                var subject = "Service request Done  " + support.SupportCode;




                //var fileName = GetExcelFileName(timeSheet);
                var callbackUrl = Url.Action("SupportEngineerEdit", "Support", new { id = support.SupportId }, protocol: Request.Url.Scheme);
                var body = "The issue is reported not fixed by user" +
                    ", you can check it by clicking <a href=\"" + callbackUrl + "\">here</a>";

                //var destinationEmail = WebConfigurationManager.AppSettings["HREmail"];
                var destinationEmail = support.AspNetUser1.Email;
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


                //TempData["status"] = "Expense Sheet submitted to your Reporting Manager";
                //return RedirectToAction("MyTimeSheetsList");

            }
            catch (Exception ex)
            {
                //TempData["error"] = "Something went wrong while sending email to your reporting manager";
                //return RedirectToAction("Index");
                //return View("MyTimeSheetsList");
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


        public ActionResult EditDocumentUpload(SupportViewModel svm, HttpPostedFileBase[] FILEPhoto)
        {
            string Name = string.Empty;
            string strImageLink = string.Empty;
            string ext = string.Empty;
            int sid = default(int);
            sid = svm.SupportId;
            SupportService supportService = new SupportService();
            //Upload Files
            foreach (HttpPostedFileBase varfile in FILEPhoto)
            {
                if (varfile != null && varfile.ContentLength > 0)
                {
                    SupportAttachment supportDoc = new SupportAttachment()
                    {
                        SupportId = svm.SupportId,
                        FileServerPath = ConfigurationManager.AppSettings["SupportAttachments"] + svm.SupportId.ToString() + "/",
                        FileName = varfile.FileName,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedUserId = User.Identity.GetUserId(),
                        CreatedDate = DateTime.Now,
                        ModifiedUserId = User.Identity.GetUserId(),
                        ModifiedDate = DateTime.Now
                    };
                    supportService.SetSupportAttachment(supportDoc);
                    string subDirectory = Server.MapPath(supportDoc.FileServerPath);
                    System.IO.Directory.CreateDirectory(subDirectory);
                    var newFileName = Path.Combine(subDirectory, supportDoc.FileName);
                    varfile.SaveAs(newFileName);

                }
            }

            
            return RedirectToAction("MySupportEdit", "Support", new { id = sid });
        }

        public ActionResult DetailDocumentUpload(SupportViewModel svm, HttpPostedFileBase[] FILEPhoto)
        {
            string Name = string.Empty;
            string strImageLink = string.Empty;
            string ext = string.Empty;
            int sid = default(int);
            sid = svm.SupportId;
            SupportService supportService = new SupportService();
            //Upload Files
            foreach (HttpPostedFileBase varfile in FILEPhoto)
            {
                if (varfile != null && varfile.ContentLength > 0)
                {
                    SupportAttachment supportDoc = new SupportAttachment()
                    {
                        SupportId = svm.SupportId,
                        FileServerPath = ConfigurationManager.AppSettings["SupportAttachments"] + svm.SupportId.ToString() + "/",
                        FileName = varfile.FileName,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedUserId = User.Identity.GetUserId(),
                        CreatedDate = DateTime.Now,
                        ModifiedUserId = User.Identity.GetUserId(),
                        ModifiedDate = DateTime.Now
                    };
                    supportService.SetSupportAttachment(supportDoc);
                    string subDirectory = Server.MapPath(supportDoc.FileServerPath);
                    System.IO.Directory.CreateDirectory(subDirectory);
                    var newFileName = Path.Combine(subDirectory, supportDoc.FileName);
                    varfile.SaveAs(newFileName);

                }
            }


            return RedirectToAction("MySupportDetails", "Support", new { id = sid });
        }

        public ActionResult FileExists(string imgname, string supportId)
        {
            if (!string.IsNullOrEmpty(imgname))
            {
                try
                {
                    // var fileId = Guid.Parse(id);
                    byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~" + ConfigurationManager.AppSettings["SupportAttachments"] + supportId.ToString() + "/" + imgname));
                    //var myFile = Server.MapPath("~" + ConfigurationManager.AppSettings["SupportAttachments"] + supportId.ToString() + imgname);

                    if (fileBytes != null)
                    {
                        //byte[] fileBytes = myFile.FileData;
                        // return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, myFile.FileName);
                        return Json(
             new { isFileExists = true },
             JsonRequestBehavior.AllowGet
         );
                    }
                }
                catch (Exception ex)
                {
                    return Json(
         new { isFileExists = false },
         JsonRequestBehavior.AllowGet
     );
                }
            }
            return Json(
          new { isFileExists = false },
          JsonRequestBehavior.AllowGet
      );

        }

        public FileResult GetImage(string imgname, string supportId)
        {
            if (!string.IsNullOrEmpty(imgname))
            {
                try
                {
                    // var fileId = Guid.Parse(id);
                    byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~" + ConfigurationManager.AppSettings["SupportAttachments"] + supportId.ToString() + "/"+ imgname));
                    //var myFile = Server.MapPath("~" + ConfigurationManager.AppSettings["SupportAttachments"] + supportId.ToString() + imgname);

                    if (fileBytes != null)
                    {
                        //byte[] fileBytes = myFile.FileData;
                        // return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, myFile.FileName);
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, imgname);
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return null;
        }

        [HttpPost]
        public ActionResult RemoveImg(int imgid, string imgname, int supportId)
        {
            string imgresult = string.Empty;
            bool isDeleted = false;
            SupportService supportService = new SupportService();
            isDeleted = supportService.DeleteSupportDocumentFile(imgid);
            if (isDeleted)
            {
                //Remove file from Phyical Location
                string newFileName = Path.Combine(Server.MapPath("~" + ConfigurationManager.AppSettings["SupportAttachments"] + supportId.ToString()), imgname);
                if (System.IO.File.Exists(newFileName))
                {
                    System.IO.File.Delete(newFileName);
                }
                imgresult = "File Removed Successfully!";
            }
            else
            {
                imgresult = "File Not Removed";
            }

            return Json(imgresult, JsonRequestBehavior.AllowGet);
        }
    }
}
