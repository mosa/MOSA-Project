// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Common.Exceptions;

[Serializable]
public class InvalidOperationCompilerException : CompilerException
{
	/// <summary>
	/// Initializes a new instance of the <see cref="InvalidOperationCompilerException"/> class.
	/// </summary>
	public InvalidOperationCompilerException() : base()
	{
		var callStack = new StackFrame(1, true);
		var method = callStack.GetMethod();

		BaseMessage = $"Invalid Operation: {method.DeclaringType.Name}.{method.Name} at line {callStack.GetFileLineNumber}";
	}

	public InvalidOperationCompilerException(string message, bool includeMethod = true) : base(message)
	{
		if (includeMethod)
		{
			var callStack = new StackFrame(1, true);
			var method = callStack.GetMethod();

			BaseMessage = $"{message}: {method.DeclaringType.Name}.{method.Name} at line {callStack.GetFileLineNumber}";
		}
	}
}
