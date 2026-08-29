using IdentityService.dtos.Auth;

namespace IdentityService.services.Interfaces
{
    public interface IAuthService
    {

        Task<LoginResponce?> LoginAsync(LoginRequestDto request);
    }
}
