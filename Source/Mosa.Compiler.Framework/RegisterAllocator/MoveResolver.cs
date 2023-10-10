// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator;

public sealed class MoveResolver
{
	public readonly InstructionNode Node;

	public readonly bool Before;

	public readonly List<LiveIntervalTransition> Moves = new();

	private readonly List<LiveIntervalTransition> ResolvedMoves = new();

	public MoveResolver(InstructionNode node, bool before)
	{
		Node = node;
		Before = before;
	}

	public void AddMove(LiveInterval from, LiveInterval to)
	{
		Moves.Add(new LiveIntervalTransition(from, to));
	}

	private void ResolveMoves()
	{
		TrySimpleMoves();
		TryExchange();
		CreateMemoryMoves();
	}

	private int FindIndex(PhysicalRegister register, bool source)
	{
		for (var i = 0; i < Moves.Count; i++)
		{
			var move = Moves[i];

			var operand = source ? move.Source : move.Destination;

			if (!operand.IsCPURegister)
				continue;

			if (operand.Register == register)
				return i;
		}

		return -1;
	}

	private void TrySimpleMoves()
	{
		var loop = true;

		while (loop)
		{
			loop = false;

			for (var i = 0; i < Moves.Count; i++)
			{
				var move = Moves[i];

				if (!(move.Source.IsCPURegister || move.Destination.IsCPURegister))
					continue;

				var other = FindIndex(move.Destination.Register, true);

				if (other != -1)
					continue;

				Debug.Assert(move.Destination.IsCPURegister);

				ResolvedMoves.Add(new LiveIntervalTransition(
					move.Source.IsCPURegister ? ResolvedMoveType.Move : ResolvedMoveType.Load,
					move.From,
					move.To)
				);

				Moves.RemoveAt(i);

				loop = true;
			}
		}
	}

	private void TryExchange()
	{
		var loop = true;

		while (loop)
		{
			loop = false;

			for (var i = 0; i < Moves.Count; i++)
			{
				var move = Moves[i];

				if (!(move.Source.IsCPURegister || move.Destination.IsCPURegister))
					continue;

				var other = FindIndex(move.Destination.Register, true);

				if (other == -1)
					continue;

				Debug.Assert(Moves[other].Source.IsCPURegister);
				Debug.Assert(move.Source.IsCPURegister);

				ResolvedMoves.Add(new LiveIntervalTransition(
					ResolvedMoveType.Exchange,
					Moves[other].From,
					move.From)
				);

				Moves[other] = new LiveIntervalTransition(move.From, Moves[other].To);

				Moves.RemoveAt(i);

				if (other > i)
					other--;

				if (Moves[other].Source.Register == Moves[other].Destination.Register)
					Moves.RemoveAt(other);

				loop = true;
			}
		}
	}

	private void CreateMemoryMoves()
	{
		for (var i = 0; i < Moves.Count; i++)
		{
			var move = Moves[i];

			if (!(move.Source.IsCPURegister || move.Destination.IsCPURegister))
				continue;

			Debug.Assert(move.Destination.IsCPURegister);
			Debug.Assert(move.Source.IsCPURegister);

			ResolvedMoves.Add(new LiveIntervalTransition(
				ResolvedMoveType.Move,
				move.To,
				move.From)
			);
		}
	}

	public int InsertResolvingMoves(BaseArchitecture architecture, Operand stackFrame)
	{
		if (Moves.Count == 0)
			return 0;

		ResolveMoves();

		var context = new Context(Node);

		if (Before)
		{
			context.GotoPrevious();

			// Note: This won't work for expanded switch statements... but we can't insert into the end of those blocks anyway
			while (context.IsEmpty
				|| context.Instruction.IsUnconditionalBranch
				|| context.Instruction.IsConditionalBranch
				|| context.Instruction.IsReturn)
			{
				context.GotoPrevious();
			}
		}

		foreach (var move in ResolvedMoves)
		{
			Debug.Assert(move.Destination.IsCPURegister);

			switch (move.ResolvedMoveType)
			{
				case ResolvedMoveType.Move:
					architecture.InsertMoveInstruction(context, move.Destination, move.Source);
					break;

				case ResolvedMoveType.Exchange:
					architecture.InsertExchangeInstruction(context, move.Destination, move.Source);
					break;

				case ResolvedMoveType.Load when !move.From.Register.IsParamLoadOnly:
					architecture.InsertLoadInstruction(context, move.Destination, stackFrame, move.Source);
					break;

				case ResolvedMoveType.Load when move.From.Register.IsParamLoadOnly:
					// Assumes that loads are three operands
					var node = move.From.Register.ParamLoadNode;
					context.AppendInstruction(node.Instruction, move.Destination, node.Operand1, node.Operand2);
					break;
			}

			context.Marked = true;
		}

		Debug.Assert(Moves.Count == 0);

		return Moves.Count;
	}
}
