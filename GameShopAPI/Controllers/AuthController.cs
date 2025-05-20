using GameShopAPI.AppResponse;
using GameShopAPI.Models.UserModel.Dto;
using GameShopAPI.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authService;
        public AuthController(IAuthServices authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Register([FromForm] UserRegDto request)
        {
            try
            {
                var result = await _authService.Register(request);
                if(result is null)
                {
                    return BadRequest(new APIResponse<string>(400, "Registration Failed"));
                }
                return Ok(new APIResponse<string>(200, result));
            }catch(Exception ex)
            {
                return BadRequest(new APIResponse<string>(400, "Registration Failed", null, ex.Message));
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            try
            {
                var loginRes = await _authService.Login(request);
                return Ok(new APIResponse<UserLoginResDto>(200, "Login Successfull", loginRes, null));
            }
            catch(Exception ex)
            {
                return Unauthorized(new APIResponse<string>(401, "Login Failed", null, ex.Message));
            }
        }
    }
}
