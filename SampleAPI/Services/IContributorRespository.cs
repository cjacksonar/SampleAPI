using Data.Entities;

namespace Services
{
    public interface IContributorRepository : IDisposable
    {
        Task<List<Contributor>> GetContributorsAsync();
        Task<Contributor> GetContributorAsync(int ContributorId);
        Task<List<Contribution>> GetContributionsByContributorAsync(int ContributorId);
        Task<bool> ContributorExistsAsync(int ContributorId);
        Task<bool> ContributionExistsForContributotAsync(int ContributorId);
        void AddContributor(Contributor Contributor);    // Contributor from Entities
        Task<Contributor> DeleteContributor(int ContributorId);
        void UpdateContributor(Contributor Contributor);
        Task<bool> SaveChangesAsync();
    }
}