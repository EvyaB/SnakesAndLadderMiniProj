using System.Collections.Generic;
using System.Linq;
using SnakesAndLadderEvyatar.DTO.Game;

namespace SnakesAndLadderEvyatar.DTO.Player
{
    public class GetPlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GetGameDto> Games { get; set; }

        public GetPlayerDto(Models.Player player)
        {
            Id = player.Id;
            Name = player.PlayerName;
            Games = player.Games?.Select(game => new GetGameDto(game)).ToList();
        }
    }
}
