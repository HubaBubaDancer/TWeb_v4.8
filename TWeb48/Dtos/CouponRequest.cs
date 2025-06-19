using System;

namespace TWeb48.Models
{
    public class CouponRequest
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal DiscountInPercents { get; set; }
        public DateTime ValidUntil { get; set; }
        public DateTime ValidFrom { get; set; }
        public bool IsActive { get; set; } = true;
    }
}