using ASC.Utilities;
using ASP.Web.Models;
using ASP.Web.Web.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace ASP.Web.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        private IOptions<ApplicationSettings> _settings;

        public HomeController(IOptions<ApplicationSettings> settings)
        {
            _settings = settings;
        }



        public IActionResult Index()
        {
            //ViewData.Model = "Test";
            //throw new Exception("Login Fail!!!");
            // Set Session
            HttpContext.Session.SetSession("Test", _settings.Value);
            // Get Session
            var settings = HttpContext.Session.GetSession<ApplicationSettings>("Test");
            // Usage of IOptions
            ViewBag.Title = _settings.Value.ApplicationTitle;
            return View();
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
