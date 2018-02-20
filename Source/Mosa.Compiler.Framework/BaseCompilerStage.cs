// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.Trace;
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
		protected BaseLinker Linker { get { return Compiler.Linker; } }

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
		protected CompilationScheduler CompilationScheduler { get { return Compiler.CompilationScheduler; } }

		#endregion Properties

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public virtual string Name { get { return GetType().Name; } }

		#endregion IPipelineStage Members

		#region ICompilerStage members

		public void Initialize(Compiler compiler)
		{
			Debug.Assert(compiler != null);

			Compiler = compiler;

			Setup();
		}

		/// <summary>
		/// Executes the pre compile phase.
		/// </summary>
		public void ExecutePreCompile()
		{
			RunPreCompile();
		}

		/// <summary>
		/// Executes the post compile phase.
		/// </summary>
		public void ExecutePostCompile()
		{
			RunPostCompile();
		}

		#endregion ICompilerStage members

		#region Overrides

		/// <summary>
		/// Setups this stage.
		/// </summary>
		protected virtual void Setup()
		{
		}

		/// <summary>
		/// Runs pre compile stage.
		/// </summary>
		protected virtual void RunPreCompile()
		{
		}

		/// <summary>
		/// Runs post compile stage.
		/// </summary>
		protected virtual void RunPostCompile()
		{
		}

		#endregion Overrides

		#region Helper Methods

		protected void NewCompilerTraceEvent(CompilerEvent compilerEvent, string message)
		{
			CompilerTrace.NewCompilerTraceEvent(compilerEvent, message, 0);
		}

		#endregion Helper Methods

		#region Constant Helper Methods

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
			return Operand.CreateConstant(type, value);
		}

		protected static Operand CreateConstant(MosaType type, ulong value)
		{
			return Operand.CreateConstant(type, value);
		}

		protected static Operand CreateConstant(MosaType type, int value)
		{
			return Operand.CreateConstant(type, value);
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
