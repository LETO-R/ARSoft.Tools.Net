using System;
using System.Runtime.Serialization;

namespace ARSoft.Tools.Net.Socks
{
    public class SocksException : Exception
    {
        public SocksException()
        {
        }

        public SocksException(string message) 
            : base(message)
        {
        }

        public SocksException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected SocksException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {

        }
    }
}
