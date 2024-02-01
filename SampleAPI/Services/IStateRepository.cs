using Data.Entities;

namespace Services
{
    public interface IStateRepository : IDisposable
    {
        Task<IEnumerable<State>> GetStatesAsync();
    }
}