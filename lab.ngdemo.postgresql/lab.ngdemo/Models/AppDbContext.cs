using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace lab.ngdemo.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            this.Database.Connection.Open();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<ContactJobTiteRatingStaffingRate>()
            //        .Map(e => e.ToTable("ContactJobTiteRatingStaffingRate"));
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

    }

    #region Initial data

    // Change the base class as follows if you want to drop and create the database during development:
    //public class DbInitializer : DropCreateDatabaseAlways<AppDbContext>
    //public class DbInitializer : CreateDatabaseIfNotExists<AppDbContext>
    public class DbInitializer : DropCreateDatabaseIfModelChanges<AppDbContext>
    {
        private static void CreateUserWithRole(string username, string password, string email, string rolename, AppDbContext context)
        {
            var status = new MembershipCreateStatus();

            Membership.CreateUser(username, password, email);
            if (status == MembershipCreateStatus.Success)
            {
                // Add the role.
                var user = context.Users.Find(username);
                var adminRole = context.Roles.Find(rolename);
                user.Roles = new List<Role> { adminRole };
            }
        }

        protected override void Seed(AppDbContext context)
        {
            // Create default roles.
            var roles = new List<Role>
                            {
                                new Role {RoleName = "Admin"},
                                new Role {RoleName = "User"}
                            };

            roles.ForEach(r => context.Roles.Add(r));

            context.SaveChanges();

            // Create some users.
            CreateUserWithRole("Rasel", "@123456", "raselahmmed@gmail.com", "Admin", context);
            CreateUserWithRole("Ahmmed", "@123456", "raselbappi@gmail.com", "Admin", context);
            CreateUserWithRole("Sohel", "@123456", "sohel@gmail.com", "User", context);
            CreateUserWithRole("Shafin", "@123456", "shafin@gmail.com", "User", context);

            context.SaveChanges();
        }
    }

    #endregion
}