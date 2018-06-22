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
using System.IO;
using System.Configuration;
using System.Data.Entity.Validation;

namespace QuickSearchWeb.Controllers
{
    [Authorize(Roles = "RecruitmentAdmin,RecruitmentUser")]
    public class RecruitmentController : Controller
    {
        // GET: Recruitment

        public ActionResult Index()
        {
            RecruitmentService userService = new RecruitmentService();
            UserService uService = new UserService();
            var recruiters = userService.GetRecruiterList();
            List<RecruitmentListViewModel> recruiterlist = new List<RecruitmentListViewModel>();

            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            foreach (Recruitment u in recruiters)
            {
                RecruitmentListViewModel rcvm = new RecruitmentListViewModel();
                rcvm.RecruitmentId = u.RecruitmentId;
                rcvm.ModifiedDate = u.ModifiedDate;
                rcvm.FirstName = u.FirstName;
                rcvm.LastName = u.LastName;
                rcvm.PhoneNumber = u.PhoneNumber;
                rcvm.PrimarySkillSet = u.PrimarySkillSet;
                rcvm.RecruiterName = User.Identity.Name;
                rcvm.AvailableDate = u.AvailableDate;
                rcvm.CreatedUser = uService.GetUserWithId(u.CreatedUserId).Firstname + ' ' + uService.GetUserWithId(u.CreatedUserId).LastName;
                rcvm.ModifiedUser = uService.GetUserWithId(u.ModifiedUserId).Firstname + ' ' + uService.GetUserWithId(u.ModifiedUserId).LastName;
                recruiterlist.Add(rcvm);
            }

            return View(recruiterlist);
        }

        public ActionResult Test()
        {

            return View();
        }


        [Authorize(Roles = "RecruitmentAdmin")]
        // GET: Recruitment/Create
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

            RecruitmentViewModel pvm = new RecruitmentViewModel();
            RecruitmentService recruitmentService = new RecruitmentService();



            pvm.VisaStatusList = GetLookUpDropDownList("VisaStatus", null);
            pvm.EmployementTypeList = GetLookUpDropDownList("EmployementType", null);
            pvm.RecruiterName = User.Identity.Name;
            pvm.RecruiterId = User.Identity.GetUserId();
            return View(pvm);
        }


        [Authorize(Roles = "RecruitmentAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RecruitmentViewModel rvm/*, HttpPostedFileBase[] FILEPhoto*/)
        {
            try
            {

                RecruitmentViewModel pvm = new RecruitmentViewModel();
                int NewRecruitmentID = 0;
                //string Name = string.Empty;
                //string strImageLink = string.Empty;
                //string ext = string.Empty;
                if (!ModelState.IsValid)
                {
                    //RecruitmentViewModel rvm = new RecruitmentViewModel();
                    rvm.VisaStatusList = GetLookUpDropDownList("VisaStatus", null);
                    rvm.EmployementTypeList = GetLookUpDropDownList("EmployementType", null);
                    rvm.RecruiterName = User.Identity.Name;
                    rvm.RecruiterId = User.Identity.GetUserId();

                    return View(rvm);
                }

                Recruitment recruitment = new Recruitment();
                RecruitmentService recruitmentService = new RecruitmentService();


                recruitment.Email = rvm.Email;
                recruitment.RecruiterId = rvm.RecruiterId;
                recruitment.PhoneNumber = rvm.PhoneNumber;
                recruitment.FirstName = rvm.FirstName;
                recruitment.LastName = rvm.LastName;
                recruitment.LookupEmployementType = rvm.LookupEmployementType;
                recruitment.AvailableDate = rvm.AvailableDate;
                recruitment.Notes = rvm.Notes;
                recruitment.CurrentLocation = rvm.CurrentLocation;
                recruitment.C2CCompanyName = rvm.C2CCompanyName;
                recruitment.C2CContactNumber = rvm.C2CContactNumber;
                recruitment.C2CContactPerson = rvm.C2CContactPerson;
                recruitment.C2CEmail = rvm.C2CEmail;

                //RecruitmentClientListViewModel rlvm = new RecruitmentClientListViewModel();
                //rlvm.JobLocation = rvm.JobLocation;
                //rlvm.JobTitle = rvm.JobTitle;
                //rlvm.CandidateStatusList = rvm.CandidateStatusList;
                //rlvm.ClientName = rvm.ClientName;
                //rlvm.Comments = rvm.Comments;
                //rlvm.LookupCandidateStatus = rvm.LookupCandidateStatus;

                recruitment.LookupVisaStatus = rvm.LookupVisaStatus;
                recruitment.PrimarySkillSet = rvm.PrimarySkillSet;
                recruitment.SecondarySkillSet = rvm.SecondarySkillSet;
                recruitment.IsActive = true;
                recruitment.IsDeleted = false;
                recruitment.CreatedUserId = User.Identity.GetUserId();
                recruitment.CreatedDate = DateTime.Now;
                recruitment.ModifiedDate = DateTime.Now;
                recruitment.ModifiedUserId = User.Identity.GetUserId();

                //Insert data for recruitment table and get newly inserted ID
                NewRecruitmentID = recruitmentService.SetRecruitment(recruitment);
                ////Upload Files
                //foreach (HttpPostedFileBase varfile in FILEPhoto)
                //{
                //    if (varfile != null && varfile.ContentLength > 0)
                //    {
                //        ext = Path.GetExtension(varfile.FileName);
                //        //Name = Guid.NewGuid() + ext;
                //        Name = varfile.FileName;
                //        strImageLink = strImageLink + Name + ",";
                //        var newFileName = Path.Combine(Server.MapPath("~" + ConfigurationManager.AppSettings["RecruitDocument"]), Name);
                //        varfile.SaveAs(newFileName);
                //    }

                //}
                ////Insert data into RecruitmentDocuments
                //foreach (HttpPostedFileBase varfile in FILEPhoto)
                //{
                //    if (varfile != null && varfile.ContentLength > 0)
                //    {
                //        RecruitmentDocument Objrd = new RecruitmentDocument()
                //        {
                //            RecruitmentId = NewRecruitmentID,
                //            FileServerPath = ConfigurationManager.AppSettings["RecruitDocument"],
                //            RecruitmentDocumentName = varfile.FileName,
                //            IsActive = true,
                //            IsDeleted = false,
                //            CreatedUserId = User.Identity.GetUserId(),
                //            CreatedDate = DateTime.Now
                //        };
                //        recruitmentService.SetRecruitmentDocument(Objrd);
                //    }
                //}




                TempData["status"] = "Consultant details successfully added. Please upload documents if any";

                return RedirectToAction("Edit", "Recruitment", new { id = NewRecruitmentID });
            }
            catch (Exception ex)
            {
                rvm.VisaStatusList = GetLookUpDropDownList("VisaStatusList", null);
                TempData["error"] = "Something went wrong while adding Recruiter";
                return View(rvm);
            }
        }

        [Authorize(Roles = "RecruitmentAdmin")]
        // GET: Recruitment/Edit/5
        public async Task<ActionResult> Edit(int id)
        {

            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }


            RecruitmentService recruiterService = new RecruitmentService();

            var recruitment = recruiterService.GetRecruiterWithId(id);
            RecruitmentViewModel rvm = new RecruitmentViewModel();

            rvm.RecruitmentId = recruitment.RecruitmentId;
            rvm.Email = recruitment.Email;
            rvm.RecruiterId = recruitment.RecruiterId;
            rvm.RecruiterName = User.Identity.Name;
            rvm.PhoneNumber = recruitment.PhoneNumber;
            rvm.FirstName = recruitment.FirstName;
            rvm.LastName = recruitment.LastName;
            rvm.LookupEmployementType = recruitment.LookupEmployementType;
            rvm.AvailableDate = recruitment.AvailableDate;
            rvm.Notes = recruitment.Notes;
            rvm.CurrentLocation = recruitment.CurrentLocation;
            rvm.C2CCompanyName = recruitment.C2CCompanyName;
            rvm.C2CContactNumber = recruitment.C2CContactNumber;
            rvm.C2CContactPerson = recruitment.C2CContactPerson;
            rvm.C2CEmail = recruitment.C2CEmail;
            rvm.LookupVisaStatus = recruitment.LookupVisaStatus;
            rvm.PrimarySkillSet = recruitment.PrimarySkillSet;
            rvm.SecondarySkillSet = recruitment.SecondarySkillSet;
            rvm.CreatedDate = recruitment.CreatedDate;
            rvm.CreatedUserId = recruitment.CreatedUserId;
            rvm.VisaStatusList = GetLookUpDropDownList("VisaStatus", null);
            rvm.EmployementTypeList = GetLookUpDropDownList("EmployementType", null);
            rvm.CandidateStatusList = GetLookUpDropDownList("CandidateStatus", null);
            rvm.RFileList = recruiterService.GetRecruitFilesWithId(id);

            rvm.RecruitmentClientList = recruiterService.GetRecruitmentClientWithId(id);
            return View(rvm);
        }


        [Authorize(Roles = "RecruitmentAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RecruitmentViewModel rvm, FormCollection fc)
        {
            try
            {
                RecruitmentViewModel pvm = new RecruitmentViewModel();

                if (!ModelState.IsValid)
                {
                    //RecruitmentViewModel rcvm = new RecruitmentViewModel();
                    rvm.VisaStatusList = GetLookUpDropDownList("VisaStatus", null);
                    rvm.EmployementTypeList = GetLookUpDropDownList("EmployementType", null);
                    rvm.CandidateStatusList = GetLookUpDropDownList("CandidateStatus", null);
                    rvm.RecruiterName = User.Identity.Name;
                    rvm.RecruiterId = User.Identity.GetUserId();

                    return View(rvm);
                }

                Recruitment recruitment = new Recruitment();
                RecruitmentService recruitmentService = new RecruitmentService();


                recruitment.Email = rvm.Email;
                recruitment.RecruiterId = rvm.RecruiterId;
                recruitment.RecruitmentId = rvm.RecruitmentId;
                recruitment.PhoneNumber = rvm.PhoneNumber;
                recruitment.FirstName = rvm.FirstName;
                recruitment.LastName = rvm.LastName;
                recruitment.LookupEmployementType = rvm.LookupEmployementType;
                recruitment.AvailableDate = rvm.AvailableDate;
                recruitment.Notes = rvm.Notes;
                recruitment.CurrentLocation = rvm.CurrentLocation;
                recruitment.C2CCompanyName = rvm.C2CCompanyName;
                recruitment.C2CContactNumber = rvm.C2CContactNumber;
                recruitment.C2CContactPerson = rvm.C2CContactPerson;
                recruitment.C2CEmail = rvm.C2CEmail;


                recruitment.LookupVisaStatus = rvm.LookupVisaStatus;
                recruitment.PrimarySkillSet = rvm.PrimarySkillSet;
                recruitment.SecondarySkillSet = rvm.SecondarySkillSet;
                recruitment.IsActive = true;
                recruitment.IsDeleted = false;
                recruitment.CreatedUserId = rvm.CreatedUserId;
                recruitment.CreatedDate = rvm.CreatedDate;
                recruitment.ModifiedDate = DateTime.Now;
                recruitment.ModifiedUserId = User.Identity.GetUserId();


                recruitmentService.UpdateRecruitment(recruitment);

                //new PasswordHasher.HashPassword
                TempData["status"] = "Recruitment successfully Updated";

                return RedirectToAction("Index");
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                rvm.EmployementTypeList = GetLookUpDropDownList("EmployementType", null);
                rvm.RecruiterName = User.Identity.Name;
                rvm.RecruiterId = User.Identity.GetUserId();
                rvm.VisaStatusList = GetLookUpDropDownList("VisaStatus", null);
                rvm.CandidateStatusList = GetLookUpDropDownList("CandidateStatus", null);

                TempData["error"] = "Something went wrong while adding Recruiter";
                return View(rvm);
            }
        }

        private List<DropDownListViewModel> GetLookUpDropDownList(string lookupTypeName, int? lookupCodeId)
        {

            var _list = new List<DropDownListViewModel>();

            RecruitmentService recruitmentService = new RecruitmentService();

            List<LookupCodeMaster> lookUpCategories = new List<LookupCodeMaster>();
            if (lookupTypeName != null)
            {
                lookUpCategories = recruitmentService.GetLookUpDropDownList(lookupTypeName);
            }
            else if (lookupCodeId != null)
            {
                lookUpCategories = recruitmentService.GetLookUpDropDownList(lookupCodeId ?? 0);
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


        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "RecruitmentAdmin")]
        public ActionResult SoftDelete(int id)
        {
            try
            {
                RecruitmentService recruitmentservice = new RecruitmentService();
                Recruitment employeerecord = recruitmentservice.GetRecruiterWithId(id);

                employeerecord.IsActive = false;
                employeerecord.IsDeleted = true;
                recruitmentservice.SoftDeleteRecruitment(employeerecord);
                TempData["status"] = "Employee Record successfully deleted";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while deleting Employee Record";
                return View();
            }
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "RecruitmentAdmin")]
        public ActionResult HardDelete(int id)
        {

            try
            {
                RecruitmentService recruitmentservice = new RecruitmentService();
                recruitmentservice.HardDeleteRecruitment(id);
                TempData["status"] = "Employee Record successfully deleted";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while deleting Employee Record";
                return View();
            }

        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    file.SaveAs(Path.Combine(Server.MapPath("~/App_Data/Images"), fileName));
                    //var path = Path.Combine(Server.MapPath("~/App_Data/Images"), fileName);
                    //file.SaveAs(path);
                }
                ViewBag.Message = "Upload successful";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Upload failed";
                return RedirectToAction("Uploads");
            }
        }
        public FileResult GetImage(string imgname)
        {
            if (!string.IsNullOrEmpty(imgname))
            {
                try
                {
                    // var fileId = Guid.Parse(id);

                    var myFile = Server.MapPath("~" + ConfigurationManager.AppSettings["RecruitDocument"] + imgname);

                    if (myFile != null)
                    {
                        //byte[] fileBytes = myFile.FileData;
                        // return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, myFile.FileName);
                        return File(myFile, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(myFile));
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return null;
        }

        [HttpPost]
        public JsonResult BindVisaStatusList()
        {
            List<DropDownListViewModel> objVisaStatusList = GetLookUpDropDownList("CandidateStatus", null);

            return Json(objVisaStatusList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RemoveImg(int imgid, string imgpath, string imgname)
        {
            string imgresult = string.Empty;
            bool isDeleted = false;
            RecruitmentService recruitmentservice = new RecruitmentService();
            isDeleted = recruitmentservice.DeleteDocumentFile(imgid);
            if (isDeleted)
            {
                //Remove file from Phyical Location
                string newFileName = Path.Combine(Server.MapPath("~" + ConfigurationManager.AppSettings["RecruitDocument"]), imgname);
                if (System.IO.File.Exists(newFileName))
                {
                    System.IO.File.Delete(newFileName);
                }
                imgresult = "Image Remove Successfully!";
            }
            else
            {
                imgresult = "Image Not Remove";
            }

            return Json(imgresult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditDocumentUpload(RecruitmentViewModel rvm, HttpPostedFileBase[] FILEPhoto)
        {
            string Name = string.Empty;
            string strImageLink = string.Empty;
            string ext = string.Empty;
            int rid = default(int);
            rid = rvm.RecruitmentId;
            RecruitmentService recruitmentService = new RecruitmentService();
            //Upload Files
            foreach (HttpPostedFileBase varfile in FILEPhoto)
            {
                if (varfile != null && varfile.ContentLength > 0)
                {
                    ext = Path.GetExtension(varfile.FileName);
                    //Name = Guid.NewGuid() + ext;
                    Name = varfile.FileName;
                    strImageLink = strImageLink + Name + ",";
                    var newFileName = Path.Combine(Server.MapPath("~" + ConfigurationManager.AppSettings["RecruitDocument"]), Name);
                    varfile.SaveAs(newFileName);
                }

            }
            //Insert data into RecruitmentDocuments
            foreach (HttpPostedFileBase varfile in FILEPhoto)
            {
                if (varfile != null && varfile.ContentLength > 0)
                {
                    RecruitmentDocument Objrd = new RecruitmentDocument()
                    {
                        RecruitmentId = rid,
                        FileServerPath = ConfigurationManager.AppSettings["RecruitDocument"],
                        RecruitmentDocumentName = varfile.FileName,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedUserId = User.Identity.GetUserId(),
                        CreatedDate = DateTime.Now
                    };
                    recruitmentService.SetRecruitmentDocument(Objrd);
                }
            }
            return RedirectToAction("Edit", "Recruitment", new { id = rid });
        }

        public ActionResult RemoveClientDocument(int RecruitmentId, int FileId, int ClientId)
        {
            string UserId = string.Empty;
            UserId = User.Identity.GetUserId();
            RecruitmentService recruitmentService = new RecruitmentService();
            recruitmentService.DeleteClientDocumentById(UserId, FileId, ClientId);

            return RedirectToAction("Edit", "Recruitment", new { id = RecruitmentId });
        }
        [HttpPost]
        public ActionResult RemoveRecruitmentClient(int clientId)
        {
            string result = string.Empty;
            RecruitmentService recruitmentService = new RecruitmentService();
            recruitmentService.DeleteClients(clientId);
            result = "Row Deleted Successfully!";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateClientInformation(FormCollection fc)
        {
            int id = Convert.ToInt32(fc["RecruitmentId"]);
            RecruitmentService recruitmentService = new RecruitmentService();
            string hidClientCurrentId = Convert.ToString(fc["hidClientCurrentId"]);

            List<RecruitmentClient> rcList = new List<RecruitmentClient>();
            List<RecruitmentClient> existingRCList = new List<RecruitmentClient>();
            List<RecruitmentClientDocument> rcDocumentList = new List<RecruitmentClientDocument>();

            existingRCList = recruitmentService.GetRecruitmentClientWithId(id);

            recruitmentService.DeleteClientsByRecruitment(id);
            var originalDirectory = Server.MapPath(ConfigurationManager.AppSettings["ClientDocuments"]);
            int rowno = 0;
            for (int i = 0; i < hidClientCurrentId.Split(',').Length - 1; i++)
            {
                if (!string.IsNullOrEmpty(fc["txtClientName" + (i + 1)]) && !string.IsNullOrEmpty(fc["txtJobTitle" + (i + 1)]) && !string.IsNullOrEmpty(fc["txtCandidateStatus" + (i + 1)]))
                {
                    RecruitmentClient rlvm = new RecruitmentClient();
                    rlvm.JobLocation = Convert.ToString(fc["txtJobLocation" + (i + 1)]);
                    rlvm.JobTitle = Convert.ToString(fc["txtJobTitle" + (i + 1)]);
                    // rlvm.CandidateStatusList = rvm.CandidateStatusList;
                    rlvm.ClientName = Convert.ToString(fc["txtClientName" + (i + 1)]);
                    //rlvm.Comments = Convert.ToString(fc["txtJobLocation" + (i + 1)]);
                    if (!string.IsNullOrEmpty(fc["txtSubmitionDate" + (i + 1)]))
                    {
                        DateTime dtSubmissionDate = DateTime.MinValue;
                        DateTime.TryParse(fc["txtSubmitionDate" + (i + 1)], out dtSubmissionDate);

                        if (dtSubmissionDate != DateTime.MinValue)
                        {
                            rlvm.SubmissionDate = Convert.ToDateTime(fc["txtSubmitionDate" + (i + 1)]);
                        }
                        else
                        {
                            rlvm.SubmissionDate = null;
                        }
                    }
                    else
                    {
                        rlvm.SubmissionDate = null;
                    }
                    rlvm.LookupCandidateStatus = Convert.ToInt32(fc["txtCandidateStatus" + (i + 1)]);
                    rlvm.RecruitmentId = id;
                    rlvm.IsActive = true;
                    rlvm.IsDeleted = false;
                    rlvm.CreatedDate = DateTime.Now;
                    rlvm.CreatedUserId = User.Identity.GetUserId();
                    ////rlvm.ClientDocument1 = Convert.ToString(fc["ClientDocument1" + (i + 1)]);
                    //rlvm.ClientDocument2 = Convert.ToString(fc["ClientDocument2" + (i + 1)]);
                    rowno = i + 1;

                    if (Request.Files.Count > 0)
                    {
                        string FileDoc = string.Format("flClientDoc{0}", (i + 1));
                        string FileDoc2 = string.Format("flClientDoctwo{0}", (i + 1));
                        string oldClientdoc1 = string.Empty;
                        string oldClientdoc2 = string.Empty;
                        if (existingRCList.Count >= rowno)
                        {
                            oldClientdoc1 = existingRCList[i].ClientDocument1;
                            oldClientdoc2 = existingRCList[i].ClientDocument2;
                        }
                        // if (Request.Files.ToString() == FileDoc)
                        //     {
                        HttpPostedFileBase file = Request.Files[FileDoc];
                        HttpPostedFileBase file1 = Request.Files[FileDoc2];
                        //If Document 1 is null then only upload

                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName1 = Path.GetFileName(file.FileName);
                            string extension = Path.GetExtension(fileName1);
                            //If no document selected for upload
                            if (!string.IsNullOrEmpty(extension))
                            {
                                string newFileName = Convert.ToString(Guid.NewGuid() + extension);
                                bool isExists = System.IO.Directory.Exists(originalDirectory);

                                if (!isExists)
                                    System.IO.Directory.CreateDirectory(originalDirectory);

                                var path = string.Format("{0}{1}", originalDirectory, newFileName);
                                file.SaveAs(path);
                                rlvm.ClientDocument1 = newFileName;
                                rlvm.DocumentName1 = fileName1;
                            }
                        }
                        else
                        {
                            if (existingRCList.Count > i)
                            {
                                rlvm.ClientDocument1 = existingRCList[i].ClientDocument1;
                                rlvm.DocumentName1 = existingRCList[i].DocumentName1;
                            }
                        }

                        //    }
                        //else
                        // {
                        //          if (existingRCList.Count > i)
                        //          {
                        //              rlvm.ClientDocument1 = existingRCList[i].ClientDocument1;
                        //              rlvm.ClientDocument2 = existingRCList[i].ClientDocument2;
                        //              rlvm.DocumentName1 = existingRCList[i].DocumentName1;
                        //              rlvm.DocumentName2 = existingRCList[i].DocumentName2;
                        //          }
                        //  }

                        //for File Uploader2
                        //  if (Request.Files.ToString() == FileDoc2)
                        //  {

                        //If Document 2 is null then only upload

                        if (file1 != null && file1.ContentLength > 0)
                        {
                            var fileName1 = Path.GetFileName(file1.FileName);
                            string extension = Path.GetExtension(fileName1);
                            //If no document selected for upload
                            if (!string.IsNullOrEmpty(extension))
                            {
                                string newFileName = Convert.ToString(Guid.NewGuid() + extension);
                                bool isExists = System.IO.Directory.Exists(originalDirectory);

                                if (!isExists)
                                    System.IO.Directory.CreateDirectory(originalDirectory);

                                var path = string.Format("{0}{1}", originalDirectory, newFileName);
                                file.SaveAs(path);
                                rlvm.ClientDocument2 = newFileName;
                                rlvm.DocumentName2 = fileName1;
                            }
                        }
                        else
                        {

                            if (existingRCList.Count > i)
                            {
                                rlvm.ClientDocument2 = existingRCList[i].ClientDocument2;
                                rlvm.DocumentName2 = existingRCList[i].DocumentName2;
                            }
                        }
                        //  }
                        //else
                        //{
                        //    if (existingRCList.Count > i)
                        //    {
                        //        rlvm.ClientDocument1 = existingRCList[i].ClientDocument1;
                        //        rlvm.ClientDocument2 = existingRCList[i].ClientDocument2;
                        //        rlvm.DocumentName1 = existingRCList[i].DocumentName1;
                        //        rlvm.DocumentName2 = existingRCList[i].DocumentName2;
                        //    }
                        //}
                    }
                    else
                    {
                        if (existingRCList.Count > i)
                        {
                            rlvm.ClientDocument1 = existingRCList[i].ClientDocument1;
                            rlvm.ClientDocument2 = existingRCList[i].ClientDocument2;
                            rlvm.DocumentName1 = existingRCList[i].DocumentName1;
                            rlvm.DocumentName2 = existingRCList[i].DocumentName2;
                        }
                    }
                    rcList.Add(rlvm);
                }
            }
            rcList = recruitmentService.UpdateClient(rcList);

            return RedirectToAction("Edit", "Recruitment", new { id = id });
        }

    }
}