using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnakesAndLadderEvyatar.Data
{
    public class GameBoard
    {
        private static int DEFAULT_BOARD_SIZE_ROWS = 6;
        private static int DEFAULT_BOARD_SIZE_COLUMNS = 6;

        public Tuple<int, int> BoardSize { get; protected set; }

        protected List<CellModifier> CellModifiers { get; set; }

        // Fill up the board with Snakes & Ladders
        public void Initialize()
        {
            BoardSize = new Tuple<int, int>(DEFAULT_BOARD_SIZE_ROWS, DEFAULT_BOARD_SIZE_COLUMNS);
            CellModifiers = new List<CellModifier>();

            CellModifiers.Add(new LadderCell(new Cell(0, 2), new Cell(1, 4)));
            CellModifiers.Add(new LadderCell(new Cell(5, 1), new Cell(6, 1)));
            CellModifiers.Add(new LadderCell(new Cell(2, 1), new Cell(3, 2)));
            CellModifiers.Add(new LadderCell(new Cell(3, 6), new Cell(4, 4)));
            CellModifiers.Add(new LadderCell(new Cell(2, 5), new Cell(3, 5)));
            CellModifiers.Add(new SnakeCell(new Cell(6, 3), new Cell(4, 1)));
            CellModifiers.Add(new SnakeCell(new Cell(1, 2), new Cell(0, 5)));
            CellModifiers.Add(new SnakeCell(new Cell(4, 6), new Cell(3, 2)));
            CellModifiers.Add(new SnakeCell(new Cell(5, 5), new Cell(3, 0)));
        }

        public CellModifier GetCellModifier(Cell cell)
        {
            return CellModifiers.Find(modifier => modifier.StartCell == cell);
        }
    }
}
