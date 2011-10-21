/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Compiler.Linker;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Basic base class for assembly compiler pipeline stages
	/// </summary>
	public abstract class BaseAssemblyCompilerStage
	{
		#region Data members

		/// <summary>
		/// Holds the Architecture during compilation.
		/// </summary>
		protected IArchitecture architecture;

		/// <summary>
		/// Holds the assembly Compiler.
		/// </summary>
		protected AssemblyCompiler compiler;

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

		#region IAssemblyCompilerStage members

		public void Setup(AssemblyCompiler compiler)
		{
			this.compiler = compiler;
			architecture = compiler.Architecture;
			typeSystem = compiler.TypeSystem;
			typeLayout = compiler.TypeLayout;
		}

		#endregion // IAssemblyCompilerStage members

		#region Methods

		/// <summary>
		/// Retrieves the assembly linker from compiler.
		/// </summary>
		/// <returns>The retrieved assembly linker.</returns>
		protected IAssemblyLinker RetrieveAssemblyLinkerFromCompiler()
		{
			IAssemblyLinker linker = compiler.Pipeline.FindFirst<IAssemblyLinker>();

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

		#endregion

	}
}
