/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Trace;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Basic base class for compiler stages
	/// </summary>
	public abstract class BaseCompilerStage : ICompilerStage
	{
		#region Properties

		/// <summary>
		/// Holds the compiler.
		/// </summary>
		protected BaseCompiler Compiler { get; private set; }

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
		public virtual string Name { get { return this.GetType().Name; } }

		#endregion IPipelineStage Members

		#region ICompilerStage members

		void ICompilerStage.Initialize(BaseCompiler compiler)
		{
			Debug.Assert(compiler != null);

			this.Compiler = compiler;

			Setup();
		}

		void ICompilerStage.Execute()
		{
			Run();
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
		/// Runs this stage.
		/// </summary>
		protected virtual void Run()
		{
		}

		#endregion Overrides

		#region Helper Methods

		protected void NewCompilerTraceEvent(CompilerEvent compilerEvent, string message)
		{
			CompilerTrace.NewCompilerTraceEvent(compilerEvent, message, 0);
		}

		#endregion Helper Methods
	}
}