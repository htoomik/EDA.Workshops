namespace RPS.Tests
{
    public class GameState
    {
        public string Creator { get; set; }
        public string PlayerOne { get; set; }
        public string PlayerTwo { get; set; }
        public Hand PlayerOneHand { get; set; }
        public Hand PlayerTwoHand { get; set; }
        public int HandsShown { get; set; }
        public int Rounds { get; set; }
        public int CurrentRound { get; set; }

        public GameState When(IEvent @event)
        {
            return this;
        }

        public GameState When(GameCreated @event)
        {
            Creator = @event.PlayerId;
            Rounds = @event.Rounds;
            return this;
        }

        public GameState When(RoundStarted @event)
        {
            CurrentRound = @event.Round;
            return this;
        }

        public GameState When(HandShown @event)
        {
            if (@event.PlayerId == PlayerOne)
            {
                PlayerOneHand = @event.Hand;
            }

            if (@event.PlayerId == PlayerTwo)
            {
                PlayerTwoHand = @event.Hand;
            }

            HandsShown++;

            return this;
        }

        public enum GameStatus
        {
            None = 0,
            ReadyToStart = 10,
            Started = 20,
            Ended = 50
        }
    }
}
