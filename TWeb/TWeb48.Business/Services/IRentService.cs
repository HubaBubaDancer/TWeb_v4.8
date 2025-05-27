using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using TWeb48.Api.Reference.Dtos;
using TWeb48.Business.Helpers;
using TWeb48.Data.Models;

namespace TWeb48.Business.Services
{
    public interface IRentService
    {
        ActionResult RentCar(RentRequest request);
        Rent GetRent(Guid id);
        IEnumerable<Rent> GetRents(Guid userId);
        IEnumerable<Rent> GetAllRents();
        Car GetCarById(Guid id);
        IndexViewModel<Car> GetCars(int page = 1, int pageSize = 7);
    }
    
    public class RentService : IRentService
    {
        private readonly TWebDbContext _context;
        private readonly IMapper _mapper;

        public RentService()
        {
            _context = new TWebDbContext();
            _mapper = new MapperConfiguration(cfg => cfg.CreateMap<RentRequest, Rent>()).CreateMapper();
        }
        
        public ActionResult RentCar(RentRequest request)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserId == request.UserId);
            var car = _context.Cars.FirstOrDefault(x => x.Id == request.CarId);
            
            if (user == null || car == null)
            {
                return new HttpNotFoundResult();
            }
            
            var rent = _mapper.Map<Rent>(request);
            rent.Price = car.Price * (rent.EndDate - rent.StartDate).Days;
            rent.Car = car;
            
            _context.Rents.Add(rent);
            _context.SaveChanges();
            return new RedirectResult($"/Account/Rent?id={rent.Id}");
        }
        
        public Rent GetRent(Guid id)
        {
            var rent = _context.Rents
                .Include(x => x.Car)
                .FirstOrDefault(x => x.Id == id);
            return rent;
        }

        public IEnumerable<Rent> GetRents(Guid userId)
        {
            var rents = _context.Rents
                .Where(x => x.UserId == userId)
                .Include(x => x.Car)
                .ToList();
            return rents;
        }
        
        public IEnumerable<Rent> GetAllRents()
        {
            var rents = _context.Rents
                .Include(x => x.Car)
                .ToList();
            return rents;
        }

        public Car GetCarById(Guid id)
        {
           return _context.Cars.FirstOrDefault(c => c.Id == id);
        }

        public IndexViewModel<Car> GetCars(int page = 1, int pageSize = 7)
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

            return model;
        }
    }
    
    
}