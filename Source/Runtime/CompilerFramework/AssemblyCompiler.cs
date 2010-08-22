/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;

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
		/// The assembly of this compiler.
		/// </summary>
		private IMetadataModule assembly;

		/// <summary>
		/// The pipeline of the assembly compiler.
		/// </summary>
		private CompilerPipeline pipeline;

		/// <summary>
		/// Holds the current type system during compilation.
		/// </summary>
		protected ITypeSystem typeSystem;

		/// <summary>
		/// Holds the assembly loader.
		/// </summary>
		/// <value>The assembly loader.</value>
		protected IAssemblyLoader assemblyLoader;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new compiler instance.
		/// </summary>
		/// <param name="architecture">The compiler target architecture.</param>
		/// <param name="assembly">The assembly of this compiler.</param>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="assemblyLoader">The assembly loader.</param>
		/// <exception cref="System.ArgumentNullException">Either <paramref name="architecture"/> or <paramref name="assembly"/> is null.</exception>
		protected AssemblyCompiler(IArchitecture architecture, IMetadataModule assembly, ITypeSystem typeSystem, IAssemblyLoader assemblyLoader)
		{
			if (architecture == null)
				throw new ArgumentNullException(@"architecture");

			this.architecture = architecture;
			this.assembly = assembly;
			this.pipeline = new CompilerPipeline();
			this.typeSystem = typeSystem;
			this.assemblyLoader = assemblyLoader;
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
		/// Gets the assembly.
		/// </summary>
		/// <value>The assembly.</value>
		public IMetadataModule MainAssembly
		{
			get { return assembly; }
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
		/// Gets the assembly loader.
		/// </summary>
		/// <value>The assembly loader.</value>
		public IAssemblyLoader AssemblyLoader
		{
			get { return assemblyLoader; }
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Creates a method compiler
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="method">The method to compile.</param>
		/// <returns>An instance of a MethodCompilerBase for the given type/method pair.</returns>
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

			Pipeline.Execute<IAssemblyCompilerStage>(stage => stage.Run());

			EndCompile();
		}

		/// <summary>
		/// Called when compilation is about to begin.
		/// </summary>
		protected virtual void BeginCompile()
		{
			Pipeline.Execute<IAssemblyCompilerStage>(stage => stage.Setup(this));
		}

		/// <summary>
		/// Called when compilation has completed.
		/// </summary>
		protected virtual void EndCompile() { }

		#endregion // Methods
	}
}
