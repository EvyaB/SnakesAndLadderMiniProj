using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace SnakesAndLadderEvyatar.GameLogic
{
    public class CellModifier : IComparable<CellModifier>
    {
        public Cell StartCell { get; set; }
        public Cell TargetCell { get; set; }

        public CellModifier(Cell start, Cell target)
        {
            StartCell = start;
            TargetCell = target;
        }

        public int CompareTo([AllowNull] CellModifier other)
        {
            return StartCell.CompareTo(other.StartCell);
        }
    }

    public class LadderCell : CellModifier
    {
        // No special behavior for now. exists only to allow differentating between ladders and snakes in the future (for example in a UI)
        public LadderCell(Cell start, Cell target) : base(start, target)
        {
        }
    }
    public class SnakeCell : CellModifier
    {
        // No special behavior for now. exists only to allow differentating between ladders and snakes in the future (for example in a UI)
        public SnakeCell(Cell start, Cell target) : base(start, target)
        {
        }
    }
}
