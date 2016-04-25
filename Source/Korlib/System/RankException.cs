// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	/// The exception that is thrown when an array with the wrong number of dimensions is passed to a method.
	/// </summary>
	public class RankException : SystemException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RankException"/> class.
		/// </summary>
		public RankException()
			: base("Attempted to operate on an array with the incorrect number of dimensions.")
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="RankException"/> class with a specified error message.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		public RankException(string message)
			: base(message)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="RankException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception. </param>
		public RankException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}
