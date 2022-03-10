using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using SnakesAndLadderEvyatar.DTO.Player;
using SnakesAndLadderEvyatar.Models;
using SnakesAndLadderEvyatar.Repositories;
using Xunit;

namespace UnitTests
{
    public class PlayerRepositoryTestSuit
    {
        private readonly PlayerRepository _playerRepository;

        private readonly Mock<DataContext> _dataContextMock;
        private readonly IList<Player> _playerInfo;

        public PlayerRepositoryTestSuit()
        {
            _playerInfo = new List<Player>
            {
                new Player() {Id = 1, PlayerName = "Shani", Games = new List<Game>() { new Game() {Id = 1}}},
                new Player() {Id = 5, PlayerName = "Ron", Games = new List<Game>() { new Game() {Id = 2}, new Game() {Id=3}}},
                new Player() {Id = 10, PlayerName = "Sagiv", Games = new List<Game>()}
            };

            _dataContextMock = new Mock<DataContext>(new DbContextOptions<DataContext>());
            _dataContextMock.Setup(c => c.Players).ReturnsDbSet(_playerInfo);
            _playerRepository = new PlayerRepository(_dataContextMock.Object);
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(5, 1)]
        [InlineData(10, 2)]
        [InlineData(0, null)]
        [InlineData(4, null)]
        public async void GetPlayerByIdTest(int playerId, int? indexInPlayerList)
        {
            var playerInfo = await _playerRepository.GetPlayer(playerId);
            
            if (indexInPlayerList == null)
            {
                // Expecting to not find anyone
                Assert.Null(playerInfo);
            }
            else
            {
                // Expecting to find specific player
                Player expectedPlayer = _playerInfo[indexInPlayerList.Value];
                Assert.Equal(expectedPlayer.Id, playerInfo.Id);
                Assert.Equal(expectedPlayer.PlayerName, playerInfo.Name);
                Assert.Equal(expectedPlayer.Games.Select(g => g.Id), playerInfo.Games.Select(g => g.Id));
            }
        }

        [Theory]
        [InlineData("Shani", 0)]
        [InlineData("Ron", 1)]
        [InlineData("Sagiv", 2)]
        [InlineData("Danny", null)]
        public async void GetPlayerByStringTest(string playerName, int? indexInPlayerList)
        {
            var playerInfo = await _playerRepository.GetPlayer(playerName);
            
            if (indexInPlayerList == null)
            {
                // Expecting to not find anyone
                Assert.Null(playerInfo);
            }
            else
            {
                // Expecting to find specific player
                Player expectedPlayer = _playerInfo[indexInPlayerList.Value];
                Assert.Equal(expectedPlayer.Id, playerInfo.Id);
                Assert.Equal(expectedPlayer.PlayerName, playerInfo.Name);
                Assert.Equal(expectedPlayer.Games.Select(g => g.Id), playerInfo.Games.Select(g => g.Id));
            }
        }

        [Fact]
        public async void GetAllPlayersTest()
        {
            var allPlayers = await _playerRepository.GetAllPlayers();
            Assert.Equal(_playerInfo.Count, allPlayers.Count());
            Assert.Equal(_playerInfo.Select(p => p.Id), allPlayers.Select(p => p.Id));
        }

        [Fact]
        public async void CreatePlayerTest()
        {
            CreatePlayerDto newPlayer = new CreatePlayerDto() {Name = "Aviv"};

            var result = await _playerRepository.CreatePlayer(newPlayer);

            _dataContextMock.Verify(x => x.Players.AddAsync(It.Is<Player>(p => p.PlayerName == "Aviv"), It.IsAny<CancellationToken>()), Times.Once);
            _dataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public async void DeletePlayerByIdSuccessTest(int playerIndex)
        {
            int playerId = _playerInfo[playerIndex].Id;

            // The find method has to be mocked anyway despite _dataContextMock.Players already returning the valid local list of players. Not sure why.
            _dataContextMock.Setup(x => x.Players.FindAsync(playerId)).ReturnsAsync(_playerInfo[playerIndex]);

            var result = await _playerRepository.DeletePlayer(playerId);

            _dataContextMock.Verify(x => x.Players.Remove(_playerInfo[playerIndex]), Times.Once);
            _dataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public async void DeletePlayerByStringSuccessTest(int playerIndex)
        {
            string playerName = _playerInfo[playerIndex].PlayerName;

            var result = await _playerRepository.DeletePlayer(playerName);

            _dataContextMock.Verify(x => x.Players.Remove(_playerInfo[playerIndex]), Times.Once);
            _dataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(result);
        }
    }
}
