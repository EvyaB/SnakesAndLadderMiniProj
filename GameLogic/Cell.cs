using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace SnakesAndLadderEvyatar.GameLogic
{
    public class Cell : IComparable<Cell>
    {
        public int Column { get; set; }
        public int Row { get; set; }

        public Cell(int row, int column) { Row = row; Column = column; }
        public Cell(Tuple<int, int> tuple) { Row = tuple.Item1; Column = tuple.Item2; }
        public static implicit operator Tuple<int, int>(Cell cell) => new Tuple<int, int>(cell.Row, cell.Column);
        public static implicit operator Cell(Tuple<int, int> tuple) => new Cell(tuple);

        public static bool operator==(Cell c1, Cell c2)
        {
            return (c1.Row == c2.Row && c1.Column == c2.Column);
        }
        public static bool operator !=(Cell c1, Cell c2)
        {
            return (c1.Row != c2.Row || c1.Column != c2.Column);
        }
        public override bool Equals(object obj)
        {
            return obj is Cell cell &&
                   Column == cell.Column &&
                   Row == cell.Row;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Column, Row);
        }
        public int CompareTo(Cell other)
        {
            int result = this.Row.CompareTo(other.Row);
            if (result == 0)
            {
                result = this.Column.CompareTo(other.Column);
            }

            return result;
        }
    }
}
