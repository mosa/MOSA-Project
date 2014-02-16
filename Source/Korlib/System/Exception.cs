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
	/// Implementation of the "System.Exception" class
	/// </summary>
    [Serializable]
	public class Exception
	{
		private readonly string message;

		/// <summary>
		/// Initializes a new instance of the <see cref="Exception"/> class.
		/// </summary>
		public Exception()
			: this("An exception was thrown.")
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="Exception"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public Exception(string message)
		{
			this.message = message;
		}

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <value>The message.</value>
		public virtual string Message
		{
			get { return message; }
		}
	}
}