namespace GoldenCrown.Services
{
    public interface IUserService
    {
        Task<bool> Register(string login, string name, string password);
    }
}
