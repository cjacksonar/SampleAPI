namespace Data.DTOs
{
    public class ViewContributionsList
    {
        public int ContributionId { get; set; }
        public string FundName { get; set; }

        public string ContributorName { get; set; }

        public DateTime ContributionDate { get; set; }

        public decimal Amount { get; set; }

        public int CheckNumber { get; set; }
        public string Comments { get; set; }
    }
}