/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework
{

	/// <summary>
	/// Base class for just-in-time and ahead-of-time compilers, which use
	/// the Mosa.Compiler.Framework framework.
	/// </summary>
	public abstract class AssemblyCompiler : IDisposable
	{
		#region Data members

		/// <summary>
		/// The compiler target architecture.
		/// </summary>
		private IArchitecture architecture;

		/// <summary>
		/// The pipeline of the assembly compiler.
		/// </summary>
		private CompilerPipeline pipeline;

		/// <summary>
		/// Holds the current type system during compilation.
		/// </summary>
		private ITypeSystem typeSystem;

		/// <summary>
		/// Holds the current type layout during complication
		/// </summary>
		private ITypeLayout typeLayout;

		/// <summary>
		/// Holds the current internal log
		/// </summary>
		private IInternalTrace internalTrace;

		/// <summary>
		/// Holds the compiler option set
		/// </summary>
		private CompilerOptions compilerOptions;

		/// <summary>
		/// Holds the generic type patcher
		/// </summary>
		private IGenericTypePatcher genericTypePatcher;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new compiler instance.
		/// </summary>
		/// <param name="architecture">The compiler target architecture.</param>
		/// <param name="typeSystem">The type system.</param>
		protected AssemblyCompiler(IArchitecture architecture, ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalTrace internalTrace, CompilerOptions compilerOptions)
		{
			if (architecture == null)
				throw new ArgumentNullException(@"architecture");

			this.pipeline = new CompilerPipeline();
			this.architecture = architecture;
			this.typeSystem = typeSystem;
			this.typeLayout = typeLayout;
			this.internalTrace = internalTrace;
			this.compilerOptions = compilerOptions;
			this.genericTypePatcher = new GenericTypePatcher(typeSystem);
		}

		#endregion // Construction

		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public virtual void Dispose()
		{
		}

		#endregion // IDisposable Members

		#region Properties

		/// <summary>
		/// Returns the architecture used by the compiler.
		/// </summary>
		public IArchitecture Architecture
		{
			get { return architecture; }
		}

		/// <summary>
		/// Gets the pipeline.
		/// </summary>
		/// <value>The pipeline.</value>
		public CompilerPipeline Pipeline
		{
			get { return pipeline; }
		}

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>The type system.</value>
		public ITypeSystem TypeSystem
		{
			get { return typeSystem; }
		}

		/// <summary>
		/// Gets the type layout.
		/// </summary>
		/// <value>The type layout.</value>
		public ITypeLayout TypeLayout
		{
			get { return typeLayout; }
		}

		/// <summary>
		/// Gets the internal log.
		/// </summary>
		/// <value>The internal log.</value>
		public IInternalTrace InternalTrace
		{
			get { return internalTrace; }
		}

		/// <summary>
		/// Gets the compiler options.
		/// </summary>
		/// <value>The compiler options.</value>
		public CompilerOptions CompilerOptions
		{
			get { return compilerOptions; }
		}

		/// <summary>
		/// Gets the generic type patcher.
		/// </summary>
		public IGenericTypePatcher GenericTypePatcher
		{
			get
			{
				return genericTypePatcher;
			}
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Creates a method compiler
		/// </summary>
		/// <param name="schedulerStage">The scheduler stage.</param>
		/// <param name="type">The type.</param>
		/// <param name="method">The method to compile.</param>
		/// <returns>
		/// An instance of a MethodCompilerBase for the given type/method pair.
		/// </returns>
		public abstract IMethodCompiler CreateMethodCompiler(ICompilationSchedulerStage schedulerStage, RuntimeType type, RuntimeMethod method);

		/// <summary>
		/// Executes the compiler using the configured stages.
		/// </summary>
		/// <remarks>
		/// The method iterates the compilation stage chain and runs each 
		/// stage on the input.
		/// </remarks>
		protected void Compile()
		{
			BeginCompile();

			foreach (IAssemblyCompilerStage stage in Pipeline)
			{
				Trace(CompilerEvent.AssemblyStageStart, stage.Name);

				// Execute stage
				stage.Run();

				Trace(CompilerEvent.AssemblyStageEnd, stage.Name);
			}

			EndCompile();
		}

		/// <summary>
		/// Called when compilation is about to begin.
		/// </summary>
		protected virtual void BeginCompile()
		{
			foreach (IAssemblyCompilerStage stage in Pipeline)
			{
				// Setup Compiler
				stage.Setup(this);
			}
		}

		/// <summary>
		/// Called when compilation has completed.
		/// </summary>
		protected virtual void EndCompile() { }

		#endregion // Methods

		#region Helper Methods

		protected void Trace(CompilerEvent compilerEvent, string message)
		{
			internalTrace.CompilerEventListener.SubmitTraceEvent(compilerEvent, message);
		}

		#endregion
	}
}
