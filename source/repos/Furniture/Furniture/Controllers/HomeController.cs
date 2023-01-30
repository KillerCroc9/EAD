using Furniture.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Furniture.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            if (!HttpContext.Request.Cookies.ContainsKey("first_request"))
            {
                CookieOptions cookieOpt = new CookieOptions();
                cookieOpt.Expires= new DateTimeOffset(DateTime.Now.AddMinutes(5));//expires in 5 mins
                HttpContext.Response.Cookies.Append("first_request", DateTime.Now.ToString(),cookieOpt);
                return View();
            }
            else
            {
                
                return View();
            }
            
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