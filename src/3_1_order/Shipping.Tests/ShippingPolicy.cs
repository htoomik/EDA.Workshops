using System;
using System.Collections.Generic;
using System.Text;

namespace Shipping.Tests
{
    public class ShippingPolicy
    {
        public static ICommand When(PaymentRecieved @event, Order state)
        {
            if (state.Packed)
            {
                return new Ship();
            }

            return Ship(state);
        }

        public static ICommand When(GoodsPicked @event, Order state)
        {
            if (state.Payed)
            {
                return new Ship();
            }

            return Ship(state);
        }

        private static ICommand Ship(Order state)
        {
            return null;
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
