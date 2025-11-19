namespace HotelManagement.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public string? TransactionId { get; set; }
        
        // Navigation property
        public virtual Booking Booking { get; set; } = null!;
    }
    
    public enum PaymentMethod
    {
        CreditCard,
        DebitCard,
        PayPal,
        BankTransfer
    }
    
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }
}

