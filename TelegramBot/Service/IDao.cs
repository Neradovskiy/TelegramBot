namespace TelegramBot.Service
{
    public interface IDao<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> AddAsync(T obj);
        Task<T> UpdateAsync(T obj);
        Task<T>? DeleteAsync(int id);
        Task<T>? GetAsync(int id);
    }
}
