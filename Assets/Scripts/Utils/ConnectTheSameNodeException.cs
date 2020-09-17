using System;

namespace BoardGame
{
    public class ConnectTheSameNodeException : Exception
    {
        public ConnectTheSameNodeException()
        {
        }

        public ConnectTheSameNodeException(string message) : base(message)
        {
        }
    }
}