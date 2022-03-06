using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SnakesAndLadderEvyatar.Data;

namespace SnakesAndLadderEvyatar.GameLogic
{
    public class GameManagerService : BackgroundService
    {
        private static int TURNS_TIMER_MILLISECONDS = 3000;
        private Repositories.IGameRepository _gameRepository;
        private Repositories.IPlayerRepository _playerRepository;

        public GameManagerService(Repositories.IGameRepository gameRepository, Repositories.IPlayerRepository playerRepository)
        {
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await RunGame(stoppingToken);
        }

        private async Task RunGame(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Go over every 'ingame' (actively playing) player and run his/her turn
                foreach (Player currentPlayer in _playerRepository.GetAllPlayers().Where(player => player.PlayerGameState == Player.GameState.Playing))
                {
                    PlayPlayerTurn(currentPlayer);
                }

                await Task.Delay(TURNS_TIMER_MILLISECONDS, stoppingToken);
            }
        }

        private void PlayPlayerTurn(Player player)
        {
            // Update player's turn counter
            player.TurnNumber++;

            MovePlayer(player);

            // Check if reached final cell (=won game)
            if (player.CurrentCell == _gameRepository.GetFinalCell())
            {
                player.PlayerGameState = Player.GameState.Finished;
                _gameRepository.ReportPlayerScore(player);
            }
        }

        // Move player according to dice roll, snakes/ladders modiferes and the boundaries of the board
        private void MovePlayer(Player player)
        {
            MovePlayerPerDiceRoll(player);
            MovePlayerPerCellEffect(player);
            ClampToBoardCorners(player);
        }

        private void MovePlayerPerDiceRoll(Player player)
        {
            // Roll a dice 
            Random random = new Random();
            int steps = random.Next(1, 6);

            // Determine movement direction according to current row (moving to the right on Even rows and to the left on Odd rows)
            if (player.CurrentCell.Row % 2 == 0)
            {
                // This is an Even row
                // Check if we are going to pass the border of the board and have to go up
                if (player.CurrentCell.Column + steps > _gameRepository.GetBoardColumnsCount())
                {
                    player.CurrentCell.Row++;
                    int remainingSteps = steps - (_gameRepository.GetBoardColumnsCount() - player.CurrentCell.Column) - 1; // Move to the border, then up one cell
                    player.CurrentCell.Column = _gameRepository.GetBoardColumnsCount() - remainingSteps;
                }
                else
                {
                    player.CurrentCell.Column = player.CurrentCell.Column + steps;
                }
            }
            else
            {
                // Odd row
                // Check if we are going to pass the border of the board and have to go up
                if (player.CurrentCell.Column - steps < 0)
                {
                    player.CurrentCell.Row++;
                    int remainingSteps = steps - player.CurrentCell.Column - 1; // Move to the border, then up one cell
                    player.CurrentCell.Column = remainingSteps; // technically 0 + remainingSteps
                }
                else
                {
                    player.CurrentCell.Column = player.CurrentCell.Column - steps;
                }
            }
        }

        private void MovePlayerPerCellEffect(Player player)
        {
            // Check if there is any modifier at player location
            CellModifier cellModifier;
            bool hasCellModifier = _gameRepository.GetCellModifier(player.CurrentCell, out cellModifier);

            if (hasCellModifier)
            {
                // All cell modifiers change the players position to a specific TargetCell that might be ahead(ladders) or behind(snakes) the player's current cell position
                player.CurrentCell = cellModifier.TargetCell; 
            }
        }
        private void ClampToBoardCorners(Player player)
        {
            // Check if we somehow went back from the minimium cell position (shouldn't happen!)
            if (player.CurrentCell.Row < 0) { player.CurrentCell.Row = 0; }
            if (player.CurrentCell.Column < 0) { player.CurrentCell.Column = 0; }

            // Check if we 'went pass' the final cell (went to the row above the final cell), if so clamp back to it
            if (player.CurrentCell.Row > _gameRepository.GetBoardRowsCount())
            {
                player.CurrentCell = _gameRepository.GetFinalCell();
            }
        }

    }
}
