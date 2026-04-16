using GoldenCrown.Database;
using GoldenCrown.DTOs.Finance;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

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

        public async Task<Result> DepositAsync(string token, decimal amount)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(s => s.Token == token);
            if (session == null)
            {
                return Result<decimal>.Failure("User is not authorized");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == session.UserId);
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == user!.Id);

            account!.Balance += amount;
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> TransferAsync(string fromToken, string toLogin, decimal amount)
        {
            var fromSession = await _context.Sessions.FirstOrDefaultAsync(s => s.Token == fromToken);
            if (fromSession == null)
            {
                return Result<decimal>.Failure("User is not authorized");
            }

            var fromUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == fromSession.UserId);
            var fromAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == fromUser!.Id);

            var toUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == toLogin);
            if (toUser == null)
            {
                return Result.Failure("User is not found");
            }

            var toAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == toUser.Id);
            if (toUser == null)
            {
                return Result.Failure("Not enough money");
            }

            fromAccount.Balance -= amount;
            toAccount!.Balance += amount;

            var transaction = new Models.Transaction {
                ReceiverAccountId = toAccount.Id,
                SenderAccountId = fromAccount.Id,
                Amount = amount,
                CreatedAt = DateTime.UtcNow,
            };
            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<IEnumerable<TransactionHistoryResponse>>> GetTransactionHistoryAsync(string token, DateTime? dateFrom, DateTime? dateTo, int skip, int take)
        {
            if (dateFrom != null && dateTo != null && dateFrom > dateTo)
            {
                return Result<IEnumerable<TransactionHistoryResponse>>.Failure("Uncorrect date range");
            }

            var session = await _context.Sessions.FirstOrDefaultAsync(s => s.Token == token);
            if (session == null)
            {
                return Result<IEnumerable<TransactionHistoryResponse>>.Failure("User is not authorized");
            }
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == session.UserId);

            var transactions = _context.Transactions.Where(x => x.SenderAccountId == account!.Id || x.ReceiverAccountId == account.Id);
            if (dateFrom != null)
                transactions = transactions.Where(x => x.CreatedAt >= dateFrom.Value);

            if (dateTo != null)
                transactions = transactions.Where(x => x.CreatedAt <= dateTo.Value);

            transactions = transactions.Skip(count: skip).Take(count: take);

            var dbTransactions = await transactions.ToListAsync();
            var result = new List<TransactionHistoryResponse>();
            var allSenders = transactions.Select(selector: x => x.SenderAccountId);
            var allReceivers = transactions.Select(selector: x => x.ReceiverAccountId);
            var allAccounts = allSenders.ToHashSet();
            foreach (var receiver in allReceivers)
            {
                allAccounts.Add(receiver);
            }

            var names = await _context.Accounts.Where(x => allAccounts.Contains(x.Id))
                .Join(_context.Users, 
                acc => acc.UserId, 
                u => u.Id,
                (acc, u) => new
                {
                    Name = u.Name,
                    AccId = acc.Id
                }).ToDictionaryAsync(x => x.AccId);

            foreach (var transaction in transactions)
            {
                var senderName = names[transaction.SenderAccountId].Name;
                var receiverName = names[transaction.ReceiverAccountId].Name;
                result.Add(new TransactionHistoryResponse
                {
                    SenderName = senderName,
                    ReceiverName = receiverName,
                    Amount = transaction.Amount,
                    Date = transaction.CreatedAt
                });
            }

            return result;
        }
    }
}
