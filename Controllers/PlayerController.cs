﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnakesAndLadderEvyatar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private Repositories.IPlayerRepository _playerRepository;

        public PlayerController(Repositories.IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        // Get the player and a flag reporting if he is the best player in the scoreboard
        [HttpGet("{name}")]
        public IActionResult GetPlayerStatus(string name)
        {
            Tuple<GameLogic.Player, bool> player = _playerRepository.Get(name);
            
            // Check if the player was found
            if (player.Item1.PlayerGameState == GameLogic.Player.GameState.Unrecognized)
            {
                return NotFound();
            }
            else
            {
                return Ok(player);
            }
        }

        // Start a game as a new player 
        [HttpPost]
        public GameLogic.Player StartNewGame(string name)
        {
            return _playerRepository.CreateAndStartGame(name);
        }

        // Assistant method to check all the players
        [HttpGet("all")]
        public IEnumerable<GameLogic.Player> GetPlayerStatus()
        {
            return _playerRepository.Get();
        }
       
        // Assistant method to check for the best player (taken least turns to finish the game)
        [HttpGet("best")]
        public GameLogic.Player GetBestPlayer()
        {
            return _playerRepository.GetBestPlayer();
        }
    }
}