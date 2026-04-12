using GoldenCrown.Database;
using GoldenCrown.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GoldenCrown.Services
{
    public class UserService : IUserService
    {
        private const int PASSWORD_MIN_LENGTH = 6;
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Register(string login, string name, string password)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(x => x.Login == login);
            if (existing != null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < PASSWORD_MIN_LENGTH)
            {
                return false;
            }

            var user = new User
            {
                Login = login,
                Name = name,
                Password = password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
