using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnakesAndLadderEvyatar.Data
{
    public class Scoreboard
    {
        public Player BestPlayer { get; set; }

        public Scoreboard()
        {
            BestPlayer = new Player() { PlayerName = "None", TurnNumber = int.MaxValue, PlayerGameState = Player.GameState.Unrecognized, CurrentCell = new Tuple<int, int>(0, 0) };
        }

        public void AddPlayerScore(Player player)
        {
            // Only saving the best player right, so just check if the new player has beaten the previous best score
            if (player.TurnNumber < BestPlayer.TurnNumber)
            {
                BestPlayer = player;
            }
        }
    }
}
