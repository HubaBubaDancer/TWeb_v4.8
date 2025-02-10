using System.Collections.Generic;

namespace TWeb48.Models
{
    public class Car : BaseDbItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PhotoPath { get; set; }
        public bool IsHidden { get; set; } = true;
    }
}