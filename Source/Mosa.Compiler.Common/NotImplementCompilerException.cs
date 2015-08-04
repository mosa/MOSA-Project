// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Common
{
	[Serializable]
	public class NotImplementCompilerException : CompilerException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NotImplementCompilerException"/> class.
		/// </summary>
		public NotImplementCompilerException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NotImplementCompilerException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public NotImplementCompilerException(string message)
			: base(message)
		{ }
	}
}
