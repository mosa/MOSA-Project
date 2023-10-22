// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Utility.Configuration;

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
	protected MosaSettings MosaSettings { get; private set; }

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
	private readonly List<Counter> RegisteredCounters = new();

	protected uint ObjectHeaderSize;

	#endregion Method Properties

	public Transform Transform { get; private set; }

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
		MosaSettings = compiler.MosaSettings;
		MethodScanner = compiler.MethodScanner;

		Is32BitPlatform = Architecture.Is32BitPlatform;
		Is64BitPlatform = Architecture.Is64BitPlatform;

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
		Transform = methodCompiler.Transform;

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
	}

	public void CleanUp()
	{
		MethodCompiler = null;
		traceLogs = null;
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
		MethodData.Counters.Update(name, count);
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
	protected BasicBlock Split(Node node)
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

	#endregion Block Operations

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

	public static void ReplaceOperand(Operand target, Operand replacement)
	{
		foreach (var node in target.Uses.ToArray())
		{
			node.ReplaceOperand(target, replacement);
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

	#region Check Methods

	public bool FullCheck(bool full = true)
	{
		return CheckVirtualRegisters(full) || CheckAllPhiInstructions() || CheckAllInstructions();
	}

	protected bool CheckVirtualRegisters(bool full)
	{
		foreach (var operand in MethodCompiler.VirtualRegisters)
		{
			if ((full && operand.IsUsed && !operand.IsDefined)
			|| (!full && operand.IsUsed && !operand.IsDefined && !operand.HasParent && !operand.IsParent))
			{
				throw new CompilerException($"CHECK-FAILED: Virtual register used by not defined: {operand}");
			}
		}

		return true;
	}

	protected bool CheckAllInstructions()
	{
		foreach (var block in BasicBlocks)
		{
			for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop || node.Instruction.HasVariableOperands)
					continue;

				if (node.Instruction.DefaultResultCount != node.ResultCount)
				{
					throw new CompilerException($"CHECK-FAILED: Too many results: {block} at {node}");
				}

				if (node.Instruction.DefaultOperandCount != node.OperandCount)
				{
					throw new CompilerException($"CHECK-FAILED: Too many operands: {block} at {node}");
				}
			}
		}

		return true;
	}

	protected bool CheckAllPhiInstructions()
	{
		foreach (var block in BasicBlocks)
		{
			for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop || !node.Instruction.IsPhi)
					break;

				foreach (var phiblock in node.PhiBlocks)
				{
					if (!block.PreviousBlocks.Contains(phiblock))
					{
						throw new CompilerException($"CHECK-FAILED: PHI consistency: {block} at {node}");
					}
				}
			}
		}

		return true;
	}

	// FUTURE: Check Block CompletelyEmpty flag

	#endregion Check Methods
}
