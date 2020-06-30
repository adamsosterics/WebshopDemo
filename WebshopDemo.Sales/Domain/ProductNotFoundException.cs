using System;

namespace WebshopDemo.Sales.Domain
{

    [Serializable]
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException() { }
        public ProductNotFoundException(string message) : base(message) { }
        public ProductNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ProductNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
