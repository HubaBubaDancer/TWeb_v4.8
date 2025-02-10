using System.Collections.Generic;

namespace TWeb48.Models
{
    public class CarRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<string> PhotoPaths { get; set; } = new List<string>();
        public bool IsHidden { get; set; } = true;
    }
}