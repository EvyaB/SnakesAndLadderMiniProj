using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnakesAndLadderEvyatar.Models;
using SnakesAndLadderEvyatar.DTO.Player;

namespace SnakesAndLadderEvyatar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly Repositories.IPlayerRepository _playerRepository;
        private readonly Repositories.IScoreboardRepository _scoreboardRepository;

        public PlayerController(Repositories.IPlayerRepository playerRepository, Repositories.IScoreboardRepository scoreboardRepository)
        {
            _playerRepository = playerRepository;
            _scoreboardRepository = scoreboardRepository;
        }

        // Get the player and a flag reporting if he is the best player in the scoreboard
        [HttpGet("{name:alpha}")]
        public async Task<IActionResult> GetPlayerStatus(string name)
        {
            GetPlayerDto player = await _playerRepository.GetPlayer(name);

            // Check if the player was found
            if (player != null)
            {
                bool isBestPlayer = await _scoreboardRepository.IsBestPlayer(player.Id);
                Tuple<GetPlayerDto, bool> result = new Tuple<GetPlayerDto, bool>(player, isBestPlayer);
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        // Get the player and a flag reporting if he is the best player in the scoreboard
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPlayerStatus(int id)
        {
            GetPlayerDto player = await _playerRepository.GetPlayer(id);

            // Check if the player was found
            if (player != null)
            {
                bool isBestPlayer = await _scoreboardRepository.IsBestPlayer(player.Id);
                Tuple<GetPlayerDto, bool> result = new Tuple<GetPlayerDto, bool>(player, isBestPlayer);
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        // Start a game as a new player 
        [HttpPost]
        public async Task<GetPlayerDto> CreateNewPlayer(CreatePlayerDto newPlayer)
        {
            return await _playerRepository.CreatePlayer(newPlayer);
        }

        [HttpDelete("{playerId:int}")]
        public async Task<bool> DeletePlayer(int playerId)
        {
            return await _playerRepository.DeletePlayer(playerId);
        }
        [HttpDelete("{playerName:alpha}")]
        public async Task<bool> DeletePlayer(string playerName)
        {
            return await _playerRepository.DeletePlayer(playerName);
        }

        // Assistant method to check all the players
        [HttpGet("all")]
        public async Task<IEnumerable<GetPlayerDto>> GetPlayerStatus()
        {
            return await _playerRepository.GetAllPlayers();
        }
       
        // Assistant method to check for the best player (taken least turns to finish the game)
        [HttpGet("best")]
        public async Task<GetPlayerDto> GetBestPlayer()
        {
            return await _scoreboardRepository.GetBestPlayer();
        }
    }
}
