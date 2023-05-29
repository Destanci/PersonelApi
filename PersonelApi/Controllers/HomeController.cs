using Microsoft.AspNetCore.Mvc;
using PersonelApi.Models;
using System.Diagnostics;

namespace PersonelApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet, Route("[controller]", Name = "Home")]
        public IActionResult Index()
        {
            return View();
        }
    }
}