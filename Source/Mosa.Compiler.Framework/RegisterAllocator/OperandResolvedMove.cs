// Copyright (c) MOSA Project. Licensed under the New BSD License.

using static Mosa.Compiler.Framework.RegisterAllocator.MoveResolver;

namespace Mosa.Compiler.Framework.RegisterAllocator;

public struct OperandResolvedMove
{
	public readonly Operand Source;

	public readonly Operand Destination;

	public readonly ResolvedMoveType ResolvedMoveType;

	public OperandResolvedMove(Operand source, Operand destination, ResolvedMoveType resolvedMoveType)
	{
		Source = source;
		Destination = destination;
		ResolvedMoveType = resolvedMoveType;
	}
}
