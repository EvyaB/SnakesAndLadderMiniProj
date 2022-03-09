using Microsoft.EntityFrameworkCore;
using SnakesAndLadderEvyatar.Data;

namespace SnakesAndLadderEvyatar.Repositories
{
    public class DataContext : DbContext
    {
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().OwnsOne(game => game.PlayerPosition);
            modelBuilder.Entity<Game>().HasIndex(game => new {game.CurrentGameState, game.TurnNumber}); // filtering by gamestate, then by turn number
        }
    }
}
