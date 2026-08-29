using IdentityService.data;
using IdentityService.dtos.Users;
using IdentityService.models;
using IdentityService.security;
using IdentityService.services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.NativeInterop;


namespace IdentityService.services
{
    public class UserService : IUserService
    {
        private readonly IdentityDbContext _DbContext;
        private readonly PasswordHasher _passwordHasher;

        public UserService(IdentityDbContext dbContext,PasswordHasher passwordHasher)
        {
            _DbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserResponse> CreateAsync(CreateUserRequest request)
        {
            var exisitngUser = await _DbContext.Users.AnyAsync(x => x.Username == request.Username || x.Email == request.Email);

            if (exisitngUser)
            {
                throw new InvalidOperationException("User or Email already Exist");
            }

                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = _passwordHasher.HashPassword(request.PasswordHash),
                    IsActive = request.IsActive,
                    CreatedAt = DateTime.UtcNow

                };
                _DbContext.Users.Add(user);
                await _DbContext.SaveChangesAsync();

                return MapToUserREsponce(user);
        }

        public async Task<List<UserResponse>> GetAllUserAsync()
        {
            return await _DbContext.Users.Select(x => new UserResponse
            {
                Username = x.Username,
                Id = x.Id,
                Email = x.Email,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            }).ToListAsync();
        }
        public async Task<UserResponse> GetByIdAsync(int id)
        {
            var user = await _DbContext.Users.Where(x => x.Id == id && x.IsActive).FirstOrDefaultAsync();
            if(user == null)
            {
                return null;
            }
            return MapToUserREsponce(user);
        }
        public async Task<UpdateUserRequest> UpdateAsync(int id ,UpdateUserRequest request) 
        {

            var exist = await _DbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if(exist == null)
            {
                return null;
            }

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                IsActive = request.IsActive
            };
            _DbContext.Users.Add(user);
            await _DbContext.SaveChangesAsync();

            return new UpdateUserRequest
            {
                Username = request.Username,
                Email = request.Email,
                IsActive = request.IsActive

            };
           

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _DbContext.Users
           .FirstOrDefaultAsync(x => x.Id == id);
            if(user == null) { return false; }

            _DbContext.Users.Remove(user);

            await _DbContext.SaveChangesAsync();

            return true;
        }
        private UserResponse MapToUserREsponce(User? user)
        {
            var respoce = new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive,
                Email = user.Email
            };
            return respoce;
        }


    }
}
