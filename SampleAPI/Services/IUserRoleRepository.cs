using Data.Entities;

namespace Services
{
    public interface IUserRoleRepository : IDisposable
    {
        Task<IEnumerable<UserRole>> GetUserRolesAsync();
    }
}