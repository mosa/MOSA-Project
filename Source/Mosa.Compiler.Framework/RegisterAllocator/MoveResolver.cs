// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public sealed class MoveResolver
	{
		public enum ResolvedMoveType { Move, Exchange, Load }

		public sealed class ResolvedMoveList : List<MoveExtended<ResolvedMoveType>>
		{
			public void Add(Operand source, Operand destination, ResolvedMoveType type)
			{
				Add(new MoveExtended<ResolvedMoveType>(source, destination, type));
			}
		}

		public readonly List<Move> Moves;

		public readonly InstructionNode Index;

		public readonly bool Before;

		public MoveResolver(InstructionNode index, bool before)
		{
			Moves = new List<Move>();

			Before = before;
			Index = index;
		}

		public MoveResolver(InstructionNode index, bool before, List<Move> moves)
			: this(index, before)
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
			Moves.Add(new Move(source, destination));
		}

		private int FindIndex(PhysicalRegister register, bool source)
		{
			for (int i = 0; i < Moves.Count; i++)
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

		private void TrySimpleMoves(ResolvedMoveList moves)
		{
			bool loop = true;

			while (loop)
			{
				loop = false;

				for (int i = 0; i < Moves.Count; i++)
				{
					var move = Moves[i];

					if (!(move.Source.IsCPURegister || move.Destination.IsCPURegister))
						continue;

					int other = FindIndex(move.Destination.Register, true);

					if (other != -1)
						continue;

					Debug.Assert(move.Destination.IsCPURegister);

					moves.Add(move.Source, move.Destination, move.Source.IsCPURegister ? ResolvedMoveType.Move : ResolvedMoveType.Load);

					Moves.RemoveAt(i);

					loop = true;
				}
			}
		}

		private void TryExchange(ResolvedMoveList moves)
		{
			bool loop = true;

			while (loop)
			{
				loop = false;

				for (int i = 0; i < Moves.Count; i++)
				{
					var move = Moves[i];

					if (!(move.Source.IsCPURegister || move.Destination.IsCPURegister))
						continue;

					int other = FindIndex(move.Destination.Register, true);

					if (other == -1)
						continue;

					Debug.Assert(Moves[other].Source.IsCPURegister);
					Debug.Assert(move.Source.IsCPURegister);

					moves.Add(Moves[other].Source, move.Source, ResolvedMoveType.Exchange);

					Moves[other] = new Move(move.Source, Moves[other].Destination);

					Moves.RemoveAt(i);

					if (other > i)
						other--;

					if (Moves[other].Source.Register == Moves[other].Destination.Register)
						Moves.RemoveAt(other);

					loop = true;
				}
			}
		}

		private void CreateMemoryMoves(ResolvedMoveList moves)
		{
			for (int i = 0; i < Moves.Count; i++)
			{
				var move = Moves[i];

				if (!(move.Source.IsCPURegister || move.Destination.IsCPURegister))
					continue;

				Debug.Assert(move.Destination.IsCPURegister);
				Debug.Assert(move.Source.IsCPURegister);

				moves.Add(move.Destination, move.Source, ResolvedMoveType.Move);
			}
		}

		public ResolvedMoveList GetResolveMoves()
		{
			var moves = new ResolvedMoveList();

			TrySimpleMoves(moves);
			TryExchange(moves);
			CreateMemoryMoves(moves);

			return moves;
		}

		public void InsertResolvingMoves(BaseArchitecture architecture, Operand stackFrame)
		{
			if (Moves.Count == 0)
				return;

			var moves = GetResolveMoves();

			var context = new Context(Index);

			if (Before)
			{
				// TODO: Generalize XXXX
				context.GotoPrevious();

				// Note: This won't work for expanded switch statements... but we can't insert into the end of those blocks anyway
				while (context.IsEmpty
					|| context.Instruction.FlowControl == FlowControl.UnconditionalBranch
					|| context.Instruction.FlowControl == FlowControl.ConditionalBranch
					|| context.Instruction.FlowControl == FlowControl.Return)
				{
					context.GotoPrevious();
				}
			}

			foreach (var move in moves)
			{
				Debug.Assert(move.Destination.IsCPURegister);

				switch (move.Value)
				{
					case ResolvedMoveType.Move: architecture.InsertMoveInstruction(context, move.Destination, move.Source); break;
					case ResolvedMoveType.Exchange: architecture.InsertExchangeInstruction(context, move.Destination, move.Source); break;
					case ResolvedMoveType.Load: architecture.InsertLoadInstruction(context, move.Destination, stackFrame, move.Source); break;
				}

				context.Marked = true;
			}

			Debug.Assert(Moves.Count == 0);
		}
	}
}
