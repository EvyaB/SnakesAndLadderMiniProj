using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Moq.EntityFrameworkCore;
using SnakesAndLadderEvyatar.Models;
using SnakesAndLadderEvyatar.Repositories;
using Xunit;

namespace UnitTests
{
    // TODO: Update these tests once the Gameboard is not hardcoded in the Gameboard class
    public class GameboardRepositoryTestSuit
    {
        private readonly GameboardRepository _gameboardRepository;
        public GameboardRepositoryTestSuit()
        {
            _gameboardRepository = new GameboardRepository();
        }

        [Fact]
        public void GetBoardColumnsCountTest()
        {
            // Currently the game board is hardcoded in the Gameboard class and is always the same size.
            Assert.Equal(6, _gameboardRepository.GetBoardColumnsCount());
        }
        [Fact]
        public void GetBoardRowsCountTest()
        {
            // Currently the game board is hardcoded in the Gameboard class and is always the same size.
            Assert.Equal(6, _gameboardRepository.GetBoardRowsCount());
        }

        [Fact]
        public void GetFinalCellTest()
        {
            Cell finalCell = _gameboardRepository.GetFinalCell();
            int rowsCount = _gameboardRepository.GetBoardRowsCount();
            int columnsCount = _gameboardRepository.GetBoardColumnsCount();

            Assert.Equal(rowsCount, finalCell.Row);

            // Final Cell's column depends on if the board has an Even or Odd number of rows.
            if (rowsCount % 2 == 0)
            {
                Assert.Equal(columnsCount, finalCell.Column);
            }
            else
            {
                Assert.Equal(0, finalCell.Column);
            }
        }

        [Theory]
        [InlineData(1, 1, false, null)]
        [InlineData(3, 5, false, null)]
        [InlineData(0, 0, false, null)]
        [InlineData(-1, 5, false, null)]
        [InlineData(1, -5, false, null)]
        [InlineData(-1, -5, false, null)]
        [InlineData(3, 6, true, typeof(LadderCell))]
        [InlineData(5, 5, true, typeof(SnakeCell))]
        public void GetCellModifierTest(int row, int column, bool expectingCellToHaveModifier, Type expectedModifierType)
        {
            Cell cell = new Cell(row, column);
            bool hasModifier = _gameboardRepository.GetCellModifier(cell, out var cellModifier);

            Assert.Equal(expectingCellToHaveModifier, hasModifier);

            if (expectingCellToHaveModifier)
            {
                Assert.IsType(expectedModifierType, cellModifier);
            }
            else
            {
                Assert.Null(cellModifier);
            }
        }
    }
}
