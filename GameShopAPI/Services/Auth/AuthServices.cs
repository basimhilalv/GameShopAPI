using AutoMapper;
using GameShopAPI.DbContextData;
using GameShopAPI.Models.UserModel;
using GameShopAPI.Models.UserModel.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameShopAPI.Services.Auth
{
    public class AuthServices : IAuthServices
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public AuthServices(IConfiguration config, IMapper mapper, AppDbContext context)
        {
            _config = config;
            _mapper = mapper;
            _context = context;
        }

        public async Task<UserLoginResDto> Login(UserLoginDto request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user is null) throw new InvalidOperationException("Invalid Email");
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password)) throw new InvalidOperationException("Invalid Password");

                var token = createToken(user);

                var loginRes = new UserLoginResDto
                {
                    Username = user.Username,
                    Email = user.Email,
                    Token = token
                };
                return loginRes;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> Register(UserRegDto request)
        {
            try
            {
                if (request is null) throw new ArgumentNullException("Userdata cannot be null");

                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (existingUser != null) throw new Exception("User with the same email already exists");

                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string hashpass = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);
                var user = _mapper.Map<User>(request);
                user.Password = hashpass;

                //handle avatar upload
                if(request.Avatar != null && request.Avatar.Length > 0)
                {
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");

                    if(!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Avatar.FileName)}";
                    var filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.Avatar.CopyToAsync(fileStream);
                    }

                    user.Avatar = $"/avatars/{uniqueFileName}";
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return "User Registered Successfully";
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string createToken(User)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddHours(2)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
