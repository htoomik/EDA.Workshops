using System.Collections.Generic;
using System.Linq;

namespace RPS.Tests
{
    public class HighScoreView
    {
        public HighScoreView When(IEvent @event) => this;
        public ScoreRow[] Rows { get; set; }

        private Dictionary<string, int> CurrentGame { get; set; }

        public class ScoreRow
        {
            public int Rank { get; set; }
            public string PlayerId { get; set; }
            public int GamesWon { get; set; }
            public int RoundsWon { get; set; }
            public int GamesPlayed { get; set; }
            public int RoundsPlayed { get; set; }
        }

        public HighScoreView When(GameStarted @event)
        {
            // Reset scoring for current game
            return new HighScoreView
            {
                Rows = Rows,
                CurrentGame = new Dictionary<string, int>()
            };
        }

        public HighScoreView When(RoundEnded @event)
        {
            // Increment RoundsWon for winner
            var roundWinner = @event.Winner;
            var rows = Rows == null ? new Dictionary<string, ScoreRow>() : Rows.ToDictionary(r => r.PlayerId, r => r);
            if (!rows.ContainsKey(roundWinner))
            {
                rows[roundWinner] = new ScoreRow { PlayerId = roundWinner };
            }
            rows[roundWinner].RoundsWon++;

            // Update scoring for current game
            var currentGame = CurrentGame ?? new Dictionary<string, int>();
            if (!currentGame.ContainsKey(roundWinner))
            {
                currentGame[roundWinner] = 0;
            }
            currentGame[roundWinner]++;

            return new HighScoreView
            {
                Rows = rows.Values.OrderBy(r => r.Rank).ToArray(),
                CurrentGame = currentGame
            };
        }

        public HighScoreView When(GameEnded @event)
        {
            // Find winner
            var winningScore = CurrentGame.Values.Max();
            var gameWinner = CurrentGame.Single(s => s.Value == winningScore).Key;

            // Increment GamesWon for winner
            var rows = Rows.ToDictionary(r => r.PlayerId, r => r);
            rows[gameWinner].GamesWon++;

            // Order by games won and calculate ranks
            var ordered = rows.Values.OrderByDescending(r => r.GamesWon);
            var rank = 1;
            foreach (var row in ordered)
            {
                row.Rank = rank++;
            }

            return new HighScoreView
            {
                Rows = Rows.OrderBy(r => r.Rank).ToArray(),
                CurrentGame = CurrentGame
            };
        }
    }
}
