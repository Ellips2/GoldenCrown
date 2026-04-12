namespace GoldenCrown.Services
{
    public interface IAccountService
    {
        Task<bool> CreateAccountAsync(string login);
    }
}
