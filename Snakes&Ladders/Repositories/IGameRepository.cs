using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnakesAndLadderEvyatar.Data;
using SnakesAndLadderEvyatar.DTO.Game;

namespace SnakesAndLadderEvyatar.Repositories
{
    public interface IGameRepository
    {
        // Get the game and a flag reporting if this is the best game in the scoreboard
        Task<Tuple<GetGameDto, bool>> GetGame(int gameId);

        // Get all the games that active played
        Task<List<GetGameDto>> GetAllActiveGames();

        // Get all the games that were played
        Task<List<GetGameDto>> GetAllGames();

        // Get all the games that were played by a specific player
        Task<List<GetGameDto>> GetAllGames(int playerId);

        // Get the best game (fastest to the finish line
        Task<GetGameDto> GetBestGame();
        
        // Start a new game for the given player
        Task<GetGameDto> CreateGame(AddGameDto gameDto);

        // Delete an existing game
        Task<bool> DeleteGame(int gameId);
        Task<bool> DeleteGame(Game game);

        // Assistant method to edit an existing game. newGameData should have the original game's Id
        Task<GetGameDto> EditGame(EditGameDto newGameData);
    }
}
