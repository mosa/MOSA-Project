/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Linker;
using Mosa.Compiler.TypeSystem;

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
		protected IArchitecture architecture;

		/// <summary>
		/// Holds the compiler.
		/// </summary>
		protected BaseCompiler compiler;

		/// <summary>
		/// Holds the current type system during compilation.
		/// </summary>
		protected ITypeSystem typeSystem;

		/// <summary>
		/// Holds the current type layout during compilation.
		/// </summary>
		protected ITypeLayout typeLayout;

		#endregion // Data members

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public virtual string Name { get { return this.GetType().Name; } }

		#endregion // IPipelineStage Members

		#region ICompilerStage members

		public void Setup(BaseCompiler compiler)
		{
			this.compiler = compiler;
			architecture = compiler.Architecture;
			typeSystem = compiler.TypeSystem;
			typeLayout = compiler.TypeLayout;
		}

		#endregion // ICompilerStage members

		#region Methods

		/// <summary>
		/// Retrieves the linker from compiler.
		/// </summary>
		/// <returns>The retrieved linker.</returns>
		protected ILinker RetrieveLinkerFromCompiler()
		{
			ILinker linker = compiler.Pipeline.FindFirst<ILinker>();

			if (linker == null)
				throw new InvalidOperationException(@"A linker is required.");

			return linker;
		}

		#endregion // Methods

		#region Helper Methods

		protected void Trace(CompilerEvent compilerEvent, string message)
		{
			compiler.InternalTrace.CompilerEventListener.SubmitTraceEvent(compilerEvent, message);
		}

		protected void Trace(RuntimeMethod method, string stage, string line)
		{
			compiler.InternalTrace.TraceListener.SubmitDebugStageInformation(method, stage, line);
		}

		#endregion

	}
}
