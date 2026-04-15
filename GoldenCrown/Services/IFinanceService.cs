namespace GoldenCrown.Services
{
    public interface IFinanceService
    {
        Task<Result> DepositAsync(string token, decimal amount);
        Task<Result<decimal>> GetBalanceAsync(string token);
    }
}
