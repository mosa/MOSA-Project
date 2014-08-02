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
			: base("Argument cannot be null.")
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgumentNullException"/> class with the name of the parameter that causes this exception.
		/// </summary>
		/// <param name="paramName"></param>
		public ArgumentNullException(string paramName)
			: base("Argument cannot be null.", paramName)
		{ }

		/// <summary>
		/// Initializes an instance of the <see cref="ArgumentNullException"/> class with a specified error message and the name of the parameter that causes this exception.
		/// </summary>
		/// <param name="paramName">The name of the parameter that caused the exception.</param>
		/// <param name="message">A message that describes the error.</param>
		public ArgumentNullException(string paramName, string message)
			: base(message, paramName)
		{ }
	}
}