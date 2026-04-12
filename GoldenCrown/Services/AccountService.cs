using GoldenCrown.Database;
using GoldenCrown.Models;
using Microsoft.EntityFrameworkCore;

namespace GoldenCrown.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;

        AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAccountAsync(string login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Login == login);
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to find a user with login: {login}");
            }

            var newAccount = new Account
            {
                UserId = user.Id,
                Balance = 0
            };

            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
