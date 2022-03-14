using SnakesAndLadderEvyatar.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SnakesAndLadderEvyatar.DTO.Game;
using SnakesAndLadderEvyatar.DTO.Player;
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

        public async Task<GetPlayerDto> GetBestPlayer()
        {
            return await _dataContext.Games.Where(game => game.CurrentGameState == Game.GameState.Finished)
                .OrderBy(player => player.TurnNumber).Include(game => game.Player.Games).Select(game => new GetPlayerDto(game.Player)).FirstOrDefaultAsync();
        }

        public async Task<GetGameDto> GetBestGame()
        {
            return await _dataContext.Games.Where(game => game.CurrentGameState == Game.GameState.Finished)
                .OrderBy(player => player.TurnNumber).Include(game => game.Player).Select(game => new GetGameDto(game)).FirstOrDefaultAsync();
        }

        public async Task<GetGameDto> GetBestGame(int playerId)
        {
            return await _dataContext.Games.Where(game => game.CurrentGameState == Game.GameState.Finished && game.PlayerId == playerId)
                .OrderBy(player => player.TurnNumber).Include(game => game.Player).Select(game => new GetGameDto(game)).FirstOrDefaultAsync();
        }

        public async Task<bool> IsBestGame(Game game)
        {
            return (game != null && await IsBestGame(game.Id));
        }

        public async Task<bool> IsBestGame(int gameId)
        {
            GetGameDto bestGame = await GetBestGame();
            return (bestGame != null && bestGame.Id == gameId);
        }

        public async Task<bool> IsBestPlayer(Player player)
        {
            return (player != null && await IsBestPlayer(player.Id));
        }

        public async Task<bool> IsBestPlayer(int playerId)
        {
            GetPlayerDto bestPlayer = await GetBestPlayer();
            return (bestPlayer != null && bestPlayer.Id == playerId);
        }
    }
}
