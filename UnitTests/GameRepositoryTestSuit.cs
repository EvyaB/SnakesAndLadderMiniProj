using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using SnakesAndLadderEvyatar.Models;
using SnakesAndLadderEvyatar.Repositories;
using Xunit;

namespace UnitTests
{
    public class GameRepositoryTestSuit
    {
        private readonly GameRepository _gameRepository;

        private readonly Mock<DataContext> _dataContextMock;
        private readonly Mock<IScoreboardRepository> _scoreboardRepoMock;
        private readonly Mock<IPlayerRepository> _playerRepoMock;
        private readonly IList<Game> _gamesInfo;

        public GameRepositoryTestSuit()
        {
            _gamesInfo = new List<Game>
            {
                new Game() {Id = 1, TurnNumber = 15, CurrentGameState = Game.GameState.Finished, PlayerId = 4, Player = new Player() {Id=4, PlayerName = "Adi"}},
                new Game() {Id = 6, TurnNumber = 21, CurrentGameState = Game.GameState.Playing, PlayerId = 11, Player = new Player() {Id=2, PlayerName = "Eden"}},
                new Game() {Id = 2, TurnNumber = 10, CurrentGameState = Game.GameState.Finished, PlayerId = 6, Player = new Player() {Id=6, PlayerName = "Don"}},
                new Game() {Id = 6, TurnNumber = 14, CurrentGameState = Game.GameState.Unrecognized, PlayerId = 3, Player = new Player() {Id=3, PlayerName = "Glitch"}},
                new Game() {Id = 3, TurnNumber = 12, CurrentGameState = Game.GameState.Finished, PlayerId = 6, Player = new Player() {Id=6, PlayerName = "Don"}},
                new Game() {Id = 6, TurnNumber = 14, CurrentGameState = Game.GameState.Playing, PlayerId = 4, Player = new Player() {Id=2, PlayerName = "Adi"}},
                new Game() {Id = 5, TurnNumber = 3, CurrentGameState = Game.GameState.Playing, PlayerId = 2, Player = new Player() {Id=2, PlayerName = "Ray"}}
            };

            _dataContextMock = new Mock<DataContext>(new DbContextOptions<DataContext>());
            _dataContextMock.Setup(c => c.Games).ReturnsDbSet(_gamesInfo);
            _scoreboardRepoMock = new Mock<IScoreboardRepository>();
            _playerRepoMock = new Mock<IPlayerRepository>();
            _gameRepository = new GameRepository(_dataContextMock.Object, _scoreboardRepoMock.Object, _playerRepoMock.Object);
        }

        [Fact]
        public async void GetAllGamesTest()
        {
            var allGamesResult = await _gameRepository.GetAllGames();
            Assert.Equal(_gamesInfo.Count, allGamesResult.Count());
            Assert.Equal(_gamesInfo.Select(p => p.Id), allGamesResult.Select(p => p.Id));
        }

        [Fact]
        public async void GetAllActiveGamesTest()
        {
            var allGamesResult = await _gameRepository.GetAllActiveGames();
            var actualActiveGames = _gamesInfo.Where(g => g.CurrentGameState == Game.GameState.Playing).ToList();
            Assert.Equal(actualActiveGames.Count, allGamesResult.Count());
            Assert.Equal(actualActiveGames.Select(g => g.Id), allGamesResult.Select(g => g.Id));
        }
    }
}
