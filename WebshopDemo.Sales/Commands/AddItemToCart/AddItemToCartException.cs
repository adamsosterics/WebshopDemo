using System;

namespace WebshopDemo.Sales.Commands.AddItemToCart
{

    [Serializable]
    public class AddItemToCartException : Exception
    {
        public AddItemToCartException() { }
        public AddItemToCartException(string message) : base(message) { }
        public AddItemToCartException(string message, Exception inner) : base(message, inner) { }
        protected AddItemToCartException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
