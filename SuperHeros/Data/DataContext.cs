using Microsoft.EntityFrameworkCore;

namespace SuperHeros.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<SuperHero> SuperHeroes { get; set; }

        public DbSet<UserDto> UserDtos { get; set; }

    }
}
