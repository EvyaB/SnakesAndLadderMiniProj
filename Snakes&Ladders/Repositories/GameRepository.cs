using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SnakesAndLadderEvyatar.Models;
using SnakesAndLadderEvyatar.DTO.Game;
using SnakesAndLadderEvyatar.DTO.Player;

namespace SnakesAndLadderEvyatar.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly DataContext _dataContext;
        private readonly IScoreboardRepository _scoreboardRepository;
        private readonly IPlayerRepository _playerRepository;

        public GameRepository(DataContext dataContext, IScoreboardRepository scoreboardRepository, IPlayerRepository playerRepository)
        {
            _dataContext = dataContext;
            _scoreboardRepository = scoreboardRepository;
            _playerRepository = playerRepository;
        }

        public async Task<GetGameDto> GetGame(int gameId)
        {
            Game game = await _dataContext.Games.Include(game => game.Player).FirstOrDefaultAsync(game => game.Id == gameId);
            return game != null ? new GetGameDto(game) : null;
        }

        public async Task<List<GetGameDto>> GetAllActiveGames()
        {
            return await _dataContext.Games.Include(game => game.Player).Where(game => game.CurrentGameState == Game.GameState.Playing).
                                            Select(game => new GetGameDto(game)).ToListAsync();
        }

        public async Task<List<GetGameDto>> GetAllGames()
        {
            return await _dataContext.Games.Include(game => game.Player).Select(game => new GetGameDto(game)).ToListAsync();
        }

        public async Task<List<GetGameDto>> GetAllGames(int playerId)
        {
            return await _dataContext.Games.Include(game => game.Player).Where(game => game.PlayerId == playerId).Select(game => new GetGameDto(game)).ToListAsync();
        }

        public async Task<GetGameDto> CreateGame(AddGameDto gameDto)
        {
            GetPlayerDto player = await _playerRepository.GetPlayer(gameDto.PlayerId);

            if (player == null)
            {
                // player id is invalid, do not create a new game
                return null;
            }
            else
            {
                Game newGame = new Game()
                {
                    CurrentGameState = Game.GameState.Playing,
                    PlayerId = gameDto.PlayerId,
                    StartDateTime = DateTime.Now,
                    PlayerPosition = new Cell(0, 0),
                    TurnNumber = 0,
                    GameDuration = TimeSpan.Zero
                };

                await _dataContext.AddAsync(newGame);
                await _dataContext.SaveChangesAsync();

                return new GetGameDto(newGame);
            }
        }

        public async Task<bool> DeleteGame(int gameId)
        {
            Game game = await _dataContext.Games.FindAsync(gameId);
            return await DeleteGame(game);
        }
        public async Task<bool> DeleteGame(Game game)
        {
            bool result = false;

            if (game != null && _dataContext.Games.Contains(game))
            {
                _dataContext.Games.Remove(game);
                await _dataContext.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<GetGameDto> EditGame(EditGameDto editedGame)
        {
            if (editedGame == null) return null;

            Game originalGame = await _dataContext.Games.FindAsync(editedGame.Id);
            if (originalGame == null)
            {
                // Game with this newGameData.Id does not exist, do nothing
                return null;
            }
            else
            {
                originalGame.TurnNumber = (editedGame.TurnNumber ?? originalGame.TurnNumber);
                originalGame.PlayerPosition = (editedGame.PlayerPosition ?? originalGame.PlayerPosition);
                originalGame.CurrentGameState = (editedGame.CurrentGameState ?? originalGame.CurrentGameState);
                originalGame.StartDateTime = (editedGame.StartDateTime ?? originalGame.StartDateTime);
                originalGame.EndDateTime = (editedGame.EndDateTime ?? originalGame.EndDateTime);

                _dataContext.Games.Update(originalGame);
                await _dataContext.SaveChangesAsync();
                return new GetGameDto(originalGame);
            }
        }
    }
}
