using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using Moq.EntityFrameworkCore;
using SnakesAndLadderEvyatar.Repositories;
using SnakesAndLadderEvyatar.Models;

namespace UnitTests
{
    public class ScoreboardRepositoryTestSuit
    {
        private readonly ScoreboardRepository _scoreboardRepository;
        private Mock<DataContext> _dataContextMock;
        private IList<Game> _gamesDb;

        private Player _bestPlayer;
        private Game _bestGame;

        public ScoreboardRepositoryTestSuit()
        {
            _bestPlayer = new Player() { Id = 1, PlayerName = "Danny" };
            _bestGame = new Game() { Id = 4, TurnNumber = 5, Player = _bestPlayer, CurrentGameState = Game.GameState.Finished };

            _gamesDb = new List<Game>
            {
                new Game() {Id = 1, TurnNumber = 15, CurrentGameState = Game.GameState.Finished, PlayerId = 4},
                new Game() {Id = 2, TurnNumber = 10, CurrentGameState = Game.GameState.Finished, PlayerId = 6},
                new Game() {Id = 3, TurnNumber = 12, CurrentGameState = Game.GameState.Finished, PlayerId = 6},
                new Game() {Id = 5, TurnNumber = 3, CurrentGameState = Game.GameState.Playing, PlayerId = 2},
                _bestGame
            };
            
            _dataContextMock = new Mock<DataContext>();
            _dataContextMock.Setup(c => c.Games).ReturnsDbSet(_gamesDb);
            _scoreboardRepository = new ScoreboardRepository(_dataContextMock.Object);
        }

        [Fact]
        public async void GetBestGameTest()
        {
            var bestGame = await _scoreboardRepository.GetBestGame();
            Assert.NotNull(bestGame);
            Assert.Equal(bestGame, _bestGame);
        }
        
        [Fact]
        public async void GetBestPlayerTest()
        {
            var bestPlayer = await _scoreboardRepository.GetBestPlayer();
            Assert.NotNull(bestPlayer);
            Assert.Equal(bestPlayer, _bestPlayer);
        }

        [Fact]
        public async void GetBestPlayerEmptyTest()
        {
            _dataContextMock.Setup(c => c.Games).ReturnsDbSet(new List<Game>());
            var bestPlayer = await _scoreboardRepository.GetBestPlayer();
            Assert.Null(bestPlayer);
        }

        [Fact]
        public async void GetBestGameEmptyTest()
        {
            _dataContextMock.Setup(c => c.Games).ReturnsDbSet(new List<Game>());
            var bestGame = await _scoreboardRepository.GetBestGame();
            Assert.Null(bestGame);
        }

        [Fact]
        public async void IsBestPlayerTest()
        {
            Assert.True(await _scoreboardRepository.IsBestPlayer(_bestPlayer));
            Assert.False(await _scoreboardRepository.IsBestPlayer(null));
            Assert.False(await _scoreboardRepository.IsBestPlayer(new Player()));
            Assert.False(await _scoreboardRepository.IsBestPlayer(new Player() {Id = 2}));
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(0, false)]
        [InlineData(4, false)]
        [InlineData(6, false)]
        [InlineData(-2, false)]
        public async void IsBestPlayerIdTest(int playerId, bool result)
        {
            Assert.Equal(result, await _scoreboardRepository.IsBestPlayer(playerId));
        }


        [Fact]
        public async void IsBestGameTest()
        {
            Assert.True(await _scoreboardRepository.IsBestGame(_bestGame));
            Assert.False(await _scoreboardRepository.IsBestGame(null));
            Assert.False(await _scoreboardRepository.IsBestGame(new Game()));
            Assert.False(await _scoreboardRepository.IsBestGame(new Game() { Id = 2 }));
        }

        [Theory]
        [InlineData(4, true)]
        [InlineData(0, false)]
        [InlineData(1, false)]
        [InlineData(6, false)]
        [InlineData(-2, false)]
        public async void IsBestGameIdTest(int gameId, bool result)
        {
            Assert.Equal(result, await _scoreboardRepository.IsBestGame(gameId));
        }
    }
}
