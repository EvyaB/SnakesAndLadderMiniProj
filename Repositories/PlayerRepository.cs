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
        private readonly IScoreboardRepository _scoreboardRepository;
        // private readonly DataContext _dataContext;
        private readonly IServiceScopeFactory _scopeFactory;

        public PlayerRepository(IScoreboardRepository scoreboardRepository, IServiceScopeFactory scopeFactory)
        {
            _scoreboardRepository = scoreboardRepository;
            _scopeFactory = scopeFactory;
        }

        public async Task<Player> CreatePlayer(string name)
        {
            Player newPlayer = new Player()
            {
                PlayerName = name
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
            Player playerData = await _dataContext.Players.Include(p => p.Games).FirstOrDefaultAsync(p => p.PlayerName == name);

            if (playerData != null)
            {
                result = new Tuple<Player, bool>(playerData, _scoreboardRepository.GetBestPlayer() == playerData);
            }
            else
            {
                playerData = new Player();
                result = new Tuple<Player, bool>(playerData, false);
            }

            return result;
        }
        public async Task<Tuple<Player, bool>> GetPlayer(int id)
        {
            using var scope = _scopeFactory.CreateScope();
            DataContext _dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            Tuple<Player, bool> result;
            Player playerData = await _dataContext.Players.FindAsync(id);

            if (playerData != null)
            {
                result = new Tuple<Player, bool>(playerData, _scoreboardRepository.GetBestPlayer() == playerData);
            }
            else
            {
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

        public async Task<Player> GetBestPlayer()
        {
            return _scoreboardRepository.GetBestPlayer();
        }
    }
}
