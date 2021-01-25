namespace RPS.Tests
{
    public class GameState
    {
        public string Creator { get; set; }

        public GameState When(IEvent @event)
        {
            return this;
        }

        public GameState When(GameCreated @event)
        {
            return new GameState
            {
                Creator = @event.PlayerId
            };
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
