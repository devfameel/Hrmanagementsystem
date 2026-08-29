using IdentityService.dtos.Users;

namespace IdentityService.services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> CreateAsync(CreateUserRequest request);

        Task<List<UserResponse>> GetAllUserAsync();

        Task<UserResponse?> GetByIdAsync(int id);

        Task<UpdateUserRequest?> UpdateAsync(int id ,UpdateUserRequest request);

        Task<bool> DeleteAsync(int id);
    }
}
