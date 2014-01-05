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
	/// Basic base class for compiler pipeline stages
	/// </summary>
	public abstract class BaseCompilerStage
	{
		#region Data members

		/// <summary>
		/// Holds the Architecture during compilation.
		/// </summary>
		protected BaseArchitecture architecture;

		/// <summary>
		/// Holds the compiler.
		/// </summary>
		protected BaseCompiler compiler;

		/// <summary>
		/// Holds the current type system during compilation.
		/// </summary>
		protected TypeSystem typeSystem;

		/// <summary>
		/// Holds the current type layout during compilation.
		/// </summary>
		protected MosaTypeLayout typeLayout;

		/// <summary>
		/// Holds the linker
		/// </summary>
		protected ILinker linker;

		#endregion Data members

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public virtual string Name { get { return this.GetType().Name; } }

		#endregion IPipelineStage Members

		#region ICompilerStage members

		public void Setup(BaseCompiler compiler)
		{
			this.compiler = compiler;
			architecture = compiler.Architecture;
			typeSystem = compiler.TypeSystem;
			typeLayout = compiler.TypeLayout;
			linker = compiler.Linker;
		}

		#endregion ICompilerStage members

		#region Helper Methods

		protected void Trace(CompilerEvent compilerEvent, string message)
		{
			compiler.InternalTrace.CompilerEventListener.SubmitTraceEvent(compilerEvent, message);
		}

		protected void Trace(MosaMethod method, string stage, string line)
		{
			compiler.InternalTrace.TraceListener.SubmitDebugStageInformation(method, stage, line);
		}

		#endregion Helper Methods
	}
}