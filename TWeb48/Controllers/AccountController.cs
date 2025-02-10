using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using TWeb48.Helpers;
using TWeb48.Models;

namespace TWeb48.Controllers
{
    public class AccountController : Controller
    {
        
        private readonly TWebDbContext _context;
        private readonly IMapper _mapper;

        public AccountController()
        {
            _context = new TWebDbContext();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<AppMappingProfile>()).CreateMapper();
        }

        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == model.Username);
            if (user == null)
            {
                ModelState.AddModelError("Username", "User not found");
                return View(model);
            }
            
            if (!PasswordHelper.VerifyPassword(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("Password", "Invalid password");
                return View(model);
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
            Response.Cookies.Add(authCookie);
            return RedirectToAction("Index", "Home");
        }
        
        
        public ActionResult Register()
        {
            return View();
        }
        
        
        [HttpPost]
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
            
            return RedirectToAction("Index", "Home");
        }
        
        
        
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut(); // Выход пользователя
            return RedirectToAction("Login");
        }
        
  
        
        [Authorize]
        [HttpGet]
        public new ActionResult GetProfile()    
        {
            var userName = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Name == userName);
            if (user == null)
            {
                return HttpNotFound();
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

            return View(model);
        }
        
        
        public ActionResult UpdateProfile()
        {
            var userName = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Name == userName);
            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new UpdateRequest
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                LicenceNumber = user.LicenceNumber
            };

            return View(model);
        }
        
        [HttpPost]
        public ActionResult UpdateProfile(UpdateRequest request)
        {
            var userName = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Name == userName);
            if (user == null)
            {
                return HttpNotFound();
            }
            
            if (user.PasswordHash != PasswordHelper.HashPassword(request.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "Invalid password");
                return View();
            }

            user.Email = request.Email;

            user.PhoneNumber = request.PhoneNumber;
            user.Name = request.Name;
            user.LicenceNumber = request.LicenceNumber;
            user.PasswordHash = PasswordHelper.HashPassword(request.NewPassword);
            
            _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("GetProfile");
        }
        
        
        
        
        
        
        
    }
}