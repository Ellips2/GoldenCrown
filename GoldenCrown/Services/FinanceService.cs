using GoldenCrown.Database;
using Microsoft.EntityFrameworkCore;

namespace GoldenCrown.Services
{
    public class FinanceService : IFinanceService
    {
        private readonly ApplicationDbContext _context;

        public FinanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<decimal>> GetBalanceAsync(string token)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(s => s.Token == token);
            if (session == null) 
            {
                return Result<decimal>.Failure("User is not authorized");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == session.UserId);
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == user!.Id);
            return Result<decimal>.Success(account!.Balance);
        }
    }
}
