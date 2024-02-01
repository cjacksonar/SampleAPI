namespace Data.Repository
{
    public interface IRepository<T>
    {
        List<T> GetList();
        void Add(ref T item);
        void Save(T item);
        void Delete(T item);
    }
}
