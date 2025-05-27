using System;

namespace TWeb48.Api.Reference.Dtos
{
    public class RentRequest
    {
        public Guid CarId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; }
    }
}