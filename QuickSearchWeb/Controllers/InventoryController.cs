

using QuickSearchBusiness.Services;
using QuickSearchData;
using QuickSearchWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AutoMapper;
using System.Collections;
using Microsoft.AspNet.Identity;

namespace QuickSearch.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {

        


        [Authorize(Roles = "InventoryAdmin,InventoryManager")]
        // GET: Inventory
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

            ProductService productService = new ProductService();
            var products = productService.GetProductList();
            List<ProductListViewModel> productList = new List<ProductListViewModel>();
            

           foreach (Product p in products)
            {
                ProductListViewModel prod = new ProductListViewModel();
                prod.ProductId = p.ProductId;
                prod.ProductCode = p.ProductCode;
                prod.ProductName = p.ProductName;
                if(p.AssignedUser != null)
                {
                    prod.AssignTo = p.AssignedUser.Firstname +" "+ p.AssignedUser.LastName;

                }
                else if( p.OtherAssignedUser != null)
                {
                    prod.AssignTo = p.OtherAssignedUser;
                }
                else
                {
                    prod.AssignTo = "N/A";
                }
                
                prod.SelectedCategory = p.Category.LookupCodeName;  
                if(p.Status != null)
                {
                    prod.Status = p.Status.LookupCodeName;
                }
                prod.Location = p.Location;
                prod.Description = p.Description;
                prod.IsActive = p.IsActive;
                prod.IsDeleted = p.IsDeleted;
                prod.ModifiedDate = p.ModifiedDate;
                
                productList.Add(prod);
            }

            return View(productList);
            
        }

        


        [Authorize(Roles = "InventoryAdmin")]
        // GET: Inventory/Create
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

            ProductViewModel pvm = new ProductViewModel();
            ProductService productService = new ProductService();
            int lastProductCode = productService.GetLastProductCode();
            pvm.ProductCode = "10052005-" + (1100+lastProductCode+1).ToString();
            pvm.AssignedDate = DateTime.Now;
            pvm.CategoryList = GetProductDropDownList("Category", null);
            pvm.ProductTypeList = GetProductDropDownList("ProductType", null);
            pvm.PlatformList = GetProductDropDownList("Platform", null);
            pvm.StatusList = GetProductDropDownList("ProductStatus", null);
            return View(pvm);
        }

        // POST: Inventory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "InventoryAdmin")]
        public ActionResult Create(ProductViewModel pvm)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    pvm.CategoryList = GetProductDropDownList("Category", null);
                    var lookupCodeName = GetNameFromCode(pvm.LookupCategory);
                    pvm.ProductTypeList = GetProductDropDownList(lookupCodeName, null);
                    pvm.PlatformList = GetProductDropDownList("Platform", null);
                    pvm.StatusList = GetProductDropDownList("ProductStatus", null);
                    return View(pvm);
                }

                

                var product = Mapper.Map<Product>(pvm);
                product.IsActive = true;
                product.IsDeleted = false;
                product.CreatedUserId = User.Identity.GetUserId();
                product.CreatedDate = DateTime.Now;
                product.ModifiedUserId = User.Identity.GetUserId();
                product.ModifiedDate = DateTime.Now;
                ProductService productService = new ProductService();
                productService.CreateNewProduct(product);
                
                TempData["status"] = "Inventory successfully added";
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                pvm.CategoryList = GetProductDropDownList("Category",null);
                var lookupCodeName = GetNameFromCode(pvm.LookupCategory);
                pvm.ProductTypeList = GetProductDropDownList(lookupCodeName, null);
                pvm.PlatformList = GetProductDropDownList("Platform", null);
                pvm.StatusList = GetProductDropDownList("ProductStatus", null);
                TempData["error"] = "Something went wrong while adding inventory";
                return View(pvm);
            }
        }

        [Authorize(Roles = "InventoryAdmin")]
        // GET: Inventory/Edit/5
        public ActionResult Edit(int id)
        {
            
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            UserService userService = new UserService();
            ProductService productService = new ProductService();
            var product = productService.GetProductWithId(id);

           
            var pvm = Mapper.Map<ProductViewModel>(product);

            if (pvm.AssignTo != null)
            {
                var user = userService.GetUserWithId(pvm.AssignTo);
                pvm.AssignToUserName = user.Firstname + " " + user.LastName + "(" + user.UserName + ")";
            }
            pvm.CategoryList = GetProductDropDownList("Category", null);
            pvm.ProductTypeList = GetProductDropDownList(null,pvm.LookupCategory);
            pvm.PlatformList = GetProductDropDownList("Platform", null);
            pvm.StatusList = GetProductDropDownList("ProductStatus", null);
            if(pvm.AssignTo == null)
            {
                if(!string.IsNullOrEmpty(pvm.OtherAssignedUser))
                {
                    pvm.OtherAssign = true;
                }
            }
            else
            {
                pvm.OtherAssign = false;
            }
           


            return View(pvm);
        }


        [Authorize(Roles = "InventoryAdmin")]
        // POST: Inventory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel pvm)
        {
            try
            {
               
                if (!ModelState.IsValid)
                {
                    pvm.CategoryList = GetProductDropDownList("Category", null);
                    var lookupCodeName = GetNameFromCode(pvm.LookupCategory);
                    pvm.ProductTypeList = GetProductDropDownList(lookupCodeName, null);
                    pvm.PlatformList = GetProductDropDownList("Platform", null);
                    pvm.StatusList = GetProductDropDownList("ProductStatus", null);
                    return View(pvm);
                }
                ProductService productService = new ProductService();
                var product = productService.GetProductWithId(pvm.ProductId);


                
                product.ProductCode = pvm.ProductCode;
                product.ProductCompany = pvm.ProductCompany;
                product.LookupProductType = pvm.LookupProductType;
                product.ProductName = pvm.ProductName;
                product.LookupCategory = pvm.LookupCategory;
                product.MakeModel = pvm.MakeModel;
                product.Version = pvm.Version;
                product.AssignTo = pvm.AssignTo;
                product.AssignedDate = pvm.AssignedDate;
                product.Description = pvm.Description;
                product.LookupStatus = pvm.LookupStatus;
                product.GrantNumber = pvm.GrantNumber;
                product.ActivationKey = pvm.ActivationKey;
                product.LookupPlatform = pvm.LookupPlatform;
                product.ExpirationDate = pvm.ExpirationDate;
                product.PurchaseDate = pvm.PurchaseDate;
                product.AssignToHardware = pvm.AssignToHardware;
                product.OtherAssignedUser = pvm.OtherAssignedUser;
                product.Location = pvm.Location;
                product.ModifiedUserId = User.Identity.GetUserId();
                product.ModifiedDate = DateTime.Now;
                
                productService.UpdateProduct(product);
                TempData["status"] = "Inventory changes are successfully saved";
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["error"] = "Something went wrong while editing inventory";
                pvm.CategoryList = GetProductDropDownList("Category", null);
                var lookupCodeName = GetNameFromCode(pvm.LookupCategory);
                pvm.ProductTypeList = GetProductDropDownList(lookupCodeName, null);
                pvm.PlatformList = GetProductDropDownList("Platform", null);
                pvm.StatusList = GetProductDropDownList("ProductStatus", null);
                return View(pvm);
            }
        }


        [Authorize(Roles = "InventoryManager")]
        // GET: Inventory/Edit/5
        public ActionResult ManagerEdit(int id)
        {
           
            if (TempData["error"] != null)
            {
                ViewBag.error = TempData["error"];

            }
            if (TempData["status"] != null)
            {
                ViewBag.status = TempData["status"];
            }

            UserService userService = new UserService();
            ProductService productService = new ProductService();
            var product = productService.GetProductWithId(id);

            
            var pvm = Mapper.Map<ProductViewModel>(product);

            if (pvm.AssignTo != null)
            {
                var user = userService.GetUserWithId(pvm.AssignTo);
                pvm.AssignToUserName = user.Firstname + " " + user.LastName + "(" + user.UserName + ")";
            }
            pvm.CategoryList = GetProductDropDownList("Category", null);
            pvm.ProductTypeList = GetProductDropDownList(null, pvm.LookupCategory);
            pvm.PlatformList = GetProductDropDownList("Platform", null);
            pvm.StatusList = GetProductDropDownList("ProductStatus", null);
          
            if (pvm.AssignTo == null)
            {
                if (!string.IsNullOrEmpty(pvm.OtherAssignedUser))
                {
                    pvm.OtherAssign = true;
                }
            }
            else
            {
                pvm.OtherAssign = false;
            }

            return View(pvm);
        }


        [Authorize(Roles = "InventoryManager")]
        // POST: Inventory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManagerEdit(ProductViewModel pvm)
        {
            try
            {
                // TODO: Add update logic here
                if (!ModelState.IsValid)
                {
                    pvm.CategoryList = GetProductDropDownList("Category", null);
                    var lookupCodeName = GetNameFromCode(pvm.LookupCategory);
                    pvm.ProductTypeList = GetProductDropDownList(lookupCodeName, null);
                    pvm.PlatformList = GetProductDropDownList("Platform", null);
                    pvm.StatusList = GetProductDropDownList("ProductStatus", null);
                    return View(pvm);
                }
                ProductService productService = new ProductService();
                var product = productService.GetProductWithId(pvm.ProductId);
                product.AssignTo = pvm.AssignTo;
                product.OtherAssignedUser = pvm.OtherAssignedUser;
                

                product.ModifiedUserId = User.Identity.GetUserId();
                product.ModifiedDate = DateTime.Now;

                productService.UpdateProduct(product);
                TempData["status"] = "Inventory changes are successfully Assigned";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while editing inventory";
                pvm.CategoryList = GetProductDropDownList("Category", null);
                var lookupCodeName = GetNameFromCode(pvm.LookupCategory);
                pvm.ProductTypeList = GetProductDropDownList(lookupCodeName, null);
                pvm.PlatformList = GetProductDropDownList("Platform", null);
                pvm.StatusList = GetProductDropDownList("ProductStatus", null);
                return View(pvm);
            }
        }

       


        [Authorize(Roles = "InventoryAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                ProductService productService = new ProductService();
                Product product = productService.GetProductWithId(id);

                product.IsActive = false;
                product.IsDeleted = true;
                productService.SoftDeleteProduct(product);
                TempData["status"] = "Inventory successfully deleted";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["error"] = "Something went wrong while deleting inventory";
                return View();
            }
        }



        private List<DropDownListViewModel> GetProductDropDownList(string lookupTypeName,int? lookupCodeId)
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
                lookUpCategories = productService.GetProductDropDownList(lookupCodeId??0);
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

       
        [Authorize(Roles = "InventoryAdmin")]
        public ActionResult SoftDelete(int id)
        {
            try
            {
                ProductService productService = new ProductService();
                Product product = productService.GetProductWithId(id);

                product.IsActive = false;
                product.IsDeleted = true;
                productService.SoftDeleteProduct(product);
                TempData["status"] = "Inventory successfully deleted";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while deleting inventory";
                return View();
            }
        }


        
        [Authorize(Roles = "InventoryAdmin")]
        public ActionResult HardDelete(int id)
        {

            try
            {
                ProductService productService = new ProductService();
                productService.HardDeleteProduct(id);
                TempData["status"] = "Inventory successfully deleted";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong while deleting inventory";
                return View();
            }

        }

        private string GetNameFromCode(int lookupCode)
        {
            ProductService productService = new ProductService();
            var lookupCodeName = productService.GetLookupNameFromId(lookupCode);
            return lookupCodeName;
        }

        public ActionResult GetProductTypeList(int categoryId)
        {
           List<DropDownListViewModel> categories = GetProductDropDownList(null, categoryId);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(categories);
            return Json(result, JsonRequestBehavior.AllowGet);
            
        }

        public JsonResult GetSearchValue(string search)
        {
            ProductService productService = new ProductService();
            var obj = productService.GetListOfUserStartsWith(search);
            var result = obj.Select(a => new { Name = a.Firstname +" "+ a.LastName +" ("+ a.UserName + ")", Value = a.Id });         
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }

 


}
