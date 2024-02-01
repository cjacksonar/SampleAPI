namespace Data.DTOs
{
    public class ContributionAnnualReportData
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime ContributionDate { get; set; }
        public string? Comments { get; set; }
        public string? CheckNumber { get; set; }
        public decimal Amount { get; set; }
        public string? ContributionYear { get; set; }
    }
}