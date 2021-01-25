using System;
using System.Collections.Generic;

namespace Shipping.Tests
{
    public class App
    {
        private readonly List<IEvent> _history = new List<IEvent>();

        public void Given(params IEvent[] events) => _history.AddRange(events);

        public void When(IEvent @event)
        {
            var cmd = ShippingPolicy.When((dynamic)@event);
            var state = _history.Rehydrate<Order>();
            _history.AddRange(OrderBehavior.Handle(state, (dynamic)cmd));
        }

        public void Then(Action<IEvent[]> f)
            => f(_history.ToArray());
    }


    public class ShippingPolicy
    {
        public static ICommand When(PaymentRecieved @event) => new CompletePayment();
        public static ICommand When(GoodsPicked @event) => new CompletePacking();
    }

    public static class OrderBehavior
    {
        public static IEnumerable<IEvent> Handle(this Order order, CompletePayment command)
        {
            var events = new List<IEvent> { new PaymentComplete() };
            if (order.Packed)
            {
                events.Add(new GoodsShipped());
            }

            return events;
        }

        public static IEnumerable<IEvent> Handle(this Order order, CompletePacking command)
        {
            var events = new List<IEvent> { new PackingComplete() };
            if (order.Payed)
            {
                events.Add(new GoodsShipped());
            }

            return events;
        }
    }

    public class Order
    {
        public bool Payed;
        public bool Packed;

        public Order When(IEvent @event) => this;

        public Order When(PaymentRecieved @event)
        {
            Payed = true;
            return this;
        }

        public Order When(GoodsPicked @event)
        {
            Packed = true;
            return this;
        }
    }
}
