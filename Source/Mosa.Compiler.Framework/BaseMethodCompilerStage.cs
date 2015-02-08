/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.Trace;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Basic base class for method compiler pipeline stages
	/// </summary>
	public abstract class BaseMethodCompilerStage : IMethodCompilerStage, ITraceFactory
	{
		#region Data members

		protected int instructionCount = 0;

		private List<TraceLog> traceLogs;

		#endregion Data members

		#region Properties

		/// <summary>
		/// Hold the method compiler
		/// </summary>
		protected BaseMethodCompiler MethodCompiler { get; private set; }

		/// <summary>
		/// The architecture of the compilation process
		/// </summary>
		protected BaseArchitecture Architecture { get; private set; }

		/// <summary>
		/// Holds the instruction set
		/// </summary>
		protected InstructionSet InstructionSet { get; private set; }

		/// <summary>
		/// List of basic blocks found during decoding
		/// </summary>
		protected BasicBlocks BasicBlocks { get; private set; }

		/// <summary>
		/// Holds the type system
		/// </summary>
		protected TypeSystem TypeSystem { get; private set; }

		/// <summary>
		/// Holds the type layout interface
		/// </summary>
		protected MosaTypeLayout TypeLayout { get; private set; }

		/// <summary>
		/// Holds the calling convention interface
		/// </summary>
		protected BaseCallingConvention CallingConvention { get; private set; }

		/// <summary>
		/// Holds the native pointer size
		/// </summary>
		protected int NativePointerSize { get; private set; }

		/// <summary>
		/// Holds the native pointer alignment
		/// </summary>
		protected int NativePointerAlignment { get; private set; }

		/// <summary>
		/// Gets the type of the platform internal runtime.
		/// </summary>
		/// <value>
		/// The type of the platform internal runtime.
		/// </value>
		protected MosaType PlatformInternalRuntimeType { get { return MethodCompiler.Compiler.PlatformInternalRuntimeType; } }

		/// <summary>
		/// Gets the size of the native instruction.
		/// </summary>
		/// <value>
		/// The size of the native instruction.
		/// </value>
		protected InstructionSize NativeInstructionSize { get; private set; }

		#endregion Properties

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
		void IMethodCompilerStage.Initialize(BaseMethodCompiler compiler)
		{
			MethodCompiler = compiler;
			InstructionSet = compiler.InstructionSet;
			BasicBlocks = compiler.BasicBlocks;
			Architecture = compiler.Architecture;
			TypeSystem = compiler.TypeSystem;
			TypeLayout = compiler.TypeLayout;
			CallingConvention = Architecture.CallingConvention;

			NativePointerSize = Architecture.NativePointerSize;
			NativePointerAlignment = Architecture.NativeAlignment;
			NativeInstructionSize = Architecture.NativeInstructionSize;

			traceLogs = new List<TraceLog>();

			Setup();
		}

		void IMethodCompilerStage.Execute()
		{
			Run();

			SubmitTraceLogs(traceLogs);

			Finish();
		}

		#endregion IMethodCompilerStage members

		#region Overrides

		protected virtual void Setup()
		{ }

		protected virtual void Run()
		{ }

		protected virtual void Finish()
		{ }

		#endregion Overrides

		#region Methods

		/// <summary>
		/// Gets a value indicating whether this instance has code.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance has code; otherwise, <c>false</c>.
		/// </value>
		protected bool HasCode { get { return BasicBlocks.HeadBlocks.Count != 0; } }

		/// <summary>
		/// Gets a value indicating whether this instance has protected regions.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance has protected regions; otherwise, <c>false</c>.
		/// </value>
		protected bool HasProtectedRegions { get { return MethodCompiler.Method.ExceptionHandlers.Count != 0; } }

		/// <summary>
		/// Creates the context.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <returns></returns>
		protected Context CreateContext(BasicBlock block)
		{
			return new Context(InstructionSet, block);
		}

		/// <summary>
		/// Creates the context.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		protected Context CreateContext(int index)
		{
			return new Context(InstructionSet, index);
		}

		/// <summary>
		/// Allocates the virtual register.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		protected Operand AllocateVirtualRegister(MosaType type)
		{
			return MethodCompiler.VirtualRegisters.Allocate(type);
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
			BasicBlocks.LinkBlocks(source.BasicBlock, destination);
		}

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		protected void LinkBlocks(Context source, Context destination)
		{
			BasicBlocks.LinkBlocks(source.BasicBlock, destination.BasicBlock);
		}

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="destination2">The destination2.</param>
		protected void LinkBlocks(Context source, Context destination, Context destination2)
		{
			BasicBlocks.LinkBlocks(source.BasicBlock, destination.BasicBlock);
			BasicBlocks.LinkBlocks(source.BasicBlock, destination2.BasicBlock);
		}

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="destination2">The destination2.</param>
		protected void LinkBlocks(Context source, Context destination, BasicBlock destination2)
		{
			BasicBlocks.LinkBlocks(source.BasicBlock, destination.BasicBlock);
			BasicBlocks.LinkBlocks(source.BasicBlock, destination2);
		}

		/// <summary>
		/// Links the blocks.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="destination2">The destination2.</param>
		protected void LinkBlocks(Context source, BasicBlock destination, BasicBlock destination2)
		{
			BasicBlocks.LinkBlocks(source.BasicBlock, destination);
			BasicBlocks.LinkBlocks(source.BasicBlock, destination2);
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		protected Context CreateNewBlockWithContext(int label)
		{
			return InstructionSet.CreateNewBlock(BasicBlocks, label);
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <returns></returns>
		protected Context CreateNewBlockWithContext()
		{
			return InstructionSet.CreateNewBlock(BasicBlocks);
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
			BasicBlock nextBlock = BasicBlocks.CreateBlockWithAutoLabel(next.Index, current.BasicBlock.EndIndex);
			Context nextContext = new Context(InstructionSet, nextBlock);

			foreach (var block in current.BasicBlock.NextBlocks)
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

			var ctx = new Context(InstructionSet, block);

			Debug.Assert(ctx.IsBlockStartInstruction);
			ctx.GotoNext();

			while (!ctx.IsBlockEndInstruction)
			{
				if (!ctx.IsEmpty)
				{
					if (ctx.Instruction.FlowControl != FlowControl.UnconditionalBranch)
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
			var ctx = new Context(InstructionSet, block);
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
			var ctx = new Context(InstructionSet, block, block.EndIndex);
			Debug.Assert(ctx.IsBlockEndInstruction);

			do
			{
				ctx.GotoPrevious();
			}
			while (ctx.IsEmpty);

			// Find branch or jump to (to) and replace it with js
			while (!ctx.IsBlockStartInstruction)
			{
				if (ctx.Instruction.FlowControl == FlowControl.ConditionalBranch ||
					ctx.Instruction.FlowControl == FlowControl.UnconditionalBranch ||
					ctx.Instruction.FlowControl == FlowControl.Switch)
				{
					var targets = ctx.Targets;

					for (int index = 0; index < targets.Count; index++)
					{
						if (targets[index] == oldTarget)
						{
							targets[index] = newTarget;
						}
					}
				}

				do
				{
					ctx.GotoPrevious();
				}
				while (ctx.IsEmpty);
			}
		}

		protected void RemoveEmptyBlockWithSingleJump(BasicBlock block)
		{
			Debug.Assert(block.NextBlocks.Count == 1);

			BasicBlock target = block.NextBlocks[0];

			target.PreviousBlocks.Remove(block);

			foreach (var from in block.PreviousBlocks)
			{
				from.NextBlocks.Remove(block);
				from.NextBlocks.AddIfNew(target);

				target.PreviousBlocks.AddIfNew(from);

				ReplaceBranchTargets(from, block, target);
			}

			block.NextBlocks.Clear();
			block.PreviousBlocks.Clear();

			EmptyBlockOfAllInstructions(block);
		}

		#endregion Block Operations

		#region Protected Region Methods

		protected MosaExceptionHandler FindImmediateExceptionHandler(Context context)
		{
			MosaExceptionHandler innerClause = null;

			int label = context.Label;

			foreach (var handler in MethodCompiler.Method.ExceptionHandlers)
			{
				if (handler.IsLabelWithinTry(label) || handler.IsLabelWithinHandler(label))
				{
					return handler;
				}
			}

			return null;
		}

		protected MosaExceptionHandler FindFinallyHandler(Context context)
		{
			MosaExceptionHandler innerClause = null;

			int label = context.Label;

			foreach (var handler in MethodCompiler.Method.ExceptionHandlers)
			{
				if (handler.IsLabelWithinHandler(label))
				{
					return handler;
				}
			}

			return null;
		}

		#endregion Protected Region Methods

		#region ITraceSectionFactory

		TraceLog ITraceFactory.CreateTraceLog(string section)
		{
			return CreateTraceLog(section);
		}

		#endregion ITraceSectionFactory

		#region Trace Helper Methods

		public string GetFormattedStageName()
		{
			return MethodCompiler.FormatStageName(this as IPipelineStage);
		}

		public bool IsTraceable()
		{
			return MethodCompiler.Trace.TraceFilter.IsMatch(MethodCompiler.Method, GetFormattedStageName());
		}

		protected TraceLog CreateTraceLog()
		{
			bool active = IsTraceable();

			var traceLog = new TraceLog(TraceType.DebugTrace, MethodCompiler.Method, GetFormattedStageName(), active);

			if (active)
				traceLogs.Add(traceLog);

			return traceLog;
		}

		public TraceLog CreateTraceLog(string section)
		{
			bool active = IsTraceable();

			var traceLog = new TraceLog(TraceType.DebugTrace, MethodCompiler.Method, GetFormattedStageName(), section, active);

			if (active)
				traceLogs.Add(traceLog);

			return traceLog;
		}

		private void SubmitTraceLog(TraceLog traceLog)
		{
			if (!traceLog.Active)
				return;

			MethodCompiler.Trace.NewTraceLog(traceLog);
		}

		private void SubmitTraceLogs(IList<TraceLog> traceLogs)
		{
			if (traceLogs == null)
				return;

			foreach (var traceLog in traceLogs)
			{
				if (traceLog != null)
				{
					SubmitTraceLog(traceLog);
				}
			}
		}

		protected void NewCompilerTraceEvent(CompilerEvent compileEvent, string message)
		{
			MethodCompiler.Trace.NewCompilerTraceEvent(compileEvent, message, MethodCompiler.ThreadID);
		}

		#endregion Trace Helper Methods

		/// <summary>
		/// Updates the counter.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="count">The count.</param>
		public void UpdateCounter(string name, int count)
		{
			MethodCompiler.Compiler.Counters.UpdateCounter(name, count);
		}

		/// <summary>
		/// Dumps this instance.
		/// </summary>
		protected void Dump(bool before)
		{
			Debug.WriteLine(string.Empty);

			Debug.WriteLine("METHOD: " + MethodCompiler.Method.FullName);
			Debug.WriteLine("STAGE : " + (before ? "[BEFORE] " : "[AFTER] ") + this.GetType().Name);
			Debug.WriteLine(string.Empty);

			for (int index = 0; index < BasicBlocks.Count; index++)
				for (Context ctx = new Context(InstructionSet, BasicBlocks[index]); !ctx.IsBlockEndInstruction; ctx.GotoNext())
					if (!ctx.IsEmpty)
						Debug.WriteLine(ctx.ToString());
		}

		#region Instruction Size Helpers

		/// <summary>
		/// Gets the size of the instruction.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static InstructionSize GetInstructionSize(MosaType type)
		{
			if (type.IsPointer && type.ElementType != null)
			{
				return GetInstructionSize(type.ElementType);
			}

			if (type.IsUI1 || type.IsBoolean)
				return InstructionSize.Size8;

			if (type.IsUI2 || type.IsChar)
				return InstructionSize.Size16;

			if (type.IsR4)
				return InstructionSize.Size32;

			if (type.IsR8)
				return InstructionSize.Size64;

			return InstructionSize.Size32;
		}

		/// <summary>
		/// Gets the size of the instruction.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns></returns>
		public static InstructionSize GetInstructionSize(Operand operand)
		{
			if (operand.IsPointer && operand.Type.ElementType != null)
			{
				GetInstructionSize(operand.Type.ElementType);
			}

			if (operand.IsByte || operand.IsBoolean)
				return InstructionSize.Size8;

			if (operand.IsChar || operand.IsShort)
				return InstructionSize.Size16;

			if (operand.IsR4)
				return InstructionSize.Size32;

			if (operand.IsR8)
				return InstructionSize.Size64;

			return InstructionSize.Size32;
		}

		/// <summary>
		/// Gets the size of the instruction.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="operand">The operand.</param>
		/// <returns></returns>
		public static InstructionSize GetInstructionSize(InstructionSize size, Operand operand)
		{
			if (size != InstructionSize.None)
				return size;

			return GetInstructionSize(operand);
		}

		#endregion Instruction Size Helpers
	}
}