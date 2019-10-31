// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	/// Implementation of the "System.ArgumentException" class
	/// </summary>
	public class FormatException : Exception
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="FormatException"/> class.
		/// </summary>
		public FormatException()
			: this("Bad Format")
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="FormatException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public FormatException(string message)
			: base(message)
		{ }

	}
}
