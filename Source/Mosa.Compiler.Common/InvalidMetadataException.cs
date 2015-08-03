// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Common
{
	[Serializable]
	public class InvalidMetadataException : CompilerException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidMetadataException"/> class.
		/// </summary>
		public InvalidMetadataException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidMetadataException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public InvalidMetadataException(string message)
			: base(message)
		{ }
	}
}