using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Имейлът е задължителен")]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес")]
        [Display(Name = "Имейл")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Първото име е задължително")]
        [Display(Name = "Първо име")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Фамилията е задължителна")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Паролата е задължителна")]
        [StringLength(100, ErrorMessage = "Паролата трябва да бъде поне {2} символа.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Потвърди парола")]
        [Compare("Password", ErrorMessage = "Паролите не съвпадат.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Имейлът е задължителен")]
        [EmailAddress]
        [Display(Name = "Имейл")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Паролата е задължителна")]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Запомни ме")]
        public bool RememberMe { get; set; }
    }

    public class BookingViewModel
    {
        public int RoomId { get; set; }
        
        [Required(ErrorMessage = "Датата на настаняване е задължителна")]
        [Display(Name = "Дата на настаняване")]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }
        
        [Required(ErrorMessage = "Датата на напускане е задължителна")]
        [Display(Name = "Дата на напускане")]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }
        
        [Required(ErrorMessage = "Броят гости е задължителен")]
        [Range(1, 10, ErrorMessage = "Броят гости трябва да бъде между 1 и 10")]
        [Display(Name = "Брой гости")]
        public int NumberOfGuests { get; set; }
    }

    public class AdminDashboardViewModel
    {
        public int TotalRooms { get; set; }
        public int TotalBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PendingBookings { get; set; }
    }

    public class RoomViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Номерът на стаята е задължителен")]
        [Display(Name = "Номер на стая")]
        public string RoomNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Типът на стаята е задължителен")]
        [Display(Name = "Тип стая")]
        public string RoomType { get; set; } = string.Empty;
        
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Цената е задължителна")]
        [Range(0.01, 10000, ErrorMessage = "Цената трябва да бъде между 0.01 и 10000")]
        [Display(Name = "Цена за нощ")]
        public decimal PricePerNight { get; set; }
        
        [Required(ErrorMessage = "Капацитетът е задължителен")]
        [Range(1, 20, ErrorMessage = "Капацитетът трябва да бъде между 1 и 20")]
        [Display(Name = "Капацитет")]
        public int Capacity { get; set; }
        
        [Display(Name = "Налична")]
        public bool IsAvailable { get; set; } = true;
        
        public string? ImageUrl { get; set; }
    }

    public class ReportsViewModel
    {
        public decimal TotalRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public int TotalBookings { get; set; }
        public int ConfirmedBookings { get; set; }
        public decimal OccupancyRate { get; set; }
        public List<RoomTypeBooking> RoomBookings { get; set; } = new();
    }

    public class RoomTypeBooking
    {
        public string RoomType { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal Revenue { get; set; }
    }
}

