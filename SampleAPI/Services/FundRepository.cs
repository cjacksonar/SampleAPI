using Classes;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Services
{
    public class FundRepository : IFundRepository, IDisposable
    {
        private APIDbContext _context;

        public FundRepository(APIDbContext context)
        {
            _context = context;
        }

        public bool IsValidProductId(string productId)
        {
            var list = _context.Funds.Where(x => x.ProductId == productId).ToList();
            if (list.Count == 0) { return false; }
            return true;
        }
        public async Task<bool> FundExistsAsync(int FundId)
        {
            if (FundId == 0)
            {
                throw new ArgumentNullException(nameof(FundId));
            }
            return await _context.Funds.AnyAsync(a => a.Id == FundId);
        }
        public async Task<Data.Entities.Fund> GetFundAsync(int FundId)
        {
            if (FundId == 0)
            {
                throw new ArgumentNullException(nameof(FundId));
            }
            return await _context.Funds.FirstOrDefaultAsync(a => a.Id == FundId);
        }
        public async Task<List<Data.Entities.Fund>> GetFundsAsync()
        {
            return await _context.Funds.Where(x => x.ProductId == Globals.ApplicationProductId).ToListAsync();
        }
        public void AddFund(Data.Entities.Fund Fund)
        {
            // Fund is from Entities
            if (Fund == null)
            {
                throw new ArgumentNullException(nameof(Fund));
            }
            _context.Funds.Add(Fund);
        }
        public void UpdateFund(Data.Entities.Fund Fund)
        {
            // no code in this implementation
        }
        public async Task<Data.Entities.Fund> DeleteFund(int FundId)
        {
            var result = await _context.Funds.FirstOrDefaultAsync(a => a.Id == FundId);
            if (result != null)
            {
                _context.Funds.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
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