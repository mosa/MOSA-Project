/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;

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
		protected ILinker Linker { get { return Compiler.Linker; } }

		/// <summary>
		/// Holds the compiler options
		/// </summary>
		protected CompilerOptions CompilerOptions { get { return Compiler.CompilerOptions; } }

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

		protected void Trace(CompilerEvent compilerEvent, string message)
		{
			Compiler.InternalTrace.CompilerEventListener.SubmitTraceEvent(compilerEvent, message);
		}

		protected void Trace(MosaMethod method, string stage, string line)
		{
			Compiler.InternalTrace.TraceListener.SubmitDebugStageInformation(method, stage, line);
		}

		#endregion Helper Methods
	}
}