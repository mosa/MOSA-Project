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
    /// Implementation of the "System.ArgumentOutOfRangeException" class
    /// </summary>
    public class ArgumentOutOfRangeException : ArgumentException
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="ArgumentOutOfRangeException"/> class.
		/// </summary>
        public ArgumentOutOfRangeException()
            : this("Argument is out of range.")
        {}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgumentOutOfRangeException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
        public ArgumentOutOfRangeException(string message)
            : base(message)
        {}

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgumentOutOfRangeException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="paramName">Name of the param.</param>
        public ArgumentOutOfRangeException(string message, string paramName)
            : base(message)
        {
            this.paramName = this.paramName;
        }
    }
}
