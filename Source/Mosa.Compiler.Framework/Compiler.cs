// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

public abstract class Compiler
{
	#region Properties

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

	#endregion

	public abstract void Setup();

	public abstract void ExecuteCompile(int maxThreads = 0);

	public abstract void Finalization();

	public abstract IntrinsicMethodDelegate GetIntrinsicMethod(string name);

	public abstract StubMethodDelegate GetStubMethod(string name);

	public abstract void CompileMethod(MosaMethod method);

	public abstract void CompileMethod(MosaMethod method, BasicBlocks basicBlocks, int threadID = 0);

	public abstract void Stop();

	#region Methods

	/// <summary>
	/// Compiles the linker method.
	/// </summary>
	/// <param name="methodName">Name of the method.</param>
	/// <returns></returns>
	public MosaMethod CreateLinkerMethod(string methodName)
	{
		return TypeSystem.CreateLinkerMethod(methodName, TypeSystem.BuiltIn.Void, false, null);
	}

	public string SearchPathsForFile(string filename)
	{
		foreach (var path in CompilerSettings.SearchPaths)
		{
			var file = Path.Combine(path, filename);

			if (File.Exists(file))
			{
				return file;
			}
		}

		return null;
	}

	public byte[] SearchPathsForFileAndLoad(string filename)
	{
		var file = SearchPathsForFile(filename);

		if (file == null)
			return null;

		return File.ReadAllBytes(file);
	}

	/// <summary>
	/// Traces the specified compiler event.
	/// </summary>
	/// <param name="compilerEvent">The compiler event.</param>
	/// <param name="message">The message.</param>
	/// <param name="threadID">The thread identifier.</param>
	public void PostEvent(CompilerEvent compilerEvent, string message = null, int threadID = 0)
	{
		CompilerHooks.NotifyEvent?.Invoke(compilerEvent, message ?? string.Empty, threadID);
	}

	#endregion

	#region Helper Methods

	public bool IsTraceable(int traceLevel)
	{
		return TraceLevel >= traceLevel;
	}

	public void PostTraceLog(TraceLog traceLog)
	{
		if (traceLog == null)
			return;

		CompilerHooks.NotifyTraceLog?.Invoke(traceLog);
	}

	public MosaType GetPlatformInternalRuntimeType()
	{
		return TypeSystem.GetTypeByName("Mosa.Runtime." + Architecture.PlatformName + ".Internal");
	}

	public MosaType GeInternalRuntimeType()
	{
		return TypeSystem.GetTypeByName("Mosa.Runtime.Internal");
	}

	public MethodData GetMethodData(MosaMethod method)
	{
		return CompilerData.GetMethodData(method);
	}

	#endregion Helper Methods

	#region Type Methods

	public MosaType GetTypeFromTypeCode(MosaTypeCode code)
	{
		return code switch
		{
			MosaTypeCode.Void => TypeSystem.BuiltIn.Void,
			MosaTypeCode.Boolean => TypeSystem.BuiltIn.Boolean,
			MosaTypeCode.Char => TypeSystem.BuiltIn.Char,
			MosaTypeCode.I1 => TypeSystem.BuiltIn.I1,
			MosaTypeCode.U1 => TypeSystem.BuiltIn.U1,
			MosaTypeCode.I2 => TypeSystem.BuiltIn.I2,
			MosaTypeCode.U2 => TypeSystem.BuiltIn.U2,
			MosaTypeCode.I4 => TypeSystem.BuiltIn.I4,
			MosaTypeCode.U4 => TypeSystem.BuiltIn.U4,
			MosaTypeCode.I8 => TypeSystem.BuiltIn.I8,
			MosaTypeCode.U8 => TypeSystem.BuiltIn.U8,
			MosaTypeCode.R4 => TypeSystem.BuiltIn.R4,
			MosaTypeCode.R8 => TypeSystem.BuiltIn.R8,
			MosaTypeCode.I => TypeSystem.BuiltIn.I,
			MosaTypeCode.U => TypeSystem.BuiltIn.U,
			MosaTypeCode.String => TypeSystem.BuiltIn.String,
			MosaTypeCode.TypedRef => TypeSystem.BuiltIn.TypedRef,
			MosaTypeCode.Object => TypeSystem.BuiltIn.Object,
			_ => throw new CompilerException("Can't convert type code {code} to type")
		};
	}

	public StackTypeCode GetStackTypeCode(MosaType type)
	{
		switch (type.IsEnum ? type.GetEnumUnderlyingType().TypeCode : type.TypeCode)
		{
			case MosaTypeCode.Boolean:
			case MosaTypeCode.Char:
			case MosaTypeCode.I1:
			case MosaTypeCode.U1:
			case MosaTypeCode.I2:
			case MosaTypeCode.U2:
			case MosaTypeCode.I4:
			case MosaTypeCode.U4:
				if (Architecture.Is32BitPlatform)
					return StackTypeCode.Int32;
				else
					return StackTypeCode.Int64;

			case MosaTypeCode.I8:
			case MosaTypeCode.U8:
				return StackTypeCode.Int64;

			case MosaTypeCode.R4:
			case MosaTypeCode.R8:
				return StackTypeCode.F;

			case MosaTypeCode.I:
			case MosaTypeCode.U:
				if (Architecture.Is32BitPlatform)
					return StackTypeCode.Int32;
				else
					return StackTypeCode.Int64;

			case MosaTypeCode.ManagedPointer:
				return StackTypeCode.ManagedPointer;

			case MosaTypeCode.UnmanagedPointer:
			case MosaTypeCode.FunctionPointer:
				return StackTypeCode.UnmanagedPointer;

			case MosaTypeCode.String:
			case MosaTypeCode.ValueType:
			case MosaTypeCode.ReferenceType:
			case MosaTypeCode.Array:
			case MosaTypeCode.Object:
			case MosaTypeCode.SZArray:
			case MosaTypeCode.Var:
			case MosaTypeCode.MVar:
				return StackTypeCode.O;

			case MosaTypeCode.Void:
				return StackTypeCode.Unknown;
		}

		throw new CompilerException($"Can't transform Type {type} to StackTypeCode");
	}

	public MosaType GetStackType(MosaType type)
	{
		switch (GetStackTypeCode(type))
		{
			case StackTypeCode.Int32:
				return type.TypeSystem.BuiltIn.I4;

			case StackTypeCode.Int64:
				return type.TypeSystem.BuiltIn.I8;

			case StackTypeCode.N:
				return type.TypeSystem.BuiltIn.I;

			case StackTypeCode.F:
				if (type.IsR4)
					return type.TypeSystem.BuiltIn.R4;
				else
					return type.TypeSystem.BuiltIn.R8;

			case StackTypeCode.O:
				return type;

			case StackTypeCode.UnmanagedPointer:
			case StackTypeCode.ManagedPointer:
				return type;
		}

		throw new CompilerException($"Can't convert {type.FullName} to stack type");
	}

	public MosaType GetStackTypeFromCode(StackTypeCode code)
	{
		return code switch
		{
			StackTypeCode.Int32 => TypeSystem.BuiltIn.I4,
			StackTypeCode.Int64 => TypeSystem.BuiltIn.I8,
			StackTypeCode.N => TypeSystem.BuiltIn.I,
			StackTypeCode.F => TypeSystem.BuiltIn.R8,
			StackTypeCode.O => TypeSystem.BuiltIn.Object,
			StackTypeCode.UnmanagedPointer => TypeSystem.BuiltIn.Pointer,
			StackTypeCode.ManagedPointer => TypeSystem.BuiltIn.Object.ToManagedPointer(),
			_ => throw new CompilerException($"Can't convert stack type code {code} to type")
		};
	}

	#endregion Type Methods
}
