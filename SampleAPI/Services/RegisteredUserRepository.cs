using Classes;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class RegisteredUserRepository : IRegisteredUserRepository, IDisposable
    {
        private APIDbContext _context;

        public RegisteredUserRepository(APIDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Data.Entities.UserRole>> GetUserRolesAsync()
        {
            return await _context.UserRoles.ToListAsync();
        }
        public async Task<bool> RegisteredUserExistsAsync(int RegisteredUserId)
        {
            if (RegisteredUserId == 0)
            {
                throw new ArgumentNullException(nameof(RegisteredUserId));
            }
            return await _context.RegisteredUsers.AnyAsync(a => a.Id == RegisteredUserId);
        }
        public async Task<Data.Entities.RegisteredUser> GetRegisteredUserAsync(int RegisteredUserId)
        {
            if (RegisteredUserId == 0)
            {
                throw new ArgumentNullException(nameof(RegisteredUserId));
            }
            return await _context.RegisteredUsers.FirstOrDefaultAsync(a => a.Id == RegisteredUserId);
        }
        public async Task<List<Data.Entities.RegisteredUser>> GetRegisteredUsersAsync()
        {
            // return only users that have been setup by registration process which should have AllowEditing set to true
            return await _context.RegisteredUsers.Where(x => x.ProductId == Globals.ApplicationProductId && x.AllowEditing == true).ToListAsync();
        }
        public void AddRegisteredUser(Data.Entities.RegisteredUser RegisteredUser)
        {
            // RegisteredUser is from Entities
            if (RegisteredUser == null)
            {
                throw new ArgumentNullException(nameof(RegisteredUser));
            }
            _context.RegisteredUsers.Add(RegisteredUser);
        }
        public void UpdateRegisteredUser(Data.Entities.RegisteredUser RegisteredUser)
        {
            // no code in this implementation
        }
        public async Task<Data.Entities.RegisteredUser> DeleteRegisteredUser(int RegisteredUserId)
        {
            var result = await _context.RegisteredUsers.FirstOrDefaultAsync(a => a.Id == RegisteredUserId);
            if (result != null)
            {
                _context.RegisteredUsers.Remove(result);
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