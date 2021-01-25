using System;
using System.Collections.Generic;
using System.Linq;

namespace Login.Tests
{
    public class App
    {
        public App(Func<DateTime> timeProvider)
        {
            _timeProvider = timeProvider;
        }

        readonly List<IEvent> _history = new List<IEvent>();
        readonly Func<DateTime> _timeProvider;

        public void Given(params IEvent[] events) => _history.AddRange(events);


        public void Handle(Login command)
        {
            if (_history.ToManyAttempts(_timeProvider))
            {
                throw new AuthenticationException("To many attempts");
            }
        }
    }

    public static class LoginRules
    {
        public static bool ToManyAttempts(this IEnumerable<IEvent> events, Func<DateTime> timeProvider)
            => events
                .OfType<AuthenticationAttemptFailed>()
                .Count(e => timeProvider().Subtract(e.Time).TotalMinutes < 15) >= 3;
    }
}
