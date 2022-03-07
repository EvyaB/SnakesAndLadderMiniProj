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
        private Scoreboard _scoreboard;
        private readonly GameboardRepository _gameboardRepository;

        public ScoreboardRepository()
        {
            _scoreboard = new Scoreboard();

            _gameboardRepository = new GameboardRepository();
        }

        public Player GetBestPlayer()
        {
            return _scoreboard.BestPlayer;
        }

        public Cell GetFinalCell()
        {
            return _gameboardRepository.GetFinalCell();
        }

        public int GetBoardRowsCount()
        {
            return _gameboardRepository.GetBoardRowsCount();
        }

        public int GetBoardColumnsCount()
        {
            return _gameboardRepository.GetBoardColumnsCount();
        }

        public void ReportPlayerScore(Player player)
        {
            _scoreboard.AddPlayerScore(player);
        }

        public bool GetCellModifier(Cell cell, out CellModifier cellModifier)
        {
            return _gameboardRepository.GetCellModifier(cell, out cellModifier);
        }
    }
}
