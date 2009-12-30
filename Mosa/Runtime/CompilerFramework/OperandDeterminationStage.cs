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
        public void Run ()
        {
            BasicBlock firstBlock = FindBlock(-1);

            AssignOperands(firstBlock);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        private void AssignOperands (BasicBlock block)
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
                if (IsNotProcessed (b))
                    AssignOperands(b);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        private void MarkAsProcessed (BasicBlock block)
        {
            if (_processed.Contains(block))
                return;
            _processed.Add(block);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        private bool IsNotProcessed(BasicBlock block)
        {
            return !_processed.Contains(block);
        }

        /// <summary>
        /// Gets the current stack.
        /// </summary>
        /// <param name="stack">The stack.</param>
        /// <returns></returns>
        private static Stack<Operand> GetCurrentStack (Stack<Operand> stack)
        {
            Stack<Operand> result = new Stack<Operand>();
            Operand[] copy = new Operand[stack.Count];
            stack.CopyTo(copy, 0);

            foreach (Operand operand in copy)
                result.Push(operand);
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx">A <see cref="Context"/></param>
        /// <param name="block">A <see cref="BasicBlock"/></param>
        /// <param name="stack"></param>
        private void CreateTemporaryMoves (Context ctx, BasicBlock block, Stack<Operand> stack)
        {
            Context context = ctx.InsertBefore();
            context.SetInstruction(IR.Instruction.NopInstruction);

            BasicBlock nextBlock;

            //if (block.InitialStack == null)
            //    block.InitialStack = stack;

            if (NextBlockHasInitialStack (block, out nextBlock))
                LinkTemporaryMoves(context, block, nextBlock, stack);
            else
                CreateNewTemporaryMoves(context, block, stack);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="block">A <see cref="BasicBlock"/></param>
        /// <param name="nextBlock">A <see cref="BasicBlock"/></param>
        /// <returns>A <see cref="System.Boolean"/></returns>
        private static bool NextBlockHasInitialStack (BasicBlock block, out BasicBlock nextBlock)
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
        ///
        /// </summary>
        /// <param name="ctx">A <see cref="Context"/></param>
        /// <param name="block">A <see cref="BasicBlock"/></param>
        /// <param name="nextBlock">A <see cref="BasicBlock"/></param>
        /// <param name="stack"></param>
        private void LinkTemporaryMoves(Context ctx, BasicBlock block, BasicBlock nextBlock, Stack<Operand> stack)
        {
            Stack<Operand> initialStack = GetCurrentStack(stack);
            Stack<Operand> nextInitialStack = GetCurrentStack(nextBlock.InitialStack);

            for (int i = 0; i < nextBlock.InitialStack.Count; ++i)
                ctx.AppendInstruction(IR.Instruction.MoveInstruction, nextInitialStack.Pop(), initialStack.Pop());

            if (nextBlock.InitialStack.Count > 0)
                foreach (BasicBlock nBlock in block.NextBlocks)
                    nBlock.InitialStack = GetCurrentStack (nextBlock.InitialStack);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx">A <see cref="Context"/></param>
        /// <param name="block">A <see cref="BasicBlock"/></param>
        /// <param name="stack"></param>
        private void CreateNewTemporaryMoves (Context ctx, BasicBlock block, Stack<Operand> stack)
        {
            Stack<Operand> initialStack = GetCurrentStack(stack);
            Stack<Operand> nextStack = new Stack<Operand>();
            for (int i = 0; i < stack.Count; ++i)
            {
                Operand operand = initialStack.Pop();
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
        private void AssignOperandsFromCILStack (Context ctx, Stack<Operand> currentStack)
        {
            for (int index = ctx.OperandCount - 1; index >= 0; --index)
            {
                if (ctx.GetOperand (index) != null)
                    continue;
         
                if (ctx.BasicBlock.InitialStack != null && ctx.BasicBlock.InitialStack.Count > 0)
                    ctx.SetOperand (index, ctx.BasicBlock.InitialStack.Pop());
                else if (currentStack.Count > 0)
                    ctx.SetOperand (index, currentStack.Pop());
            }

            if (ctx.BasicBlock.InitialStack != null)
                foreach (Operand operand in ctx.BasicBlock.InitialStack)
                    _operandStack.Push(operand);
        }

        /// <summary>
        /// Pushes the result operands on to the stack
        /// </summary>
        /// <param name="ctx">The context.</param>
        /// <param name="currentStack">The current stack.</param>
        private static void PushResultOperands (Context ctx, Stack<Operand> currentStack)
        {
            if ((ctx.Instruction as ICILInstruction).PushResult)
                foreach (Operand operand in ctx.Results)
                    currentStack.Push (operand);
        }
        
        #endregion
    }
}
