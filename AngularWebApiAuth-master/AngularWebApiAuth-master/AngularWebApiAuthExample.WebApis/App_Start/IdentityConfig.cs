using System.Data.Entity;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using AngularWebApiAuthExample.WebApis.Models;

namespace AngularWebApiAuthExample.WebApis
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        //If you wanted to use your own database, you can simply override these two methods.
        //public override async Task<ApplicationUser> FindAsync(string userName, string password)
        //{
        //    var myUser = new MyUserTable();
        //    using (var ctx = new MyDbContext())
        //    {
        //        myUser =
        //                ctx.MyUserTables.FirstOrDefault(
        //                    x => x.UserName == userName && x.Password == password && x.Active);
        //    }
        //    if (myUser == null)
        //    {
        //        return null;
        //    }
        //    //Because I'm changing the minimum I can, I'm returning an ApplicationUser even though this
        //    //is not what I retrieve from the database.
        //    return await Task.Run(() => new ApplicationUser
        //    {
        //        UserName = myUser.UserName,
        //        Id = myUser.UserName,
        //    });
        //}

        //public override async Task<ClaimsIdentity> CreateIdentityAsync(ApplicationUser user, string authenticationType)
        //{
        //    var claimCollection = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, user.UserName),
        //        //The Roles below are examples. Of course, these would be coming from a database
        //        //and populated that way.
        //        new Claim(ClaimTypes.Role, "Admin"),
        //        new Claim(ClaimTypes.Role, "User")
        //        //http://www.remondo.net/simple-claims-based-identity/
        //    };

        //    var claimsIdentity = new ClaimsIdentity(claimCollection, authenticationType);

        //    return await Task.Run(() => claimsIdentity);
        //}
    }

    // Configure the RoleManager used in the application. RoleManager is defined in the ASP.NET Identity core assembly
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
        }
    }
    
    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            const string adminName = "admin@example.com";
            const string adminPassword = "Admin@123456";
            const string adminFirstName = "Admin";
            const string adminLastName = "Smith";
            
            const string userName = "user@example.com";
            const string userPassword = "User@123456";
            const string userFirstName = "User";
            const string userLastName = "Doe";

            const string adminRoleName = "Admin";
            const string userRoleName = "User";
            
            var role = roleManager.FindByName(adminRoleName);
            if (role == null)
            {
                role = new IdentityRole(adminRoleName);
                var roleresult = roleManager.Create(role);
            }

            var role2 = roleManager.FindByName(userRoleName);
            if (role2 == null)
            {
                role2 = new IdentityRole(userRoleName);
                var roleresult = roleManager.Create(role2);
            }

            var user = userManager.FindByName(adminName);
            if (user == null)
            {
                user = new ApplicationUser { UserName = adminName, Email = adminName, FirstName = adminFirstName, LastName = adminLastName };
                var result = userManager.Create(user, adminPassword);
                result = userManager.SetLockoutEnabled(user.Id, false);                
            }

            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }

            user = userManager.FindByName(userName);
            if (user == null)
            {
                user = new ApplicationUser { UserName = userName, Email = userName, FirstName = userFirstName, LastName = userLastName };
                var result = userManager.Create(user, userPassword);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role2.Name))
            {
                var result = userManager.AddToRole(user.Id, role2.Name);
            }
        }
    }
}
