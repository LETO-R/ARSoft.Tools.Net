using System;
using System.Runtime.Serialization;

namespace ARSoft.Tools.Net.Socks
{
    public class Socks4Exception : SocksException
    {
        public readonly Socks4ResponseCommand RejectReason;

        public Socks4Exception(Socks4ResponseCommand rejectReason)
        {
            RejectReason = rejectReason;
        }

        public Socks4Exception()
        {

        }

        public Socks4Exception(string message) : base(message)
        {
        }

        public Socks4Exception(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected Socks4Exception(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
