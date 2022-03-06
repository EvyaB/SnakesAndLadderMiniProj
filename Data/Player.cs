using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnakesAndLadderEvyatar.Data
{
    public class Player
    {
        public int Id { get; set; }

        public enum GameState
        {
            Unrecognized = 0,
            Playing = 1,
            Finished = 2
        }

        public string PlayerName { get; set; }
        public GameState PlayerGameState { get; set; }
        public int TurnNumber { get; set; }
        public Cell CurrentCell { get; set; }
        public DateTime GameStartDateTime { get; set; }
    }
}
