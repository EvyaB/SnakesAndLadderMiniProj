using System;
using System.Collections.Generic;
using System.Text;
using SnakesAndLadderEvyatar.Models;
using Xunit;

namespace UnitTests
{
    public class CellTestSuit
    {
        [Theory]
        [InlineData(1, 1, 1, 1, true)]
        [InlineData(0, 0, 0, 0, true)]
        [InlineData(15, 15, 15, 15, true)]
        [InlineData(1, 1, 1, 2, false)]
        [InlineData(1, 1, 2, 1, false)]
        [InlineData(1, 1, 2, 2, false)]
        [InlineData(1, 2, 1, 1, false)]
        [InlineData(2, 1, 1, 1, false)]
        [InlineData(2, 2, 1, 1, false)]
        public void CellCompareEqualTest(int row1, int column1, int row2, int column2, bool result)
        {
            Cell c1 = new Cell(row1, column1);
            Cell c2 = new Cell(row2, column2);
            
            Assert.Equal(result, c1 == c2);
            Assert.Equal(result, c1.Equals(c2));
        }

        [Theory]
        [InlineData(1, 1, 1, 1, false)]
        [InlineData(0, 0, 0, 0, false)]
        [InlineData(15, 15, 15, 15, false)]
        [InlineData(1, 1, 1, 2, true)]
        [InlineData(1, 1, 2, 1, true)]
        [InlineData(1, 1, 2, 2, true)]
        [InlineData(1, 2, 1, 1, true)]
        [InlineData(2, 1, 1, 1, true)]
        [InlineData(2, 2, 1, 1, true)]
        public void CellCompareUnequalsTest(int row1, int column1, int row2, int column2, bool result)
        {
            Cell c1 = new Cell(row1, column1);
            Cell c2 = new Cell(row2, column2);

            Assert.Equal(result, c1 != c2);
        }

        [Theory]
        [InlineData(1, 1, 1, 1, 0)]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(15, 15, 15, 15, 0)]
        [InlineData(1, 1, 1, 2, -1)]
        [InlineData(1, 1, 2, 1, -1)]
        [InlineData(1, 1, 2, 2, -1)]
        [InlineData(2, 5, 3, 1, -1)]
        [InlineData(1, 2, 1, 1, 1)]
        [InlineData(2, 1, 1, 1, 1)]
        [InlineData(2, 2, 1, 1, 1)]
        [InlineData(3, 2, 2, 5, 1)]
        public void CellCompareToTest(int row1, int column1, int row2, int column2, int result)
        {
            Cell c1 = new Cell(row1, column1);
            Cell c2 = new Cell(row2, column2);
            
            Assert.Equal(result, c1.CompareTo(c2));
        }
    }
}
