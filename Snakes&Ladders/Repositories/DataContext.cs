using Microsoft.EntityFrameworkCore;
using SnakesAndLadderEvyatar.Models;

namespace SnakesAndLadderEvyatar.Repositories
{
    public class DataContext : DbContext
    {
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Game> Games { get; set; }

        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().OwnsOne(game => game.PlayerPosition);
            modelBuilder.Entity<Game>().HasIndex(game => new {game.CurrentGameState, game.TurnNumber}); // filtering by gamestate, then by turn number
            modelBuilder.Entity<Player>().HasMany(p => p.Games).WithOne(g => g.Player);
        }
    }
}
