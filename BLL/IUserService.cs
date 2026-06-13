using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTOs;

namespace BLL
{
    public interface IUserService
    {
        Task<IEnumerable<UserReadDto>> GetAllUsersAsync();
        Task<UserDetailsDto?> GetUserByIdAsync(string id);

        Task<bool> RegisterAsync(UserRegisterDto dto);
        Task<bool> LoginAsync(UserLoginDto dto);
        Task LogoutAsync();
        Task<bool> IsAdminAsync(string email);
    }
}