using System;
using System.Linq;
using System.Web.Mvc;
using TWeb48.Api.Reference.Dtos;
using TWeb48.Data.Models;
using TWeb48.Business.Helpers;
using TWeb48.Business.Services;

namespace TWeb48.Api.Controllers
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
        
        
        public ActionResult Cars(int page = 1, int pageSize = 7)
        {
            var cars = _context.Cars
                .OrderBy(c => c.Name)
                .Where(x => x.IsHidden == false)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalItems = _context.Cars.Count();

            var model = new IndexViewModel<Car>
            {
                Items = cars,
                PageInfo = new PageInfo
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };
            return View(model);
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