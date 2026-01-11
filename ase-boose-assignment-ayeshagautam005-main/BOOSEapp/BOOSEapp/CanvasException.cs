using System;

namespace BOOSEapp
{
    /// <summary>
    /// Represents errors that occur during canvas-related operations.
    /// </summary>
    [Serializable]
    public class CanvasException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasException"/> class.
        /// </summary>
        public CanvasException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CanvasException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public CanvasException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
