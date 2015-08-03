// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Common
{
	[Serializable]
	public class InvalidCompilerException : CompilerException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidCompilerException"/> class.
		/// </summary>
		public InvalidCompilerException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidCompilerException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public InvalidCompilerException(string message)
			: base(message)
		{ }
	}
}