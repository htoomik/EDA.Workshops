namespace RPS.Tests
{
    public class GameState
    {
        public GameState When(IEvent @event) => this;

        public GameState When(GameCreated @event)
        {
            return new GameState { Status = GameStatus.ReadyToStart };
        }

        public GameState When(GameStarted @event)
        {
            return new GameState { Status = GameStatus.Started };
        }

        public GameState When(RoundStarted @event)
        {
            return this;
        }

        public GameState When(HandShown @event)
        {
            return this;
        }

        public GameState When(RoundEnded @event)
        {
            return this;
        }

        public GameState When(GameEnded @event)
        {
            return new GameState { Status = GameStatus.Ended };
        }

        public GameStatus Status { get; set; }
    }
}
