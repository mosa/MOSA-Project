// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
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
		protected BaseArchitecture Architecture { get { return Compiler.Architecture; } }

		/// <summary>
		/// Holds the current type system during compilation.
		/// </summary>
		protected TypeSystem TypeSystem { get { return Compiler.TypeSystem; } }

		/// <summary>
		/// Holds the current type layout during compilation.
		/// </summary>
		protected MosaTypeLayout TypeLayout { get { return Compiler.TypeLayout; } }

		/// <summary>
		/// Holds the linker
		/// </summary>
		protected MosaLinker Linker { get { return Compiler.Linker; } }

		/// <summary>
		/// Holds the compiler options
		/// </summary>
		protected CompilerOptions CompilerOptions { get { return Compiler.CompilerOptions; } }

		/// <summary>
		/// Holds the compiler trace
		/// </summary>
		protected CompilerTrace CompilerTrace { get { return Compiler.CompilerTrace; } }

		/// <summary>
		/// Holds the compiler scheduler
		/// </summary>
		protected MethodScheduler CompilationScheduler { get { return Compiler.MethodScheduler; } }

		protected MethodScanner MethodScanner { get { return Compiler.MethodScanner; } }

		#endregion Properties

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public virtual string Name { get { return GetType().Name; } }

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
			{
				return;
			}

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
			CompilerTrace.PostCompilerTraceEvent(compilerEvent, message, 0);
		}

		protected void PostTrace(TraceLog traceLog)
		{
			if (traceLog == null)
				return;

			CompilerTrace.PostTraceLog(traceLog);
		}

		#endregion Helper Methods

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
