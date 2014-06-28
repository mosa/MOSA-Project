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
		protected List<Move> moves;

		protected int index;
		protected bool before;

		public MoveResolver(BasicBlock anchor, BasicBlock source, BasicBlock destination)
		{
			Debug.Assert(source != null);
			Debug.Assert(destination != null);
			Debug.Assert(anchor != null);

			this.moves = new List<Move>();

			this.before = source == anchor;
			this.index = before ? source.EndIndex : destination.StartIndex;			         
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

				for (int i = 0; i < moves.Count; i++)
				{
					var move = moves[i];

					if (!(move.Source.IsCPURegister || move.Destination.IsCPURegister))
						continue;

					int other = FindIndex(move.Destination.Register, true);

					if (other != -1)
						continue;

					architecture.InsertMoveInstruction(context, move.Destination, move.Source);
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

					architecture.InsertExchangeInstruction(context, moves[other].Source, move.Source);
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

				architecture.InsertMoveInstruction(context, move.Destination, move.Source);
			}
		}

		public void InsertResolvingMoves(BaseArchitecture architecture, InstructionSet instructionSet)
		{
			if (moves.Count == 0)
				return;

			var context = new Context(instructionSet, index);

			if (before)
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

			Debug.Assert(moves.Count == 0);
		}
	}
}