using Microsoft.EntityFrameworkCore;

namespace TaskBackEnd.Models
{
    public class UsersDbContext:DbContext
    {
        public UsersDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Signature> Signatures { get; set; }
    }
}
