/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.InternalLog;

namespace Mosa.Runtime.CompilerFramework
{

	/// <summary>
	/// Base class for just-in-time and ahead-of-time compilers, which use
	/// the Mosa.Runtime.CompilerFramework framework.
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
		protected ITypeSystem typeSystem;

		/// <summary>
		/// Holds the current type layout during complication
		/// </summary>
		protected ITypeLayout typeLayout;

		/// <summary>
		/// Holds the current internal log
		/// </summary>
		protected IInternalLog internalLog;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new compiler instance.
		/// </summary>
		/// <param name="architecture">The compiler target architecture.</param>
		/// <param name="typeSystem">The type system.</param>
		protected AssemblyCompiler(IArchitecture architecture, ITypeSystem typeSystem, ITypeLayout typeLayout, IInternalLog internalLog)
		{
			if (architecture == null)
				throw new ArgumentNullException(@"architecture");

			this.architecture = architecture;
			this.pipeline = new CompilerPipeline();
			this.typeSystem = typeSystem;
			this.typeLayout = typeLayout;
			this.internalLog = internalLog;
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

		public ITypeLayout TypeLayout
		{
			get { return typeLayout; }
		}

		public IInternalLog InternalLog
		{
			get { return internalLog; }
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

			//Pipeline.Execute<IAssemblyCompilerStage>(stage => stage.Run());
			foreach (IAssemblyCompilerStage stage in Pipeline)
			{
				NotifyCompilerEvent(CompilerEvent.AssemblyStageStart, stage.Name);
				stage.Run();
				NotifyCompilerEvent(CompilerEvent.AssemblyStageEnd, stage.Name);
			}

			EndCompile();
		}

		/// <summary>
		/// Called when compilation is about to begin.
		/// </summary>
		protected virtual void BeginCompile()
		{
			//Pipeline.Execute<IAssemblyCompilerStage>(stage => stage.Setup(this));
			foreach (IAssemblyCompilerStage stage in Pipeline)
			{
				stage.Setup(this);
			}
		}

		/// <summary>
		/// Called when compilation has completed.
		/// </summary>
		protected virtual void EndCompile() { }

		#endregion // Methods

		#region Helper Methods

		protected void NotifyCompilerEvent(CompilerEvent compilerEvent, string message)
		{
			internalLog.CompilerEventListener.NotifyCompilerEvent(compilerEvent, message);
		}

		#endregion
	}
}
