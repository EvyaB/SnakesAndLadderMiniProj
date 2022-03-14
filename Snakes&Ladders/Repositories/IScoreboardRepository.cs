using SnakesAndLadderEvyatar.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnakesAndLadderEvyatar.DTO.Game;
using SnakesAndLadderEvyatar.DTO.Player;
using SnakesAndLadderEvyatar.Models;

namespace SnakesAndLadderEvyatar.Repositories
{
    public interface IScoreboardRepository
    {
        public Task<GetPlayerDto> GetBestPlayer();
        public Task<GetGameDto> GetBestGame();
        public Task<GetGameDto> GetBestGame(int playerId);
        public Task<bool> IsBestGame(Game game);
        public Task<bool> IsBestGame(int gameId);
        public Task<bool> IsBestPlayer(Player player);
        public Task<bool> IsBestPlayer(int playerId);
    }
}
