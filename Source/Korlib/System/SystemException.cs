/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace System
{
    /// <summary>
    /// Implementation of the "System.SystemException" class
    /// </summary>
    [Serializable]
    public class SystemException : Exception
    {
        private readonly string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemException"/> class.
        /// </summary>
        public SystemException()
            : base("A system exception was thrown.")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SystemException(string message)
            : base(message)
        { }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public override string Message
        {
            get { return message; }
        }
    }
}