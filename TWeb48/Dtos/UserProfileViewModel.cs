using System.Collections.Generic;

namespace TWeb48.Models
{
    public class UserProfileViewModel
    {
        public User User { get; set; }
        public List<string> Roles { get; set; }
    }
}