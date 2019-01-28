using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GodSharp.Sockets
{
    public class SocketAggregateException : Exception
    {
        private ICollection<Exception> Exceptions { get; set; }

        public int Count => InnerExceptions == null ? 0 : InnerExceptions.Count;

        public ReadOnlyCollection<Exception> InnerExceptions { get; }

        public SocketAggregateException()
        {
        }

        public SocketAggregateException(string message) : this(message, null)
        {
        }

        public SocketAggregateException(string message, params Exception[] exceptions) : base(message)
        {
            if (exceptions?.Length > 0) InnerExceptions = new ReadOnlyCollection<Exception>(exceptions);
        }
    }
}
