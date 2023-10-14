// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Common.Exceptions;

[Serializable]
public class InvalidMetadataException : CompilerException
{
	/// <summary>
	/// Initializes a new instance of the <see cref="InvalidMetadataException"/> class.
	/// </summary>
	public InvalidMetadataException() : base("Invalid Metadata Exception")
	{ }

	/// <summary>
	/// Initializes a new instance of the <see cref="InvalidMetadataException"/> class.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public InvalidMetadataException(string message) : base(message)
	{ }
}
