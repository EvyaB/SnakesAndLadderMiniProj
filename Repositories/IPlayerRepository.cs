using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnakesAndLadderEvyatar.Data;

namespace SnakesAndLadderEvyatar.Repositories
{
    public interface IPlayerRepository
    {
        // Get the player and a flag reporting if he is the best player in the scoreboard
        Task<Tuple<Player, bool>> GetPlayer(string name);
        Task<Tuple<Player, bool>> GetPlayer(int playerId);

        // Create a new player with a given name, and immediately start playing the game for him
        Task<Player> CreatePlayer(string name);
        // Assistant methods for debugging
        Task<IEnumerable<Player>> GetAllPlayers();
        Task<Player> GetBestPlayer();
    }
}
