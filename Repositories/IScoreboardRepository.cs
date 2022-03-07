using SnakesAndLadderEvyatar.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnakesAndLadderEvyatar.Data;

namespace SnakesAndLadderEvyatar.Repositories
{
    public interface IScoreboardRepository
    {
        public Player GetBestPlayer();
        public void ReportPlayerScore(Player player);
    }
}
