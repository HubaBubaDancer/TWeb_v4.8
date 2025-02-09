using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TWeb48.Helpers;
using TWeb48.Models;

namespace TWeb48.Controllers
{
    public class AccountController : Controller
    {
        
        private readonly TWebDbContext _context;

        public AccountController()
        {
            _context = new TWebDbContext();
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
            
            var ticket = new FormsAuthenticationTicket(
                1,                     // Версия тикета
                user.Name,             // Имя пользователя
                DateTime.Now,          // Дата создания
                DateTime.Now.AddMinutes(30), // Время истечения
                true,                  // Запомнить (Persistent)// UserData (можно записать ID, роль, JSON и т. д.)
                FormsAuthentication.FormsCookiePath
            );

            // Шифруем тикет и создаем куку
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,  // Запрещает доступ через JS
                Secure = FormsAuthentication.RequireSSL // Включает Secure, если сайт работает по HTTPS
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
        
        
        
        public new ActionResult Profile()
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
        
        
        
        
        
        
        
        
        
        
    }
}