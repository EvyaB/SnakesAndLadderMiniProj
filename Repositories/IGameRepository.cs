using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnakesAndLadderEvyatar.Data;

namespace SnakesAndLadderEvyatar.Repositories
{
    public interface IGameRepository
    {
        // Get the game and a flag reporting if this is the best game in the scoreboard
        Task<Tuple<Game, bool>> GetGame(int gameId);

        // Get all the games that active played
        Task<List<Game>> GetAllActiveGames();

        // Get all the games that were played
        Task<List<Game>> GetAllGames();

        // Get all the games that were played by a specific player
        Task<List<Game>> GetAllGames(int playerId);

        // Get the best game (fastest to the finish line
        Task<Game> GetBestGame();
        
        // Start a new game for the given player
        Task<Game> CreateGame(int playerId);

        // Delete an existing game
        Task<bool> DeleteGame(int gameId);
        Task<bool> DeleteGame(Game game);

        // Assistant method to edit an existing game. newGameData should have the original game's Id
        Task<Game> EditGame(Game newGameData);
    }
}
