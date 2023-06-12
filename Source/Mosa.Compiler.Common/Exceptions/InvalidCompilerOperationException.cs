// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Diagnostics;

namespace Mosa.Compiler.Common.Exceptions;

[Serializable]
public class InvalidCompilerOperationException : CompilerException
{
	/// <summary>
	/// Initializes a new instance of the <see cref="InvalidCompilerOperationException"/> class.
	/// </summary>
	public InvalidCompilerOperationException() : base()
	{
		var callStack = new StackFrame(1, true);
		var method = callStack.GetMethod();

		BaseMessage = $"Invalid Operation: {method.DeclaringType.Name}.{method.Name} at line {callStack.GetFileLineNumber}";
	}

	public InvalidCompilerOperationException(string message, bool includeMethod = true) : base(message)
	{
		if (includeMethod)
		{
			var callStack = new StackFrame(1, true);
			var method = callStack.GetMethod();

			BaseMessage = $"{message}: {method.DeclaringType.Name}.{method.Name} at line {callStack.GetFileLineNumber}";
		}
	}
}
