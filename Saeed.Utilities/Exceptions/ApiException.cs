using System;

namespace Saeed.Utilities.Exceptions
{
    public class ApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the NotFoundException class with a specified name of the queried object and its key.
        /// </summary>
        /// <param name="message">message</param>
        public ApiException(string message)
            : base($"API Couldn't Respond, Message: {message}")
        {
        }

        /// <summary>
        /// Initializes a new instance of the NotFoundException class with a specified name of the queried object, its key,
        /// and the exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ApiException(string message, Exception innerException)
            : base($"API Couldn't Respond, Message: {message}", innerException)
        {
        }
    }
}