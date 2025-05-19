using GameShopAPI.Models.UserModel;
using Microsoft.EntityFrameworkCore;

namespace GameShopAPI.DbContextData
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
