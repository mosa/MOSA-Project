/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using System.Diagnostics;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.CompilerFramework.CIL;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// The Operand Determination Stage determines the operands for each instructions.
	/// </summary>
	public class OperandDeterminationStage : BaseMethodCompilerStage, IMethodCompilerStage
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private Stack<Operand> operandStack = new Stack<Operand>();
		/// <summary>
		/// 
		/// </summary>
		private List<BasicBlock> processed = new List<BasicBlock>();

		/// <summary>
		/// 
		/// </summary>
		private Stack<Operand>[] initialStack;

		#endregion

		#region IPipelineStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name
		{
			get { return "Operand Determination Stage"; }
		}

		#endregion

		#region IMethodCompilerStage Members

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

			initialStack = null;
			operandStack = null;
			processed = null;
		}

		/// <summary>
		/// Traces the specified label.
		/// </summary>
		/// <param name="label">The label.</param>
		private void Trace(int label, ExceptionClause clause)
		{
			initialStack = new Stack<Operand>[basicBlocks.Count];
			operandStack.Clear();

			if (clause != null && clause.Kind != ExceptionClauseType.Finally)
			{
				Token token =  new Token(clause.ClassToken);
			}

			BasicBlock firstBlock = FindBlock(label);
			AssignOperands(firstBlock);
		}

		/// <summary>
		/// Assigns the operands.
		/// </summary>
		/// <param name="block">The block.</param>
		private void AssignOperands(BasicBlock block)
		{
			//Debug.WriteLine(@"OperandDeterminationStage: Assigning operands to block " + block);

			if (initialStack[block.Sequence] != null)
				foreach (Operand operand in initialStack[block.Sequence])
					operandStack.Push(operand);

			for (Context ctx = new Context(instructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
			{
				if (!(ctx.Instruction is IBranchInstruction) && !(ctx.Instruction is ICILInstruction))
					continue;

				if (!(ctx.Instruction is IR.JmpInstruction))
				{
					AssignOperandsFromCILStack(ctx, operandStack);
					(ctx.Instruction as ICILInstruction).Validate(ctx, methodCompiler);
					PushResultOperands(ctx, operandStack);
				}

				if (ctx.Instruction is IBranchInstruction)
				{
					Stack<Operand> stack = GetCurrentStack(operandStack);
					CreateTemporaryMoves(ctx, block, stack);
					break;
				}
			}

			MarkAsProcessed(block);

			foreach (BasicBlock b in block.NextBlocks)
			{
				if (IsNotProcessed(b))
					AssignOperands(b);
			}
		}

		/// <summary>
		/// Marks as processed.
		/// </summary>
		/// <param name="block">The block.</param>
		private void MarkAsProcessed(BasicBlock block)
		{
			if (processed.Contains(block))
				return;
			processed.Add(block);
		}

		/// <summary>
		/// Determines whether [is not processed] [the specified block].
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns>
		/// 	<c>true</c> if [is not processed] [the specified block]; otherwise, <c>false</c>.
		/// </returns>
		private bool IsNotProcessed(BasicBlock block)
		{
			return !processed.Contains(block);
		}

		/// <summary>
		/// Gets the current stack.
		/// </summary>
		/// <param name="stack">The stack.</param>
		/// <returns></returns>
		private static Stack<Operand> GetCurrentStack(Stack<Operand> stack)
		{
			Stack<Operand> result = new Stack<Operand>();

			foreach (Operand operand in stack)
				result.Push(operand);
			return result;
		}

		/// <summary>
		/// Creates the temporary moves.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="block">The block.</param>
		/// <param name="stack">The stack.</param>
		private void CreateTemporaryMoves(Context ctx, BasicBlock block, Stack<Operand> stack)
		{
			Context context = ctx.InsertBefore();
			context.SetInstruction(IR.Instruction.NopInstruction);

			BasicBlock nextBlock;

			if (NextBlockHasInitialStack(block, out nextBlock))
				LinkTemporaryMoves(context, block, nextBlock, stack);
			else
				CreateNewTemporaryMoves(context, block, stack);
		}

		/// <summary>
		/// Nexts the block has initial stack.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="nextBlock">The next block.</param>
		/// <returns></returns>
		private bool NextBlockHasInitialStack(BasicBlock block, out BasicBlock nextBlock)
		{
			nextBlock = null;
			foreach (BasicBlock next in block.NextBlocks)
			{
				if (initialStack[next.Sequence] == null)
					continue;

				nextBlock = next;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Links the temporary moves.
		/// </summary>
		/// <param name="ctx">The CTX.</param>
		/// <param name="block">The block.</param>
		/// <param name="nextBlock">The next block.</param>
		/// <param name="stack">The stack.</param>
		private void LinkTemporaryMoves(Context ctx, BasicBlock block, BasicBlock nextBlock, Stack<Operand> stack)
		{
			Stack<Operand> currentStack = GetCurrentStack(stack);
			Stack<Operand> nextInitialStack = GetCurrentStack(initialStack[nextBlock.Sequence]);

			for (int i = 0; i < initialStack[nextBlock.Sequence].Count; ++i)
				ctx.AppendInstruction(IR.Instruction.MoveInstruction, nextInitialStack.Pop(), currentStack.Pop());

			if (initialStack[nextBlock.Sequence].Count > 0)
				foreach (BasicBlock nBlock in block.NextBlocks)
					initialStack[nBlock.Sequence] = GetCurrentStack(initialStack[nextBlock.Sequence]);
		}

		/// <summary>
		/// Creates the new temporary moves.
		/// </summary>
		/// <param name="ctx">The CTX.</param>
		/// <param name="block">The block.</param>
		/// <param name="stack">The stack.</param>
		private void CreateNewTemporaryMoves(Context ctx, BasicBlock block, Stack<Operand> stack)
		{
			Stack<Operand> nextStack = new Stack<Operand>();
			foreach (Operand operand in stack)
			{
				Operand temp = methodCompiler.CreateTemporary(operand.Type);
				nextStack.Push(temp);
				operandStack.Pop();
				ctx.AppendInstruction(IR.Instruction.MoveInstruction, temp, operand);
			}

			if (nextStack.Count > 0)
				foreach (BasicBlock nextBlock in block.NextBlocks)
					initialStack[nextBlock.Sequence] = GetCurrentStack(nextStack);
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
			if ((ctx.Instruction as ICILInstruction).PushResult)
				foreach (Operand operand in ctx.Results)
					currentStack.Push(operand);
		}

		#endregion
	}
}
