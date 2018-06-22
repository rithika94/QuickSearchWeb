using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickSearchData;
//using AutoMapper;

namespace QuickSearchBusiness.Services
{

    public class ProductService
    {
        //private readonly AdminPortalEntities _context;
        //public ProductService(AdminPortalEntities context) { _context = context; }

        

        public bool CreateNewProduct(Product product)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return true;
            }
            
        }

        public bool UpdateProduct(Product product)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.Products.Attach(product);
                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            
        }

        public List<Product> GetProductList()
        {
            using (var _context = new AdminPortalEntities())
            {
                var products = _context.Products.Include("Category").Include("ProductType").Include(x=>x.Status).Include("AssignedUser").ToList();
                return products;
            }
        }

        public int GetProductCount()
        {
            using (var _context = new AdminPortalEntities())
            {
                var products = _context.Products.Count();
                return products;
            }
        }

        public int GetLastProductCode()
        {
            using (var _context = new AdminPortalEntities())
            {
                var lastProduct = _context.Products.OrderByDescending(o => o.ProductId).FirstOrDefault();
                if (lastProduct != null)
                {
                    return lastProduct.ProductId;
                }
                else
                {
                    return 0;
                }
                
            }
        }

        public List<AspNetUser> GetListOfUserStartsWith(string term)
        {
            using (var _context = new AdminPortalEntities())
            {
                var model = _context.AspNetUsers.Where(a => (a.Firstname.Contains(term)) && (a.IsActive == true) && (a.IsDeleted==false)).ToList()/*.Select(a => new { label = a.UserName })*/;
                return model;
            }
                
        }

        //public bool DeleteProduct(int id)
        //{
        //    using (var _context = new AdminPortalEntities())
        //    {
        //        var product = _context.Products.SingleOrDefault(p => p.ProductId == id);

        //        product.IsActive = false;
        //        product.IsDeleted = true;
        //        _context.Products.Attach(product);
        //        _context.Entry(product).State = EntityState.Modified;
        //        _context.SaveChanges();
        //        return true;
        //    }

        //}


        public Product GetProductWithId(int? id)
        {
            using (var _context = new AdminPortalEntities())
            {
                var product = _context.Products.SingleOrDefault(p => p.ProductId == id && p.IsDeleted == false);
                return product;
            }
        }

        //public List<LookupCodeMaster> GetCategoryList()
        //{
        //    using (var _context = new AdminPortalEntities())
        //    {
        //        var categories = _context.LookupCodeMasters.Where(l => l.LookupTypeMaster.LookupTypeName == "Category").ToList();
        //        return categories;
        //    }
        //}

        public List<LookupCodeMaster> GetProductDropDownList(string lookupTypeName)
        {
            using (var _context = new AdminPortalEntities())
            {
                var list = _context.LookupCodeMasters.Where(l => l.LookupTypeMaster.LookupTypeName == lookupTypeName && l.IsActive == true).ToList();
                return list;
            }
        }

        public List<LookupCodeMaster> GetProductDropDownList(int lookupCodeId)
        {
            using (var _context = new AdminPortalEntities())
            {
                List<LookupCodeMaster> list = new List<LookupCodeMaster>();
                var lookupCode = _context.LookupCodeMasters.SingleOrDefault(l => l.LookupCodeId == lookupCodeId);
                if(lookupCode != null)
                {
                    list = _context.LookupCodeMasters.Where(l => l.LookupTypeMaster.LookupTypeName == lookupCode.LookupCodeName).ToList();
                    
                }
                return list;

            }
        }


        public bool SoftDeleteProduct(Product product)
        {
            using (var _context = new AdminPortalEntities())
            {
                _context.Products.Attach(product);
                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();

                return true;
            }
        }


        public bool HardDeleteProduct(int productId)
        {
            using (var _context = new AdminPortalEntities())
            {
                var product = _context.Products.SingleOrDefault(s => s.ProductId == productId);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    _context.SaveChanges();
                }
                return true;
            }
        }

        public string GetLookupNameFromId(int id)
        {
            using (var _context = new AdminPortalEntities())
            {
                var lookupCode = _context.LookupCodeMasters.SingleOrDefault(p => p.LookupCodeId == id).LookupCodeName;
                return lookupCode;
            }
        }
    }
}
