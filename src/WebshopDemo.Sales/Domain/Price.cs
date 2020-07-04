using System;

namespace WebshopDemo.Sales.Domain
{
    public class Price
    {
        public Price(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        private Price() { }

        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        public override bool Equals(object obj)
        {
            return obj is Price price &&
                   Amount == price.Amount &&
                   Currency == price.Currency;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }
    }
}