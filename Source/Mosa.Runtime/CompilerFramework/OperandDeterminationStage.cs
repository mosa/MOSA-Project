using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.CompilerFramework.CIL;
using System.Collections;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// 
	/// </summary>
	public class OperandDeterminationStage : BaseMethodCompilerStage, IMethodCompilerStage
	{
		/// <summary>
		/// 
		/// </summary>
		private class WorkItem
		{
			/// <summary>
			/// 
			/// </summary>
			public BasicBlock Block;
			/// <summary>
			/// 
			/// </summary>
			public Stack<Operand> IncomingStack;

			/// <summary>
			/// Initializes a new instance of the <see cref="WorkItem"/> class.
			/// </summary>
			/// <param name="block">The block.</param>
			/// <param name="incomingStack">The incoming stack.</param>
			public WorkItem(BasicBlock block, Stack<Operand> incomingStack)
			{
				this.Block = block;
				this.IncomingStack = incomingStack;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private Queue<WorkItem> workList = new Queue<WorkItem>();
		/// <summary>
		/// 
		/// </summary>
		private BitArray processed;
		/// <summary>
		/// 
		/// </summary>
		private BitArray enqueued;
		/// <summary>
		/// 
		/// </summary>
		private Stack<Operand>[] outgoingStack;
		/// <summary>
		/// 
		/// </summary>
		private Stack<Operand>[] scheduledMoves;

		/// <summary>
		/// Runs the specified compiler.
		/// </summary>
		public void Run()
		{
			// Main Code
			Trace(-1, null);

			// Handler Code
			foreach (ExceptionClause clause in methodCompiler.ExceptionClauseHeader.Clauses)
			{
				Trace(clause.HandlerOffset, clause);
			}
		}

		/// <summary>
		/// Traces the specified label.
		/// </summary>
		/// <param name="label">The label.</param>
		private void Trace(int label, ExceptionClause clause)
		{
			this.outgoingStack = new Stack<Operand>[basicBlocks.Count];
			this.scheduledMoves = new Stack<Operand>[basicBlocks.Count];
			this.processed = new BitArray(basicBlocks.Count);
			this.processed.SetAll(false);
			this.enqueued = new BitArray(basicBlocks.Count);
			this.enqueued.SetAll(false);

			if (clause != null && clause.Kind != ExceptionClauseType.Finally)
			{
				var token = new Token(clause.ClassToken);
			}

			var firstBlock = FindBlock(label);
			this.processed.Set(firstBlock.Sequence, true);
			this.workList.Enqueue(new WorkItem(firstBlock, new Stack<Operand>()));	

			while (workList.Count > 0)
				AssignOperands(workList.Dequeue());
		}

		/// <summary>
		/// Assigns the operands.
		/// </summary>
		/// <param name="workItem">The work item.</param>
		private void AssignOperands(WorkItem workItem)
		{
			var operandStack = workItem.IncomingStack;
			var block = workItem.Block;

			operandStack = this.CreateMovesForIncomingStack(block, operandStack);
			this.AssignOperands(block, operandStack);
			operandStack = this.CreateScheduledMoves(block, operandStack);
			
			this.outgoingStack[block.Sequence] = operandStack;
			this.processed.Set(block.Sequence, true);

			foreach (var b in block.NextBlocks)
			{
				if (this.enqueued.Get(b.Sequence))
					continue;

				this.workList.Enqueue(new WorkItem(b, new Stack<Operand>(operandStack)));
				this.enqueued.Set(b.Sequence, true);
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
			if (this.scheduledMoves[block.Sequence] != null)
			{
				this.CreateOutgoingMoves(block, new Stack<Operand>(operandStack), new Stack<Operand>(this.scheduledMoves[block.Sequence]));
				operandStack = new Stack<Operand>(this.scheduledMoves[block.Sequence]);
				this.scheduledMoves[block.Sequence] = null;
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
			for (var ctx = new Context(instructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
			{
				if (!(ctx.Instruction is IBranchInstruction) && !(ctx.Instruction is ICILInstruction))
					continue;

				if (!(ctx.Instruction is IR.JmpInstruction))
				{
					AssignOperandsFromCILStack(ctx, operandStack);
					(ctx.Instruction as ICILInstruction).Validate(ctx, methodCompiler);
					PushResultOperands(ctx, operandStack);
				}
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
				joinStack.Push(this.methodCompiler.CreateTemporary(operand.Type));
			}

			foreach (var b in block.PreviousBlocks)
			{
				if (this.processed.Get(b.Sequence) && joinStack.Count > 0)
				{
					CreateOutgoingMoves(b, new Stack<Operand>(this.outgoingStack[b.Sequence]), new Stack<Operand>(joinStack));
					this.outgoingStack[b.Sequence] = new Stack<Operand>(joinStack);
				}
				else if (joinStack.Count > 0)
				{
					this.scheduledMoves[b.Sequence] = new Stack<Operand>(joinStack);
				}
			}
			return joinStack;
		}

		/// <summary>
		/// Creates the outgoing moves.
		/// </summary>
		/// <param name="b">The b.</param>
		/// <param name="operandStack">The operand stack.</param>
		/// <param name="joinStack">The join stack.</param>
		private void CreateOutgoingMoves(BasicBlock b, Stack<Operand> operandStack, Stack<Operand> joinStack)
		{

			var context = new Context(this.instructionSet, b);
			while (!context.EndOfInstruction && !(context.Instruction is IBranchInstruction))
				context.GotoNext();

			while (operandStack.Count > 0)
			{
				var operand = operandStack.Pop();
				var destination = joinStack.Pop();
				context.InsertBefore().SetInstruction(IR.Instruction.MoveInstruction, destination, operand);
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
		private static void PushResultOperands(Context ctx, Stack<Operand> currentStack)
		{
			if (!(ctx.Instruction as ICILInstruction).PushResult)
				return;

			foreach (Operand operand in ctx.Results)
				currentStack.Push(operand);
		}

	}
}
