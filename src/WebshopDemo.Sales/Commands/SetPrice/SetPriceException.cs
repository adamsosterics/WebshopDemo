using System;

namespace WebshopDemo.Sales.Commands.SetPrice
{

    [Serializable]
    public class SetPriceException : Exception
    {
        public SetPriceException() { }
        public SetPriceException(string message) : base(message) { }
        public SetPriceException(string message, Exception inner) : base(message, inner) { }
        protected SetPriceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
