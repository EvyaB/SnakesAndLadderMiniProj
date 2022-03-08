using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SnakesAndLadderEvyatar.Data;

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

        public async Task<Tuple<Game, bool>> GetGame(int gameId)
        {
            Game game = await _dataContext.Games.FindAsync(gameId);
            return new Tuple<Game, bool>(game, await _scoreboardRepository.IsBestGame(game));
        }

        public async Task<List<Game>> GetAllActiveGames()
        {
            return await _dataContext.Games.Include(game => game.Player).Where(game => game.CurrentGameState == Game.GameState.Playing).ToListAsync();
        }

        public async Task<List<Game>> GetAllGames()
        {
            return await _dataContext.Games.Include(game => game.Player).ToListAsync();
        }

        public async Task<List<Game>> GetAllGames(int playerId)
        {
            return await _dataContext.Games.Include(game => game.Player).Where(game => game.PlayerId == playerId).ToListAsync();
        }

        public async Task<Game> GetBestGame()
        {
            return await _scoreboardRepository.GetBestGame();
        }

        public async Task<Game> CreateGame(int playerId)
        {
            var result = await _playerRepository.GetPlayer(playerId);
            Player player = result.Item1;

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
                    PlayerId = playerId,
                    Player = player,
                    StartDateTime = DateTime.Now,
                    PlayerPosition = new Cell(0, 0),
                    TurnNumber = 0
                };

                //result.Item1.Games.Add(newGame);
                //_dataContext.Update(player);

                await _dataContext.AddAsync(newGame);
                await _dataContext.SaveChangesAsync();

                return newGame;
            }
        }

        public async Task<bool> DeleteGame(int gameId)
        {
            bool result = false;
            Game game = await _dataContext.Games.FindAsync(gameId);

            if (game != null)
            {
                _dataContext.Games.Remove(game);
                await _dataContext.SaveChangesAsync();
                result = true;
            }

            return result;
        }
        public async Task<bool> DeleteGame(Game game)
        {
            if (game == null) return false;
            return await DeleteGame(game.Id);
        }

        public async Task<Game> EditGame(Game newGameData)
        {
            if (newGameData == null) return null;

            Game originalGame = await _dataContext.Games.FindAsync(newGameData.Id);
            if (originalGame == null)
            {
                // Game with this newGameData.Id does not exist, do nothing
                return null;
            }
            else
            {
                originalGame.CurrentGameState = newGameData.CurrentGameState;
                originalGame.PlayerId = newGameData.PlayerId;
                originalGame.Player = newGameData.Player;
                originalGame.PlayerPosition = newGameData.PlayerPosition;
                originalGame.TurnNumber = newGameData.TurnNumber;
                originalGame.StartDateTime = newGameData.StartDateTime;
                originalGame.EndDateTime = newGameData.EndDateTime;

                _dataContext.Games.Update(originalGame);
                await _dataContext.SaveChangesAsync();
                return newGameData;
            }
        }
    }
}
