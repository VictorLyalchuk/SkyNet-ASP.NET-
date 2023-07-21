using Microsoft.AspNetCore.Mvc;
using SkyNet.Web.Models;
using System.Diagnostics;

namespace SkyNet.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Route("Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            switch (statusCode)
            {
                case 404: 
                    return View("NotFound");
                default:
                    return View("Error");
            }
        }
    }
}