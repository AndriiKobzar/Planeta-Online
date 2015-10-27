using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Planeta_Online.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public bool Gender { get; set; }
        public int Age { get; set; }
        public string Specialization { get; set; }
        public bool IsMember { get; set; }
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
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Planeta_Online.Models.Event> Events { get; set; }
        public System.Data.Entity.DbSet<Planeta_Online.Models.EventRegistration> EventRegistrations { get; set; }
        public System.Data.Entity.DbSet<Planeta_Online.Models.EventApplication> EventApplications { get; set; }
        public System.Data.Entity.DbSet<Planeta_Online.Models.Book> Books { get; set; }

    }
}