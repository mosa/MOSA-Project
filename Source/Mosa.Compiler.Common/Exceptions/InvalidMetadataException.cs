// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Common.Exceptions
{
	[Serializable]
	public class InvalidMetadataException : CompilerException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidMetadataException"/> class.
		/// </summary>
		public InvalidMetadataException()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidMetadataException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public InvalidMetadataException(string message) : base(message)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidMetadataException"/> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public InvalidMetadataException(string message, Exception innerException) : base(message, innerException)
		{ }
	}
}
