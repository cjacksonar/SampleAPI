namespace Data.DTOs
{
    public class vwContributions
    {
        public int ContributorId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }

        public string Comments { get; set; }
        public int ContributionId { get; set; }

        public DateTime ContributionDate { get; set; }

        public decimal Amount { get; set; }

        public int CheckNumber { get; set; }
        public int FundId { get; set; }

        public string FundName { get; set; }

    }
}
