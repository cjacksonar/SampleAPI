using Classes;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class ContributorRepository : IContributorRepository
    {
        private APIDbContext _context;

        public ContributorRepository(APIDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ContributorExistsAsync(int ContributorId)
        {
            if (ContributorId == 0)
            {
                throw new ArgumentNullException(nameof(ContributorId));
            }
            return await _context.Contributors.AnyAsync(a => a.Id == ContributorId);
        }
        public async Task<Data.Entities.Contributor> GetContributorAsync(int ContributorId)
        {
            if (ContributorId == 0)
            {
                throw new ArgumentNullException(nameof(ContributorId));
            }
            return await _context.Contributors.FirstOrDefaultAsync(a => a.Id == ContributorId);
        }
        public async Task<List<Data.Entities.Contributor>> GetContributorsAsync()
        {
            return await _context.Contributors.Where(x => x.ProductId == Globals.ApplicationProductId).ToListAsync();
        }
        public void AddContributor(Data.Entities.Contributor Contributor)
        {
            // Contributor is from Entities
            if (Contributor == null)
            {
                throw new ArgumentNullException(nameof(Contributor));
            }
            _context.Contributors.Add(Contributor);
        }
        public void UpdateContributor(Data.Entities.Contributor Contributor)
        {
            // no code in this implementation
        }
        public async Task<Data.Entities.Contributor> DeleteContributor(int ContributorId)
        {
            var result = await _context.Contributors.FirstOrDefaultAsync(a => a.Id == ContributorId);
            if (result != null)
            {
                _context.Contributors.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }
        public async Task<bool> ContributionExistsForContributotAsync(int ContributorId)
        {
            var result = await _context.Contributions.FirstOrDefaultAsync(a => a.ContributorId == ContributorId);
            if (result != null) { return true;} else { return false; }           
        }
        public async Task<List<Data.Entities.Contribution>> GetContributionsByContributorAsync(int ContributorId)
        {
            return await _context.Contributions
               .Where(x => x.ContributorId == ContributorId).ToListAsync();           
        }
        public async Task<bool> SaveChangesAsync()
        {
            // return true if 1 or more entities were changed
            return (await _context.SaveChangesAsync() > 0);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
    }
}