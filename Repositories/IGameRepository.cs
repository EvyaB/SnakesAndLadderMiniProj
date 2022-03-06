using SnakesAndLadderEvyatar.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnakesAndLadderEvyatar.Data;

namespace SnakesAndLadderEvyatar.Repositories
{
    public interface IGameRepository
    {
        public Player GetBestPlayer();
        public Cell GetFinalCell();
        public int GetBoardRowsCount();
        public int GetBoardColumnsCount();
        public bool GetCellModifier(Cell cell, out CellModifier cellModifier);
        public void ReportPlayerScore(Player player);
    }
}
