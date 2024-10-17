using System;

namespace exception // Make sure this matches the namespace structure of your project
{
    public class InvalidLoanException : Exception
    {
        // Default constructor
        public InvalidLoanException() : base() { }

        // Constructor that accepts a custom message
        public InvalidLoanException(string message) : base(message) { }

        // Constructor that accepts a custom message and inner exception
        public InvalidLoanException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
