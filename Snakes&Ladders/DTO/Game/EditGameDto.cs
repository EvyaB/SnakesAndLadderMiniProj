using System;
using Newtonsoft.Json;
using SnakesAndLadderEvyatar.Models;

namespace SnakesAndLadderEvyatar.DTO.Game
{
    public class EditGameDto
    {
        public int Id { get; set; }
        public int? TurnNumber { get; set; }
        public Cell? PlayerPosition { get; set; }
        public Models.Game.GameState? CurrentGameState { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }

        [JsonConstructor]
        public EditGameDto(int id, int? turnNumber = null, Cell? playerPosition = null,
                           Models.Game.GameState? currentGameState = null,
                           DateTime? startDateTime = null, DateTime? endDateTime = null)
        {
            Id = id;
            TurnNumber = turnNumber;
            PlayerPosition = playerPosition;
            CurrentGameState = currentGameState;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
        }

        public EditGameDto(Models.Game originalGame)
        {
            Id = originalGame.Id;
            PlayerPosition = originalGame.PlayerPosition;
            TurnNumber = originalGame.TurnNumber;
            CurrentGameState = originalGame.CurrentGameState;
            StartDateTime = originalGame.StartDateTime;
            EndDateTime = originalGame.EndDateTime;
        }
        public EditGameDto(GetGameDto originalGame)
        {
            Id = originalGame.Id;
            PlayerPosition = originalGame.PlayerPosition;
            TurnNumber = originalGame.TurnNumber;
            CurrentGameState = originalGame.GameState;
            StartDateTime = originalGame.StartDateTime;
            EndDateTime = originalGame.EndDateTime;
        }
    }
}
