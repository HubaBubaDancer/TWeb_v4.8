namespace TWeb48.Api.Reference.Dtos
{
    public class CarRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PhotoPath { get; set; }
        public bool IsHidden { get; set; } = true;
    }
}