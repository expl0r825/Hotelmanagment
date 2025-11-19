using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using HotelManagement.Data;
using HotelManagement.Models;

namespace HotelManagement.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }

            ViewBag.Room = room;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CheckInDate >= model.CheckOutDate)
                {
                    ModelState.AddModelError("", "Датата на напускане трябва да бъде след датата на настаняване.");
                    var room = await _context.Rooms.FindAsync(model.RoomId);
                    ViewBag.Room = room;
                    return View(model);
                }

                if (model.CheckInDate < DateTime.Today)
                {
                    ModelState.AddModelError("", "Датата на настаняване не може да бъде в миналото.");
                    var room = await _context.Rooms.FindAsync(model.RoomId);
                    ViewBag.Room = room;
                    return View(model);
                }

                // Check availability
                var isAvailable = await IsRoomAvailable(model.RoomId, model.CheckInDate, model.CheckOutDate);
                if (!isAvailable)
                {
                    ModelState.AddModelError("", "Стаята не е налична за избраните дати.");
                    var room = await _context.Rooms.FindAsync(model.RoomId);
                    ViewBag.Room = room;
                    return View(model);
                }

                var user = await _userManager.GetUserAsync(User);
                var selectedRoom = await _context.Rooms.FindAsync(model.RoomId);
                
                var nights = (model.CheckOutDate - model.CheckInDate).Days;
                var totalPrice = selectedRoom!.PricePerNight * nights;

                var booking = new Booking
                {
                    UserId = user!.Id,
                    RoomId = model.RoomId,
                    CheckInDate = model.CheckInDate,
                    CheckOutDate = model.CheckOutDate,
                    NumberOfGuests = model.NumberOfGuests,
                    TotalPrice = totalPrice,
                    Status = BookingStatus.Pending
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                return RedirectToAction("Payment", new { bookingId = booking.Id });
            }

            var roomForView = await _context.Rooms.FindAsync(model.RoomId);
            ViewBag.Room = roomForView;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Payment(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null || booking.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(int bookingId, PaymentMethod paymentMethod)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null || booking.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            // Simulate payment processing
            var payment = new Payment
            {
                BookingId = bookingId,
                Amount = booking.TotalPrice,
                PaymentMethod = paymentMethod,
                Status = PaymentStatus.Completed,
                TransactionId = Guid.NewGuid().ToString()
            };

            booking.Status = BookingStatus.Confirmed;
            
            _context.Payments.Add(payment);
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction("Confirmation", new { bookingId = bookingId });
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .Include(b => b.Payment)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null || booking.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            return View(booking);
        }

        [HttpGet]
        public async Task<IActionResult> MyBookings()
        {
            var user = await _userManager.GetUserAsync(User);
            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.Payment)
                .Where(b => b.UserId == user!.Id)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Payment)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null || booking.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            if (booking.Status == BookingStatus.Confirmed || booking.Status == BookingStatus.Pending)
            {
                booking.Status = BookingStatus.Cancelled;
                if (booking.Payment != null)
                {
                    booking.Payment.Status = PaymentStatus.Refunded;
                }
                
                _context.Bookings.Update(booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("MyBookings");
        }

        private async Task<bool> IsRoomAvailable(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var conflictingBookings = await _context.Bookings
                .Where(b => b.RoomId == roomId &&
                           b.Status != BookingStatus.Cancelled &&
                           ((b.CheckInDate <= checkIn && b.CheckOutDate > checkIn) ||
                            (b.CheckInDate < checkOut && b.CheckOutDate >= checkOut) ||
                            (b.CheckInDate >= checkIn && b.CheckOutDate <= checkOut)))
                .AnyAsync();

            return !conflictingBookings;
        }
    }
}

