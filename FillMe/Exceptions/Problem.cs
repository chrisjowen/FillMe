using System;

namespace FillMe.Exceptions
{
    public class Problem : ApplicationException
    {
        public Problem() { }
        public Problem(string message)
            : base(message)
        { }
        public Problem(string message, Exception innerException)
            : base(message, innerException)
        { }

    }
}