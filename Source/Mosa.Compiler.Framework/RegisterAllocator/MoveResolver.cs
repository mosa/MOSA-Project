/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public class MoveResolver
	{
		public readonly List<Move> Moves;

		public readonly int Index;
		public readonly bool Before;

		public MoveResolver(int index, bool before)
		{
			Moves = new List<Move>();

			Before = before;
			Index = index;
		}

		public MoveResolver(BasicBlock anchor, BasicBlock source, BasicBlock destination)
			: this(source == anchor ? source.EndIndex : destination.StartIndex, source == anchor)
		{
		}

		public void AddMove(Operand source, Operand destination)
		{
			Moves.Add(new Move(source, destination));
		}

		private int FindIndex(Register register, bool source)
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

		protected void TrySimpleMoves(BaseArchitecture architecture, Context context)
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

					architecture.InsertMoveInstruction(context, move.Destination, move.Source);
					context.Marked = true;

					Moves.RemoveAt(i);

					loop = true;
				}
			}
		}

		protected void TryExchange(BaseArchitecture architecture, Context context)
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

					architecture.InsertExchangeInstruction(context, Moves[other].Source, move.Source);
					context.Marked = true;

					//Moves[other].Source = move.Source;
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

		protected void CreateMemoryMoves(BaseArchitecture architecture, Context context)
		{
			for (int i = 0; i < Moves.Count; i++)
			{
				var move = Moves[i];

				if (!(move.Source.IsCPURegister || move.Destination.IsCPURegister))
					continue;

				architecture.InsertMoveInstruction(context, move.Destination, move.Source);
			}
		}

		public void InsertResolvingMoves(BaseArchitecture architecture, InstructionSet instructionSet)
		{
			if (Moves.Count == 0)
				return;

			var context = new Context(instructionSet, Index);

			if (Before)
			{
				context.GotoPrevious();

				// Note: This won't work for expanded switch statements... but we can't insert into the end of those blocks anyway
				while (context.IsEmpty || context.Instruction.FlowControl == FlowControl.UnconditionalBranch || context.Instruction.FlowControl == FlowControl.ConditionalBranch || context.Instruction.FlowControl == FlowControl.Return)
				{
					context.GotoPrevious();
				}
			}

			TrySimpleMoves(architecture, context);
			TryExchange(architecture, context);
			CreateMemoryMoves(architecture, context);

			Debug.Assert(Moves.Count == 0);
		}
	}
}