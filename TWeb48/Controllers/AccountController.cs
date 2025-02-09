using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.Security;
using TWeb48.Helpers;
using TWeb48.Models;
using TWeb48.Services;

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
            
            FormsAuthentication.SetAuthCookie(user.Name, true);
            return RedirectToAction("Index", "Home");
        }
        
        
        public ActionResult Register()
        {
            // TODO
            
            return View();
        }
        
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut(); // Выход пользователя
            return RedirectToAction("Login");
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
    }
}