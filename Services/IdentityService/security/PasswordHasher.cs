using Microsoft.AspNetCore.Identity;

namespace IdentityService.security
{
    public class PasswordHasher
    {
        private readonly PasswordHasher<Object> _hasher = new();

        public string HashPassword(string password)
        {
            return _hasher.HashPassword(null!, password);
        }

        public bool VerifyPassowrd(string password , string passwordHashed)
        {
            var result = _hasher.VerifyHashedPassword(null!, passwordHashed, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
