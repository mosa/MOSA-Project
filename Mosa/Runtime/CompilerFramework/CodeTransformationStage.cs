/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Base class for code transformation stages.
	/// </summary>
	public abstract class CodeTransformationStage : BaseStage, IMethodCompilerStage, IVisitor
	{

		#region Data members

		private readonly DataConverter LittleEndianBitConverter = DataConverter.LittleEndian;

		/// <summary>
		/// The architecture of the compilation process.
		/// </summary>
		protected IArchitecture Architecture;

		/// <summary>
		/// Holds the executing method compiler.
		/// </summary>
		protected IMethodCompiler Compiler;

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
		public override void Run(IMethodCompiler compiler)
		{
			base.Run(compiler);

			// Save the architecture & compiler
			Architecture = compiler.Architecture;
			Compiler = compiler;

			for (int index = 0; index < BasicBlocks.Count; index++)
				for (Context ctx = new Context(InstructionSet, BasicBlocks[index]); !ctx.EndOfInstruction; ctx.GotoNext())
					if (ctx.Instruction != null) 
						ctx.Clone().Visit(this);
		}

		/// <summary>
		/// Adds this stage to the given pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add this stage to.</param>
		public abstract void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline);

		#endregion // IMethodCompilerStage Members

		#region Block Operations

		/// <summary>
		/// Links the new Blocks.
		/// </summary>
		/// <param name="blocks">The Blocks.</param>
		/// <param name="currentBlock">The current block.</param>
		/// <param name="nextBlock">The next block.</param>
		protected void LinkBlocks(Context[] blocks, Context currentBlock, Context nextBlock)
		{
			// Create label to block dictionary
			Dictionary<int, BasicBlock> blockLabels = new Dictionary<int, BasicBlock>();

			foreach (Context ctx in blocks)
				blockLabels.Add(ctx.BasicBlock.Label, ctx.BasicBlock);

			AddBlockLabels(blockLabels, nextBlock.BasicBlock);
			AddBlockLabels(blockLabels, currentBlock.BasicBlock);
		}

		/// <summary>
		/// Adds the block labels.
		/// </summary>
		/// <param name="blockLabels">The block labels.</param>
		/// <param name="basicBlock">The basic block.</param>
		private static void AddBlockLabels(IDictionary<int, BasicBlock> blockLabels, BasicBlock basicBlock)
		{
			if (basicBlock != null) {
				foreach (BasicBlock block in basicBlock.NextBlocks)
					if (!blockLabels.ContainsKey(block.Label))
						blockLabels.Add(block.Label, block);

				if (!blockLabels.ContainsKey(basicBlock.Label))
					blockLabels.Add(basicBlock.Label, basicBlock);
			}
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <returns></returns>
		protected Context CreateEmptyBlockContext()
		{
			Context ctx = new Context(InstructionSet, -1);
			BasicBlock block = new BasicBlock(BasicBlocks.Count + 0x10000000);
			ctx.BasicBlock = block;
			BasicBlocks.Add(block);

			// Need a dummy instruction at the start of each block to establish an index
			ctx.InsertInstructionAfter(null);
			block.Index = ctx.Index;
			ctx.Ignore = true;

			return ctx;
		}

		/// <summary>
		/// Creates empty Blocks.
		/// </summary>
		/// <param name="blocks">The Blocks.</param>
		/// <returns></returns>
		protected Context[] CreateEmptyBlockContexts(int blocks)
		{
			// Allocate the block array
			Context[] result = new Context[blocks];

			for (int index = 0; index < blocks; index++)
				result[index] = CreateEmptyBlockContext();

			return result;
		}

		/// <summary>
		/// Splits the block.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <returns></returns>
		protected Context SplitContext(Context ctx)
		{
			int label = BasicBlocks.Count + 0x10000000;

			BasicBlock nextBlock = new BasicBlock(label);

			foreach (BasicBlock block in ctx.BasicBlock.NextBlocks)
				nextBlock.NextBlocks.Add(block);

			ctx.BasicBlock.NextBlocks.Clear();
			ctx.BasicBlock.NextBlocks.Add(nextBlock);

			ctx.InsertInstructionAfter(IR.Instruction.JmpInstruction, nextBlock);
			ctx.SliceAfter();

			nextBlock.Index = ctx.Next.Index;

			// FIXME PG - does not handle split on last instruction

			return new Context(InstructionSet, nextBlock);
		}

		#endregion

		#region Emit Methods

		/// <summary>
		/// Emits the constant operands.
		/// </summary>
		/// <param name="ctx">The context.</param>
		protected void EmitOperandConstants(Context ctx)
		{
			if (ctx.OperandCount > 0)
				ctx.Operand1 = EmitConstant(ctx.Operand1);
			else if (ctx.OperandCount > 1)
				ctx.Operand2 = EmitConstant(ctx.Operand2);
			else if (ctx.OperandCount > 2)
				ctx.Operand3 = EmitConstant(ctx.Operand3);
		}

		/// <summary>
		/// Emits the constant operands.
		/// </summary>
		/// <param name="ctx">The context.</param>
		protected void EmitResultConstants(Context ctx)
		{
			if (ctx.ResultCount > 0)
				ctx.Result = EmitConstant(ctx.Result);
			else if (ctx.OperandCount > 1)
				ctx.Result2 = EmitConstant(ctx.Result2);
		}

		/// <summary>
		/// This function emits a constant variable into the read-only data section.
		/// </summary>
		/// <param name="op">The operand to emit as a constant.</param>
		/// <returns>An operand, which represents the reference to the read-only constant.</returns>
		/// <remarks>
		/// This function checks if the given operand needs to be moved into the read-only data
		/// section of the executable. For x86 this concerns only floating point operands, as these
		/// can't be specified in inline assembly.<para/>
		/// This function checks if the given operand needs to be moved and rewires the operand, if
		/// it must be moved.
		/// </remarks>
		protected Operand EmitConstant(Operand op)
		{
			ConstantOperand cop = op as ConstantOperand;
			if (cop != null && cop.StackType == StackTypeCode.F) {
				int size, alignment;
				Architecture.GetTypeRequirements(cop.Type, out size, out alignment);

				string name = String.Format("C_{0}", Guid.NewGuid());
				using (Stream stream = Compiler.Linker.Allocate(name, SectionKind.ROData, size, alignment)) {
					byte[] buffer;

					switch (cop.Type.Type) {
						case CilElementType.R4:
							buffer = LittleEndianBitConverter.GetBytes((float)cop.Value);
							break;

						case CilElementType.R8:
							buffer = LittleEndianBitConverter.GetBytes((double)cop.Value);
							break;

						default:
							throw new NotSupportedException();
					}

					stream.Write(buffer, 0, buffer.Length);
				}

				// FIXME: Attach the label operand to the linker symbol
				// FIXME: Rename the LabelOperand to SymbolOperand
				// FIXME: Use the provided name to link
				LabelOperand lop = new LabelOperand(cop.Type, name);
				op = lop;
			}
			return op;
		}

		#endregion // Emit Methods
	}
}
