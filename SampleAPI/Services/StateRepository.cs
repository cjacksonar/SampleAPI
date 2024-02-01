using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class StateRepository : IStateRepository, IDisposable
    {
        private APIDbContext _context;

        public StateRepository(APIDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<State>> GetStatesAsync()
        {            
            return await _context.States.ToListAsync();
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