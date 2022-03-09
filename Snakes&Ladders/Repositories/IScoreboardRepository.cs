using SnakesAndLadderEvyatar.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnakesAndLadderEvyatar.Models;

namespace SnakesAndLadderEvyatar.Repositories
{
    public interface IScoreboardRepository
    {
        public Task<Player> GetBestPlayer();
        public Task<Game> GetBestGame();
        public Task<bool> IsBestGame(Game game);
        public Task<bool> IsBestGame(int gameId);
        public Task<bool> IsBestPlayer(Player player);
        public Task<bool> IsBestPlayer(int playerId);
    }
}
