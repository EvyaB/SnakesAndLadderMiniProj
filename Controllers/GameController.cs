﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnakesAndLadderEvyatar.Data;

namespace SnakesAndLadderEvyatar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly Repositories.IGameRepository _gameRepository;

        public GameController(Repositories.IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        // Get the game and a flag reporting if this is the best game in the scoreboard
        [HttpGet("{gameId}")]
        public async Task<IActionResult> GetGameStatus(int gameId)
        {
            Tuple<Game, bool> game = await _gameRepository.GetGame(gameId);
          
            // Check if the game was found
            if (game.Item1 != null)
            {
                return Ok(game);
            }
            else
            {
                return NotFound();
            }
        }

        // Start a game as a specific player 
        [HttpPost]
        public async Task<Game> CreateNewGame(int playerId)
        {
            return await _gameRepository.CreateGame(playerId);
        }

        // Delete an existing game
        [HttpDelete("{gameId}")]
        public async Task<bool> DeleteGame(int gameId)
        {
            return await _gameRepository.DeleteGame(gameId);
        }

        // Assistant method to check all the active games
        [HttpGet("allActive")]
        public async Task<IEnumerable<Game>> GetAllActiveGames()
        {
            return await _gameRepository.GetAllActiveGames();
        }

        // Assistant method to check all the games
        [HttpGet("all")]
        public async Task<IEnumerable<Game>> GetAllGames()
        {
            return await _gameRepository.GetAllGames();
        }

        // Assistant method to check all the games of specific Player
        [HttpGet("all{playerId}")]
        public async Task<IEnumerable<Game>> GetAllGames(int playerId)
        {
            return await _gameRepository.GetAllGames(playerId);
        }

        // Assistant method to check for the best game (taken least turns to finish the game)
        [HttpGet("best")]
        public async Task<Game> GetBestGame()
        {
            return await _gameRepository.GetBestGame();
        }
    }
}
