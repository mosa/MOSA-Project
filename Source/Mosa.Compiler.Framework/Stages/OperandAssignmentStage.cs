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
		private sealed class WorkItem
		{
			public BasicBlock Block;
			public Stack<Operand> IncomingStack;

			/// <summary>
			/// Initializes a new instance of the <see cref="WorkItem"/> class.
			/// </summary>
			/// <param name="block">The block.</param>
			/// <param name="incomingStack">The incoming stack.</param>
			public WorkItem(BasicBlock block, Stack<Operand> incomingStack)
			{
				Block = block;
				IncomingStack = incomingStack;
			}
		}

		private Queue<WorkItem> workList = new Queue<WorkItem>();
		private BitArray processed;
		private BitArray enqueued;
		private Stack<Operand>[] outgoingStack;
		private Stack<Operand>[] scheduledMoves;
		private List<InstructionNode> dupNodes = new List<InstructionNode>();
		private TraceLog trace;

		protected override void Run()
		{
			if (MethodCompiler.Method.Code.Count == 0)
				return;

			trace = CreateTraceLog();

			foreach (var headBlock in BasicBlocks.HeadBlocks)
			{
				Trace(headBlock);
			}

			RemoveDuplicateInstructions();
		}

		protected override void Finish()
		{
			workList = null;
			outgoingStack = null;
			scheduledMoves = null;
			processed = null;
			enqueued = null;
			dupNodes = null;
			trace = null;
		}

		/// <summary>
		/// Traces the specified label.
		/// </summary>
		/// <param name="headBlock">The head block.</param>
		private void Trace(BasicBlock headBlock)
		{
			outgoingStack = new Stack<Operand>[BasicBlocks.Count];
			scheduledMoves = new Stack<Operand>[BasicBlocks.Count];
			processed = new BitArray(BasicBlocks.Count, false);
			enqueued = new BitArray(BasicBlocks.Count, false);

			processed.Set(headBlock.Sequence, true);
			workList.Enqueue(new WorkItem(headBlock, new Stack<Operand>()));

			while (workList.Count > 0)
			{
				AssignOperands(workList.Dequeue());
			}
		}

		/// <summary>
		/// Assigns the operands.
		/// </summary>
		/// <param name="workItem">The work item.</param>
		private void AssignOperands(WorkItem workItem)
		{
			var operandStack = workItem.IncomingStack;
			var block = workItem.Block;

			operandStack = CreateMovesForIncomingStack(block, operandStack);

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

			operandStack = CreateScheduledMoves(block, operandStack);

			outgoingStack[block.Sequence] = operandStack;
			processed.Set(block.Sequence, true);

			foreach (var b in block.NextBlocks)
			{
				if (enqueued.Get(b.Sequence))
					continue;

				workList.Enqueue(new WorkItem(b, new Stack<Operand>(operandStack)));
				enqueued.Set(b.Sequence, true);
			}
		}

		/// <summary>
		/// Creates the scheduled moves.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="operandStack">The operand stack.</param>
		/// <returns></returns>
		private Stack<Operand> CreateScheduledMoves(BasicBlock block, Stack<Operand> operandStack)
		{
			if (scheduledMoves[block.Sequence] != null)
			{
				CreateOutgoingMoves(block, new Stack<Operand>(operandStack), new Stack<Operand>(scheduledMoves[block.Sequence]));
				operandStack = new Stack<Operand>(scheduledMoves[block.Sequence]);
				scheduledMoves[block.Sequence] = null;
			}
			return operandStack;
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
		/// Creates the moves for incoming stack.
		/// </summary>
		/// <param name="operandStack">The operand stack.</param>
		/// <returns></returns>
		private Stack<Operand> CreateMovesForIncomingStack(BasicBlock block, Stack<Operand> operandStack)
		{
			var joinStack = new Stack<Operand>();

			foreach (var operand in operandStack)
			{
				joinStack.Push(AllocateVirtualRegisterOrStackSlot(operand.Type));
			}

			foreach (var b in block.PreviousBlocks)
			{
				if (processed.Get(b.Sequence) && joinStack.Count > 0)
				{
					CreateOutgoingMoves(b, new Stack<Operand>(outgoingStack[b.Sequence]), new Stack<Operand>(joinStack));
					outgoingStack[b.Sequence] = new Stack<Operand>(joinStack);
				}
				else if (joinStack.Count > 0)
				{
					scheduledMoves[b.Sequence] = new Stack<Operand>(joinStack);
				}
			}
			return joinStack;
		}

		/// <summary>
		/// Creates the outgoing moves.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="operandStack">The operand stack.</param>
		/// <param name="joinStack">The join stack.</param>
		private void CreateOutgoingMoves(BasicBlock block, Stack<Operand> operandStack, Stack<Operand> joinStack)
		{
			var context = new Context(block.Last);

			context.GotoPrevious();

			while (context.Instruction.FlowControl == FlowControl.ConditionalBranch ||
				context.Instruction.FlowControl == FlowControl.UnconditionalBranch ||
				context.Instruction.FlowControl == FlowControl.Return ||
				context.Instruction == IRInstruction.Jmp)
			{
				context.GotoPrevious();
			}

			while (operandStack.Count > 0)
			{
				var operand = operandStack.Pop();
				var destination = joinStack.Pop();

				if (StoreOnStack(operand.Type))
				{
					context.AppendInstruction(IRInstruction.MoveCompound, destination, operand);
				}
				else
				{
					var moveInstruction = GetMoveInstruction(destination.Type);
					context.AppendInstruction(moveInstruction, destination, operand);
				}
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
