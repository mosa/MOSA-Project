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
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Compiler.Common;
using Mosa.Compiler.Linker;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Base class for code transformation stages.
	/// </summary>
	public abstract class BaseCodeTransformationStage : BaseMethodCompilerStage, IMethodCompilerStage, IVisitor, IPipelineStage
	{

		#region Data members

		private readonly DataConverter LittleEndianBitConverter = DataConverter.LittleEndian;

		#endregion // Data members

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value></value>
		string IPipelineStage.Name { get { return @"CodeTransformationStage"; } }

		#endregion // IPipelineStage Members

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public virtual void Run()
		{
			for (int index = 0; index < basicBlocks.Count; index++)
				for (Context ctx = new Context(instructionSet, basicBlocks[index]); !ctx.EndOfInstruction; ctx.GotoNext())
					if (ctx.Instruction != null)
						ctx.Clone().Visit(this);
		}

		#endregion // IMethodCompilerStage Members

		#region Block Operations

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		protected void LinkBlocks(BasicBlock source, BasicBlock destination)
		{
			if (!source.NextBlocks.Contains(destination))
				source.NextBlocks.Add(destination);

			if (!destination.PreviousBlocks.Contains(source))
				destination.PreviousBlocks.Add(source);
		}
		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		protected void LinkBlocks(Context source, BasicBlock destination)
		{
			LinkBlocks(source.BasicBlock, destination);
		}

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		protected void LinkBlocks(Context source, Context destination)
		{
			LinkBlocks(source.BasicBlock, destination.BasicBlock);
		}

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="destination2">The destination2.</param>
		protected void LinkBlocks(Context source, Context destination, Context destination2)
		{
			LinkBlocks(source.BasicBlock, destination.BasicBlock);
			LinkBlocks(source.BasicBlock, destination2.BasicBlock);
		}

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="destination2">The destination2.</param>
		protected void LinkBlocks(Context source, Context destination, BasicBlock destination2)
		{
			LinkBlocks(source.BasicBlock, destination.BasicBlock);
			LinkBlocks(source.BasicBlock, destination2);
		}

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="destination2">The destination2.</param>
		protected void LinkBlocks(Context source, BasicBlock destination, BasicBlock destination2)
		{
			LinkBlocks(source.BasicBlock, destination);
			LinkBlocks(source.BasicBlock, destination2);
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		protected Context CreateEmptyBlockContext(int label)
		{
			Context ctx = new Context(instructionSet);
			BasicBlock block = CreateBlock(basicBlocks.Count + 0x10000000);
			ctx.BasicBlock = block;

			// Need a dummy instruction at the start of each block to establish a starting point of the block
			ctx.AppendInstruction(null);
			ctx.Label = label;
			block.Index = ctx.Index;
			ctx.Ignore = true;

			return ctx;
		}

		/// <summary>
		/// Creates empty Blocks.
		/// </summary>
		/// <param name="blocks">The Blocks.</param>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		protected Context[] CreateEmptyBlockContexts(int label, int blocks)
		{
			// Allocate the block array
			Context[] result = new Context[blocks];

			for (int index = 0; index < blocks; index++)
				result[index] = CreateEmptyBlockContext(label);

			return result;
		}

		/// <summary>
		/// Splits the block.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="addJump">if set to <c>true</c> [add jump].</param>
		/// <returns></returns>
		protected Context SplitContext(Context ctx, bool addJump)
		{
			Context current = ctx.Clone();

			int label = basicBlocks.Count + 0x10000000;

			BasicBlock nextBlock = CreateBlock(label);

			foreach (BasicBlock block in current.BasicBlock.NextBlocks)
				nextBlock.NextBlocks.Add(block);

			current.BasicBlock.NextBlocks.Clear();

			if (addJump)
			{
				current.BasicBlock.NextBlocks.Add(nextBlock);
				nextBlock.PreviousBlocks.Add(ctx.BasicBlock);
			}

			if (current.IsLastInstruction)
			{
				current.AppendInstruction(null);
				current.Ignore = true;
				nextBlock.Index = current.Index;
				current.SliceBefore();
			}
			else
			{
				nextBlock.Index = current.Next.Index;
				current.SliceAfter();
			}

			if (addJump)
				current.AppendInstruction(IR.Instruction.JmpInstruction, nextBlock);

			return CreateContext(nextBlock);
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
			if (cop != null && (cop.StackType == StackTypeCode.F || cop.StackType == StackTypeCode.Int64))
			{
				int size, alignment;
				architecture.GetTypeRequirements(cop.Type, out size, out alignment);

				string name = String.Format("C_{0}", Guid.NewGuid());
				using (Stream stream = methodCompiler.Linker.Allocate(name, SectionKind.ROData, size, alignment))
				{
					byte[] buffer;

					switch (cop.Type.Type)
					{
						case CilElementType.R4:
							buffer = LittleEndianBitConverter.GetBytes((float)cop.Value);
							break;

						case CilElementType.R8:
							buffer = LittleEndianBitConverter.GetBytes((double)cop.Value);
							break;

						case CilElementType.I8:
							buffer = LittleEndianBitConverter.GetBytes((long)cop.Value);
							break;

						case CilElementType.U8:
							buffer = LittleEndianBitConverter.GetBytes((ulong)cop.Value);
							break;
						default:
							throw new NotSupportedException();
					}

					stream.Write(buffer, 0, buffer.Length);
				}

				// FIXME: Attach the label operand to the linker symbol
				// FIXME: Rename the operand to SymbolOperand
				// FIXME: Use the provided name to link
				LabelOperand lop = new LabelOperand(cop.Type, name);
				op = lop;
			}
			return op;
		}

		#endregion // Emit Methods
	}
}
