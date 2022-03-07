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
        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().OwnsOne(game => game.PlayerPosition);
            modelBuilder.Entity<Game>().HasIndex(game => new {game.CurrentGameState, game.TurnNumber}); // filtering by gamestate, then by turn number
        }
    }
}
