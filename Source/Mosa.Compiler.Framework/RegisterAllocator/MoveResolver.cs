/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
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
		protected BasicBlock Anchor;
		protected BasicBlock Source;
		protected BasicBlock Destination;

		protected List<Move> moves;

		protected class Move
		{
			public Operand Source { get; set; }

			public Operand Destination { get; set; }

			public Move(Operand source, Operand destination)
			{
				Debug.Assert(source != null);
				Debug.Assert(destination != null);

				Source = source;
				Destination = destination;
			}
		}

		public MoveResolver(BasicBlock anchor, BasicBlock source, BasicBlock destination)
		{
			Debug.Assert(source != null);
			Debug.Assert(destination != null);
			Debug.Assert(anchor != null);

			this.Anchor = anchor;
			this.Source = source;
			this.Destination = destination;
			this.moves = new List<Move>();
		}

		public void AddMove(Operand source, Operand destination)
		{
			moves.Add(new Move(source, destination));
		}

		private int FindIndex(Register register, bool source)
		{
			for (int i = 0; i < moves.Count; i++)
			{
				var move = moves[i];

				Operand operand = source ? move.Source : move.Destination;

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

				for (int i = 0; i < moves.Count; i++)
				{
					var move = moves[i];

					if (!(move.Source.IsCPURegister || move.Destination.IsCPURegister))
						continue;

					int other = FindIndex(move.Destination.Register, true);

					if (other != -1)
						continue;

					architecture.InsertMove(context, move.Destination, move.Source);
					context.Marked = true;

					moves.RemoveAt(i);

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

				for (int i = 0; i < moves.Count; i++)
				{
					var move = moves[i];

					if (!(move.Source.IsCPURegister || move.Destination.IsCPURegister))
						continue;

					int other = FindIndex(move.Destination.Register, true);

					if (other == -1)
						continue;

					architecture.InsertExchange(context, moves[other].Source, move.Source);
					context.Marked = true;
					moves[other].Source = move.Source;
					moves.RemoveAt(i);

					if (other > i)
						other--;

					if (moves[other].Source.Register == moves[other].Destination.Register)
						moves.RemoveAt(other);

					loop = true;
				}
			}
		}

		protected void CreateMemoryMoves(BaseArchitecture architecture, Context context)
		{
			for (int i = 0; i < moves.Count; i++)
			{
				var move = moves[i];

				if (!(move.Source.IsCPURegister || move.Destination.IsCPURegister))
					continue;

				architecture.InsertMove(context, move.Destination, move.Source);
			}
		}

		public void InsertResolvingMoves(BaseArchitecture architecture, InstructionSet instructionSet)
		{
			if (moves.Count == 0)
				return;

			Context context = new Context(instructionSet, Source == Anchor ? Source.EndIndex : Destination.StartIndex);

			if (Source == Anchor)
			{
				context.GotoPrevious();

				// Note: This won't work for expanded switch statements... but we can't insert into the end of those blocks anyway
				while (context.IsEmpty || context.Instruction.FlowControl == FlowControl.Branch || context.Instruction.FlowControl == FlowControl.ConditionalBranch || context.Instruction.FlowControl == FlowControl.Return)
				{
					context.GotoPrevious();
				}
			}

			TrySimpleMoves(architecture, context);
			TryExchange(architecture, context);
			CreateMemoryMoves(architecture, context);

			Debug.Assert(moves.Count == 0);
		}
	}
}