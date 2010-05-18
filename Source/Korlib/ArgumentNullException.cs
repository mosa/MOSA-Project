/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System
{
    /// <summary>
    /// Implementation of the "System.ArgumentNullException" class
    /// </summary>
    public class ArgumentNullException : ArgumentException
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="ArgumentNullException"/> class.
		/// </summary>
        public ArgumentNullException()
            : this("Argument cannot be null.")
        {}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgumentNullException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
        public ArgumentNullException(string message)
            : base(message)
        {}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgumentNullException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="paramName">Name of the param.</param>
        public ArgumentNullException(string message, string paramName)
            : base(message)
        {
            this.paramName = this.paramName;
        }
    }
}
