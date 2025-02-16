using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using AutoMapper;
using TWeb48.Helpers;
using TWeb48.Models;

namespace TWeb48.Services
{
    public interface IAccountService
    {
        string Login(LoginModel model);
        ActionResult Register(RegisterModel model);
        ActionResult UpdateProfile(UpdateRequest request);
        ActionResult GetProfile();
        UpdateRequest PreUpdateProfile();
    }

    public class AccountService : IAccountService
    {
        private readonly TWebDbContext _context;
        private readonly IMapper _mapper;

        public AccountService()
        {
            _context = new TWebDbContext();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<AppMappingProfile>()).CreateMapper();
        }

        public string Login(LoginModel model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == model.Username);
            if (user == null)
            {
                return "/Home/Index";
            }

            if (!PasswordHelper.VerifyPassword(model.Password, user.PasswordHash))
            {
                return null;
            }

            var roles = _context.UserRoles
                .Where(ur => ur.UserId == user.UserId)
                .Select(ur => ur.Role.Name)
                .ToArray();

            var ticket = new FormsAuthenticationTicket(
                1,
                user.Name,
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                true,
                string.Join(",", roles),
                FormsAuthentication.FormsCookiePath
            );

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL
            };
            HttpContext.Current.Response.Cookies.Add(authCookie);
            return "/Home/Index";
        }
        
        public ActionResult Register(RegisterModel model)
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Name = model.Username,
                Email = model.Email,
                PasswordHash = PasswordHelper.HashPassword(model.Password),
            };

            var role = _context.Roles.FirstOrDefault(r => r.Name == "User");

            _context.Users.Add(user);
            _context.UserRoles.Add(new UserRole
            {
                RoleId = role.RoleId,
                UserId = user.UserId
            });
            _context.SaveChanges();

            return new RedirectResult("/Home/Index");
        }
        
        public ActionResult GetProfile()
        {
            var userName = HttpContext.Current.User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Name == userName);
            if (user == null)
            {
                return new HttpNotFoundResult();
            }

            var roles = _context.UserRoles
                .Where(ur => ur.UserId == user.UserId)
                .Select(ur => ur.Role.Name)
                .ToList();

            var model = new UserProfileViewModel
            {
                User = user,
                Roles = roles
            };

            return new ViewResult { ViewName = "GetProfile", ViewData = new ViewDataDictionary(model) };
        }
        
        public ActionResult UpdateProfile(UpdateRequest request)
        {
            var userName = HttpContext.Current.User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Name == userName);
            if (user == null)
            {
                return new HttpNotFoundResult();
            }

            if (user.PasswordHash != PasswordHelper.HashPassword(request.OldPassword))
            {
                return new ViewResult { ViewName = "UpdateProfile", ViewData = new ViewDataDictionary { { "OldPassword", "Invalid password" } } };
            }

            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.Name = request.Name;
            user.LicenceNumber = request.LicenceNumber;
            user.PasswordHash = PasswordHelper.HashPassword(request.NewPassword);

            _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();

            return new RedirectResult("/Account/GetProfile");
        }
        
        public UpdateRequest PreUpdateProfile()
        {
            var userName = HttpContext.Current.User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Name == userName);
            if (user == null)
            {
                return null;
            }

            var model = new UpdateRequest
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                LicenceNumber = user.LicenceNumber
            };

            return model;
        }
        
    }
}