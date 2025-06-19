using System;

namespace TWeb48.Dtos
{
    public class CarTakesReport
    {
        public Guid CarId { get; set; }
        public string Name { get; set; }
        public int TakesCount { get; set; }
        public decimal TotalPrice { get; set; }
        public string PhotoPath { get; set; }
    }
}