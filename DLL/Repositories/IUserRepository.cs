using Domain;

namespace DLL.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();

        Task<User?> GetByIdAsync(string id);

        Task<User?> GetByIdWithOrdersAsync(string id);
    }
}