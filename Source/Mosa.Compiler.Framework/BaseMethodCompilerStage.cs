// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.Trace;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Basic base class for method compiler pipeline stages
	/// </summary>
	public abstract class BaseMethodCompilerStage : ITraceFactory
	{
		#region Data Members

		protected int instructionCount = 0;

		protected string formattedStageName;

		private List<TraceLog> traceLogs;

		#endregion Data Members

		#region Properties

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public virtual string Name { get { return GetType().Name; } }

		/// <summary>
		/// Gets or sets the name of the formatted stage.
		/// </summary>
		public string FormattedStageName { get; private set; }

		#endregion Properties

		#region Compiler Properties

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
		/// Gets the compiler options.
		/// </summary>
		protected CompilerOptions CompilerOptions { get; private set; }

		/// <summary>
		/// Holds the native pointer size
		/// </summary>
		protected int NativePointerSize { get; private set; }

		/// <summary>
		/// Holds the native alignment
		/// </summary>
		protected int NativeAlignment { get; private set; }

		/// <summary>
		/// Gets the size of the native instruction.
		/// </summary>
		/// <value>
		/// The size of the native instruction.
		/// </value>
		protected InstructionSize NativeInstructionSize { get; private set; }

		/// <summary>
		/// Gets a value indicating whether [is32 bit platform].
		/// </summary>
		/// <value>
		///   <c>true</c> if [is32 bit platform]; otherwise, <c>false</c>.
		/// </value>
		protected bool Is32BitPlatform { get; private set; }

		/// <summary>
		/// Gets a value indicating whether [is64 bit platform].
		/// </summary>
		/// <value>
		///   <c>true</c> if [is64 bit platform]; otherwise, <c>false</c>.
		/// </value>
		protected bool Is64BitPlatform { get; private set; }

		#endregion Compiler Properties

		#region Method Properties

		/// <summary>
		/// Hold the method compiler
		/// </summary>
		protected MethodCompiler MethodCompiler { get; private set; }

		/// <summary>
		/// Gets the method data.
		/// </summary>
		protected CompilerMethodData MethodData { get { return MethodCompiler.MethodData; } }

		/// <summary>
		/// Gets the linker.
		/// </summary>
		protected BaseLinker Linker { get { return MethodCompiler.Linker; } }

		/// <summary>
		/// Gets the type of the platform internal runtime.
		/// </summary>
		public MosaType PlatformInternalRuntimeType { get { return MethodCompiler.Compiler.PlatformInternalRuntimeType; } }

		/// <summary>
		/// Gets the type of the internal runtime.
		/// </summary>
		public MosaType InternalRuntimeType { get { return MethodCompiler.Compiler.InternalRuntimeType; } }

		/// <summary>
		/// Gets a value indicating whether this instance is plugged.
		/// </summary>
		public bool IsPlugged { get { return MethodCompiler.IsPlugged; } }

		/// <summary>
		/// Gets the method.
		/// </summary>
		protected MosaMethod Method { get { return MethodCompiler.Method; } }

		/// <summary>
		/// Gets the constant zero.
		/// </summary>
		protected Operand ConstantZero { get { return MethodCompiler.ConstantZero; } }

		/// <summary>
		/// Gets the stack frame.
		/// </summary>
		protected Operand StackFrame { get { return MethodCompiler.StackFrame; } }

		/// <summary>
		/// Gets the stack pointer.
		/// </summary>
		protected Operand StackPointer { get { return MethodCompiler.StackPointer; } }

		/// <summary>
		/// Gets a value indicating whether this instance has protected regions.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance has protected regions; otherwise, <c>false</c>.
		/// </value>
		protected bool HasProtectedRegions { get { return MethodCompiler.Method.ExceptionHandlers.Count != 0; } }

		/// <summary>
		/// Gets a value indicating whether this instance has code.
		/// </summary>
		protected bool HasCode { get { return BasicBlocks.HeadBlocks.Count != 0; } }

		#endregion Method Properties

		#region Methods

		/// <summary>
		/// Setups the specified compiler.
		/// </summary>
		/// <param name="baseCompiler">The base compiler.</param>
		public void Initialize(Compiler baseCompiler)
		{
			Architecture = baseCompiler.Architecture;
			TypeSystem = baseCompiler.TypeSystem;
			TypeLayout = baseCompiler.TypeLayout;

			NativePointerSize = Architecture.NativePointerSize;
			NativeAlignment = Architecture.NativeAlignment;
			NativeInstructionSize = Architecture.NativeInstructionSize;
			Is32BitPlatform = Architecture.Is32BitPlatform;
			Is64BitPlatform = Architecture.Is64BitPlatform;

			CompilerOptions = baseCompiler.CompilerOptions;

			Initialize();
		}

		/// <summary>
		/// Setups the specified compiler.
		/// </summary>
		/// <param name="methodCompiler">The compiler.</param>
		public void Setup(MethodCompiler methodCompiler, int position)
		{
			MethodCompiler = methodCompiler;
			BasicBlocks = methodCompiler.BasicBlocks;

			traceLogs = new List<TraceLog>();

			FormattedStageName = "[" + position.ToString("00") + "] " + Name;

			Setup();
		}

		public void Execute()
		{
			Run();

			SubmitTraceLogs(traceLogs);

			Finish();

			MethodCompiler = null;
			traceLogs = null;
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

		/// <summary>
		/// Allocates the virtual register or stack slot.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public Operand AllocateVirtualRegisterOrStackSlot(MosaType type)
		{
			return MethodCompiler.AllocateVirtualRegisterOrStackSlot(type);
		}

		#endregion Methods

		#region Overrides

		protected virtual void Initialize()
		{ }

		protected virtual void Setup()
		{
			instructionCount = 0;
		}

		protected virtual void Run()
		{ }

		protected virtual void Finish()
		{ }

		#endregion Overrides

		#region Block Operations

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
		/// <param name="blockLabel">The label.</param>
		/// <returns></returns>
		protected BasicBlock CreateNewBlock(int blockLabel)
		{
			return BasicBlocks.CreateBlock(blockLabel);
		}

		/// <summary>
		/// Creates the new block.
		/// </summary>
		/// <param name="blockLabel">The label.</param>
		/// <param name="instructionLabel">The instruction label.</param>
		/// <returns></returns>
		protected BasicBlock CreateNewBlock(int blockLabel, int instructionLabel)
		{
			return BasicBlocks.CreateBlock(blockLabel, instructionLabel);
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <param name="blockLabel">The label.</param>
		/// <returns></returns>
		protected Context CreateNewBlockContext(int blockLabel, int instructionLabel)
		{
			return new Context(CreateNewBlock(blockLabel, instructionLabel));
		}

		/// <summary>
		/// Creates empty blocks.
		/// </summary>
		/// <param name="blocks">The Blocks.</param>
		/// <returns></returns>
		protected BasicBlock[] CreateNewBlocks(int blocks, int instructionLabel)
		{
			// Allocate the block array
			var result = new BasicBlock[blocks];

			for (int index = 0; index < blocks; index++)
			{
				result[index] = CreateNewBlock(-1, instructionLabel);
			}

			return result;
		}

		/// <summary>
		/// Create an empty block.
		/// </summary>
		/// <returns></returns>
		protected Context CreateNewBlockContext(int instructionLabel)
		{
			return new Context(CreateNewBlock(-1, instructionLabel));
		}

		/// <summary>
		/// Creates empty blocks.
		/// </summary>
		/// <param name="blocks">The Blocks.</param>
		/// <returns></returns>
		protected Context[] CreateNewBlockContexts(int blocks, int instructionLabel)
		{
			// Allocate the context array
			var result = new Context[blocks];

			for (int index = 0; index < blocks; index++)
			{
				result[index] = CreateNewBlockContext(instructionLabel);
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
			var newblock = CreateNewBlock(-1, node.Label);

			node.Split(newblock);

			return newblock;
		}

		/// <summary>
		/// Splits the block.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		protected Context Split(Context context)
		{
			return new Context(Split(context.Node));
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

				if (node.BranchTargetsCount == 0)
					continue;

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

		protected void RemoveEmptyBlockWithSingleJump(BasicBlock block)
		{
			Debug.Assert(block.NextBlocks.Count == 1);

			BasicBlock target = block.NextBlocks[0];

			foreach (var previous in block.PreviousBlocks.ToArray())
			{
				ReplaceBranchTargets(previous, block, target);
			}

			EmptyBlockOfAllInstructions(block);

			Debug.Assert(block.PreviousBlocks.Count == 0);
		}

		protected static void RemoveBlockFromPhiInstructions(BasicBlock removedBlock, BasicBlock[] nextBlocks)
		{
			foreach (var next in nextBlocks)
			{
				for (var node = next.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.Instruction != IRInstruction.Phi)
						continue; // FUTURE: change to break, instead of continue

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

			Debug.Assert(removedBlock.NextBlocks.Count == 0);
		}

		protected static void UpdatePhiInstructionTargets(List<BasicBlock> targets, BasicBlock source, BasicBlock newSource)
		{
			foreach (var target in targets)
			{
				Debug.Assert(target.PreviousBlocks.Count > 0);

				for (var node = target.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.Instruction != IRInstruction.Phi)
						continue; // FUTURE: change to break, instead of continue

					int index = node.PhiBlocks.IndexOf(source);

					node.PhiBlocks[index] = newSource;
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

		public bool IsTraceable()
		{
			return MethodCompiler.Trace.TraceFilter.IsMatch(MethodCompiler.Method, FormattedStageName);
		}

		protected TraceLog CreateTraceLog()
		{
			bool active = IsTraceable();

			var traceLog = new TraceLog(TraceType.DebugTrace, MethodCompiler.Method, FormattedStageName, active);

			if (active)
				traceLogs.Add(traceLog);

			return traceLog;
		}

		public TraceLog CreateTraceLog(string section)
		{
			bool active = IsTraceable();

			var traceLog = new TraceLog(TraceType.DebugTrace, MethodCompiler.Method, FormattedStageName, section, active);

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
			MethodData.Counters.UpdateNoLock(name, count);
		}

		#region Helpers

		/// <summary>
		/// Gets the size of the type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="align">if set to <c>true</c> [align].</param>
		/// <returns></returns>
		public int GetTypeSize(MosaType type, bool align)
		{
			return MethodCompiler.GetReferenceOrTypeSize(type, align);
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

		protected BaseInstruction GetLoadInstruction(MosaType type)
		{
			if (type.IsPointer || type.IsReferenceType)
				return Select(IRInstruction.LoadInt32, IRInstruction.LoadInt64);
			if (type.IsPointer)
				return Select(IRInstruction.LoadInt32, IRInstruction.LoadInt64);
			else if (type.IsI1)
				return Select(IRInstruction.LoadSignExtend8x32, IRInstruction.LoadSignExtend8x64);
			else if (type.IsI2)
				return Select(IRInstruction.LoadSignExtend16x32, IRInstruction.LoadSignExtend16x64);
			else if (type.IsI4)
				return Select(IRInstruction.LoadInt32, IRInstruction.LoadSignExtend32x64);
			else if (type.IsI8)
				return IRInstruction.LoadInt64;
			else if (type.IsU1 || type.IsBoolean)
				return Select(IRInstruction.LoadZeroExtend8x32, IRInstruction.LoadZeroExtend8x64);
			else if (type.IsU2 || type.IsChar)
				return Select(IRInstruction.LoadZeroExtend16x32, IRInstruction.LoadZeroExtend16x64);
			else if (type.IsU4)
				return Select(IRInstruction.LoadInt32, IRInstruction.LoadZeroExtend32x64);
			else if (type.IsU8)
				return IRInstruction.LoadInt64;
			else if (type.IsR4)
				return IRInstruction.LoadFloatR4;
			else if (type.IsR8)
				return IRInstruction.LoadFloatR8;
			else if (Is32BitPlatform)   // review
				return IRInstruction.LoadInt32;
			else if (Is64BitPlatform)
				return IRInstruction.LoadInt64;

			throw new InvalidOperationException();
		}

		public BaseInstruction GetMoveInstruction(MosaType type)
		{
			if (type.IsPointer || type.IsReferenceType)
				return Select(IRInstruction.MoveInt32, IRInstruction.MoveInt64);
			else if (type.IsI1)
				return Select(IRInstruction.SignExtend8x32, IRInstruction.SignExtend8x64);
			else if (type.IsI2)
				return Select(IRInstruction.SignExtend16x32, IRInstruction.SignExtend16x64);
			else if (type.IsI4)
				return Select(IRInstruction.MoveInt32, IRInstruction.MoveInt32);
			else if (type.IsI8)
				return IRInstruction.MoveInt64;
			else if (type.IsU1 || type.IsBoolean)
				return Select(IRInstruction.ZeroExtend8x32, IRInstruction.ZeroExtend8x64);
			else if (type.IsU2 || type.IsChar)
				return Select(IRInstruction.ZeroExtend16x32, IRInstruction.ZeroExtend16x64);
			else if (type.IsU4)
				return Select(IRInstruction.MoveInt32, IRInstruction.ZeroExtend32x64);
			else if (type.IsU8)
				return IRInstruction.MoveInt64;
			else if (type.IsR4)
				return IRInstruction.MoveFloatR4;
			else if (type.IsR8)
				return IRInstruction.MoveFloatR8;
			else if (Is32BitPlatform)   // review
				return IRInstruction.MoveInt32;
			else if (Is64BitPlatform)
				return IRInstruction.MoveInt64;

			throw new InvalidOperationException();
		}

		protected BaseIRInstruction GetStoreParameterInstruction(MosaType type)
		{
			return GetStoreParameterInstruction(type, Is32BitPlatform);
		}

		public BaseIRInstruction GetLoadParameterInstruction(MosaType type)
		{
			return GetLoadParameterInstruction(type, Is32BitPlatform);
		}

		public static BaseIRInstruction GetStoreParameterInstruction(MosaType type, bool is32bitPlatform)
		{
			if (type.IsR4)
				return IRInstruction.StoreParamFloatR4;
			else if (type.IsR8)
				return IRInstruction.StoreParamFloatR8;
			else if (type.IsUI1 || type.IsBoolean)
				return IRInstruction.StoreParamInt8;
			else if (type.IsUI2 || type.IsChar)
				return IRInstruction.StoreParamInt16;
			else if (type.IsUI4)
				return IRInstruction.StoreParamInt32;
			else if (type.IsUI8)
				return IRInstruction.StoreParamInt64;
			else if (is32bitPlatform)
				return IRInstruction.StoreParamInt32;
			else if (is32bitPlatform)
				return IRInstruction.StoreParamInt64;

			throw new NotSupportedException();
		}

		public static BaseIRInstruction GetLoadParameterInstruction(MosaType type, bool is32bitPlatform)
		{
			if (type.IsR4)
				return IRInstruction.LoadParamFloatR4;
			else if (type.IsR8)
				return IRInstruction.LoadParamFloatR8;

			if (is32bitPlatform)
			{
				if (type.IsU1 || type.IsBoolean)
					return IRInstruction.LoadParamZeroExtend8x32;
				if (type.IsI1)
					return IRInstruction.LoadParamSignExtend8x32;
				else if (type.IsU2 || type.IsChar)
					return IRInstruction.LoadParamZeroExtend16x32;
				else if (type.IsI2)
					return IRInstruction.LoadParamSignExtend16x32;
				else if (type.IsUI4)
					return IRInstruction.LoadParamInt32;
				else if (type.IsUI8)
					return IRInstruction.LoadParamInt64;
				else if (type.IsEnum && type.ElementType.IsUI8)
					return IRInstruction.LoadParamInt64;

				return IRInstruction.LoadParamInt32;
			}
			else
			{
				if (type.IsU1 || type.IsBoolean)
					return IRInstruction.LoadParamZeroExtend8x64;
				if (type.IsI1)
					return IRInstruction.LoadParamSignExtend8x64;
				else if (type.IsU2 || type.IsChar)
					return IRInstruction.LoadParamZeroExtend16x64;
				else if (type.IsI2)
					return IRInstruction.LoadParamSignExtend16x64;
				else if (type.IsU4)
					return IRInstruction.LoadParamZeroExtend32x64;
				else if (type.IsI4)
					return IRInstruction.LoadParamSignExtend32x64;
				else if (type.IsUI8)
					return IRInstruction.LoadParamInt64;
				else if (type.IsEnum && type.ElementType.IsI4)
					return IRInstruction.LoadParamSignExtend32x64;
				else if (type.IsEnum && type.ElementType.IsU4)
					return IRInstruction.LoadParamZeroExtend32x64;

				return IRInstruction.LoadParamInt64;
			}
		}

		public static BaseIRInstruction GetSetReturnInstruction(MosaType type, bool is32bitPlatform)
		{
			if (type == null)
				return null;

			if (type.IsR4)
				return IRInstruction.SetReturnR4;
			else if (type.IsR8)
				return IRInstruction.SetReturnR8;

			if (!is32bitPlatform)
				return IRInstruction.SetReturn64;

			if (type.IsUI8 || (type.IsEnum && type.ElementType.IsUI8))
				return IRInstruction.SetReturn64;

			if (MosaTypeLayout.IsStoredOnStack(type))
				return IRInstruction.SetReturnCompound;

			return IRInstruction.SetReturn32;
		}

		public BaseIRInstruction GetStoreInstruction(MosaType type)
		{
			if (type.IsR4)
				return IRInstruction.StoreFloatR4;
			else if (type.IsR8)
				return IRInstruction.StoreFloatR8;
			else if (type.IsUI1 || type.IsBoolean)
				return IRInstruction.StoreInt8;
			else if (type.IsUI2 || type.IsChar)
				return IRInstruction.StoreInt16;
			else if (type.IsUI4)
				return IRInstruction.StoreInt32;
			else if (type.IsUI8)
				return IRInstruction.StoreInt64;
			else if (Is32BitPlatform)
				return IRInstruction.StoreInt32;
			else if (Is64BitPlatform)
				return IRInstruction.StoreInt64;

			throw new NotSupportedException();
		}

		public BaseInstruction Select(Operand operand, BaseInstruction instruction32, BaseInstruction instruction64)
		{
			return !operand.Is64BitInteger ? instruction32 : instruction64;
		}

		public BaseInstruction Select(BaseInstruction instruction32, BaseInstruction instruction64)
		{
			return Is32BitPlatform ? instruction32 : instruction64;
		}

		public BaseInstruction Select(bool is64bit, BaseInstruction instruction32, BaseInstruction instruction64)
		{
			return !is64bit ? instruction32 : instruction64;
		}

		#endregion Helpers

		#region Constant Helper Methods

		public Operand CreateConstant(byte value)
		{
			return Operand.CreateConstant(TypeSystem.BuiltIn.U1, value);
		}

		protected Operand CreateConstant(int value)
		{
			return Operand.CreateConstant(TypeSystem.BuiltIn.I4, value);
		}

		protected Operand CreateConstant(uint value)
		{
			return Operand.CreateConstant(TypeSystem.BuiltIn.U4, value);
		}

		protected Operand CreateConstant(long value)
		{
			return Operand.CreateConstant(TypeSystem.BuiltIn.I8, value);
		}

		protected Operand CreateConstant(ulong value)
		{
			return Operand.CreateConstant(TypeSystem.BuiltIn.U8, value);
		}

		protected static Operand CreateConstant(MosaType type, long value)
		{
			return Operand.CreateConstant(type, (ulong)value);
		}

		protected static Operand CreateConstant(MosaType type, ulong value)
		{
			return Operand.CreateConstant(type, value);
		}

		protected static Operand CreateConstant(MosaType type, int value)
		{
			return Operand.CreateConstant(type, (long)value);
		}

		protected static Operand CreateConstant(MosaType type, uint value)
		{
			return Operand.CreateConstant(type, value);
		}

		protected Operand CreateConstant(float value)
		{
			return Operand.CreateConstant(value, TypeSystem);
		}

		protected Operand CreateConstant(double value)
		{
			return Operand.CreateConstant(value, TypeSystem);
		}

		#endregion Constant Helper Methods
	}
}
