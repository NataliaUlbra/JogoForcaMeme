using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class LeaderboardViewModel
    {
        public string UserName { get; set; }
        public int Score { get; set; }

        public LeaderboardViewModel(string userName, int score)
        {
            UserName = userName;
            Score = score;
        }
    }
}
