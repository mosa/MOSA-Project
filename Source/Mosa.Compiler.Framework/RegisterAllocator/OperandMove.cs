// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator;

public struct OperandMove
{
	public readonly Operand Source;

	public readonly Operand Destination;

	public OperandMove(Operand source, Operand destination)
	{
		Debug.Assert(source != null);
		Debug.Assert(destination != null);

		Source = source;
		Destination = destination;
	}
}
