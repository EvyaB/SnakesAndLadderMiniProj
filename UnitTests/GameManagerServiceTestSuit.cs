using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.EntityFrameworkCore;
using SnakesAndLadderEvyatar.GameLogic;
using SnakesAndLadderEvyatar.Models;
using SnakesAndLadderEvyatar.Repositories;
using Xunit;

namespace UnitTests
{
    public class GameManagerServiceTestSuit
    {
        private GameManagerService _gameManagerService;

        private Mock<IGameboardRepository> _gameboardRepositoryMock;
        private CellModifier _ladderCell;
        private CellModifier _snakeCell;
        private CellModifier _noCellModifier;
        private Mock<IServiceScopeFactory> _scopeFactoryMock;
        private Mock<IDice> _diceMock;
        private readonly int _boardSize = 10;

        public GameManagerServiceTestSuit()
        {
            // Setup the board
            _gameboardRepositoryMock = new Mock<IGameboardRepository>();
            _ladderCell = new LadderCell(new Cell(2, 6), new Cell(4, 4));
            _snakeCell = new SnakeCell(new Cell(6, 3), new Cell(4, 1));
            _gameboardRepositoryMock.Setup(x => x.GetBoardRowsCount()).Returns(_boardSize);
            _gameboardRepositoryMock.Setup(x => x.GetBoardColumnsCount()).Returns(_boardSize);
            _gameboardRepositoryMock.Setup(x => x.GetFinalCell()).Returns(new Cell(_boardSize, _boardSize));
            _gameboardRepositoryMock.Setup(x => x.GetCellModifier(It.IsAny<Cell>(), out _noCellModifier)).Returns(false);
            _gameboardRepositoryMock.Setup(x => x.GetCellModifier(_snakeCell.StartCell, out _snakeCell)).Returns(true);
            _gameboardRepositoryMock.Setup(x => x.GetCellModifier(_ladderCell.StartCell, out _ladderCell)).Returns(true);
            
            _scopeFactoryMock = new Mock<IServiceScopeFactory>();
            _diceMock = new Mock<IDice>();

            _gameManagerService = new GameManagerService(_gameboardRepositoryMock.Object, _scopeFactoryMock.Object, _diceMock.Object);
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(1, 3, 4)]
        [InlineData(1, 6, 7)]
        [InlineData(4, 6, 10)]
        [InlineData(4, 0, 4)]
        public void MovePerDiceNoBoundariesEvenRowTest(int initialColumn, int diceRoll, int expectedColumn)
        {
            // Setup
            int initialTurnNum = 1;
            int playerRow = 0;
            _diceMock.Setup(d => d.DiceRoll()).Returns(diceRoll);
            Game gameToTest = new Game()
            {
                CurrentGameState = Game.GameState.Playing, 
                TurnNumber = initialTurnNum,
                PlayerPosition = new Cell(playerRow, initialColumn)
            };

            // Execute
            _gameManagerService.PlayTurnInGame(gameToTest);

            // Verify
            Assert.Equal(initialTurnNum + 1, gameToTest.TurnNumber);
            Assert.Equal(Game.GameState.Playing, gameToTest.CurrentGameState);
            Assert.Equal(expectedColumn, gameToTest.PlayerPosition.Column);
            Assert.Equal(playerRow, gameToTest.PlayerPosition.Row);
        }
        [Theory]
        [InlineData(3, 1, 2)]
        [InlineData(4, 4, 0)]
        [InlineData(7, 6, 1)]
        [InlineData(7, 0, 7)]
        public void MovePerDiceNoBoundariesOddRowTest(int initialColumn, int diceRoll, int expectedColumn)
        {
            // Setup
            int initialTurnNum = 1;
            int playerRow = 1;
            _diceMock.Setup(d => d.DiceRoll()).Returns(diceRoll);
            Game gameToTest = new Game()
            {
                CurrentGameState = Game.GameState.Playing,
                TurnNumber = initialTurnNum,
                PlayerPosition = new Cell(playerRow, initialColumn)
            };

            // Execute
            _gameManagerService.PlayTurnInGame(gameToTest);

            // Verify
            Assert.Equal(initialTurnNum + 1, gameToTest.TurnNumber);
            Assert.Equal(Game.GameState.Playing, gameToTest.CurrentGameState);
            Assert.Equal(expectedColumn, gameToTest.PlayerPosition.Column);
            Assert.Equal(playerRow, gameToTest.PlayerPosition.Row);
        }

        [Theory]
        [InlineData(10, 1, 10)]
        [InlineData(8, 4, 9)]
        [InlineData(9, 5, 7)]
        public void MovePerDiceAtRightBoundaryTest(int initialColumn, int diceRoll, int expectedColumn)
        {
            // Setup
            int initialTurnNum = 1;
            int playerRow = 0;
            _diceMock.Setup(d => d.DiceRoll()).Returns(diceRoll);
            Game gameToTest = new Game()
            {
                CurrentGameState = Game.GameState.Playing,
                TurnNumber = initialTurnNum,
                PlayerPosition = new Cell(playerRow, initialColumn)
            };

            // Execute
            _gameManagerService.PlayTurnInGame(gameToTest);

            // Verify
            Assert.Equal(initialTurnNum + 1, gameToTest.TurnNumber);
            Assert.Equal(Game.GameState.Playing, gameToTest.CurrentGameState);
            Assert.Equal(expectedColumn, gameToTest.PlayerPosition.Column);
            Assert.Equal(playerRow + 1, gameToTest.PlayerPosition.Row);
        }
        [Theory]
        [InlineData(0, 1, 0)]
        [InlineData(1, 4, 2)]
        public void MovePerDiceAtLeftBoundaryTest(int initialColumn, int diceRoll, int expectedColumn)
        {
            // Setup
            int initialTurnNum = 1;
            int playerRow = 1;
            _diceMock.Setup(d => d.DiceRoll()).Returns(diceRoll);
            Game gameToTest = new Game()
            {
                CurrentGameState = Game.GameState.Playing,
                TurnNumber = initialTurnNum,
                PlayerPosition = new Cell(playerRow, initialColumn)
            };

            // Execute
            _gameManagerService.PlayTurnInGame(gameToTest);

            // Verify
            Assert.Equal(initialTurnNum + 1, gameToTest.TurnNumber);
            Assert.Equal(Game.GameState.Playing, gameToTest.CurrentGameState);
            Assert.Equal(expectedColumn, gameToTest.PlayerPosition.Column);
            Assert.Equal(playerRow + 1, gameToTest.PlayerPosition.Row);
        }

        [Fact]
        public void MovePerDiceToFinalCell()
        {
            int initialTurnNum = 10;
            _diceMock.Setup(d => d.DiceRoll()).Returns(1);
            Game gameToTest = new Game()
            {
                CurrentGameState = Game.GameState.Playing,
                TurnNumber = initialTurnNum,
                PlayerPosition = new Cell(_boardSize, _boardSize - 1)
            };

            // Execute
            _gameManagerService.PlayTurnInGame(gameToTest);

            // Verify
            Assert.Equal(initialTurnNum + 1, gameToTest.TurnNumber);
            Assert.Equal(Game.GameState.Finished, gameToTest.CurrentGameState);
            Assert.Equal(_boardSize, gameToTest.PlayerPosition.Column);
            Assert.Equal(_boardSize, gameToTest.PlayerPosition.Row);
        }

        [Theory]
        [InlineData(-2, 4, 0, 4)]
        [InlineData(2, -4, 2, 0)]
        [InlineData(-2, -4, 0, 0)]
        public void ClampToGameBoardNegativeValuesTest(int initialRow, int initialColumn, int expectedRow, int expectedColumn)
        {
            int initialTurnNum = 10;
            _diceMock.Setup(d => d.DiceRoll()).Returns(0);
            Game gameToTest = new Game()
            {
                CurrentGameState = Game.GameState.Playing,
                TurnNumber = initialTurnNum,
                PlayerPosition = new Cell(initialRow, initialColumn)
            };
            
            // Execute
            _gameManagerService.PlayTurnInGame(gameToTest);

            // Verify
            Assert.Equal(initialTurnNum + 1, gameToTest.TurnNumber);
            Assert.Equal(Game.GameState.Playing, gameToTest.CurrentGameState);
            Assert.Equal(expectedColumn, gameToTest.PlayerPosition.Column);
            Assert.Equal(expectedRow, gameToTest.PlayerPosition.Row);
        }

        [Theory]
        [InlineData(10, 9, 2)]
        [InlineData(10, 5, 6)]
        [InlineData(10, 9, 6)]
        public void ClampToGameBoardFinalCellTest(int initialRow, int initialColumn, int diceRoll)
        {
            int initialTurnNum = 10;
            _diceMock.Setup(d => d.DiceRoll()).Returns(diceRoll);
            Game gameToTest = new Game()
            {
                CurrentGameState = Game.GameState.Playing,
                TurnNumber = initialTurnNum,
                PlayerPosition = new Cell(initialRow, initialColumn)
            };

            // Execute
            _gameManagerService.PlayTurnInGame(gameToTest);

            // Verify
            Assert.Equal(initialTurnNum + 1, gameToTest.TurnNumber);
            Assert.Equal(Game.GameState.Finished, gameToTest.CurrentGameState);
            Assert.Equal(_boardSize, gameToTest.PlayerPosition.Column);
            Assert.Equal(_boardSize, gameToTest.PlayerPosition.Row);
        }

        [Fact]
        public void MovePlayerPerCellModifierLadder()
        {
            int initialTurnNum = 10;
            _diceMock.Setup(d => d.DiceRoll()).Returns(1);
            Game gameToTest = new Game()
            {
                CurrentGameState = Game.GameState.Playing,
                TurnNumber = initialTurnNum,
                PlayerPosition = new Cell(_ladderCell.StartCell.Row, _ladderCell.StartCell.Column - 1)
            };

            // Execute
            _gameManagerService.PlayTurnInGame(gameToTest);

            // Verify
            Assert.Equal(initialTurnNum + 1, gameToTest.TurnNumber);
            Assert.Equal(Game.GameState.Playing, gameToTest.CurrentGameState);
            Assert.Equal(_ladderCell.TargetCell.Column, gameToTest.PlayerPosition.Column);
            Assert.Equal(_ladderCell.TargetCell.Row, gameToTest.PlayerPosition.Row);
        }

        [Fact]
        public void MovePlayerPerCellModifierSnake()
        {
            int initialTurnNum = 10;
            _diceMock.Setup(d => d.DiceRoll()).Returns(1);
            Game gameToTest = new Game()
            {
                CurrentGameState = Game.GameState.Playing,
                TurnNumber = initialTurnNum,
                PlayerPosition = new Cell(_snakeCell.StartCell.Row, _snakeCell.StartCell.Column - 1)
            };

            // Execute
            _gameManagerService.PlayTurnInGame(gameToTest);

            // Verify
            Assert.Equal(initialTurnNum + 1, gameToTest.TurnNumber);
            Assert.Equal(Game.GameState.Playing, gameToTest.CurrentGameState);
            Assert.Equal(_snakeCell.TargetCell.Column, gameToTest.PlayerPosition.Column);
            Assert.Equal(_snakeCell.TargetCell.Row, gameToTest.PlayerPosition.Row);
        }
    }
}
