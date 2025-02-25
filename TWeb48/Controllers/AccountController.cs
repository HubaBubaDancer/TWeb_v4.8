using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using TWeb48.Helpers;
using TWeb48.Models;
using TWeb48.Services;

namespace TWeb48.Controllers
{
    public class AccountController : Controller
    {
        
        private readonly IAccountService _accountService;
        private readonly IRentService _rentService;
        
        public AccountController()
        {
            _accountService = new AccountService();
            _rentService = new RentService();
        }
        
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var result = _accountService.Login(model);
            if (result == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return new RedirectResult(result);
        }
        
        
        public ActionResult Register()
        {
            return View();
        }
        
        
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            return _accountService.Register(model);
        }
        
        
        
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        
  
        
        [Authorize]
        [HttpGet]
        public new ActionResult GetProfile()    
        {
            return _accountService.GetProfile();
        }
        
        
        public ActionResult UpdateProfile()
        {
            var model = _accountService.PreUpdateProfile();
            return View(model);
        }
        
        [HttpPost]
        public ActionResult UpdateProfile(UpdateRequest request)
        {
            return _accountService.UpdateProfile(request);
        }
        
        
        public ActionResult Rent(Guid id)
        {
            var rent = _rentService.GetRent(id);
            if (rent == null)
            {
                return HttpNotFound();
            }
            return View(rent);
        }
        
        
        public ActionResult Rents(Guid userId, int page = 1, int pageSize = 10)
        {
            var rents = _rentService
                .GetRents(userId)
                .OrderByDescending(x => x.StartDate);
            
            var totalItems = _rentService.GetRents(userId).Count();

            var model = new IndexViewModel<Rent>
            {
                Items = rents,
                PageInfo = new PageInfo
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };
            return View(rents);
        }
    }
}