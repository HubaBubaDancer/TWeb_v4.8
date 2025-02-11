﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TWeb48.Models;

namespace TWeb48.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly TWebDbContext _context;
        
        public HomeController()
        {
            _context = new TWebDbContext();
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
        
    }
}