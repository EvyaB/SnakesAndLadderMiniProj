using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnakesAndLadderEvyatar.Data
{
    public class Player
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public List<Game> Games { get; set; }
    }
}
