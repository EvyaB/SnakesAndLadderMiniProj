using SnakesAndLadderEvyatar.GameLogic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnakesAndLadderEvyatar.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private ConcurrentDictionary<string, Player> _players;
        private readonly IGameRepository _gameRepository;

        public PlayerRepository(IGameRepository gameRepository)
        {
            _players = new ConcurrentDictionary<string, Player>();
            _gameRepository = gameRepository;
        }

        public Player CreateAndStartGame(string name)
        {
            Player newPlayer = new Player()
            {
                PlayerName = name,
                PlayerGameState = Player.GameState.Ingame,
                TurnNumber = 0,
                CurrentCell = new Tuple<int, int>(0, 0)
            };

            // Add the player to the list of players
            _players[name] = newPlayer;

            return newPlayer;
        }

        public Tuple<Player, bool> Get(string name)
        {
            Tuple<Player, bool> result;
            Player playerData;
            bool foundPlayer = _players.TryGetValue(name, out playerData);

            if (foundPlayer)
            {
                result = new Tuple<Player, bool>(playerData, _gameRepository.GetBestPlayer() == playerData);
            }
            else
            {
                playerData = new Player() { PlayerName = name, PlayerGameState = Player.GameState.Unrecognized };
                result = new Tuple<Player, bool>(playerData, false);
            }

            return result;
        }

        public IEnumerable<Player> Get()
        {
            return _players.Values;
        }
        public Player GetBestPlayer()
        {
            return _gameRepository.GetBestPlayer();
        }
    }
}
