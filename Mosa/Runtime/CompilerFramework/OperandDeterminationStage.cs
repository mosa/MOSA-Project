/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using Mosa.Runtime.CompilerFramework.CIL;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// The Operand Determination Stage determines the operands for each instructions.
	/// </summary>
    public class OperandDeterminationStage : BaseStage, IMethodCompilerStage
    {
        #region Data members

        /// <summary>
        /// 
        /// </summary>
        private Stack<Operand> _operandStack = new Stack<Operand>();
        /// <summary>
        /// 
        /// </summary>
        private List<BasicBlock> _processed = new List<BasicBlock>();

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
            BasicBlock firstBlock = FindBlock(-1);

            AssignOperands(firstBlock);
        }


		/// <summary>
		/// Assigns the operands.
		/// </summary>
		/// <param name="block">The block.</param>
        private void AssignOperands(BasicBlock block)
        {
            for (Context ctx = new Context(InstructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
            {
                if (!(ctx.Instruction is IBranchInstruction) && !(ctx.Instruction is ICILInstruction))
                    continue;

                if (!(ctx.Instruction is IR.JmpInstruction))
                {
                    AssignOperandsFromCILStack(ctx, _operandStack);
                    (ctx.Instruction as ICILInstruction).Validate(ctx, MethodCompiler);
                    PushResultOperands(ctx, _operandStack);
                }

                if (ctx.Instruction is IBranchInstruction)
                {
                    Stack<Operand> initialStack = GetCurrentStack(_operandStack);
                    CreateTemporaryMoves(ctx, block, initialStack);
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
            if (_processed.Contains(block))
                return;
            _processed.Add(block);
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
            return !_processed.Contains(block);
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
		/// <param name="ctx">The CTX.</param>
		/// <param name="block">The block.</param>
		/// <param name="stack">The stack.</param>
       private void CreateTemporaryMoves(Context ctx, BasicBlock block, Stack<Operand> stack)
        {
            Context context = ctx.InsertBefore();
            context.SetInstruction(IR.Instruction.NopInstruction);

            BasicBlock nextBlock;

            //if (block.InitialStack == null)
            //    block.InitialStack = stack;

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
        private static bool NextBlockHasInitialStack(BasicBlock block, out BasicBlock nextBlock)
        {
            nextBlock = null;
            foreach (BasicBlock b in block.NextBlocks)
            {
                if (b.InitialStack == null)
                    continue;

                nextBlock = b;
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
            Stack<Operand> initialStack = GetCurrentStack(stack);
            Stack<Operand> nextInitialStack = GetCurrentStack(nextBlock.InitialStack);

            for (int i = 0; i < nextBlock.InitialStack.Count; ++i)
                ctx.AppendInstruction(IR.Instruction.MoveInstruction, nextInitialStack.Pop(), initialStack.Pop());

            if (nextBlock.InitialStack.Count > 0)
                foreach (BasicBlock nBlock in block.NextBlocks)
                    nBlock.InitialStack = GetCurrentStack(nextBlock.InitialStack);
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
                Operand temp = MethodCompiler.CreateTemporary(operand.Type);
                nextStack.Push(temp);
                _operandStack.Pop();
                ctx.AppendInstruction(IR.Instruction.MoveInstruction, temp, operand);
            }

            if (nextStack.Count > 0)
                foreach (BasicBlock nextBlock in block.NextBlocks)
                    nextBlock.InitialStack = GetCurrentStack(nextStack);
        }

        /// <summary>
        /// Assigns the operands from CIL stack.
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="currentStack">The current stack.</param>
        private void AssignOperandsFromCILStack(Context ctx, Stack<Operand> currentStack)
        {
            if (ctx.BasicBlock.InitialStack != null)
            {
                foreach (Operand operand in ctx.BasicBlock.InitialStack)
                    _operandStack.Push(operand);
                ctx.BasicBlock.InitialStack.Clear();
            }

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
