/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Simon Wollwage (<mailto:kintaro@think-in-co.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Base class for code transformation stages.
    /// </summary>
    public abstract class CodeTransformationStage : IMethodCompilerStage, IInstructionVisitor<CodeTransformationStage.Context>
    {
        #region Types

        /// <summary>
        /// Provides context for transformations.
        /// </summary>
        public class Context
        {
            #region Data members

            /// <summary>
            /// Holds the block being operated on.
            /// </summary>
            private BasicBlock _block;

            /// <summary>
            /// Holds the instruction index operated on.
            /// </summary>
            private int _index;

            #endregion // Data members

            /// <summary>
            /// Gets or sets the basic block currently processed.
            /// </summary>
            public BasicBlock Block
            {
                get { return _block; }
                set { _block = value; }
            }

            /// <summary>
            /// Gets or sets the instruction index currently processed.
            /// </summary>
            public int Index
            {
                get { return _index; }
                set { _index = value; }
            }
        };

        #endregion // Types

        #region Data members

        /// <summary>
        /// The architecture of the compilation process.
        /// </summary>
        protected IArchitecture _architecture;

        /// <summary>
        /// Holds the list of basic blocks.
        /// </summary>
        protected List<BasicBlock> _blocks;

        /// <summary>
        /// Holds the executing method compiler.
        /// </summary>
        protected IMethodCompiler _compiler;

        /// <summary>
        /// Holds the current block.
        /// </summary>
        protected int _currentBlock;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeTransformationStage"/> class.
        /// </summary>
        protected CodeTransformationStage()
        {
        }

        #endregion // Construction

        #region IMethodCompilerStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public abstract string Name { get; }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public void Run(IMethodCompiler compiler)
        {
            if (null == compiler)
                throw new ArgumentNullException(@"compiler");
            IBasicBlockProvider blockProvider = (IBasicBlockProvider)compiler.GetPreviousStage(typeof(IBasicBlockProvider));
            if (null == blockProvider)
                throw new InvalidOperationException(@"Instruction stream must have been split to basic blocks.");

            // Save the architecture & compiler
            _architecture = compiler.Architecture;
            _compiler = compiler;
            _blocks = blockProvider.Blocks;

            Context ctx = new Context();
            for (_currentBlock = 0; _currentBlock < _blocks.Count; _currentBlock++)
            {
                BasicBlock block = _blocks[_currentBlock];
                ctx.Block = block;
                for (ctx.Index = 0; ctx.Index < block.Instructions.Count; ctx.Index++)
                {
                    block.Instructions[ctx.Index].Visit(this, ctx);
                }
            }
        }

        /// <summary>
        /// Adds this stage to the given pipeline.
        /// </summary>
        /// <param name="pipeline">The pipeline to add this stage to.</param>
        public abstract void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline);

        #endregion // IMethodCompilerStage Members

        #region IInstructionVisitor<Context> Members

        /// <summary>
        /// Visitation method for instructions not caught by more specific visitation methods.
        /// </summary>
        /// <param name="instruction">The visiting instruction.</param>
        /// <param name="arg">A visitation context argument.</param>
        public virtual void Visit(Instruction instruction, Context arg)
        {
            Trace.WriteLine(String.Format(@"Unknown instruction {0} has visited stage {1}.", instruction.GetType().FullName, this.Name));
            throw new NotSupportedException();
        }

        #endregion // IInstructionVisitor<Context> Members

        #region Methods

        /// <summary>
        /// Removes the current instruction from the instruction stream.
        /// </summary>
        /// <param name="ctx">The context of the instruction to remove.</param>
        protected void Remove(Context ctx)
        {
            Remove(ctx.Block.Instructions[ctx.Index]);
            ctx.Block.Instructions.RemoveAt(ctx.Index--);
        }

        /// <summary>
        /// Removes the specified instruction.
        /// </summary>
        /// <param name="instruction">The instruction to remove.</param>
        protected void Remove(Instruction instruction)
        {
            // FIXME: Remove this sequence, once we have a smart instruction collection which does this.
            for (int i = 0; i < instruction.Operands.Length; i++)
                instruction.SetOperand(i, null);
            for (int i = 0; i < instruction.Results.Length; i++)
                instruction.SetResult(i, null);
        }

        /// <summary>
        /// Replaces the currently processed instruction with another instruction.
        /// </summary>
        /// <param name="arg">The transformation context.</param>
        /// <param name="instruction">The instruction to replace with.</param>
        protected void Replace(Context arg, Instruction instruction)
        {
            arg.Block.Instructions[arg.Index] = instruction;
        }

        /// <summary>
        /// Replaces the currently processed instruction with a set of instruction.
        /// </summary>
        /// <param name="arg">The transformation context.</param>
        /// <param name="instructions">The instructions to replace with.</param>
        protected void Replace(Context arg, IEnumerable<Instruction> instructions)
        {
            List<Instruction> insts = arg.Block.Instructions;
            insts.RemoveAt(arg.Index);
            int oldCount = insts.Count;
            insts.InsertRange(arg.Index, instructions);
            arg.Index += (insts.Count - oldCount) - 1;
        }

        #endregion // Methods

		#region Block Operations

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="from">The block issuing the jump.</param>
		/// <param name="to">The block, where from is jumping to.</param>
		protected void LinkBlocks(BasicBlock from, BasicBlock to)
		{
			Debug.Assert(false == from.NextBlocks.Contains(to), @"A link already exists?");
			Debug.Assert(false == to.PreviousBlocks.Contains(from), @"A link already exists?");
			from.NextBlocks.Add(to);
			to.PreviousBlocks.Add(from);
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <returns></returns>
		protected BasicBlock CreateEmptyBlock()
		{
			BasicBlock block = new BasicBlock(_blocks.Count + 0x10000000);
			block.Index = _blocks.Count;
			_blocks.Add(block);
			return block;
		}

		/// <summary>
		/// Creates empty blocks.
		/// </summary>
		/// <param name="blocks">The blocks.</param>
		/// <returns></returns>
		protected BasicBlock[] CreateEmptyBlocks(int blocks)
		{
			// Allocate the block array
			BasicBlock[] result = new BasicBlock[blocks];

			for (int index = 0; index < blocks; index++)
				result[index] = CreateEmptyBlock();

			return result;
		}

		/// <summary>
		/// Splits the block.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="instruction">The instruction index to split on.</param>
		/// <param name="insert">The insert to be called after the split.</param>
		/// <returns></returns>
		protected BasicBlock SplitBlock(Context ctx, Instruction instruction, BasicBlock insert)
		{
			int label = _blocks.Count + 0x10000000;

			BasicBlock nextBlock = ctx.Block.Split(ctx.Index + 1, label);
			nextBlock.Index = _blocks.Count - 1;
			_blocks.Add(nextBlock);

			foreach (BasicBlock block in ctx.Block.NextBlocks)
				nextBlock.NextBlocks.Add(block);

			ctx.Block.NextBlocks.Clear();

			if (insert != null) {
				ctx.Block.NextBlocks.Add(insert);
				insert.PreviousBlocks.Add(ctx.Block);
				ctx.Block.Instructions.Add(new IR.JmpInstruction(insert.Label));
			}
			else {
				ctx.Block.NextBlocks.Add(nextBlock);
				nextBlock.PreviousBlocks.Add(ctx.Block);
				ctx.Block.Instructions.Add(new IR.JmpInstruction(label));
			}

			return nextBlock;
		}

		#endregion

    }
}
