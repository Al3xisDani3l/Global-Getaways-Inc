using System;

namespace GG.Core
{

    [Serializable]
    public class BusisnessException : Exception
    {
        public BusisnessException() { }
        public BusisnessException(string message) : base(message) { }
        public BusisnessException(string message, Exception inner) : base(message, inner) { }
        protected BusisnessException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public object Details { get; set; }

        public int Status { get; set; }


    }


}
