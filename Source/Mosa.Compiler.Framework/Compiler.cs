// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

public abstract class Compiler
{
	/// <summary>
	/// Returns the architecture used by the compiler.
	/// </summary>
	public BaseArchitecture Architecture { get; init; }

	/// <summary>
	/// Gets the compiler pipeline.
	/// </summary>
	public Pipeline<BaseCompilerStage> CompilerPipeline { get; } = new Pipeline<BaseCompilerStage>();

	/// <summary>
	/// Gets the type system.
	/// </summary>
	public TypeSystem TypeSystem { get; init; }

	/// <summary>
	/// Gets the type layout.
	/// </summary>
	public MosaTypeLayout TypeLayout { get; init; }

	/// <summary>
	/// Gets the compiler options.
	/// </summary>
	public CompilerSettings CompilerSettings { get; init; }

	/// <summary>
	/// Gets the method scanner.
	/// </summary>
	public MethodScanner MethodScanner { get; init; }

	/// <summary>
	/// Gets the counters.
	/// </summary>
	public Counters GlobalCounters { get; } = new Counters();

	/// <summary>
	/// Gets the scheduler.
	/// </summary>
	public MethodScheduler MethodScheduler { get; init; }

	/// <summary>
	/// Gets the linker.
	/// </summary>
	public MosaLinker Linker { get; init; }

	/// <summary>
	/// Gets the plug system.
	/// </summary>
	public PlugSystem PlugSystem { get; init; }

	/// <summary>
	/// Gets the type of the platform internal runtime.
	/// </summary>
	public MosaType PlatformInternalRuntimeType { get; init; }

	/// <summary>
	/// Gets the type of the internal runtime.
	/// </summary>
	public MosaType InternalRuntimeType { get; init; }

	/// <summary>
	/// Gets the compiler data.
	/// </summary>
	public CompilerData CompilerData { get; } = new CompilerData();

	/// <summary>
	/// Gets or sets a value indicating whether [all stop].
	/// </summary>
	public bool IsStopped { get; set; }

	/// <summary>
	/// The stack frame
	/// </summary>
	public Operand StackFrame { get; init; }

	/// <summary>
	/// The stack frame
	/// </summary>
	public Operand StackPointer { get; init; }

	/// <summary>
	/// The program counter
	/// </summary>
	public Operand ProgramCounter { get; init; }

	/// <summary>
	/// The link register
	/// </summary>
	public Operand LinkRegister { get; init; }

	/// <summary>
	/// The exception register
	/// </summary>
	public Operand ExceptionRegister { get; init; }

	/// <summary>
	/// The ;eave target register
	/// </summary>
	public Operand LeaveTargetRegister { get; init; }

	public CompilerHooks CompilerHooks { get; init; }

	public int TraceLevel { get; init; }

	public bool Statistics { get; init; }

	public uint ObjectHeaderSize { get; init; }

	public abstract void Setup();

	public abstract void ExecuteCompile(int maxThreads = 0);

	public abstract void Finalization();

	public abstract IntrinsicMethodDelegate GetIntrinsicMethod(string name);

	public abstract StubMethodDelegate GetStubMethod(string name);

	public abstract void CompileMethod(MosaMethod method);

	public abstract void CompileMethod(MosaMethod method, BasicBlocks basicBlocks, int threadID = 0);

	public abstract void Stop();

	public abstract void PostEvent(CompilerEvent compilerEvent, string message = null, int threadID = 0);

	public abstract MosaType GetTypeFromTypeCode(MosaTypeCode code);

	public abstract StackTypeCode GetStackTypeCode(MosaType type);

	public abstract MosaType GetStackType(MosaType type);

	public abstract MosaType GetStackTypeFromCode(StackTypeCode code);

	public abstract MosaMethod CreateLinkerMethod(string methodName);

	public abstract byte[] SearchPathsForFileAndLoad(string filename);

	public abstract bool IsTraceable(int traceLevel);

	public abstract void PostTraceLog(TraceLog traceLog);

	public abstract MethodData GetMethodData(MosaMethod method);
}
