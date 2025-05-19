using AutoMapper;
using GameShopAPI.DbContextData;
using GameShopAPI.Models.UserModel;
using GameShopAPI.Models.UserModel.Dto;
using Microsoft.EntityFrameworkCore;

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

        public Task<UserLoginResDto> Login(UserLoginDto request)
        {
            throw new NotImplementedException();
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

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return "User Registered Successfully";
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
