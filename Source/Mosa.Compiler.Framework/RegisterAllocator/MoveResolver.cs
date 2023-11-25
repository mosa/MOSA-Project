﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator;

public sealed class MoveResolver
{
	public readonly Node Node;

	public readonly bool Before;

	public readonly List<LiveIntervalTransition> Moves = new();

	private readonly List<LiveIntervalTransition> ResolvedMoves = new();

	public MoveResolver(Node node, bool before)
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
		CreateConstantLoads();

		Debug.Assert(Moves.Count == 0);
	}

	private int FindIndex(PhysicalRegister register, bool source)
	{
		for (var i = 0; i < Moves.Count; i++)
		{
			var move = Moves[i];

			var operand = source ? move.Source : move.Destination;

			if (!operand.IsPhysicalRegister)
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

				if (!(!move.Source.IsResolvedConstant || move.Source.IsOnStack))
					continue;

				if (!(move.Source.IsPhysicalRegister || move.Destination.IsPhysicalRegister))
					continue;

				var other = FindIndex(move.Destination.Register, true);

				if (other != -1)
					continue;

				Debug.Assert(move.Destination.IsPhysicalRegister);

				ResolvedMoves.Add(new LiveIntervalTransition(
					move.Source.IsPhysicalRegister ? ResolvedMoveType.Move : ResolvedMoveType.Load,
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

				if (!(!move.Source.IsResolvedConstant || move.Source.IsOnStack))
					continue;

				if (!(move.Source.IsPhysicalRegister || move.Destination.IsPhysicalRegister))
					continue;

				var other = FindIndex(move.Destination.Register, true);

				if (other == -1)
					continue;

				Debug.Assert(Moves[other].Source.IsPhysicalRegister);
				Debug.Assert(move.Source.IsPhysicalRegister);

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

			if (!(!move.Source.IsResolvedConstant || move.Source.IsOnStack))
				continue;

			if (!(move.Source.IsPhysicalRegister || move.Destination.IsPhysicalRegister))
				continue;

			Debug.Assert(move.Destination.IsPhysicalRegister);
			Debug.Assert(move.Source.IsPhysicalRegister);

			ResolvedMoves.Add(new LiveIntervalTransition(
				ResolvedMoveType.Move,
				move.From,
				move.To)
			);

			Moves.RemoveAt(i);
			i--;
		}
	}

	private void CreateConstantLoads()
	{
		for (var i = 0; i < Moves.Count; i++)
		{
			var move = Moves[i];

			if (!move.Source.IsResolvedConstant || move.Source.IsOnStack)
				continue;

			Debug.Assert(move.Destination.IsPhysicalRegister);

			ResolvedMoves.Add(new LiveIntervalTransition(
				ResolvedMoveType.ConstantLoad,
				move.From,
				move.To)
			);

			Moves.RemoveAt(i);
			i--;
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
			Debug.Assert(move.Destination.IsPhysicalRegister);

			switch (move.ResolvedMoveType)
			{
				case ResolvedMoveType.ConstantLoad:
					architecture.InsertMoveInstruction(context, move.Destination, move.Source); // FIXME: Change to InsertConstantLoad
					break;

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

			context.IsMarked = true;
		}

		return Moves.Count;
	}
}
