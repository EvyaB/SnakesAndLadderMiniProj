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
            if (_gameBoard.BoardSize.Item1 % 2 == 0)
            {
                return new Cell(_gameBoard.BoardSize);
            }
            else
            {
                // If the board size is odd, game ends at the left side of the board
                return new Cell(_gameBoard.BoardSize.Item1, 0); 
            }
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