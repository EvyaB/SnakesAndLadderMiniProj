using System;
using SnakesAndLadderEvyatar.Models;

namespace SnakesAndLadderEvyatar.DTO.Game
{
    public class EditGameDto
    {
        public int Id { get; set; }
        public int? PlayerId { get; set; }
        public int? TurnNumber { get; set; }
        public Cell? PlayerPosition { get; set; }
        public Models.Game.GameState? CurrentGameState { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
    }
}
