/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;
using System;
using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Basic base class for method compiler pipeline stages
	/// </summary>
	public abstract class BaseMethodCompilerStage
	{
		#region Data members

		/// <summary>
		/// Hold the method compiler
		/// </summary>
		protected BaseMethodCompiler methodCompiler;

		/// <summary>
		/// The architecture of the compilation process
		/// </summary>
		protected BaseArchitecture architecture;

		/// <summary>
		/// Holds the instruction set
		/// </summary>
		protected InstructionSet instructionSet;

		/// <summary>
		/// List of basic blocks found during decoding
		/// </summary>
		protected BasicBlocks basicBlocks;

		/// <summary>
		/// Holds the type system
		/// </summary>
		protected ITypeSystem typeSystem;

		/// <summary>
		/// Holds the modules type system
		/// </summary>
		protected ITypeModule typeModule;

		/// <summary>
		/// Holds the assembly loader
		/// </summary>
		protected IAssemblyLoader assemblyLoader;

		/// <summary>
		/// Holds the type layout interface
		/// </summary>
		protected ITypeLayout typeLayout;

		/// <summary>
		/// Holds the calling convention interface
		/// </summary>
		protected BaseCallingConvention callingConvention;

		/// <summary>
		/// Holds the Native Pointer Size
		/// </summary>
		protected int nativePointerSize;

		/// <summary>
		/// Holds the Native Pointer Alignment
		/// </summary>
		protected int nativePointerAlignment;

		#endregion Data members

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public virtual string Name { get { return this.GetType().Name; } }

		#endregion IPipelineStage Members

		#region IMethodCompilerStage members

		/// <summary>
		/// Setups the specified compiler.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		public void Setup(BaseMethodCompiler compiler)
		{
			if (compiler == null)
				throw new ArgumentNullException(@"compiler");

			methodCompiler = compiler;
			instructionSet = compiler.InstructionSet;
			basicBlocks = compiler.BasicBlocks;
			architecture = compiler.Architecture;
			typeModule = compiler.Method.Module;
			typeSystem = compiler.TypeSystem;
			typeLayout = compiler.TypeLayout;
			callingConvention = architecture.CallingConvention;

			architecture.GetTypeRequirements(BuiltInSigType.IntPtr, out nativePointerSize, out nativePointerAlignment);
		}

		#endregion IMethodCompilerStage members

		#region Methods

		/// <summary>
		/// Gets a value indicating whether this instance has exception or finally.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance has exception or finally; otherwise, <c>false</c>.
		/// </value>
		protected bool HasExceptionOrFinally { get { return methodCompiler.ExceptionHandlingClauses.Count != 0; } }

		/// <summary>
		/// Creates the context.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns></returns>
		protected Context CreateContext(BasicBlock block)
		{
			return new Context(instructionSet, block);
		}

		/// <summary>
		/// Creates the context.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		protected Context CreateContext(int index)
		{
			return new Context(instructionSet, index);
		}

		/// <summary>
		/// Allocates the virtual register.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		protected Operand AllocateVirtualRegister(SigType type)
		{
			return methodCompiler.VirtualRegisters.Allocate(type);
		}

		#endregion Methods

		#region Block Operations

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		protected void LinkBlocks(Context source, BasicBlock destination)
		{
			basicBlocks.LinkBlocks(source.BasicBlock, destination);
		}

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		protected void LinkBlocks(Context source, Context destination)
		{
			basicBlocks.LinkBlocks(source.BasicBlock, destination.BasicBlock);
		}

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="destination2">The destination2.</param>
		protected void LinkBlocks(Context source, Context destination, Context destination2)
		{
			basicBlocks.LinkBlocks(source.BasicBlock, destination.BasicBlock);
			basicBlocks.LinkBlocks(source.BasicBlock, destination2.BasicBlock);
		}

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="destination2">The destination2.</param>
		protected void LinkBlocks(Context source, Context destination, BasicBlock destination2)
		{
			basicBlocks.LinkBlocks(source.BasicBlock, destination.BasicBlock);
			basicBlocks.LinkBlocks(source.BasicBlock, destination2);
		}

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="destination2">The destination2.</param>
		protected void LinkBlocks(Context source, BasicBlock destination, BasicBlock destination2)
		{
			basicBlocks.LinkBlocks(source.BasicBlock, destination);
			basicBlocks.LinkBlocks(source.BasicBlock, destination2);
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		protected Context CreateNewBlockWithContext(int label)
		{
			return instructionSet.CreateNewBlock(basicBlocks, label);
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <returns></returns>
		protected Context CreateNewBlockWithContext()
		{
			return instructionSet.CreateNewBlock(basicBlocks);
		}

		/// <summary>
		/// Creates empty blocks.
		/// </summary>
		/// <param name="blocks">The Blocks.</param>
		/// <returns></returns>
		protected Context[] CreateNewBlocksWithContexts(int blocks)
		{
			// Allocate the context array
			Context[] result = new Context[blocks];

			for (int index = 0; index < blocks; index++)
				result[index] = CreateNewBlockWithContext();

			return result;
		}

		/// <summary>
		/// Splits the block.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <returns></returns>
		protected Context Split(Context ctx)
		{
			Context current = ctx.Clone();

			Context next = ctx.Clone();
			next.AppendInstruction(IRInstruction.BlockStart);
			BasicBlock nextBlock = basicBlocks.CreateBlockWithAutoLabel(next.Index, current.BasicBlock.EndIndex);
			Context nextContext = new Context(instructionSet, nextBlock);

			foreach (BasicBlock block in current.BasicBlock.NextBlocks)
			{
				nextBlock.NextBlocks.Add(block);
				block.PreviousBlocks.Remove(current.BasicBlock);
				block.PreviousBlocks.Add(nextBlock);
			}

			current.BasicBlock.NextBlocks.Clear();

			current.AppendInstruction(IRInstruction.BlockEnd);
			current.BasicBlock.EndIndex = current.Index;

			return nextContext;
		}

		/// <summary>
		/// Determines whether [is empty block with single jump] [the specified block].
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns>
		///   <c>true</c> if [is empty block with single jump] [the specified block]; otherwise, <c>false</c>.
		/// </returns>
		protected bool IsEmptyBlockWithSingleJump(BasicBlock block)
		{
			if (block.NextBlocks.Count != 1)
				return false;

			var ctx = new Context(instructionSet, block);

			Debug.Assert(ctx.IsBlockStartInstruction);
			ctx.GotoNext();

			while (!ctx.IsBlockEndInstruction)
			{
				if (!ctx.IsEmpty)
				{
					if (ctx.Instruction.FlowControl != FlowControl.Branch)
						return false;
				}

				ctx.GotoNext();
			}

			return true;
		}

		/// <summary>
		/// Empties the block of all instructions.
		/// </summary>
		/// <param name="block">The block.</param>
		protected void EmptyBlockOfAllInstructions(BasicBlock block)
		{
			var ctx = new Context(instructionSet, block);
			Debug.Assert(ctx.IsBlockStartInstruction);
			ctx.GotoNext();

			while (!ctx.IsBlockEndInstruction)
			{
				if (!ctx.IsEmpty)
				{
					ctx.Remove();
				}

				ctx.GotoNext();
			}
		}

		/// <summary>
		/// Replaces the branch targets.
		/// </summary>
		/// <param name="block">The current from block.</param>
		/// <param name="oldTarget">The current destination block.</param>
		/// <param name="newTarget">The new target block.</param>
		protected void ReplaceBranchTargets(BasicBlock block, BasicBlock oldTarget, BasicBlock newTarget)
		{
			// Replace any jump/branch target in block (from) with js
			var ctx = new Context(instructionSet, block, block.EndIndex);
			Debug.Assert(ctx.IsBlockEndInstruction);

			do
			{
				ctx.GotoPrevious();
			}
			while (ctx.IsEmpty);

			// Find branch or jump to (to) and replace it with js
			//while (ctx.BranchTargets != null)
			while (!ctx.IsBlockStartInstruction)
			{
				if (ctx.BranchTargets != null)
				{
					int[] targets = ctx.BranchTargets;
					for (int index = 0; index < targets.Length; index++)
					{
						if (targets[index] == oldTarget.Label)
							targets[index] = newTarget.Label;
					}
				}

				do
				{
					ctx.GotoPrevious();
				}
				while (ctx.IsEmpty);
			}
		}

		#endregion Block Operations

		#region Trace Helper Methods

		public CompilerTrace CreateTrace()
		{
			return new CompilerTrace(this.methodCompiler.InternalTrace, this.methodCompiler.Method, this.methodCompiler.FormatStageName(this as IPipelineStage));
		}

		public CompilerTrace CreateTrace(string section)
		{
			return new CompilerTrace(this.methodCompiler.InternalTrace, this.methodCompiler.Method, this.methodCompiler.FormatStageName(this as IPipelineStage), section);
		}

		#endregion Trace Helper Methods

		/// <summary>
		/// Updates the counter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="count">The count.</param>
		public void UpdateCounter(string name, int count)
		{
			methodCompiler.Compiler.Counters.UpdateCounter(name, count);
		}
	}
}