using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.Security;
using TWeb48.Models;

namespace TWeb48.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            // TODO
            
            if (model.Username == "admin" && model.Password == "1234") // Проверка логина и пароля
            {
                FormsAuthentication.SetAuthCookie(model.Username, false); // Создание куки авторизации
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid username or password";
            return View(model);
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