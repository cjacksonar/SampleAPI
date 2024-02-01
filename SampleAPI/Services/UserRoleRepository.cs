using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class UserRoleRepository : IUserRoleRepository, IDisposable
    {
        private APIDbContext _context;

        public UserRoleRepository(APIDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<UserRole>> GetUserRolesAsync()
        {
            return await _context.UserRoles.ToListAsync();
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