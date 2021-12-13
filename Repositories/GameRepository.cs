using SnakesAndLadderEvyatar.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnakesAndLadderEvyatar.Repositories
{
    public class GameRepository : IGameRepository
    {
        private GameLogic.GameBoard _gameBoard;
        private GameLogic.Scoreboard _scoreboard;

        public GameRepository()
        {
            _gameBoard = new GameBoard();
            _scoreboard = new Scoreboard();

            _gameBoard.Initialize();
        }

        public Player GetBestPlayer()
        {
            return _scoreboard.BestPlayer;
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

        public void SetBestPlayer(Player player)
        {
            _scoreboard.BestPlayer = player;
        }

        public bool GetCellModifier(Cell cell, out CellModifier cellModifier)
        {
            cellModifier = _gameBoard.GetCellModifier(cell);

            // Report if the cell modifier is valid (=cell has a modifier)
            return (cellModifier != null); 
        }
    }
}
