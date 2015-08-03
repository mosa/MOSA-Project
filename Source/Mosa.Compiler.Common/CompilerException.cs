// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Common
{
	[Serializable]
	public class CompilerException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CompilerException"/> class.
		/// </summary>
		public CompilerException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CompilerException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public CompilerException(string message)
			: base(message)
		{ }
	}
}