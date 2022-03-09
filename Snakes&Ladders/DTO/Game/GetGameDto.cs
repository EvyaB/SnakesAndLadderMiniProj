using System;
using SnakesAndLadderEvyatar.Models;

namespace SnakesAndLadderEvyatar.DTO.Game
{
    public class GetGameDto
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public Models.Game.GameState GameState { get; set; }
        public int TurnNumber { get; set; }
        public Cell PlayerPosition { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public GetGameDto(Models.Game game)
        {
            Id = game.Id;
            PlayerName = game.Player?.PlayerName;
            GameState = game.CurrentGameState;
            TurnNumber = game.TurnNumber;
            PlayerPosition = game.PlayerPosition;
            StartDateTime = game.StartDateTime;
            EndDateTime = game.EndDateTime;
        }
    }
}
