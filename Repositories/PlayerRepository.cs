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
        private readonly DataContext _dataContext;

        public PlayerRepository(IScoreboardRepository scoreboardRepository, DataContext dataContext)
        {
            _scoreboardRepository = scoreboardRepository;
            _dataContext = dataContext;
        }

        public async Task<Player> CreatePlayer(string name)
        {
            Player newPlayer = new Player()
            {
                PlayerName = name
            };

            // Add the player to the list of players
            await _dataContext.Players.AddAsync(newPlayer);
            await _dataContext.SaveChangesAsync();

            return newPlayer;
        }

        public async Task<Tuple<Player, bool>> GetPlayer(string name)
        {
            Player playerData = await _dataContext.Players.Include(p => p.Games).FirstOrDefaultAsync(p => p.PlayerName == name);
            return new Tuple<Player, bool>(playerData, await _scoreboardRepository.IsBestPlayer(playerData));
        }

        public async Task<Tuple<Player, bool>> GetPlayer(int playerId)
        {
            Player playerData = await _dataContext.Players.Include(p => p.Games).FirstOrDefaultAsync(p => p.Id == playerId);
            return new Tuple<Player, bool>(playerData, await _scoreboardRepository.IsBestPlayer(playerData));
        }

        public async Task<IEnumerable<Player>> GetAllPlayers()
        {
            return await _dataContext.Players.Include(player => player.Games).ToListAsync();
        }

        public async Task<Player> GetBestPlayer()
        {
            return await _scoreboardRepository.GetBestPlayer();
        }
    }
}
