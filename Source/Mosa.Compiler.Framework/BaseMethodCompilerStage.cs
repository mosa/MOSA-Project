// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Basic base class for method compiler pipeline stages
/// </summary>
public abstract class BaseMethodCompilerStage
{
	public delegate TraceLog CreateTraceHandler(string name, int tracelevel);

	#region Data Members

	private List<TraceLog> traceLogs;

	#endregion Data Members

	#region Stage Properties

	/// <summary>
	/// Retrieves the name of the compilation stage.
	/// </summary>
	/// <value>The name of the compilation stage.</value>
	public virtual string Name => GetType().Name;

	/// <summary>
	/// Gets or sets the name of the formatted stage.
	/// </summary>
	public string FormattedStageName { get; private set; }

	#endregion Stage Properties

	#region Instructions Properties

	protected BaseInstruction MoveInstruction { get; private set; }

	//protected BaseInstruction LoadInstruction { get; private set; }

	//protected BaseInstruction StoreInstruction { get; private set; }

	//protected BaseInstruction SubInstruction { get; private set; }

	//protected BaseInstruction AddInstruction { get; private set; }

	//protected BaseInstruction BranchInstruction { get; private set; }

	#endregion Instructions Properties

	#region Compiler Properties

	protected Compiler Compiler { get; private set; }

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
	protected CompilerSettings CompilerSettings { get; private set; }

	/// <summary>
	/// Holds the native pointer size
	/// </summary>
	protected uint NativePointerSize { get; private set; }

	/// <summary>
	/// Holds the native alignment
	/// </summary>
	protected uint NativeAlignment { get; private set; }

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
	/// Hold the method compiler
	/// </summary>
	protected MethodScanner MethodScanner { get; private set; }

	/// <summary>
	/// Retrieves the compilation scheduler.
	/// </summary>
	/// <value>The compilation scheduler.</value>
	public MethodScheduler MethodScheduler { get; private set; }

	/// <summary>
	/// Gets the method data.
	/// </summary>
	protected MethodData MethodData => MethodCompiler.MethodData;

	/// <summary>
	/// Gets the linker.
	/// </summary>
	protected MosaLinker Linker => MethodCompiler.Linker;

	/// <summary>
	/// Gets the type of the platform internal runtime.
	/// </summary>
	public MosaType PlatformInternalRuntimeType => MethodCompiler.Compiler.PlatformInternalRuntimeType;

	/// <summary>
	/// Gets the type of the internal runtime.
	/// </summary>
	public MosaType InternalRuntimeType => MethodCompiler.Compiler.InternalRuntimeType;

	/// <summary>
	/// Gets the method.
	/// </summary>
	protected MosaMethod Method => MethodCompiler.Method;

	/// <summary>
	/// Gets the constant zero.
	/// </summary>
	protected Operand ConstantZero => MethodCompiler.ConstantZero;

	/// <summary>
	/// Gets the 32-bit constant zero.
	/// </summary>
	protected Operand Constant32_0 => MethodCompiler.Constant32_0;

	/// <summary>
	/// Gets the 64-bit constant zero.
	/// </summary>
	protected Operand Constant64_0 => MethodCompiler.Constant64_0;

	/// <summary>
	/// Gets the floating point R4 constant zero.
	/// </summary>
	protected Operand ConstantR4_0 => MethodCompiler.ConstantR4_0;

	/// <summary>
	/// Gets the floating point R8 constant zero.
	/// </summary>
	protected Operand ConstantR8_0 => MethodCompiler.ConstantR8_0;

	/// <summary>
	/// Gets the stack frame.
	/// </summary>
	protected Operand StackFrame => MethodCompiler.Compiler.StackFrame;

	/// <summary>
	/// Gets the stack pointer.
	/// </summary>
	protected Operand StackPointer => MethodCompiler.Compiler.StackPointer;

	/// <summary>
	/// Gets the link register.
	/// </summary>
	protected Operand LinkRegister => MethodCompiler.Compiler.LinkRegister;

	/// <summary>
	/// Gets the program counter
	/// </summary>
	protected Operand ProgramCounter => MethodCompiler.Compiler.ProgramCounter;

	/// <summary>
	/// Gets the exception register.
	/// </summary>
	protected Operand ExceptionRegister => MethodCompiler.Compiler.ExceptionRegister;

	/// <summary>
	/// Gets the leave target register.
	/// </summary>
	protected Operand LeaveTargetRegister => MethodCompiler.Compiler.LeaveTargetRegister;

	/// <summary>
	/// Gets a value indicating whether this instance has protected regions.
	/// </summary>
	protected bool HasProtectedRegions => MethodCompiler.HasProtectedRegions;

	/// <summary>
	/// Gets a value indicating whether this instance has code.
	/// </summary>
	protected bool HasCode => BasicBlocks.HeadBlocks.Count != 0;

	/// <summary>
	/// The counters
	/// </summary>
	private readonly List<Counter> RegisteredCounters = new List<Counter>();

	protected uint ObjectHeaderSize;

	#endregion Method Properties

	#region Methods

	/// <summary>
	/// Setups the specified compiler.
	/// </summary>
	/// <param name="compiler">The base compiler.</param>
	public void Initialize(Compiler compiler)
	{
		Compiler = compiler;
		Architecture = compiler.Architecture;
		TypeSystem = compiler.TypeSystem;
		TypeLayout = compiler.TypeLayout;
		MethodScheduler = compiler.MethodScheduler;
		CompilerSettings = compiler.CompilerSettings;
		MethodScanner = compiler.MethodScanner;

		NativePointerSize = Architecture.NativePointerSize;
		NativeAlignment = Architecture.NativeAlignment;
		NativeInstructionSize = Architecture.NativeInstructionSize;
		Is32BitPlatform = Architecture.Is32BitPlatform;
		Is64BitPlatform = Architecture.Is64BitPlatform;

		ObjectHeaderSize = compiler.ObjectHeaderSize;

		if (Is32BitPlatform)
		{
			MoveInstruction = IRInstruction.Move32;
			//LoadInstruction = IRInstruction.Load32;
			//StoreInstruction = IRInstruction.Store32;
			//AddInstruction = IRInstruction.Add32;
			//SubInstruction = IRInstruction.Sub32;
			//BranchInstruction = IRInstruction.Branch32;
		}
		else
		{
			MoveInstruction = IRInstruction.Move64;
			//LoadInstruction = IRInstruction.Load64;
			//StoreInstruction = IRInstruction.Store64;
			//AddInstruction = IRInstruction.Add64;
			//SubInstruction = IRInstruction.Sub64;
			//BranchInstruction = IRInstruction.Branch64;
		}

		Initialize();
	}

	/// <summary>
	/// Setups the specified compiler.
	/// </summary>
	/// <param name="methodCompiler">The compiler.</param>
	/// <param name="position">The position.</param>
	public void Setup(MethodCompiler methodCompiler, int position)
	{
		MethodCompiler = methodCompiler;
		BasicBlocks = methodCompiler.BasicBlocks;

		traceLogs = new List<TraceLog>();

		FormattedStageName = $"[{position:00}] {Name}";

		Setup();
	}

	public void Execute()
	{
		ResetRegisteredCounters();

		try
		{
			Run();
		}
		finally
		{
			PostTraceLogs(traceLogs);
		}

		Finish();

		UpdateRegisterCounters();

		MethodCompiler = null;
		traceLogs = null;
	}

	public Operand AllocateStackLocal32(bool pinned = false)
	{
		return MethodCompiler.AllocateStackLocal32(pinned);
	}

	public Operand AllocateStackLocal64(bool pinned = false)
	{
		return MethodCompiler.AllocateStackLocal64(pinned);
	}

	public Operand AllocateStackLocalR4(bool pinned = false)
	{
		return MethodCompiler.AllocateStackLocalR4(pinned);
	}

	public Operand AllocateStackLocalR8(bool pinned = false)
	{
		return MethodCompiler.AllocateStackLocalR8(pinned);
	}

	public Operand AllocateStackLocalObject(bool pinned = false)
	{
		return MethodCompiler.AllocateStackLocalObject(pinned);
	}

	public Operand AllocateStackLocalManagedPointer(bool pinned = false)
	{
		return MethodCompiler.AllocateStackLocalManagedPointer(pinned);
	}

	public Operand AddStackLocalValueType(MosaType type, bool pinned = false)
	{
		return MethodCompiler.AllocateStackLocalValueType(type, pinned);
	}

	public Operand AddStackLocal(Operand operand, bool pinned = false)
	{
		return MethodCompiler.AllocateStackLocal(operand, pinned);
	}

	protected Operand AllocateVirtualRegister(Operand operand)
	{
		return MethodCompiler.VirtualRegisters.AllocateOperand(operand);
	}

	protected Operand AllocateVirtualRegister32()
	{
		return MethodCompiler.VirtualRegisters.Allocate32();
	}

	protected Operand AllocateVirtualRegister64()
	{
		return MethodCompiler.VirtualRegisters.Allocate64();
	}

	protected Operand AllocateVirtualRegisterR4()
	{
		return MethodCompiler.VirtualRegisters.AllocateR4();
	}

	protected Operand AllocateVirtualRegisterR8()
	{
		return MethodCompiler.VirtualRegisters.AllocateR8();
	}

	protected Operand AllocateVirtualRegisterObject()
	{
		return MethodCompiler.VirtualRegisters.AllocateObject();
	}

	protected Operand AllocateVirtualRegisterManagedPointer()
	{
		return MethodCompiler.VirtualRegisters.AllocateManagedPointer();
	}

	protected Operand AllocateVirtualRegisterNativeInteger()
	{
		return Is32BitPlatform ? MethodCompiler.VirtualRegisters.Allocate32() : MethodCompiler.VirtualRegisters.Allocate64();
	}

	#endregion Methods

	#region Counters

	public void Register(Counter counter)
	{
		RegisteredCounters.Add(counter);
	}

	private void ResetRegisteredCounters()
	{
		foreach (var counter in RegisteredCounters)
		{
			counter.Reset();
		}
	}

	private void UpdateRegisterCounters()
	{
		foreach (var counter in RegisteredCounters)
		{
			UpdateCounter(counter.Name, counter.Count);
		}
	}

	protected void UpdateCounter(string name, int count = 1)
	{
		MethodData.Counters.UpdateSkipLock(name, count);
	}

	#endregion Counters

	#region Overrides

	protected virtual void Initialize()
	{ }

	protected virtual void Setup()
	{ }

	protected virtual void Run()
	{ }

	protected virtual void Finish()
	{ }

	#endregion Overrides

	#region Block Operations

	/// <summary>
	/// Create an empty block.
	/// </summary>
	/// <param name="blockLabel">The label.</param>
	/// <param name="instructionLabel">The instruction label.</param>
	/// <returns></returns>
	protected Context CreateNewBlockContext(int blockLabel, int instructionLabel)
	{
		return new Context(BasicBlocks.CreateBlock(blockLabel, instructionLabel));
	}

	/// <summary>
	/// Create an empty block.
	/// </summary>
	/// <param name="instructionLabel">The instruction label.</param>
	/// <returns></returns>
	protected Context CreateNewBlockContext(int instructionLabel)
	{
		return new Context(BasicBlocks.CreateBlock(-1, instructionLabel));
	}

	/// <summary>
	/// Creates empty blocks.
	/// </summary>
	/// <param name="blocks">The Blocks.</param>
	/// <param name="instructionLabel">The instruction label.</param>
	/// <returns></returns>
	protected Context[] CreateNewBlockContexts(int blocks, int instructionLabel)
	{
		// Allocate the context array
		var result = new Context[blocks];

		for (var index = 0; index < blocks; index++)
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
		var newblock = BasicBlocks.CreateBlock(-1, node.Label);

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
	protected static bool IsEmptyBlockWithSingleJump(BasicBlock block)
	{
		if (block.NextBlocks.Count != 1)
			return false;

		for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
		{
			if (node.IsEmptyOrNop)
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
	protected static bool EmptyBlockOfAllInstructions(BasicBlock block, bool useNop = false)
	{
		var found = false;

		for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
		{
			if (node.IsEmpty)
				continue;

			if (node.IsNop)
			{
				if (!useNop)
					node.Empty();

				continue;
			}

			node.SetNop();

			found = true;
		}

		return found;
	}

	/// <summary>
	/// Replaces the branch targets.
	/// </summary>
	/// <param name="target">The current from block.</param>
	/// <param name="oldTarget">The current destination block.</param>
	/// <param name="newTarget">The new target block.</param>
	protected static void ReplaceBranchTargets(BasicBlock target, BasicBlock oldTarget, BasicBlock newTarget)
	{
		for (var node = target.BeforeLast; !node.IsBlockStartInstruction; node = node.Previous)
		{
			if (node.IsEmptyOrNop)
				continue;

			if (node.BranchTargetsCount == 0)
				continue;

			// TODO: When non branch instruction encountered, return (fast out)

			var targets = node.BranchTargets;

			for (var index = 0; index < targets.Count; index++)
			{
				if (targets[index] == oldTarget)
				{
					node.UpdateBranchTarget(index, newTarget);
				}
			}
		}
	}

	protected static void RemoveEmptyBlockWithSingleJump(BasicBlock block, bool useNop = false)
	{
		Debug.Assert(block.NextBlocks.Count == 1);

		var target = block.NextBlocks[0];

		foreach (var previous in block.PreviousBlocks.ToArray())
		{
			ReplaceBranchTargets(previous, block, target);
		}

		EmptyBlockOfAllInstructions(block, useNop);

		Debug.Assert(block.PreviousBlocks.Count == 0);
	}

	#endregion Block Operations

	#region Phi Helpers

	public static void UpdatePhiBlocks(BasicBlock[] phiBlocks)
	{
		foreach (var phiBlock in phiBlocks)
		{
			UpdatePhiBlock(phiBlock);
		}
	}

	public static void UpdatePhiBlocks(List<BasicBlock> phiBlocks)
	{
		foreach (var phiBlock in phiBlocks)
		{
			UpdatePhiBlock(phiBlock);
		}
	}

	public static void UpdatePhiBlock(BasicBlock phiBlock)
	{
		for (var node = phiBlock.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
		{
			if (node.IsEmptyOrNop)
				continue;

			if (!BaseMethodCompilerStage.IsPhiInstruction(node.Instruction))
				break;

			UpdatePhi(node);
		}
	}

	public static void UpdatePhi(InstructionNode node)
	{
		Debug.Assert(node.OperandCount != node.Block.PreviousBlocks.Count);

		// One or more of the previous blocks was removed, fix up the operand blocks

		var previousBlocks = node.Block.PreviousBlocks;
		var phiBlocks = node.PhiBlocks;

		for (var index = 0; index < node.OperandCount; index++)
		{
			var phiBlock = phiBlocks[index];

			if (previousBlocks.Contains(phiBlock))
				continue;

			phiBlocks.RemoveAt(index);

			for (var i = index; index < node.OperandCount - 1; index++)
			{
				node.SetOperand(i, node.GetOperand(i + 1));
			}

			node.SetOperand(node.OperandCount - 1, null);
			node.OperandCount--;

			index--;
		}

		Debug.Assert(node.OperandCount == node.Block.PreviousBlocks.Count);
	}

	public static void UpdatePhi(Context context)
	{
		UpdatePhi(context.Node);
	}

	public static void RemoveBlockFromPhi(BasicBlock removedBlock, BasicBlock phiBlock)
	{
		for (var node = phiBlock.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
		{
			if (node.IsEmptyOrNop)
				continue;

			if (!IsPhiInstruction(node.Instruction))
				break;

			var sourceBlocks = node.PhiBlocks;

			var index = sourceBlocks.IndexOf(removedBlock);

			if (index < 0)
				continue;

			sourceBlocks.RemoveAt(index);

			for (var i = index; index < node.OperandCount - 1; index++)
			{
				node.SetOperand(i, node.GetOperand(i + 1));
			}

			node.SetOperand(node.OperandCount - 1, null);
			node.OperandCount--;
		}
	}

	public static void UpdatePhiTargets(List<BasicBlock> targets, BasicBlock source, BasicBlock newSource)
	{
		foreach (var target in targets)
		{
			UpdatePhiTarget(target, source, newSource);
		}
	}

	public static void UpdatePhiTarget(BasicBlock phiBlock, BasicBlock source, BasicBlock newSource)
	{
		Debug.Assert(phiBlock.PreviousBlocks.Count > 0);

		for (var node = phiBlock.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
		{
			if (node.IsEmptyOrNop)
				continue;

			if (!IsPhiInstruction(node.Instruction))
				break;

			var index = node.PhiBlocks.IndexOf(source);

			//Debug.Assert(index >= 0);

			node.PhiBlocks[index] = newSource;
		}
	}

	public static bool HasPhiInstruction(BasicBlock target)
	{
		for (var node = target.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
		{
			if (node.IsEmptyOrNop)
				continue;

			if (IsPhiInstruction(node.Instruction))
				return true;

			return false;
		}

		return false;
	}

	#endregion Phi Helpers

	#region Protected Region Methods

	protected MosaExceptionHandler FindImmediateExceptionHandler(int label)
	{
		foreach (var handler in Method.ExceptionHandlers)
		{
			if (handler.IsLabelWithinTry(label) || handler.IsLabelWithinHandler(label))
			{
				return handler;
			}
		}

		return null;
	}

	protected MosaExceptionHandler FindNextEnclosingFinallyHandler(MosaExceptionHandler exceptionHandler)
	{
		var index = Method.ExceptionHandlers.IndexOf(exceptionHandler);
		var at = exceptionHandler.TryStart;

		for (var i = index + 1; i < Method.ExceptionHandlers.Count; i++)
		{
			var entry = Method.ExceptionHandlers[i];

			if (entry.ExceptionHandlerType != ExceptionHandlerType.Finally
				|| !entry.IsLabelWithinTry(at))
				continue;

			return entry;
		}

		return null;
	}

	protected static BasicBlock TraverseBackToNativeBlock(BasicBlock block)
	{
		var start = block;

		while (start.IsCompilerBlock)
		{
			if (!start.HasPreviousBlocks)
				return null;

			start = start.PreviousBlocks[0]; // any one
		}

		return start;
	}

	#endregion Protected Region Methods

	#region Trace Helper Methods

	public bool IsTraceable(int traceLevel)
	{
		return MethodCompiler.IsTraceable(traceLevel);
	}

	protected TraceLog CreateTraceLog(int traceLevel = 0)
	{
		if (!IsTraceable(traceLevel))
			return null;

		var traceLog = new TraceLog(TraceType.MethodDebug, MethodCompiler.Method, FormattedStageName, MethodData.Version);

		traceLogs.Add(traceLog);

		return traceLog;
	}

	public TraceLog CreateTraceLog(string section)
	{
		return CreateTraceLog(section, -1);
	}

	public TraceLog CreateTraceLog(string section, int traceLevel)
	{
		if (!IsTraceable(traceLevel))
			return null;

		var traceLog = new TraceLog(TraceType.MethodDebug, MethodCompiler.Method, FormattedStageName, section, MethodData.Version);

		traceLogs.Add(traceLog);

		return traceLog;
	}

	private void PostTraceLog(TraceLog traceLog)
	{
		MethodCompiler.Compiler.PostTraceLog(traceLog);
	}

	private void PostTraceLogs(List<TraceLog> traceLogs)
	{
		if (traceLogs == null)
			return;

		foreach (var traceLog in traceLogs)
		{
			if (traceLog != null)
			{
				PostTraceLog(traceLog);
			}
		}
	}

	protected void PostEvent(CompilerEvent compileEvent, string message)
	{
		MethodCompiler.Compiler.PostEvent(compileEvent, message, MethodCompiler.ThreadID);
	}

	#endregion Trace Helper Methods

	#region Helper Methods

	public static bool IsMoveInstruction(BaseInstruction instruction)
	{
		return instruction == IRInstruction.Move32
			   || instruction == IRInstruction.Move64
			   || instruction == IRInstruction.MoveObject
			   || instruction == IRInstruction.MoveR8
			   || instruction == IRInstruction.MoveR4;
	}

	public static bool IsCompareInstruction(BaseInstruction instruction)
	{
		return instruction == IRInstruction.Compare32x32
			   || instruction == IRInstruction.Compare32x64
			   || instruction == IRInstruction.Compare64x32
			   || instruction == IRInstruction.Compare64x64
			   || instruction == IRInstruction.CompareObject
			   || instruction == IRInstruction.CompareR4
			   || instruction == IRInstruction.CompareR8;
	}

	public static bool IsPhiInstruction(BaseInstruction instruction)
	{
		return instruction == IRInstruction.Phi32
			   || instruction == IRInstruction.Phi64
			   || instruction == IRInstruction.PhiObject
			   || instruction == IRInstruction.PhiR4
			   || instruction == IRInstruction.PhiR8;
	}

	public static bool IsSSAForm(Operand operand)
	{
		return operand.Definitions.Count == 1;
	}

	/// <summary>
	/// Gets the size of the type.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <param name="align">if set to <c>true</c> [align].</param>
	/// <returns></returns>
	public uint GetTypeSize(MosaType type, bool align)
	{
		return MethodCompiler.GetReferenceOrTypeSize(type, align);
	}

	public List<BasicBlock> AddMissingBlocksIfRequired(List<BasicBlock> blocks)
	{
		if (blocks.Count == BasicBlocks.Count)
			return blocks;

		if (!HasProtectedRegions)
			return blocks;

		var list = new List<BasicBlock>(blocks.Count);

		foreach (var block in blocks)
		{
			if (block == null)
				continue;

			list.Add(block);
		}

		foreach (var block in BasicBlocks)
		{
			if (blocks.Contains(block))
				continue;

			list.Add(block);
		}

		return list;
	}

	protected BaseInstruction GetLoadInstruction(MosaType type)
	{
		type = MosaTypeLayout.GetUnderlyingType(type);

		if (type == null)
			return IRInstruction.LoadCompound;

		if (type.IsReferenceType)
			return IRInstruction.LoadObject;
		else if (type.IsPointer)
			return Select(IRInstruction.Load32, IRInstruction.Load64);
		else if (type.IsI1)
			return Select(IRInstruction.LoadSignExtend8x32, IRInstruction.LoadSignExtend8x64);
		else if (type.IsI2)
			return Select(IRInstruction.LoadSignExtend16x32, IRInstruction.LoadSignExtend16x64);
		else if (type.IsI4)
			return Select(IRInstruction.Load32, IRInstruction.LoadSignExtend32x64);
		else if (type.IsI8)
			return IRInstruction.Load64;
		else if (type.IsU1 || type.IsBoolean)
			return Select(IRInstruction.LoadZeroExtend8x32, IRInstruction.LoadZeroExtend8x64);
		else if (type.IsU2 || type.IsChar)
			return Select(IRInstruction.LoadZeroExtend16x32, IRInstruction.LoadZeroExtend16x64);
		else if (type.IsU4)
			return Select(IRInstruction.Load32, IRInstruction.LoadZeroExtend32x64);
		else if (type.IsU8)
			return IRInstruction.Load64;
		else if (type.IsR4)
			return IRInstruction.LoadR4;
		else if (type.IsR8)
			return IRInstruction.LoadR8;
		else if (Is32BitPlatform)   // review
			return IRInstruction.Load32;
		else if (Is64BitPlatform)
			return IRInstruction.Load64;

		throw new InvalidOperationException();
	}

	public BaseInstruction GetMoveInstruction(MosaType type)
	{
		type = MosaTypeLayout.GetUnderlyingType(type);

		if (type == null)
			return IRInstruction.MoveCompound;

		if (type.IsReferenceType)
			return IRInstruction.MoveObject;
		else if (type.IsPointer)
			return Select(IRInstruction.Move32, IRInstruction.Move64);
		else if (type.IsI1)
			return Select(IRInstruction.SignExtend8x32, IRInstruction.SignExtend8x64);
		else if (type.IsI2)
			return Select(IRInstruction.SignExtend16x32, IRInstruction.SignExtend16x64);
		else if (type.IsI4)
			return Select(IRInstruction.Move32, IRInstruction.Move32);
		else if (type.IsI8)
			return IRInstruction.Move64;
		else if (type.IsU1 || type.IsBoolean)
			return Select(IRInstruction.ZeroExtend8x32, IRInstruction.ZeroExtend8x64);
		else if (type.IsU2 || type.IsChar)
			return Select(IRInstruction.ZeroExtend16x32, IRInstruction.ZeroExtend16x64);
		else if (type.IsU4)
			return Select(IRInstruction.Move32, IRInstruction.ZeroExtend32x64);
		else if (type.IsU8)
			return IRInstruction.Move64;
		else if (type.IsR4)
			return IRInstruction.MoveR4;
		else if (type.IsR8)
			return IRInstruction.MoveR8;
		else if (Is32BitPlatform)   // review
			return IRInstruction.Move32;
		else if (Is64BitPlatform)
			return IRInstruction.Move64;

		throw new InvalidOperationException();
	}

	// TODO: Replace!
	public static BaseIRInstruction GetLoadParameterInstruction(MosaType type, bool is32bitPlatform)
	{
		if (type.IsReferenceType)
			return IRInstruction.LoadParamObject;
		else if (type.IsR4)
			return IRInstruction.LoadParamR4;
		else if (type.IsR8)
			return IRInstruction.LoadParamR8;
		else if (type.IsU1 || type.IsBoolean)
			return IRInstruction.LoadParamZeroExtend8x32;
		else if (type.IsI1)
			return IRInstruction.LoadParamSignExtend8x32;
		else if (type.IsU2 || type.IsChar)
			return IRInstruction.LoadParamZeroExtend16x32;
		else if (type.IsI2)
			return IRInstruction.LoadParamSignExtend16x32;
		else if (type.IsUI4)
			return IRInstruction.LoadParam32;
		else if (type.IsUI8)
			return IRInstruction.LoadParam64;
		else if (type.IsEnum && type.ElementType.IsI4)
			return IRInstruction.LoadParam32;
		else if (type.IsEnum && type.ElementType.IsU4)
			return IRInstruction.LoadParam32;
		else if (type.IsEnum && type.ElementType.IsUI8)
			return IRInstruction.LoadParam64;
		else if (is32bitPlatform)
			return IRInstruction.LoadParam32;
		else
			return IRInstruction.LoadParam64;
	}

	// TODO: Replace!
	public static BaseIRInstruction GetSetReturnInstruction(MosaType type, bool is32bitPlatform)
	{
		if (type == null)
			return null;

		type = MosaTypeLayout.GetUnderlyingType(type);

		if (type == null)
			return IRInstruction.SetReturnCompound;

		if (type.IsReferenceType)
			return IRInstruction.SetReturnObject;
		else if (type.IsR4)
			return IRInstruction.SetReturnR4;
		else if (type.IsR8)
			return IRInstruction.SetReturnR8;
		else if (type.IsUI8 || (type.IsEnum && type.ElementType.IsUI8))
			return IRInstruction.SetReturn64;

		return is32bitPlatform ? IRInstruction.SetReturn32 : IRInstruction.SetReturn64;
	}

	private BaseInstruction Select(BaseInstruction instruction32, BaseInstruction instruction64)
	{
		return Is32BitPlatform ? instruction32 : instruction64;
	}

	public static void ReplaceOperand(Operand target, Operand replacement)
	{
		foreach (var node in target.Uses.ToArray())
		{
			for (var i = 0; i < node.OperandCount; i++)
			{
				var operand = node.GetOperand(i);

				if (target == operand)
				{
					node.SetOperand(i, replacement);
				}
			}
		}
	}

	protected MosaMethod GetMethod(string fullName, string methodName)
	{
		var type = TypeSystem.GetTypeByName(fullName);
		var method = type?.FindMethodByName(methodName);

		return method;
	}

	protected void ReplaceWithCall(Context context, string fullName, string methodName)
	{
		var method = GetMethod(fullName, methodName);

		Debug.Assert(method != null, $"Cannot find method: {methodName}");

		// FUTURE: throw compiler exception

		var symbol = Operand.CreateLabel(method, Is32BitPlatform);

		if (context.OperandCount == 1)
		{
			context.SetInstruction(IRInstruction.CallStatic, context.Result, symbol, context.Operand1);
		}
		else if (context.OperandCount == 2)
		{
			context.SetInstruction(IRInstruction.CallStatic, context.Result, symbol, context.Operand1, context.Operand2);
		}
		else
		{
			// FUTURE: throw compiler exception
		}

		MethodScanner.MethodInvoked(method, Method);
	}

	#endregion Helper Methods

	#region Constant Helper Methods

	protected Operand CreateConstant32(int value)
	{
		return Operand.CreateConstant32((uint)value);
	}

	protected Operand CreateConstant32(uint value)
	{
		return Operand.CreateConstant32(value);
	}

	protected Operand CreateConstant64(long value)
	{
		return Operand.CreateConstant64((ulong)value);
	}

	protected Operand CreateConstant64(ulong value)
	{
		return Operand.CreateConstant64(value);
	}

	protected Operand CreateConstantR4(float value)
	{
		return Operand.CreateConstantR4(value);
	}

	protected Operand CreateConstantR8(double value)
	{
		return Operand.CreateConstantR8(value);
	}

	#endregion Constant Helper Methods

	public void AllStopWithException(string exception)
	{
		MethodCompiler.Stop();
		MethodCompiler.Compiler.Stop();
		throw new CompilerException(exception);
	}

	protected bool CheckAllPhiInstructions()
	{
		foreach (var block in BasicBlocks)
		{
			for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (!IsPhiInstruction(node.Instruction))
					break;

				foreach (var phiblock in node.PhiBlocks)
				{
					if (!block.PreviousBlocks.Contains(phiblock))
					{
						throw new CompilerException("CheckAllPhiInstructions() failed in block: {block} at {node}!");
					}
				}
			}
		}

		return true;
	}
}
