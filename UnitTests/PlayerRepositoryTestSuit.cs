using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private Mock<DataContext> _dataContextMock;
        private IList<Player> _playerInfo;

        public PlayerRepositoryTestSuit()
        {
            _playerInfo = new List<Player>
            {
                new Player() {Id = 1, PlayerName = "Shani", Games = new List<Game>() { new Game() {Id = 1}}},
                new Player() {Id = 5, PlayerName = "Ron", Games = new List<Game>() { new Game() {Id = 2}, new Game() {Id=3}}},
                new Player() {Id = 10, PlayerName = "Sagiv", Games = new List<Game>()}
            };

            _dataContextMock = new Mock<DataContext>();
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
    }
}
