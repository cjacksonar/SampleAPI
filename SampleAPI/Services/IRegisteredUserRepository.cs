namespace Services
{
    public interface IRegisteredUserRepository : IDisposable
    {
        Task<IEnumerable<Data.Entities.UserRole>> GetUserRolesAsync();
        Task<List<Data.Entities.RegisteredUser>> GetRegisteredUsersAsync();
        Task<Data.Entities.RegisteredUser> GetRegisteredUserAsync(int RegisteredUserId);
        Task<bool> RegisteredUserExistsAsync(int RegisteredUserId);
        void AddRegisteredUser(Data.Entities.RegisteredUser RegisteredUser);    // RegisteredUser from Entities
        Task<Data.Entities.RegisteredUser> DeleteRegisteredUser(int RegisteredUserId);
        void UpdateRegisteredUser(Data.Entities.RegisteredUser RegisteredUser);
        Task<bool> SaveChangesAsync();
    }
}