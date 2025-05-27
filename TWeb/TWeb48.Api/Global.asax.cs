using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TWeb48.Business.Helpers;
using TWeb48.Data.Models;

namespace TWeb48.Api
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            CheckAndCreateRoles();
            CheckAndCreateDefaultUser();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        
        
        private void CheckAndCreateRoles()
        {
            using (var context = new TWebDbContext())
            {
                if (!context.Roles.Any())
                {
                    context.Roles.Add(new Role { Name = "Admin", RoleId = Guid.NewGuid()});
                    context.Roles.Add(new Role { Name = "User", RoleId = Guid.NewGuid()});
                    context.SaveChanges();
                }
            }
        }

        private void CheckAndCreateDefaultUser()
        {
            using (var context = new TWebDbContext())
            {
                var user = context.Users.FirstOrDefault(r => r.Name == "demouser");

                if (user == null)
                {
                    var role = context.Roles.FirstOrDefault(r => r.Name == "Admin");
                    if (role != null)
                    {
                        var pass = PasswordHelper.HashPassword("1234");
                        user = new User
                        {
                            Name = "demouser",
                            PasswordHash = pass,
                            Email = "demouser@demo.demo",
                            PhoneNumber = "11111111111",
                            LicenceNumber = "LIC123456",
                            UserId = Guid.NewGuid()
                        };
                        context.Users.Add(user);
                        context.UserRoles.Add(new UserRole
                        {
                            RoleId = role.RoleId,
                            UserId = user.UserId
                        });
                        context.SaveChanges();
                    }
                }
            }
        }
        
    }
}