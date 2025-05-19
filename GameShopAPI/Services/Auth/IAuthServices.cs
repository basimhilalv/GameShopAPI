using GameShopAPI.Models.UserModel.Dto;

namespace GameShopAPI.Services.Auth
{
    public interface IAuthServices
    {
        Task<string> Register(UserRegDto request);
        Task<UserLoginResDto> Login(UserLoginDto request);
    }
}
