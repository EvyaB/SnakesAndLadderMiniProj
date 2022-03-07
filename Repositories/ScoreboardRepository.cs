using SnakesAndLadderEvyatar.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnakesAndLadderEvyatar.Data;

namespace SnakesAndLadderEvyatar.Repositories
{
    public class ScoreboardRepository : IScoreboardRepository
    {
        private readonly DataContext _dataContext;

        public ScoreboardRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Player GetBestPlayer()
        {
            Game bestGame = GetBestGame();
            return bestGame?.Player;
        }

        public Game GetBestGame()
        {
            return _dataContext.Games.Where(game => game.CurrentGameState == Game.GameState.Finished)
                .OrderBy(player => player.TurnNumber).First();
        }
    }
}
