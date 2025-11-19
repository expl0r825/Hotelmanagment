using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var stats = new AdminDashboardViewModel
            {
                TotalRooms = await _context.Rooms.CountAsync(),
                TotalBookings = await _context.Bookings.CountAsync(),
                TotalRevenue = await _context.Payments
                    .Where(p => p.Status == PaymentStatus.Completed)
                    .SumAsync(p => p.Amount),
                PendingBookings = await _context.Bookings
                    .CountAsync(b => b.Status == BookingStatus.Pending)
            };

            return View(stats);
        }

        // Rooms Management
        public async Task<IActionResult> Rooms()
        {
            var rooms = await _context.Rooms.OrderBy(r => r.RoomNumber).ToListAsync();
            return View(rooms);
        }

        [HttpGet]
        public IActionResult CreateRoom()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoom(RoomViewModel model, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                var room = new Room
                {
                    RoomNumber = model.RoomNumber,
                    RoomType = model.RoomType,
                    Description = model.Description,
                    PricePerNight = model.PricePerNight,
                    Capacity = model.Capacity,
                    IsAvailable = model.IsAvailable
                };

                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "rooms");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    room.ImageUrl = $"/images/rooms/{fileName}";
                }

                _context.Rooms.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction("Rooms");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            var model = new RoomViewModel
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType,
                Description = room.Description,
                PricePerNight = room.PricePerNight,
                Capacity = room.Capacity,
                IsAvailable = room.IsAvailable,
                ImageUrl = room.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoom(RoomViewModel model, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                var room = await _context.Rooms.FindAsync(model.Id);
                if (room == null)
                {
                    return NotFound();
                }

                room.RoomNumber = model.RoomNumber;
                room.RoomType = model.RoomType;
                room.Description = model.Description;
                room.PricePerNight = model.PricePerNight;
                room.Capacity = model.Capacity;
                room.IsAvailable = model.IsAvailable;

                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "rooms");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    room.ImageUrl = $"/images/rooms/{fileName}";
                }

                _context.Rooms.Update(room);
                await _context.SaveChangesAsync();
                return RedirectToAction("Rooms");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Rooms");
        }

        // Bookings Management
        public async Task<IActionResult> Bookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .Include(b => b.Payment)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBookingStatus(int bookingId, BookingStatus status)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null)
            {
                booking.Status = status;
                _context.Bookings.Update(booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Bookings");
        }

        // Reports
        public async Task<IActionResult> Reports()
        {
            var report = new ReportsViewModel
            {
                TotalRevenue = await _context.Payments
                    .Where(p => p.Status == PaymentStatus.Completed)
                    .SumAsync(p => p.Amount),
                MonthlyRevenue = await _context.Payments
                    .Where(p => p.Status == PaymentStatus.Completed && 
                               p.PaymentDate.Month == DateTime.Now.Month &&
                               p.PaymentDate.Year == DateTime.Now.Year)
                    .SumAsync(p => p.Amount),
                TotalBookings = await _context.Bookings.CountAsync(),
                ConfirmedBookings = await _context.Bookings
                    .CountAsync(b => b.Status == BookingStatus.Confirmed),
                OccupancyRate = await CalculateOccupancyRate(),
                RoomBookings = await _context.Bookings
                    .Include(b => b.Room)
                    .Where(b => b.Status == BookingStatus.Confirmed)
                    .GroupBy(b => b.Room.RoomType)
                    .Select(g => new RoomTypeBooking
                    {
                        RoomType = g.Key,
                        Count = g.Count(),
                        Revenue = g.Sum(b => b.TotalPrice)
                    })
                    .ToListAsync()
            };

            return View(report);
        }

        private async Task<decimal> CalculateOccupancyRate()
        {
            var totalRooms = await _context.Rooms.CountAsync();
            if (totalRooms == 0) return 0;

            var currentDate = DateTime.Today;
            var occupiedRooms = await _context.Bookings
                .Where(b => b.Status == BookingStatus.Confirmed &&
                           b.CheckInDate <= currentDate &&
                           b.CheckOutDate > currentDate)
                .Select(b => b.RoomId)
                .Distinct()
                .CountAsync();

            return (decimal)occupiedRooms / totalRooms * 100;
        }
    }
}

