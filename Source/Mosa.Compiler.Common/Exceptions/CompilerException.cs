// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Common.Exceptions
{
	[Serializable]
	public class CompilerException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CompilerException" /> class.
		/// </summary>
		public CompilerException() : base()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="CompilerException" /> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public CompilerException(string message) : base(message)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="CompilerException"/> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public CompilerException(string message, Exception innerException) : base(message, innerException)
		{ }
	}
}
