using SnakesAndLadderEvyatar.Models;

namespace SnakesAndLadderEvyatar.Repositories
{
    public class GameboardRepository : IGameboardRepository
    {
        private GameBoard _gameBoard;

        public GameboardRepository()
        {
            _gameBoard = new GameBoard();
            _gameBoard.Initialize();
        }

        public Cell GetFinalCell()
        {
            return new Cell(_gameBoard.BoardSize);
        }

        public int GetBoardRowsCount()
        {
            return _gameBoard.BoardSize.Item1;
        }

        public int GetBoardColumnsCount()
        {
            return _gameBoard.BoardSize.Item2;
        }

        public bool GetCellModifier(Cell cell, out CellModifier cellModifier)
        {
            cellModifier = _gameBoard.GetCellModifier(cell);

            // Report if the cell modifier is valid (=cell has a modifier)
            return (cellModifier != null); 
        }
    }
}