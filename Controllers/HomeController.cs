using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Data;

namespace HotelManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _context.Rooms
                .Where(r => r.IsAvailable)
                .Take(6)
                .ToListAsync();
            
            return View(rooms);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}

