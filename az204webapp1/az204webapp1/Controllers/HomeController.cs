using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using az204webapp1.Models;

namespace az204webapp1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Console.WriteLine("This is index");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            Console.WriteLine("This is about");
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            Console.WriteLine("This is contact");
            return View();
        }

        public IActionResult Privacy()
        {
            Console.WriteLine("This is privacy");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
