using SnakesAndLadderEvyatar.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SnakesAndLadderEvyatar.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IGameRepository _gameRepository;
       // private readonly DataContext _dataContext;
        private readonly IServiceScopeFactory _scopeFactory;

        public PlayerRepository(IGameRepository gameRepository, IServiceScopeFactory scopeFactory)
        {
            _gameRepository = gameRepository;
            _scopeFactory = scopeFactory;
        }

        public async Task<Player> CreateAndStartGame(string name)
        {
            Player newPlayer = new Player()
            {
                PlayerName = name,
                PlayerGameState = Player.GameState.Playing,
                TurnNumber = 0,
                CurrentCell = new Tuple<int, int>(0, 0),
                GameStartDateTime = DateTime.Now
            };

            using var scope = _scopeFactory.CreateScope();
            DataContext _dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            // Add the player to the list of players
            await _dataContext.Players.AddAsync(newPlayer);
            await _dataContext.SaveChangesAsync();

            return newPlayer;
        }

        public async Task<Tuple<Player, bool>> GetPlayer(string name)
        {
            using var scope = _scopeFactory.CreateScope();
            DataContext _dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            Tuple<Player, bool> result;
            Player playerData = await _dataContext.Players.FirstOrDefaultAsync(p => p.PlayerName == name);

            if (playerData != null)
            {
                result = new Tuple<Player, bool>(playerData, _gameRepository.GetBestPlayer() == playerData);
            }
            else
            {
                playerData = new Player() { PlayerName = name, PlayerGameState = Player.GameState.Unrecognized };
                result = new Tuple<Player, bool>(playerData, false);
            }

            return result;
        }

        public async Task<IEnumerable<Player>> GetAllPlayers()
        {
            using var scope = _scopeFactory.CreateScope();
            DataContext _dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            return await _dataContext.Players.ToListAsync();
        }
        public async Task<IEnumerable<Player>> GetAllPlayingPlayers()
        {
            using var scope = _scopeFactory.CreateScope();
            DataContext _dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            return await _dataContext.Players.Where(player => player.PlayerGameState == Player.GameState.Playing).ToListAsync();
        }
        public async Task<Player> GetBestPlayer()
        {
            return _gameRepository.GetBestPlayer();
        }
    }
}
