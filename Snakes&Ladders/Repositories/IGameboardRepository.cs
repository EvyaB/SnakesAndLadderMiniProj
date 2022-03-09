using SnakesAndLadderEvyatar.Models;

namespace SnakesAndLadderEvyatar.Repositories
{
    public interface IGameboardRepository
    {
        public Cell GetFinalCell();
        public int GetBoardRowsCount();
        public int GetBoardColumnsCount();
        public bool GetCellModifier(Cell cell, out CellModifier cellModifier);
    }
}