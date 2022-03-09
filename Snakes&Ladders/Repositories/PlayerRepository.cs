using SnakesAndLadderEvyatar.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnakesAndLadderEvyatar.DTO.Player;

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

        public async Task<Tuple<GetPlayerDto, bool>> GetPlayer(string name)
        {
            Player playerData = await _dataContext.Players.Include(p => p.Games).FirstOrDefaultAsync(p => p.PlayerName == name);
            return new Tuple<GetPlayerDto, bool>(new GetPlayerDto(playerData), await _scoreboardRepository.IsBestPlayer(playerData));
        }

        public async Task<Tuple<GetPlayerDto, bool>> GetPlayer(int playerId)
        {
            Player playerData = await _dataContext.Players.Include(p => p.Games).FirstOrDefaultAsync(p => p.Id == playerId);
            return new Tuple<GetPlayerDto, bool>(new GetPlayerDto(playerData), await _scoreboardRepository.IsBestPlayer(playerData));
        }

        public async Task<IEnumerable<GetPlayerDto>> GetAllPlayers()
        {
            return await _dataContext.Players.Include(player => player.Games).Select(player => new GetPlayerDto(player)).ToListAsync();
        }

        public async Task<GetPlayerDto> GetBestPlayer()
        {
            return new GetPlayerDto(await _scoreboardRepository.GetBestPlayer());
        }

        public async Task<GetPlayerDto> CreatePlayer(CreatePlayerDto newPlayerDto)
        {
            Player newPlayer = new Player()
            {
                PlayerName = newPlayerDto.Name
            };

            // Add the player to the list of players
            await _dataContext.Players.AddAsync(newPlayer);
            await _dataContext.SaveChangesAsync();

            return new GetPlayerDto(newPlayer);
        }

        public async Task<bool> DeletePlayer(int playerId)
        {
            bool result = false;
            Player player = await _dataContext.Players.FindAsync(playerId);

            if (player != null)
            {
                _dataContext.Players.Remove(player);
                await _dataContext.SaveChangesAsync();
                result = true;
            }

            return result;
        }
        public async Task<bool> DeletePlayer(string playerName)
        {
            bool result = false;
            Player player = await _dataContext.Players.Where(player => player.PlayerName == playerName).FirstOrDefaultAsync();

            if (player != null)
            {
                _dataContext.Players.Remove(player);
                await _dataContext.SaveChangesAsync();
                result = true;
            }

            return result;
        }
    }
}
