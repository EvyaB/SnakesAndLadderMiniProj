using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnakesAndLadderEvyatar.Repositories
{
    public interface IPlayerRepository
    {
        // Get the player and a flag reporting if he is the best player in the scoreboard
        Tuple<GameLogic.Player, bool> GetPlayer(string name);

        // Create a new player with a given name, and immediately start playing the game for him
        GameLogic.Player CreateAndStartGame(string name);

        // Assistant methods for debugging
        IEnumerable<GameLogic.Player> GetAllPlayers();
        GameLogic.Player GetBestPlayer();
    }
}
