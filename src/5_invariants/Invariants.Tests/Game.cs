using System;
using System.Collections.Generic;
using System.Text;

namespace Invariants.Tests
{
    public static class Game
    {
        public static IEnumerable<IEvent> Handle(JoinGame command, GameState state)
        {
            if (command.PlayerId == state.Creator)
            {
                yield break;
            }

            yield return new GameStarted { GameId = command.GameId, PlayerId = command.PlayerId };
            yield return new RoundStarted { GameId = command.GameId, Round = 1 };
        }

        public static IEnumerable<IEvent> Handle(JoinGame command, IEvent[] events)
        {
            var state = events.Rehydrate<GameState>();
            return Handle(command, state);
        }
    }
}
