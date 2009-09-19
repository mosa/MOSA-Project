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
using System.Diagnostics;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

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

			/// <summary>
			/// Holds the list of instructions
			/// </summary>
			InstructionSet _instructions;

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
			/// Gets or sets the instruction set.
			/// </summary>
			public InstructionSet Instructions
			{
				get { return _instructions; }
				set { _instructions = value; }
			}

			/// <summary>
			/// Gets or sets the instruction index currently processed.
			/// </summary>
			public int Index
			{
				get { return _index; }
				set { _index = value; }
			}

			/// <summary>
			/// Gets the result operand.
			/// </summary>
			/// <value>The result operand.</value>
			public Operand Result
			{
				get { return _instructions.instructions[_index].Result; }
			}

			/// <summary>
			/// Gets the second result operand.
			/// </summary>
			/// <value>The second result operand.</value>
			public Operand Result2
			{
				get { return _instructions.instructions[_index].Result2; }
			}

			/// <summary>
			/// Gets the first operand.
			/// </summary>
			/// <value>The first operand.</value>
			public Operand Operand1
			{
				get { return _instructions.instructions[_index].Operand1; }
			}

			/// <summary>
			/// Gets the first operand.
			/// </summary>
			/// <value>The first operand.</value>
			public Operand Operand2
			{
				get { return _instructions.instructions[_index].Operand2; }
			}

			/// <summary>
			/// Gets the first operand.
			/// </summary>
			/// <value>The first operand.</value>
			public Operand Operand3
			{
				get { return _instructions.instructions[_index].Operand3; }
			}

			/// <summary>
			/// Gets the operand count.
			/// </summary>
			/// <value>The operand count.</value>
			public byte OperandCount
			{
				get { return _instructions.instructions[_index].OperandCount; }
			}

			/// <summary>
			/// Gets the result count.
			/// </summary>
			/// <value>The result count.</value>
			public byte ResultCount
			{
				get { return _instructions.instructions[_index].ResultCount; }
			}

			/// <summary>
			/// Holds the function being called.
			/// </summary>
			public RuntimeMethod InvokeTarget
			{
				get { return _instructions.instructions[_index].InvokeTarget; }
			}

			/// <summary>
			/// Holds the string.
			/// </summary>
			public string String
			{
				get { return _instructions.instructions[_index].String; }
			}

			/// <summary>
			/// Holds the field of the load instruction.
			/// </summary>
			public RuntimeField RuntimeField
			{
				get { return _instructions.instructions[_index].RuntimeField; }
			}

			/// <summary>
			/// Holds the token type.
			/// </summary>
			/// <value>The token.</value>
			public TokenTypes Token
			{
				get { return _instructions.instructions[_index].Token; }
			}
		};

		#endregion // Types

		#region Data members

		/// <summary>
		/// The architecture of the compilation process.
		/// </summary>
		protected IArchitecture Architecture;

		/// <summary>
		/// Holds the list of basic Blocks.
		/// </summary>
		protected List<BasicBlock> Blocks;

		/// <summary>
		/// Holds the executing method compiler.
		/// </summary>
		protected IMethodCompiler Compiler;

		/// <summary>
		/// Holds the current block.
		/// </summary>
		protected int CurrentBlock;

		#endregion // Data members

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
				throw new InvalidOperationException(@"Instruction stream must have been split to basic Blocks.");

			// Save the architecture & compiler
			Architecture = compiler.Architecture;
			Compiler = compiler;
			Blocks = blockProvider.Blocks;

			Context ctx = new Context();
			for (CurrentBlock = 0; CurrentBlock < Blocks.Count; CurrentBlock++) {
				BasicBlock block = Blocks[CurrentBlock];
				ctx.Block = block;
				for (ctx.Index = 0; ctx.Index < block.Instructions.Count; ctx.Index++) {
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
			Trace.WriteLine(String.Format(@"Unknown instruction {0} has visited stage {1}.", instruction.GetType().FullName, Name));
			throw new NotSupportedException();
		}

		#endregion // IInstructionVisitor<Context> Members

		#region Methods

		/// <summary>
		/// Removes the current instruction From the instruction stream.
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
		/// Links the Blocks.
		/// </summary>
		/// <param name="from">The block issuing the jump.</param>
		/// <param name="to">The block, where From is jumping to.</param>
		protected void LinkBlocks(BasicBlock from, BasicBlock to)
		{
			Debug.Assert(false == from.NextBlocks.Contains(to), @"A link already exists?");
			Debug.Assert(false == to.PreviousBlocks.Contains(from), @"A link already exists?");
			from.NextBlocks.Add(to);
			to.PreviousBlocks.Add(from);
		}

		/// <summary>
		/// Links the new Blocks.
		/// </summary>
		/// <param name="blocks">The Blocks.</param>
		/// <param name="currentBlock">The current block.</param>
		/// <param name="nextBlock">The next block.</param>
		protected void LinkBlocks(BasicBlock[] blocks, BasicBlock currentBlock, BasicBlock nextBlock)
		{
			// Create label to block dictionary
			Dictionary<int, BasicBlock> blockLabels = new Dictionary<int, BasicBlock>();

			foreach (BasicBlock block in blocks)
				blockLabels.Add(block.Label, block);

			AddBlockLabels(ref blockLabels, blocks, nextBlock);	
			AddBlockLabels(ref blockLabels, blocks, currentBlock);
			UpdateBlockLinks(blocks, blockLabels);
		}
		
		private void AddBlockLabels(ref Dictionary<int, BasicBlock> blockLabels, BasicBlock[] blocks, BasicBlock basicBlock)
		{
			if (basicBlock != null) {
				foreach (BasicBlock block in basicBlock.NextBlocks)
					if (!blockLabels.ContainsKey(block.Label))
						blockLabels.Add(block.Label, block);

				if (!blockLabels.ContainsKey(basicBlock.Label))
					blockLabels.Add(basicBlock.Label, basicBlock);
			}
		}
		
		private void UpdateBlockLinks(BasicBlock[] blocks, Dictionary<int, BasicBlock> blockLabels)
		{
			foreach (BasicBlock block in blocks)
			{
				foreach (Instruction instruction in block.Instructions)
				{
					if (instruction is IBranchInstruction)
					{
						foreach (int label in (instruction as IBranchInstruction).BranchTargets) 
						{
							BasicBlock next = blockLabels[label];
							if (!block.NextBlocks.Contains(next))
								block.NextBlocks.Add(next);
							if (!next.PreviousBlocks.Contains(block))
								next.PreviousBlocks.Add(block);
						}
					}
				}
			}
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <returns></returns>
		protected BasicBlock CreateEmptyBlock()
		{
			BasicBlock block = new BasicBlock(Blocks.Count + 0x10000000);
			block.Index = Blocks.Count;
			Blocks.Add(block);
			return block;
		}

		/// <summary>
		/// Creates empty Blocks.
		/// </summary>
		/// <param name="blocks">The Blocks.</param>
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
			int label = Blocks.Count + 0x10000000;

			BasicBlock nextBlock = ctx.Block.Split(ctx.Index + 1, label);
			nextBlock.Index = Blocks.Count - 1;
			Blocks.Add(nextBlock);

			foreach (BasicBlock block in ctx.Block.NextBlocks)
				nextBlock.NextBlocks.Add(block);

			ctx.Block.NextBlocks.Clear();

			if (insert != null) 
				InsertLabel(ref ctx, ref insert, insert.Label);
			else 
				InsertLabel(ref ctx, ref nextBlock, label);

			return nextBlock;
		}
		
		private void InsertLabel(ref Context ctx, ref BasicBlock block, int label)
		{
			ctx.Block.NextBlocks.Add(block);
			block.PreviousBlocks.Add(ctx.Block);
			ctx.Block.Instructions.Add(new IR.JmpInstruction(label));
		}

		#endregion

	}
}
