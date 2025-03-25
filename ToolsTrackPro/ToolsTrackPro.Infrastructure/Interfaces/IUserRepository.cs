using ToolsTrackPro.Domain.Entities;

namespace ToolsTrackPro.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> CreateUserAsync(User user);
    }

}
