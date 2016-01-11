// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
		/// Holds the native alignment
		/// </summary>
		protected int NativeAlignment { get; private set; }

		/// <summary>
		/// Gets the type of the platform internal runtime.
		/// </summary>
		/// <value>
		/// The type of the platform internal runtime.
		/// </value>
		public MosaType PlatformInternalRuntimeType { get { return MethodCompiler.Compiler.PlatformInternalRuntimeType; } }

		public MosaType InternalRuntimeType { get { return MethodCompiler.Compiler.InternalRuntimeType; } }

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
			BasicBlocks = compiler.BasicBlocks;
			Architecture = compiler.Architecture;
			TypeSystem = compiler.TypeSystem;
			TypeLayout = compiler.TypeLayout;
			CallingConvention = Architecture.CallingConvention;

			NativePointerSize = Architecture.NativePointerSize;
			NativeAlignment = Architecture.NativeAlignment;
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
		/// Create an empty block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		protected BasicBlock CreateNewBlock(int label)
		{
			return BasicBlocks.CreateBlock(label);
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <returns></returns>
		protected BasicBlock CreateNewBlock()
		{
			return BasicBlocks.CreateBlock();
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		protected Context CreateNewBlockContext(int label)
		{
			return new Context(CreateNewBlock(label));
		}

		/// <summary>
		/// Creates empty blocks.
		/// </summary>
		/// <param name="blocks">The Blocks.</param>
		/// <returns></returns>
		protected BasicBlock[] CreateNewBlocks(int blocks)
		{
			// Allocate the block array
			var result = new BasicBlock[blocks];

			for (int index = 0; index < blocks; index++)
			{
				result[index] = CreateNewBlock();
			}

			return result;
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <returns></returns>
		protected Context CreateNewBlockContext()
		{
			return new Context(CreateNewBlock());
		}

		/// <summary>
		/// Creates empty blocks.
		/// </summary>
		/// <param name="blocks">The Blocks.</param>
		/// <returns></returns>
		protected Context[] CreateNewBlockContexts(int blocks)
		{
			// Allocate the context array
			var result = new Context[blocks];

			for (int index = 0; index < blocks; index++)
			{
				result[index] = CreateNewBlockContext();
			}

			return result;
		}

		/// <summary>
		/// Splits the block.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <returns></returns>
		protected BasicBlock Split(InstructionNode node)
		{
			var newblock = CreateNewBlock();

			node.Split(newblock);

			return newblock;
		}

		/// <summary>
		/// Splits the block.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <returns></returns>
		protected Context Split(Context ctx)
		{
			return new Context(Split(ctx.Node));
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

			for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmpty)
					continue;

				if (node.Instruction == IRInstruction.Nop)
					continue;

				if (node.Instruction.FlowControl != FlowControl.UnconditionalBranch)
					return false;
			}

			return true;
		}

		/// <summary>
		/// Empties the block of all instructions.
		/// </summary>
		/// <param name="block">The block.</param>
		protected void EmptyBlockOfAllInstructions(BasicBlock block)
		{
			for (var node = block.First.Next; !node.IsBlockEndInstruction; node = node.Next)
			{
				node.Empty();
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
			for (var node = block.Last; !node.IsBlockStartInstruction; node = node.Previous)
			{
				if (node.IsEmpty)
					continue;

				if (node.BranchTargets != null && node.BranchTargetsCount > 0)
				{
					var targets = node.BranchTargets;

					for (int index = 0; index < targets.Count; index++)
					{
						if (targets[index] == oldTarget)
						{
							node.UpdateBranchTarget(index, newTarget);
						}
					}
				}
			}
		}

		protected void RemoveEmptyBlockWithSingleJump(BasicBlock block)
		{
			Debug.Assert(block.NextBlocks.Count == 1);

			BasicBlock target = block.NextBlocks[0];

			foreach (var previous in block.PreviousBlocks.ToArray())
			{
				ReplaceBranchTargets(previous, block, target);
			}

			EmptyBlockOfAllInstructions(block);

			Debug.Assert(block.NextBlocks.Count == 0);
			Debug.Assert(block.PreviousBlocks.Count == 0);
		}

		protected static void UpdatePhiList(BasicBlock removedBlock, BasicBlock[] nextBlocks)
		{
			foreach (var next in nextBlocks)
			{
				for (var node = next.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.Instruction != IRInstruction.Phi)
						continue;

					var sourceBlocks = node.PhiBlocks;

					int index = sourceBlocks.IndexOf(removedBlock);

					if (index < 0)
						continue;

					sourceBlocks.RemoveAt(index);

					for (int i = index; index < node.OperandCount - 1; index++)
					{
						node.SetOperand(i, node.GetOperand(i + 1));
					}

					node.SetOperand(node.OperandCount - 1, null);
					node.OperandCount--;
				}
			}
		}

		#endregion Block Operations

		#region Protected Region Methods

		protected MosaExceptionHandler FindImmediateExceptionContext(int label)
		{
			foreach (var handler in MethodCompiler.Method.ExceptionHandlers)
			{
				if (handler.IsLabelWithinTry(label) || handler.IsLabelWithinHandler(label))
				{
					return handler;
				}
			}

			return null;
		}

		protected MosaExceptionHandler FindNextEnclosingFinallyContext(MosaExceptionHandler exceptionContext)
		{
			int index = MethodCompiler.Method.ExceptionHandlers.IndexOf(exceptionContext);

			for (int i = index + 1; i < MethodCompiler.Method.ExceptionHandlers.Count; i++)
			{
				var entry = MethodCompiler.Method.ExceptionHandlers[i];

				if (!entry.IsLabelWithinTry(exceptionContext.TryStart))
					return null;

				if (entry.ExceptionHandlerType != ExceptionHandlerType.Finally)
					continue;

				return entry;
			}

			return null;
		}

		protected MosaExceptionHandler FindFinallyExceptionContext(InstructionNode node)
		{
			MosaExceptionHandler innerClause = null;

			int label = node.Label;

			foreach (var handler in MethodCompiler.Method.ExceptionHandlers)
			{
				if (handler.IsLabelWithinHandler(label))
				{
					return handler;
				}
			}

			return null;
		}

		protected bool IsSourceAndTargetWithinSameTryOrException(InstructionNode node)
		{
			int leaveLabel = node.Label;
			int targetLabel = node.BranchTargets[0].First.Label;

			foreach (var handler in MethodCompiler.Method.ExceptionHandlers)
			{
				bool one = handler.IsLabelWithinTry(leaveLabel);
				bool two = handler.IsLabelWithinTry(targetLabel);

				if (one && !two)
					return false;

				if (!one && two)
					return false;

				if (one && two)
					return true;

				one = handler.IsLabelWithinHandler(leaveLabel);
				two = handler.IsLabelWithinHandler(targetLabel);

				if (one && !two)
					return false;

				if (!one && two)
					return false;

				if (one && two)
					return true;
			}

			// very odd
			return true;
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
				for (Context ctx = new Context(BasicBlocks[index]); !ctx.IsBlockEndInstruction; ctx.GotoNext())
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
			if (type.IsPointer)
				return InstructionSize.Native;

			if (type.IsI4 || type.IsU4)
				return InstructionSize.Size32;

			if (type.IsUI1 || type.IsBoolean)
				return InstructionSize.Size8;

			if (type.IsUI2 || type.IsChar)
				return InstructionSize.Size16;

			if (type.IsR4)
				return InstructionSize.Size32;

			if (type.IsR8 || type.IsUI8)
				return InstructionSize.Size64;

			if (type.IsReferenceType)
				return InstructionSize.Native;

			if (type.IsValueType)
				return InstructionSize.Native;

			return InstructionSize.Size32;
		}

		/// <summary>
		/// Gets the size of the instruction.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns></returns>
		public static InstructionSize GetInstructionSize(Operand operand)
		{
			return GetInstructionSize(operand.Type);
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

		public IList<BasicBlock> AddMissingBlocks(IList<BasicBlock> blocks, bool cleanUp)
		{
			var list = new List<BasicBlock>(blocks.Count);

			foreach (var block in blocks)
			{
				if (block != null)
				{
					list.Add(block);
				}
			}

			foreach (var block in BasicBlocks)
			{
				if (!blocks.Contains(block))
				{
					if ((!cleanUp) || (block.HasNextBlocks || block.HasPreviousBlocks || block.IsHandlerHeadBlock || block.IsTryHeadBlock))
					{
						list.Add(block);
					}
				}
			}

			return list;
		}

		#endregion Instruction Size Helpers
	}
}
