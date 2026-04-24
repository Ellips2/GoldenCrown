using MediatR;

namespace GoldenCrown.Features.User.UserRegister
{
    public class UserRegisterCommand : IRequest<Result<string>>
    {
        public string Login { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public UserRegisterCommand(string login, string name, string password)
        {
            Login = login;
            Name = name;
            Password = password;
        }
    }
}
