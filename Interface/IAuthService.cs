using AuthDemo.Models;

namespace AuthDemo.Interface
{
    public interface IAuthService
    {
        User Register(User model);

        LoginResponseModel Login(LoginRequestModel model);

        User GetUser(int id);
    }
}
