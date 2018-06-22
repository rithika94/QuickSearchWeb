using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace QuickSearchWeb.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.


    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmployeeCode { get; set; }

        public DateTime JoiningDate { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedUserId { get; set; }
        public int LookupGender { get; set; }

        
        public bool IsActive { get; set; }
        public string ReportingManager { get; set; }
        public bool IsDeleted { get; set; }
        public string ModifiedUserId { get; set; }
        public DateTime ModifiedDate { get; set; }

    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)//AdminPortalEntities  DefaultConnection
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<QuickSearchWeb.Models.UserViewModel> UserViewModels { get; set; }
    }
}