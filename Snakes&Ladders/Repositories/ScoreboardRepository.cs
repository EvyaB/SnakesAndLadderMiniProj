using SnakesAndLadderEvyatar.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SnakesAndLadderEvyatar.Models;

namespace SnakesAndLadderEvyatar.Repositories
{
    public class ScoreboardRepository : IScoreboardRepository
    {
        private readonly DataContext _dataContext;

        public ScoreboardRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Player> GetBestPlayer()
        {
            Game bestGame = await GetBestGame();
            return bestGame?.Player;
        }

        public async Task<Game> GetBestGame()
        {
            return await _dataContext.Games.Where(game => game.CurrentGameState == Game.GameState.Finished)
                .OrderBy(player => player.TurnNumber).Include(game => game.Player).FirstOrDefaultAsync();
        }

        public async Task<bool> IsBestGame(Game game)
        {
            return (game != null && await IsBestGame(game.Id));
        }

        public async Task<bool> IsBestGame(int gameId)
        {
            Game bestGame = await GetBestGame();
            return (bestGame != null && bestGame.Id == gameId);
        }

        public async Task<bool> IsBestPlayer(Player player)
        {
            return (player != null && await IsBestPlayer(player.Id));
        }

        public async Task<bool> IsBestPlayer(int playerId)
        {
            Player bestPlayer = await GetBestPlayer();
            return (bestPlayer != null && bestPlayer.Id == playerId);
        }
    }
}
