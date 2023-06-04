// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Common.Exceptions;

[Serializable]
public class AssemblyLoadException : CompilerException
{
	/// <summary>
	/// Initializes a new instance of the <see cref="AssemblyLoadException"/> class.
	/// </summary>
	public AssemblyLoadException() : base("Assembly Load Exception")
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="AssemblyLoadException"/> class.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public AssemblyLoadException(string message) : base(message)
	{ }
}
