using System;

namespace Saeed.Utilities.Exceptions
{
    public class DeleteFailureException : Exception
    {
        public DeleteFailureException(string name, object key, string message)
            : base($"Deletion of entity \"{name}\" ({key}) failed. {message}")
        {
        }
        public DeleteFailureException(string name, string message)
            : base($"Deletion of entity \"{name}\" failed. {message}")
        {
        }
    }
}
