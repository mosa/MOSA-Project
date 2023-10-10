// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.RegisterAllocator;

public class LiveIntervalTransition
{
	public readonly LiveInterval From;
	public readonly LiveInterval To;

	public readonly ResolvedMoveType ResolvedMoveType;

	public Operand Source => From.AssignedOperand;

	public Operand Destination => To.AssignedOperand;

	public LiveIntervalTransition(LiveInterval source, LiveInterval destination)
	{
		From = source;
		To = destination;
		ResolvedMoveType = ResolvedMoveType.None;
	}

	public LiveIntervalTransition(ResolvedMoveType resolvedMoveType, LiveInterval source, LiveInterval destination)
	{
		From = source;
		To = destination;
		ResolvedMoveType = resolvedMoveType;
	}
}
