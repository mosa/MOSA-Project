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
	public class AssemblyLoadException : CompilerException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyLoadException"/> class.
		/// </summary>
		public AssemblyLoadException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyLoadException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public AssemblyLoadException(string message)
			: base(message)
		{ }
	}
}