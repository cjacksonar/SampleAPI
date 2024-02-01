namespace Data.DTOs
{
    public class Contribution
    {
        public int Id { get; set; }
        public DateTime ContributionDate { get; set; }
        public Decimal Amount { get; set; }
        public int? CheckNumber { get; set; }
        public string? Comments { get; set; }
        public int FundId { get; set; }        
        public int ContributorId { get; set; }       
    }
}