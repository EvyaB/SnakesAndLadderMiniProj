using System;

namespace SnakesAndLadderEvyatar.GameLogic
{
    public interface IDice
    {
        public int DiceRoll();
    };

    public class Dice : IDice
    {
        public int DiceRoll()
        {
            Random rand = new Random();
            return rand.Next(1, 6);
        }
    }
}
