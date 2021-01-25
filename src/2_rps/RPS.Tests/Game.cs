using System;
using System.Collections.Generic;
using System.Text;

namespace RPS.Tests
{
    public static class Game
    {
        public static IEnumerable<IEvent> Handle(CreateGame command, GameState state)
        {
            return new List<IEvent>
            {
                new GameCreated
                {
                    GameId = command.GameId,
                    PlayerId = command.PlayerId,
                    Rounds = command.Rounds,
                    Title = command.Title,
                }
            };
        }

        public static IEnumerable<IEvent> Handle(JoinGame command, GameState state)
        {
            // You cannot play against yourself
            if (state.Creator == command.PlayerId)
            {
                return new List<IEvent>();
            }

            return new List<IEvent>
            {
                new GameStarted(),
                new RoundStarted()
            };
        }

        public static IEnumerable<IEvent> Handle(PlayGame command, GameState state)
        {
            var events = new List<IEvent>
            {
                new HandShown
                {
                    GameId = command.GameId,
                    Hand = command.Hand,
                    PlayerId = command.PlayerId
                }
            };

            if (state.HandsShown == 2)
            {
                events.Add(new RoundEnded());
            }

            if (state.CurrentRound == state.Rounds)
            {
                events.Add(new GameEnded());
            }

            return events;
        }
    }
}
