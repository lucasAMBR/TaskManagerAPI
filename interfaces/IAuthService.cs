using DTOs;

namespace Interfaces{
    public interface IAuthService{
        Task<LoginResultDTO?> LoginAsync(string email, string password);
    }
}