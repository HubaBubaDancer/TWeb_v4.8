using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using TWeb48.Models;
using TWeb48.Services;

namespace TWeb48.Controllers
{
    
    public class AdminController : Controller
    {
        
        private readonly IAdminService _adminService;

        public AdminController()
        {
            _adminService = new AdminService();
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
        
        
        
    }
}