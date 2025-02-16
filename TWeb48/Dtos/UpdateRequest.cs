using System;

namespace TWeb48.Models
{
    public class UpdateRequest
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string NewPassword { get; set; }
        public string LicenceNumber { get; set; }
        public string OldPassword { get; set; }
    }
}