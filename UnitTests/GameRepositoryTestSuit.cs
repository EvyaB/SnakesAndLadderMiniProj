using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using SnakesAndLadderEvyatar.DTO.Game;
using SnakesAndLadderEvyatar.DTO.Player;
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
                new Game() {Id = 18, TurnNumber = 14, CurrentGameState = Game.GameState.Unrecognized, PlayerId = 3, Player = new Player() {Id=3, PlayerName = "Glitch"}},
                new Game() {Id = 3, TurnNumber = 12, CurrentGameState = Game.GameState.Finished, PlayerId = 6, Player = new Player() {Id=6, PlayerName = "Don"}},
                new Game() {Id = 7, TurnNumber = 14, CurrentGameState = Game.GameState.Playing, PlayerId = 4, Player = new Player() {Id=2, PlayerName = "Adi"}},
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

        [Theory]
        [InlineData(4, "Adi", 2)]
        [InlineData(6, "Don", 2)]
        [InlineData(11, "Eden", 1)]
        [InlineData(10, null, 0)]
        [InlineData(0, null, 0)]
        [InlineData(-5, null, 0)]
        public async void GetAllGamesByPlayer(int playerId, string expectedPlayerName, int expectedNumOfGames)
        {
            List<GetGameDto> allGames = await _gameRepository.GetAllGames(playerId);
            Assert.Equal(expectedNumOfGames, allGames.Count);

            foreach (var game in allGames)
            {
                Assert.Equal(expectedPlayerName, game.PlayerName);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(6)]
        [InlineData(2)]
        [InlineData(18)]
        [InlineData(7)]
        [InlineData(5)]
        public async void GetGameSuccessTest(int gameId)
        {
            GetGameDto game = await _gameRepository.GetGame(gameId);
            Assert.NotNull(game);
            Assert.Equal(gameId, game.Id);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(-4)]
        [InlineData(0)]
        [InlineData(100)]
        public async void GetGameNotExistingTest(int gameId)
        {
            GetGameDto game = await _gameRepository.GetGame(gameId);
            Assert.Null(game);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(10)]
        public async void CreateGameSuccessTest(int gamePlayerId)
        {
            // Setup
            Player player = new Player() {Id = gamePlayerId, Games = new List<Game>(), PlayerName = "Wally"};
            GetPlayerDto playerDto = new GetPlayerDto(player);
            _playerRepoMock.Setup(x => x.GetPlayer(gamePlayerId)).ReturnsAsync(playerDto);
            AddGameDto newGame = new AddGameDto() {PlayerId = gamePlayerId};

            // Create the game
            var result = await _gameRepository.CreateGame(newGame);

            // Check the result
            _dataContextMock.Verify(x => x.AddAsync(It.Is<Game>(g => g.PlayerId == player.Id), It.IsAny<CancellationToken>()), Times.Once);
            _dataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(Game.GameState.Playing, result.GameState);
            Assert.Equal(0, result.PlayerPosition.Column);
            Assert.Equal(0, result.PlayerPosition.Row);
            Assert.Equal(0, result.TurnNumber);
        }

        [Fact]
        public async void CreateGameFailTest()
        {
            // Setup
            _playerRepoMock.Setup(x => x.GetPlayer(It.IsAny<int>())).ReturnsAsync(() => null);

            // Attempt to create the game with invalid player data
            var result = await _gameRepository.CreateGame(new AddGameDto());

            // Check the result
            _dataContextMock.Verify(x => x.AddAsync(It.IsAny<Game>(), It.IsAny<CancellationToken>()), Times.Never);
            _dataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            Assert.Null(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(18)]
        [InlineData(5)]
        public async void DeleteGameByIdSuccessTest(int gameId)
        {
            // Setup
            Assert.NotNull(_gamesInfo.FirstOrDefault(g => gameId == g.Id));
            // The find method has to be mocked anyway despite _dataContextMock.Players already returning the valid local list of players. Not sure why.
            _dataContextMock.Setup(x => x.Games.FindAsync(gameId)).ReturnsAsync(_gamesInfo.FirstOrDefault(g => gameId == g.Id));

            // Execute
            var result = await _gameRepository.DeleteGame(gameId);

            // Check the results
            _dataContextMock.Verify(x => x.Games.Remove(It.IsAny<Game>()), Times.Once);
            _dataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(18)]
        [InlineData(5)]
        public async void DeleteGameByGameObjectSuccessTest(int gameId)
        {
            // Setup
            Assert.NotNull(_gamesInfo.FirstOrDefault(g => gameId == g.Id));
            Game game = _gamesInfo.FirstOrDefault(g => gameId == g.Id);
            // The find method has to be mocked anyway despite _dataContextMock.Players already returning the valid local list of players. Not sure why.
            _dataContextMock.Setup(x => x.Games.FindAsync(gameId)).ReturnsAsync(game);

            // Execute
            var result = await _gameRepository.DeleteGame(game);

            // Check the results
            _dataContextMock.Verify(x => x.Games.Remove(It.IsAny<Game>()), Times.Once);
            _dataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async void DeleteGameByIdFailTest()
        {
            // The find method has to be mocked anyway despite _dataContextMock.Players already returning the valid local list of players. Not sure why.
            _dataContextMock.Setup(x => x.Games.FindAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            // Execute
            var result = await _gameRepository.DeleteGame(0);

            // Check the results
            _dataContextMock.Verify(x => x.Games.Remove(It.IsAny<Game>()), Times.Never);
            _dataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            Assert.False(result);
        }

        [Fact]
        public async void DeleteGameByGameObjectFailDueNullTest()
        {
            // Execute
            var result = await _gameRepository.DeleteGame(null);

            // Check the results
            _dataContextMock.Verify(x => x.Games.Remove(It.IsAny<Game>()), Times.Never);
            _dataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            Assert.False(result);
        }

        [Fact]
        public async void DeleteGameByGameObjectFailDueUnknownIdTest()
        {
            // The find method has to be mocked anyway despite _dataContextMock.Players already returning the valid local list of players. Not sure why.
            _dataContextMock.Setup(x => x.Games.FindAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            // Execute
            var result = await _gameRepository.DeleteGame(new Game());

            // Check the results
            _dataContextMock.Verify(x => x.Games.Remove(It.IsAny<Game>()), Times.Never);
            _dataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            Assert.False(result);
        }

        [Theory]
        [InlineData(0, Game.GameState.Playing)]
        [InlineData(0, Game.GameState.Finished)]
        [InlineData(0, Game.GameState.Unrecognized)]
        [InlineData(3, Game.GameState.Playing)]
        [InlineData(6, Game.GameState.Finished)]
        public async void EditGame_EditGameStateTest(int indexInGameInfo, Game.GameState newState)
        {
            Game gameToEdit = _gamesInfo[indexInGameInfo];
            EditGameDto editedGame = new EditGameDto(gameToEdit);
            editedGame.CurrentGameState = newState;
            // The find method has to be mocked anyway despite _dataContextMock.Players already returning the valid local list of players. Not sure why.
            _dataContextMock.Setup(x => x.Games.FindAsync(gameToEdit.Id)).ReturnsAsync(gameToEdit);

            GetGameDto result = await _gameRepository.EditGame(editedGame);

            _dataContextMock.Verify(x => x.Games.Update(It.Is<Game>(g => g.Id == editedGame.Id)), Times.Once);
            _dataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(newState, result.GameState);
        }

        [Theory]
        [InlineData(0, 26)]
        [InlineData(3, 3)]
        [InlineData(6, 11)]
        public async void EditGame_EditTurnNumberTest(int indexInGameInfo, int newTurnNumber)
        {
            Game gameToEdit = _gamesInfo[indexInGameInfo];
            EditGameDto editedGame = new EditGameDto(gameToEdit);
            editedGame.TurnNumber = newTurnNumber;
            // The find method has to be mocked anyway despite _dataContextMock.Players already returning the valid local list of players. Not sure why.
            _dataContextMock.Setup(x => x.Games.FindAsync(gameToEdit.Id)).ReturnsAsync(gameToEdit);

            GetGameDto result = await _gameRepository.EditGame(editedGame);

            _dataContextMock.Verify(x => x.Games.Update(It.Is<Game>(g => g.Id == editedGame.Id)), Times.Once);
            _dataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(newTurnNumber, result.TurnNumber);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(6)]
        public async void EditGame_EditStartDateTimeTest(int indexInGameInfo)
        {
            Game gameToEdit = _gamesInfo[indexInGameInfo];
            EditGameDto editedGame = new EditGameDto(gameToEdit);
            editedGame.StartDateTime = DateTime.Now;
            // The find method has to be mocked anyway despite _dataContextMock.Players already returning the valid local list of players. Not sure why.
            _dataContextMock.Setup(x => x.Games.FindAsync(gameToEdit.Id)).ReturnsAsync(gameToEdit);

            GetGameDto result = await _gameRepository.EditGame(editedGame);

            _dataContextMock.Verify(x => x.Games.Update(It.Is<Game>(g => g.Id == editedGame.Id)), Times.Once);
            _dataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(editedGame.StartDateTime, result.StartDateTime);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(6)]
        public async void EditGame_EditEndDateTimeTest(int indexInGameInfo)
        {
            Game gameToEdit = _gamesInfo[indexInGameInfo];
            EditGameDto editedGame = new EditGameDto(gameToEdit);
            editedGame.EndDateTime = DateTime.Now;
            // The find method has to be mocked anyway despite _dataContextMock.Players already returning the valid local list of players. Not sure why.
            _dataContextMock.Setup(x => x.Games.FindAsync(gameToEdit.Id)).ReturnsAsync(gameToEdit);

            GetGameDto result = await _gameRepository.EditGame(editedGame);

            _dataContextMock.Verify(x => x.Games.Update(It.Is<Game>(g => g.Id == editedGame.Id)), Times.Once);
            _dataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(editedGame.EndDateTime, result.EndDateTime);
        }

        [Theory]
        [InlineData(0, 3, 3)]
        [InlineData(3, 6, 6)]
        [InlineData(6, 0, 0)]
        public async void EditGame_EditPlayerPositionTest(int indexInGameInfo, int newRowPos, int newColumnPos)
        {
            Game gameToEdit = _gamesInfo[indexInGameInfo];
            EditGameDto editedGame = new EditGameDto(gameToEdit);
            editedGame.PlayerPosition = new Cell(newRowPos, newColumnPos);
            // The find method has to be mocked anyway despite _dataContextMock.Players already returning the valid local list of players. Not sure why.
            _dataContextMock.Setup(x => x.Games.FindAsync(gameToEdit.Id)).ReturnsAsync(gameToEdit);

            GetGameDto result = await _gameRepository.EditGame(editedGame);

            _dataContextMock.Verify(x => x.Games.Update(It.Is<Game>(g => g.Id == editedGame.Id)), Times.Once);
            _dataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(new Cell(newRowPos, newColumnPos), result.PlayerPosition);
        }

        [Fact]
        public async void EditGameFailDueInvalidIdTest()
        {
            EditGameDto editedGame = new EditGameDto(0);
            // The find method has to be mocked anyway despite _dataContextMock.Players already returning the valid local list of players. Not sure why.
            _dataContextMock.Setup(x => x.Games.FindAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            GetGameDto result = await _gameRepository.EditGame(editedGame);

            _dataContextMock.Verify(x => x.Games.Update(It.IsAny<Game>()), Times.Never);
            _dataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            Assert.Null(result);
        }
        [Fact]
        public async void EditGameFailDueNullTest()
        {
            GetGameDto result = await _gameRepository.EditGame(null);

            _dataContextMock.Verify(x => x.Games.Update(It.IsAny<Game>()), Times.Never);
            _dataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            Assert.Null(result);
        }
    }
}
