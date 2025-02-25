using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TWeb48.Helpers;
using TWeb48.Models;
using TWeb48.Services;

namespace TWeb48.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly TWebDbContext _context;
        private readonly IRentService _rentService;
        
        public HomeController()
        {
            _context = new TWebDbContext();
            _rentService = new RentService();
        }
        
        public ActionResult Index()
        { 
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        
        
        public ActionResult Car(Guid id)
        {
            var car = _context.Cars.FirstOrDefault(c => c.Id == id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }
        
        
        public ActionResult Cars()
        {
            var cars = _context.Cars.ToList();
            return View(cars);
        }
        
        public ActionResult GetAllRents(int page = 1, int pageSize = 10)
        {
            var rents = _rentService.GetAllRents();
            var totalItems = _rentService.GetAllRents().Count();

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
        
        
        
        [HttpPost]
        public ActionResult RentCar(RentRequest request)
        {
            return _rentService.RentCar(request);
        }
        
        
        
        
    }
}