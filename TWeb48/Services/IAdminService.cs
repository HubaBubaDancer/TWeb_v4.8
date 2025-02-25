using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using TWeb48.Models;

namespace TWeb48.Services
{
    public interface IAdminService
    {
        ActionResult AddCar(CarRequest car);
        ActionResult UploadImage(Guid id);
        ActionResult SelectPhoto(Guid carId, string photo);
        //IEquatable<Car> GetAllCars();
    }
    
    public class AdminService : IAdminService
    {
        private readonly TWebDbContext _context;
        private readonly IMapper _mapper;

        public AdminService()
        {
            _context = new TWebDbContext();
            _mapper = new MapperConfiguration(cfg => cfg.CreateMap<CarRequest, Car>()).CreateMapper();
        }

        public ActionResult AddCar(CarRequest car)
        {
            var newCar = _mapper.Map<Car>(car);
            _context.Cars.Add(newCar);
            _context.SaveChanges();
            return new RedirectResult($"/Home/Car?id={newCar.Id}");
        }

        public ActionResult UploadImage(Guid id)
        {
            var car = _context.Cars.FirstOrDefault(c => c.Id == id);
            return new ViewResult { ViewName = "UploadImage", ViewData = new ViewDataDictionary(car) };
        }

        public ActionResult SelectPhoto(Guid carId, string photo)
        {
            var car = _context.Cars.FirstOrDefault(c => c.Id == carId);
            if (car == null)
            {
                return new HttpNotFoundResult();
            }

            car.PhotoPath = $"/wwwroot/uploads/{photo}";
            _context.Entry(car).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();

            return new RedirectResult($"/Home/Car?id={carId}");
        }
        
        /*TODO*/
        /*public IEquatable<Car> GetAllCars()
        {
            var cars = _context.Cars.ToList();
            return null;
        }*/
        
        
    }
    
    
}