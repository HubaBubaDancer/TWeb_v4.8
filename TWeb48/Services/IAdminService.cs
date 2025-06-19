using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Mvc;
using AutoMapper;
using TWeb48.Dtos;
using TWeb48.Models;

namespace TWeb48.Services
{
    public interface IAdminService
    {
        ActionResult AddCar(CarRequest car);
        ActionResult UploadImage(Guid id);
        ActionResult SelectPhoto(Guid carId, string photo);
        List<User> GetUsers();
        MainStat GetMainStats();
        ActionResult EditCar(Car car);
        Car PrepareCarForEdit(Guid id);
        void AddCoupon(CouponRequest coupon);
        IEnumerable<Coupon> GetCoupons();
        ActionResult EditCoupon(Coupon coupon);
        Coupon GetCoupon(Guid id);
        IEnumerable<CarTakesReport> MonthlyReport();
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


        public MainStat GetMainStats()
        {
            var currentDate = DateTime.Now;
            var currentMonth = currentDate.Month;
            var currentYear = currentDate.Year;
            var previousMonth = currentDate.AddMonths(-1).Month;
            var previousMonthYear = currentDate.AddMonths(-1).Year;

            var rents = _context.Rents.AsQueryable();

            // Используем Sum с nullable decimal (decimal?) и преобразуем в decimal с значением по умолчанию
            var totalIncome = rents.Sum(o => (decimal?)o.Price) ?? 0m;
    
            var thisMonthIncome = rents
                .Where(o => o.StartDate.Month == currentMonth && o.StartDate.Year == currentYear)
                .Sum(o => (decimal?)o.Price) ?? 0m;
        
            var thisYearIncome = rents
                .Where(o => o.StartDate.Year == currentYear)
                .Sum(o => (decimal?)o.Price) ?? 0m;
        
            var pastMonthIncome = rents
                .Where(o => o.StartDate.Month == previousMonth && o.StartDate.Year == previousMonthYear)
                .Sum(o => (decimal?)o.Price) ?? 0m;

            // Защита от деления на ноль
            var differenceInPercent = pastMonthIncome != 0 
                ? (thisMonthIncome - pastMonthIncome) / pastMonthIncome * 100 
                : thisMonthIncome != 0 ? 100 : 0;

            return new MainStat
            {
                TotalIncome = totalIncome,
                ThisMonthIncome = thisMonthIncome,
                ThisYearIncome = thisYearIncome,
                Difference = differenceInPercent,
                TotalCars = _context.Cars.Count(),
                TotalRents = rents.Count(),
                UsersCount = _context.Users.Count(),
            };
        }
        
        public List<User> GetUsers()
        {
            var users = _context.Users.ToList();
            return users;
        }
        
        public Car PrepareCarForEdit(Guid id)
        {
            var car = _context.Cars.FirstOrDefault(c => c.Id == id);
            if (car == null)
            {
                throw new KeyNotFoundException("Car not found");
            }
            return car;
        }

        public ActionResult EditCar(Car car)
        {
            var existingCar = _context.Cars.FirstOrDefault(c => c.Id == car.Id);
            if (existingCar == null)
            {
                return new HttpNotFoundResult();
            }

            existingCar.Name = car.Name;
            existingCar.Description = car.Description;
            existingCar.Price = car.Price;
            existingCar.PhotoPath = car.PhotoPath;

            _context.Entry(existingCar).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();

            return new RedirectResult($"/Home/Car?id={existingCar.Id}");
        }

        public void AddCoupon(CouponRequest coupon)
        {
            var newCoupon = new Coupon
            {
                Code = coupon.Code,
                Description = coupon.Description,
                DiscountInPercents = coupon.DiscountInPercents,
                ValidUntil = coupon.ValidUntil,
                ValidFrom = coupon.ValidFrom,
                IsActive = coupon.IsActive
            };

            _context.Coupons.Add(newCoupon);
            _context.SaveChanges();
        }

        public IEnumerable<Coupon> GetCoupons()
        {
            return _context.Coupons.ToList();
        }

        public Coupon GetCoupon(Guid id)
        {
            var coupon = _context.Coupons.FirstOrDefault(c => c.Id == id);
            if (coupon == null)
            {
                throw new KeyNotFoundException("Coupon not found");
            }

            return coupon;
        }
        
        public ActionResult EditCoupon(Coupon coupon)
        {
            var existingCoupon = _context.Coupons.FirstOrDefault(c => c.Id == coupon.Id);
            if (existingCoupon == null)
            {
                return new HttpNotFoundResult();
            }

            existingCoupon.Code = coupon.Code;
            existingCoupon.Description = coupon.Description;
            existingCoupon.DiscountInPercents = coupon.DiscountInPercents;
            existingCoupon.ValidUntil = coupon.ValidUntil;
            existingCoupon.ValidFrom = coupon.ValidFrom;
            existingCoupon.IsActive = coupon.IsActive;

            _context.Entry(existingCoupon).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();

            return new RedirectResult($"/Admin/Coupones");

        }


        public IEnumerable<CarTakesReport> MonthlyReport()
        {
            var currentDate = DateTime.Now;
            var currentMonth = currentDate.Month;
            var currentYear = currentDate.Year;

            var monthlyRents = _context.Rents
                .Where(r => r.StartDate.Month == currentMonth && r.StartDate.Year == currentYear)
                .Include(r => r.Car) // Подгружаем связанные данные о машине
                .ToList();

            var report = monthlyRents
                .GroupBy(r => r.CarId)
                .Select(g => new CarTakesReport
                {
                    CarId = g.Key,
                    Name = g.First().Car.Name, // Предполагая, что у Car есть свойство Name
                    TakesCount = g.Count(),
                    TotalPrice = g.Sum(r => r.Price),
                    PhotoPath = g.First().Car.PhotoPath // Предполагая, что у Car есть свойство PhotoPath
                })
                .OrderByDescending(r => r.TakesCount)
                .ToList();

            return report;
        }



    }
    
    
}