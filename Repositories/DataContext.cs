using Microsoft.EntityFrameworkCore;
using SnakesAndLadderEvyatar.Data;

namespace SnakesAndLadderEvyatar.Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions dataContextOptions) : base(dataContextOptions)
        {

        }

        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().OwnsOne(x => x.CurrentCell);
        }
    }
}
