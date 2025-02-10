using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using TWeb48.Models;

namespace TWeb48.Controllers
{
    
    public class AdminController : Controller
    {
        
        private readonly TWebDbContext _context;
        private readonly IMapper _mapper;
        
        public AdminController()
        {
            _context = new TWebDbContext();
            _mapper = new MapperConfiguration(cfg => cfg.CreateMap<CarRequest, Car>()).CreateMapper();
        }

        public ActionResult AdminPanel()
        {
            return View();
        }
        
        public ActionResult AddCar()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult AddCar(CarRequest car)
        {
            var newCar = _mapper.Map<Car>(car);
            _context.Cars.Add(newCar);
            _context.SaveChanges();
            return RedirectToAction("Car", "Home", new {id = newCar.Id});
        }
        
        
        
        
        
    }
}