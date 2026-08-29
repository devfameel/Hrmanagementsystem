using IdentityService.data;
using IdentityService.dtos.Auth;
using IdentityService.security;
using IdentityService.services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.services
{
    public class AuthService :IAuthService
    {
        private readonly IdentityDbContext _Dbcontext;
        private readonly PasswordHasher _passwordHasher;
        private readonly JwtTokenService _jwtTokenService;

        public AuthService(IdentityDbContext dbcontext, PasswordHasher passwordHasher, JwtTokenService jwtTokenService)
        {
            _Dbcontext = dbcontext;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<LoginResponce?> LoginAsync(LoginRequestDto request)
        {
            var user = await _Dbcontext.Users.FirstOrDefaultAsync(x => x.Username == request.Username);

            if(user == null)
            {
                return null;
            }
            var passwordValied = _passwordHasher.VerifyPassowrd(request.Password, user.PasswordHash);

            if (!passwordValied)
            {
                return null;
            }

            var role = user.UserRoles.Select(x => x.Role.Name).FirstOrDefault()?? "User";

           var(tokenString, expiresAt) = _jwtTokenService.GenerateToken(user.Id , user.Username , role);

            return new LoginResponce
            {
                userName = user.Username,
                UserId = user.Id,
                ExperiedAt = expiresAt,
                Role = role,
                Token = tokenString,

            };

        }

                

    }
}
