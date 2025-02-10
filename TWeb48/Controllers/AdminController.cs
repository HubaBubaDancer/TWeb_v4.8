using System;
using System.IO;
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
            var model = new CarRequest();
            return View(model);
        }
        
        [HttpPost]
        public ActionResult AddCar(CarRequest car)
        {
            var newCar = _mapper.Map<Car>(car);
            _context.Cars.Add(newCar);
            _context.SaveChanges();
            return RedirectToAction("Car", "Home", new {id = newCar.Id});
        }
        
        
        
        public ActionResult UploadImage(Guid id)
        {
            var car = _context.Cars.FirstOrDefault(c => c.Id == id);
            return View(car);
        }
        
        
        [HttpPost]
        public ActionResult SelectPhoto(Guid carId, string photo)
        {
            var car = _context.Cars.FirstOrDefault(c => c.Id == carId);
            if (car == null)
            {
                return HttpNotFound();
            }

            car.PhotoPath = "/wwwroot/uploads/" + photo;
            _context.Entry(car).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("Car", "Home", new { id = carId });
        }
        
        
        
    }
}