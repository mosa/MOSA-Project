// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator;

public sealed class MoveResolver
{
	public enum ResolvedMoveType
	{ Move, Exchange, Load }

	private readonly InstructionNode Node;

	private readonly bool Before;

	private readonly List<OperandMove> Moves = new();

	private readonly List<OperandResolvedMove> ResolvedMoves = new();

	private MoveResolver(InstructionNode node, bool before)
	{
		Before = before;
		Node = node;
	}

	public MoveResolver(InstructionNode node, bool before, List<OperandMove> moves)
		: this(node, before)
	{
		foreach (var move in moves)
			Moves.Add(move);
	}

	public MoveResolver(BasicBlock anchor, BasicBlock source, BasicBlock destination)
		: this(source == anchor ? source.Last : destination.First, source == anchor)
	{
	}

	public void AddMove(Operand source, Operand destination)
	{
		Moves.Add(new OperandMove(source, destination));
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

				ResolvedMoves.Add(new OperandResolvedMove(move.Source, move.Destination, move.Source.IsCPURegister ? ResolvedMoveType.Move : ResolvedMoveType.Load));

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

				ResolvedMoves.Add(new OperandResolvedMove(Moves[other].Source, move.Source, ResolvedMoveType.Exchange));

				Moves[other] = new OperandMove(move.Source, Moves[other].Destination);

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

			ResolvedMoves.Add(new OperandResolvedMove(move.Destination, move.Source, ResolvedMoveType.Move));
		}
	}

	private void ResolveMoves()
	{
		TrySimpleMoves();
		TryExchange();
		CreateMemoryMoves();
	}

	public int InsertResolvingMoves(BaseArchitecture architecture, Operand stackFrame)
	{
		if (Moves.Count == 0)
			return 0;

		ResolveMoves();

		var context = new Context(Node);

		if (Before)
		{
			// TODO: Generalize
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
				case ResolvedMoveType.Move: architecture.InsertMoveInstruction(context, move.Destination, move.Source); break;
				case ResolvedMoveType.Exchange: architecture.InsertExchangeInstruction(context, move.Destination, move.Source); break;
				case ResolvedMoveType.Load: architecture.InsertLoadInstruction(context, move.Destination, stackFrame, move.Source); break;
			}

			context.Marked = true;
		}

		Debug.Assert(Moves.Count == 0);

		return Moves.Count;
	}
}
