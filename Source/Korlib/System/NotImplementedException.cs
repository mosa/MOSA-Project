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
    /// Implementation of the "System.NotImplementedException" class
    /// </summary>
    [Serializable]
    public class NotImplementedException : SystemException
    {
        private readonly string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotImplementedException"/> class.
        /// </summary>
        public NotImplementedException()
            : base("A Not Implemented exception was thrown.")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotImplementedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public NotImplementedException(string message)
            : base(message)
        { }
    }
}