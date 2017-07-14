// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Trace;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class OperandAssignmentStage : BaseMethodCompilerStage
	{
		private Queue<BasicBlock> worklist = new Queue<BasicBlock>();
		private BitArray processed;
		private List<InstructionNode> dupNodes = new List<InstructionNode>();
		private TraceLog trace;

		private Dictionary<BasicBlock, List<Operand>> outgoingMoves = new Dictionary<BasicBlock, List<Operand>>();
		private Dictionary<BasicBlock, List<Operand>> incomingMoves = new Dictionary<BasicBlock, List<Operand>>();

		private static List<Operand> empty = new List<Operand>();

		protected override void Run()
		{
			if (MethodCompiler.Method.Code.Count == 0)
				return;

			trace = CreateTraceLog();

			processed = new BitArray(BasicBlocks.Count, false);

			foreach (var headBlock in BasicBlocks.HeadBlocks)
			{
				Trace(headBlock);
			}

			RemoveDuplicateInstructions();
		}

		protected override void Finish()
		{
			worklist = null;
			processed = null;
			outgoingMoves = null;
			incomingMoves = null;
			dupNodes = null;
			trace = null;
		}

		/// <summary>
		/// Traces the specified label.
		/// </summary>
		/// <param name="headBlock">The head block.</param>
		private void Trace(BasicBlock headBlock)
		{
			worklist.Enqueue(headBlock);
			incomingMoves.Add(headBlock, empty); // no incoming moves

			while (worklist.Count > 0)
			{
				AssignOperands(worklist.Dequeue());
			}
		}

		/// <summary>
		/// Assigns the operands.
		/// </summary>
		/// <param name="block">The block.</param>
		private void AssignOperands(BasicBlock block)
		{
			if (processed.Get(block.Sequence))
				return;

			var incoming = incomingMoves[block];

			if (incoming == null)
			{
				worklist.Enqueue(block); // re-queue for later
				return;
			}

			Debug.Assert(incoming != null);

			var operandStack = new Stack<Operand>(incoming);

			if (trace.Active)
			{
				trace.Log("IN:    Block: " + block + " Operand Stack Count: " + operandStack.Count.ToString());
				foreach (var op in operandStack)
				{
					trace.Log("       -> " + op);
				}
			}

			AssignOperands(block, operandStack);

			if (trace.Active)
			{
				trace.Log("AFTER: Block: " + block + " Operand Stack Count: " + operandStack.Count.ToString());
				foreach (var op in operandStack)
				{
					trace.Log("       -> " + op);
				}
			}

			var outgoing = new List<Operand>(operandStack);

			outgoing.Reverse();

			processed.Set(block.Sequence, true);

			outgoingMoves.Add(block, outgoing);

			foreach (var next in block.NextBlocks)
			{
				worklist.Enqueue(next);

				List<Operand> nextIncoming = null;

				incomingMoves.TryGetValue(next, out nextIncoming);

				if (next.PreviousBlocks.Count == 0)
				{
					nextIncoming = empty;   // should never happen!
				}
				else if (next.PreviousBlocks.Count == 1)
				{
					nextIncoming = outgoing;
				}
				else
				{
					if (nextIncoming == null)
					{
						nextIncoming = new List<Operand>(outgoing.Count);

						foreach (var operand in outgoing)
						{
							var register = AllocateVirtualRegisterOrStackSlot(operand.Type);
							nextIncoming.Add(register);
						}
					}

					AddMoves(block, outgoing, nextIncoming);
				}

				incomingMoves[next] = nextIncoming;
			}
		}

		private void AddMoves(BasicBlock block, List<Operand> sourceOperands, List<Operand> destinationOperands)
		{
			var context = new Context(block.Last);

			context.GotoPrevious();

			while (context.IsEmpty
				|| context.Instruction.FlowControl == FlowControl.ConditionalBranch
				|| context.Instruction.FlowControl == FlowControl.UnconditionalBranch
				|| context.Instruction.FlowControl == FlowControl.Return
				|| context.Instruction == IRInstruction.Jmp)
			{
				context.GotoPrevious();
			}

			for (int i = 0; i < sourceOperands.Count; i++)
			{
				var source = sourceOperands[i];
				var destination = destinationOperands[i];

				if (StoreOnStack(source.Type))
				{
					context.AppendInstruction(IRInstruction.MoveCompound, destination, source);
				}
				else
				{
					var moveInstruction = GetMoveInstruction(source.Type);
					context.AppendInstruction(moveInstruction, destination, source);
				}
			}
		}

		/// <summary>
		/// Assigns the operands.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="operandStack">The operand stack.</param>
		private void AssignOperands(BasicBlock block, Stack<Operand> operandStack)
		{
			for (var ctx = new Context(block); !ctx.IsBlockEndInstruction; ctx.GotoNext())
			{
				if (ctx.IsEmpty ||
					ctx.IsBlockEndInstruction ||
					ctx.IsBlockStartInstruction ||
					ctx.Instruction == IRInstruction.Jmp)
					continue;

				if (ctx.Instruction.FlowControl != FlowControl.ConditionalBranch &&
					ctx.Instruction.FlowControl != FlowControl.UnconditionalBranch &&
					ctx.Instruction.FlowControl != FlowControl.Return &&
					ctx.Instruction != IRInstruction.ExceptionStart &&
					ctx.Instruction != IRInstruction.FilterStart &&
					!(ctx.Instruction is BaseCILInstruction))
					continue;

				AssignOperandsFromCILStack(ctx, operandStack);

				if (ctx.Instruction != IRInstruction.ExceptionStart && ctx.Instruction != IRInstruction.FilterStart)
				{
					var cilInstruction = ctx.Instruction as BaseCILInstruction;
					cilInstruction.Resolve(ctx, MethodCompiler);
				}

				PushResultOperands(ctx, operandStack);
			}
		}

		/// <summary>
		/// Assigns the operands from CIL stack.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="currentStack">The current stack.</param>
		private void AssignOperandsFromCILStack(Context ctx, Stack<Operand> currentStack)
		{
			for (int index = ctx.OperandCount - 1; index >= 0; --index)
			{
				if (ctx.GetOperand(index) != null)
					continue;

				ctx.SetOperand(index, currentStack.Pop());
			}
		}

		/// <summary>
		/// Pushes the result operands on to the stack
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="currentStack">The current stack.</param>
		private void PushResultOperands(Context ctx, Stack<Operand> currentStack)
		{
			if (ctx.ResultCount == 0)
				return;

			if (ctx.Instruction != IRInstruction.ExceptionStart &&
				ctx.Instruction != IRInstruction.FilterStart &&
				!(ctx.Instruction as BaseCILInstruction).PushResult)
				return;

			currentStack.Push(ctx.Result);

			if (ctx.Instruction is DupInstruction)
			{
				currentStack.Push(ctx.Result);
				dupNodes.Add(ctx.Node);
			}
		}

		/// <summary>
		/// Removes the duplicate instructions.
		/// </summary>
		private void RemoveDuplicateInstructions()
		{
			foreach (var node in dupNodes)
			{
				Debug.Assert(node.Instruction is DupInstruction);
				Debug.Assert(node.Result == node.Operand1);

				node.Empty();
			}
		}
	}
}
