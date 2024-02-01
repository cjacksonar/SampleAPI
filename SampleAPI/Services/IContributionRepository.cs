using Data.Entities;
using DTO = Data.DTOs;

namespace Services
{
    public interface IContributionRepository : IDisposable
    {
        Task<List<Contribution>> GetContributionsAsync();
        Task<List<DTO.ViewContributionsList>> GetContributionsViewListAsync();
        Task<List<Contribution>> GetContributionByContributorAsync(int ContributorId);
        Task<Contribution> GetContributionAsync(int ContributionId);
        Task<bool> ContributionExistsAsync(int ContributionId);
        void AddContribution(Contribution Contribution);    // Contribution from Entities
        Task<Contribution> DeleteContribution(int ContributionId);
        void UpdateContribution(Contribution Contribution);
        Task<bool> SaveChangesAsync();   
        Task<List<DTO.ContributionChartData>> GetContributionChartDataAsync(int FundId, string Year);
        Task<List<DTO.ContributionAnnualReportData>> GetContributionAnnualReportDataAsync(string Year);
        Task<List<DTO.DailyContributionData>> GetDailyContributionDataAsync(DateTime selectedDate);
        Task<IEnumerable<string>> GetContributionYearsAsync();
    }
}