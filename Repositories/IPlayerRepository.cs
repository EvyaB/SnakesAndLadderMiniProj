using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnakesAndLadderEvyatar.DTO.Player;

namespace SnakesAndLadderEvyatar.Repositories
{
    public interface IPlayerRepository
    {
        // Get the player and a flag reporting if he is the best player in the scoreboard
        Task<Tuple<GetPlayerDto, bool>> GetPlayer(string name);
        Task<Tuple<GetPlayerDto, bool>> GetPlayer(int playerId);

        // Assistant methods for debugging
        Task<IEnumerable<GetPlayerDto>> GetAllPlayers();
        Task<GetPlayerDto> GetBestPlayer();

        // Create a new player with a given name, and immediately start playing the game for him
        Task<GetPlayerDto> CreatePlayer(CreatePlayerDto newPlayerDto);

        // Delete an existing player
        Task<bool> DeletePlayer(int playerId);
        Task<bool> DeletePlayer(string playerName);
    }
}
