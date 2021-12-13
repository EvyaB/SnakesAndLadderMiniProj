using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnakesAndLadderEvyatar.GameLogic
{
    public class Scoreboard
    {
        public Player BestPlayer { get; set; }

        public Scoreboard()
        {
            BestPlayer = new Player() { PlayerName = "None", TurnNumber = int.MaxValue, PlayerGameState = Player.GameState.Unrecognized, CurrentCell = new Tuple<int, int>(0, 0) };
        }
    }
}
