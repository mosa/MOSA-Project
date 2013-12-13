/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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