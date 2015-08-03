// Copyright (c) MOSA Project. Licensed under the New BSD License.

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