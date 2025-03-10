namespace TWeb48.Models
{
    public class MainStat : BaseDbItem
    {
        public decimal TotalIncome { get; set; }
        public int TotalCars { get; set; }
        public int TotalRents { get; set; }
        public decimal ThisMonthIncome { get; set; }
        public decimal ThisYearIncome { get; set; }
        public int UsersCount { get; set; }
        public decimal Difference { get; set; }
    }
}