using System;

namespace SnakesAndLadderEvyatar.Models
{
    public class Game
    {
        public enum GameState
        {
            Unrecognized = 0,
            Playing = 1,
            Finished = 2
        }

        public int Id { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public int TurnNumber { get; set; }
        public Cell PlayerPosition { get; set; }
        public GameState CurrentGameState { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
