using Microsoft.AspNetCore.Mvc;
using HRM.SantaLucia.Web.Services;
using HRM.SantaLucia.Web.Models.ViewModels;

namespace HRM.SantaLucia.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = await _dashboardService.GetDashboardDataAsync();
            return View(dashboard);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}