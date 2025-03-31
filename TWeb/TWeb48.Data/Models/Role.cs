using System;
using System.Collections.Generic;

namespace TWeb48.Data.Models
{
    public class Role
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}