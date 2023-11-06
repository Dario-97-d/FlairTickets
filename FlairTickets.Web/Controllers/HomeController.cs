using System.Diagnostics;
using System.Threading.Tasks;
using FlairTickets.Web.Data;
using FlairTickets.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlairTickets.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataUnit _dataUnit;

        public HomeController(IDataUnit dataUnit)
        {
            _dataUnit = dataUnit;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin") || User.IsInRole("Employee"))
        {
            return View();
        }
            }

            ViewBag.ComboAirports = await _dataUnit.Airports.GetComboAirportsIataCodeAsync();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Route("/Error/404")]
        public IActionResult NotFound()
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
