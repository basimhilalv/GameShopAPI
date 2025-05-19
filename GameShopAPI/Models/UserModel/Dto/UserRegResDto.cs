namespace GameShopAPI.Models.UserModel.Dto
{
    public class UserRegResDto
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set }
    }
}
