using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCMovie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using MVCMovie.Infrastructure;

namespace MVCMovie.Controllers
{
    public class HomeController : Controller
    {
        private ILogger logger;

        public HomeController(ILogger _logger)
        {
            logger = _logger;
        }
        public IActionResult Index()
        {
           // logger.log("This is test message");
            return View();
        }

        
        public IActionResult About()
        {

            ViewData["Message"] = "Your application description page.";

            return View();
        }

       
        [Authorize]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task LogOut()
        {
            await HttpContext.SignOutAsync(
                 CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(
                OpenIdConnectDefaults.AuthenticationScheme);

        }
    }
}
