namespace Invariants.Tests
{
    public class GameState
    {
        public string Creator { get; set; }

        public GameState When(IEvent @event) => this;

        public GameState When(GameCreated @event)
        {
            Creator = @event.PlayerId;
            return this;
        }
    }
}
