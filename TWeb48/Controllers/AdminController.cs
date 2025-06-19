using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using TWeb48.Dtos;
using TWeb48.Helpers;
using TWeb48.Models;
using TWeb48.Services;

namespace TWeb48.Controllers
{
    
    public class AdminController : Controller
    {
        
        private readonly IAdminService _adminService;
        private readonly IRentService _rentService;

        public AdminController()
        {
            _adminService = new AdminService();
            _rentService = new RentService();
        }

        public ActionResult AdminPanel()
        {
            var stats = _adminService.GetMainStats();
            return View(stats);
        }

        public ActionResult AddCar()
        {
            var model = new CarRequest();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddCar(CarRequest car)
        {
            return _adminService.AddCar(car);
        }
        
        public ActionResult UploadImage(Guid id)
        {
            return _adminService.UploadImage(id);
        }

        [HttpPost]
        public ActionResult SelectPhoto(Guid carId, string photo)
        {
            return _adminService.SelectPhoto(carId, photo);
        }
        
        public ActionResult Users(int page = 1, int pageSize = 7)
        {
            var users = _adminService.GetUsers();
            var totalItems = users.Count;
            var model = new IndexViewModel<User>
            {
                Items = users,
                PageInfo = new PageInfo
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };
            
            return View(model);
        }

        
        public ActionResult AdminRents(Guid userId, int page = 1, int pageSize = 10)
        {
            if (userId == Guid.Empty)
            {
                return RedirectToAction("Users", "Admin");
            }

            var rents = _rentService.GetRents(userId);
            var totalItems = rents.Count();

            var model = new IndexViewModel<Rent>
            {
                Items = rents,
                AdditionalId = userId,
                PageInfo = new PageInfo
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(model);
        }

        public ActionResult EditCar(Guid Id)
        {
            var car = _adminService.PrepareCarForEdit(Id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        [HttpPost]
        public ActionResult EditCar(Car car)
        {
            if (ModelState.IsValid)
            {
                _adminService.EditCar(car);
                return RedirectToAction("Car", "Home", new { id = car.Id });
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult AddCoupon()
        {
            var model = new CouponRequest();
            return View(model);
        }
        
        [HttpPost]
        public ActionResult AddCoupon(CouponRequest coupon)
        {
            if (ModelState.IsValid)
            {
                _adminService.AddCoupon(coupon);
                return RedirectToAction("Coupones", "Admin");
            }
            return View(coupon);
        }
        
        public ActionResult Coupones(int page = 1, int pageSize = 10)
        {
            var coupons = _adminService.GetCoupons();
            
            var totalItems = coupons.Count();
            var model = new IndexViewModel<Coupon>
            {
                Items = coupons,
                PageInfo = new PageInfo
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = totalItems
                }
            };
            
            return View(model);
        }
        
        public ActionResult ManageCoupon(Guid id)
        {
            var coupon = _adminService.GetCoupon(id);
            return View(coupon);
        }
        
        [HttpPost]
        public ActionResult ManageCoupon(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                _adminService.EditCoupon(coupon);
                
            }
            return RedirectToAction("Coupones", "Admin", new { id = coupon.Id });
        }
        
        
        public ActionResult MonthlyReport(int page = 1, int pageSize = 10)
        {
            var report = _adminService.MonthlyReport();
            
            var model = new IndexViewModel<CarTakesReport>
            {
                Items = report,
                PageInfo = new PageInfo
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = report.Count()
                }
            };
            
            return View(model);
        }
        
        

    }
}