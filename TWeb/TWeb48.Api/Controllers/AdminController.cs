using System;
using System.Web.Mvc;
using TWeb48.Api.Reference.Dtos;
using TWeb48.Business.Services;

namespace TWeb48.Api.Controllers
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