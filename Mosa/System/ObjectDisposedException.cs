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
    /// Implementation of the "System.ObjectDisposedException" class
    /// </summary>
    public class ObjectDisposedException : InvalidOperationException
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectDisposedException"/> class.
		/// </summary>
        public ObjectDisposedException()
            : this("The object was used after being disposed.")
        {}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectDisposedException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
        public ObjectDisposedException(string message)
            : base(message)
        {}
    }
}
