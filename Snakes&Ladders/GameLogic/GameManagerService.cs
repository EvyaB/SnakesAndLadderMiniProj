using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SnakesAndLadderEvyatar.Models;
using SnakesAndLadderEvyatar.Repositories;

namespace SnakesAndLadderEvyatar.GameLogic
{
    public class GameManagerService : BackgroundService
    {
        private static int TURNS_TIMER_MILLISECONDS = 3000;
        private readonly Repositories.IGameboardRepository _gameboardRepository;
        private readonly IServiceScopeFactory _scopeFactory;

        public GameManagerService(Repositories.IGameboardRepository gameboardRepository, IServiceScopeFactory scopeFactory)
        {
            _gameboardRepository = gameboardRepository;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await RunGame(stoppingToken);
        }

        private async Task RunGame(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                DataContext dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

                // Go over every 'ingame' (actively playing) player and run his/her turn
                foreach (Game game in dataContext.Games.Where(game => game.CurrentGameState == Game.GameState.Playing))
                {
                    PlayTurnInGame(game);
                    dataContext.Update(game);
                }

                await dataContext.SaveChangesAsync(stoppingToken);

                await Task.Delay(TURNS_TIMER_MILLISECONDS, stoppingToken);
            }
        }

        private void PlayTurnInGame(Game game)
        {
            // Update player's turn counter
            game.TurnNumber++;

            MovePlayerInGame(game);

            // Check if reached final cell (=won game)
            if (game.PlayerPosition == _gameboardRepository.GetFinalCell())
            {
                game.CurrentGameState = Game.GameState.Finished;
                game.EndDateTime = DateTime.Now;
            }
        }

        // Move player according to dice roll, snakes/ladders modifiers and the boundaries of the board
        private void MovePlayerInGame(Game game)
        {
            MovePlayerPerDiceRoll(game);
            MovePlayerPerCellEffect(game);
            ClampToBoardCorners(game);
        }

        private void MovePlayerPerDiceRoll(Game game)
        {
            // Roll a dice 
            Random random = new Random();
            int steps = random.Next(1, 6);

            // Determine movement direction according to current row (moving to the right on Even rows and to the left on Odd rows)
            if (game.PlayerPosition.Row % 2 == 0)
            {
                // This is an Even row
                // Check if we are going to pass the border of the board and have to go up
                if (game.PlayerPosition.Column + steps > _gameboardRepository.GetBoardColumnsCount())
                {
                    game.PlayerPosition.Row++;
                    int remainingSteps = steps - (_gameboardRepository.GetBoardColumnsCount() - game.PlayerPosition.Column) - 1; // Move to the border, then up one cell
                    game.PlayerPosition.Column = _gameboardRepository.GetBoardColumnsCount() - remainingSteps;
                }
                else
                {
                    game.PlayerPosition.Column = game.PlayerPosition.Column + steps;
                }
            }
            else
            {
                // Odd row
                // Check if we are going to pass the border of the board and have to go up
                if (game.PlayerPosition.Column - steps < 0)
                {
                    game.PlayerPosition.Row++;
                    int remainingSteps = steps - game.PlayerPosition.Column - 1; // Move to the border, then up one cell
                    game.PlayerPosition.Column = remainingSteps; // technically 0 + remainingSteps
                }
                else
                {
                    game.PlayerPosition.Column = game.PlayerPosition.Column - steps;
                }
            }
        }

        private void MovePlayerPerCellEffect(Game game)
        {
            // Check if there is any modifier at player location
            CellModifier cellModifier;
            bool hasCellModifier = _gameboardRepository.GetCellModifier(game.PlayerPosition, out cellModifier);

            if (hasCellModifier)
            {
                // All cell modifiers change the players position to a specific TargetCell that might be ahead(ladders) or behind(snakes) the player's current cell position
                game.PlayerPosition = cellModifier.TargetCell; 
            }
        }
        private void ClampToBoardCorners(Game game)
        {
            // Check if we somehow went back from the minimium cell position (shouldn't happen!)
            if (game.PlayerPosition.Row < 0) { game.PlayerPosition.Row = 0; }
            if (game.PlayerPosition.Column < 0) { game.PlayerPosition.Column = 0; }

            // Check if we 'went pass' the final cell (went to the row above the final cell), if so clamp back to it
            if (game.PlayerPosition.Row > _gameboardRepository.GetBoardRowsCount())
            {
                game.PlayerPosition = _gameboardRepository.GetFinalCell();
            }
        }
    }
}
