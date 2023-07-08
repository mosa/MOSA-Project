// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Utility.Configuration;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Basic base class for compiler stages
/// </summary>
public abstract class BaseCompilerStage
{
	#region Properties

	/// <summary>
	/// Holds the compiler.
	/// </summary>
	protected Compiler Compiler { get; private set; }

	/// <summary>
	/// Holds the Architecture during compilation.
	/// </summary>
	protected BaseArchitecture Architecture => Compiler.Architecture;

	/// <summary>
	/// Holds the current type system during compilation.
	/// </summary>
	protected TypeSystem TypeSystem => Compiler.TypeSystem;

	/// <summary>
	/// Holds the current type layout during compilation.
	/// </summary>
	protected MosaTypeLayout TypeLayout => Compiler.TypeLayout;

	/// <summary>
	/// Holds the linker
	/// </summary>
	protected MosaLinker Linker => Compiler.Linker;

	/// <summary>
	/// Holds the compiler options
	/// </summary>
	protected MosaSettings MosaSettings => Compiler.MosaSettings;

	/// <summary>
	/// Holds the compiler scheduler
	/// </summary>
	protected MethodScheduler CompilationScheduler => Compiler.MethodScheduler;

	protected MethodScanner MethodScanner => Compiler.MethodScanner;

	/// <summary>
	/// Retrieves the name of the compilation stage.
	/// </summary>
	/// <value>The name of the compilation stage.</value>
	public virtual string Name => GetType().Name;

	#endregion Properties

	/// <summary>
	/// Executes the initialization stage.
	/// </summary>
	public void ExecuteInitialization(Compiler compiler)
	{
		Debug.Assert(compiler != null);

		Compiler = compiler;

		Initialization();
	}

	/// <summary>
	/// Executes the setup stage.
	/// </summary>
	public void ExecuteSetup()
	{
		if (Compiler.IsStopped)
			return;

		Setup();
	}

	/// <summary>
	/// Executes finalization stage.
	/// </summary>
	public void ExecuteFinalization()
	{
		if (Compiler.IsStopped)
		{
			return;
		}

		Finalization();
	}

	#region Overrides

	/// <summary>
	/// Runs the initialize stage.
	/// </summary>
	protected virtual void Initialization()
	{
	}

	/// <summary>
	/// Runs the setup stage.
	/// </summary>
	protected virtual void Setup()
	{
	}

	/// <summary>
	/// Runs the finalization stage.
	/// </summary>
	protected virtual void Finalization()
	{
	}

	#endregion Overrides

	#region Helper Methods

	protected void PostCompilerTraceEvent(CompilerEvent compilerEvent, string message)
	{
		Compiler.PostEvent(compilerEvent, message, 0);
	}

	protected void PostTraceLog(TraceLog traceLog)
	{
		Compiler.PostTraceLog(traceLog);
	}

	#endregion Helper Methods

	#region Constant Helper Methods

	protected Operand CreateConstant(int value)
	{
		return Operand.CreateConstant32((uint)value);
	}

	protected Operand CreateConstant(uint value)
	{
		return Operand.CreateConstant32(value);
	}

	protected Operand CreateConstant(long value)
	{
		return Operand.CreateConstant64((ulong)value);
	}

	protected Operand CreateConstant(ulong value)
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
}
