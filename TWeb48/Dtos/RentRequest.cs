using System;

namespace TWeb48.Models
{
    public class RentRequest
    {
        public Guid CarId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; }
        public string CouponCode { get; set; } = string.Empty;
    }
}