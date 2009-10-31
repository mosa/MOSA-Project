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
        private IArchitecture _architecture;

        /// <summary>
        /// The assembly of this compiler.
        /// </summary>
        private IMetadataModule _assembly;

        /// <summary>
        /// The pipeline of the assembly compiler.
        /// </summary>
        private CompilerPipeline<IAssemblyCompilerStage> _pipeline;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new compiler instance.
		/// </summary>
        /// <param name="architecture">The compiler target architecture.</param>
        /// <param name="assembly">The assembly of this compiler.</param>
        /// <exception cref="System.ArgumentNullException">Either <paramref name="architecture"/> or <paramref name="assembly"/> is null.</exception>
		protected AssemblyCompiler(IArchitecture architecture, IMetadataModule assembly)
		{
            if (null == architecture)
                throw new ArgumentNullException(@"architecture");

            _architecture = architecture;
            _assembly = assembly;
            _pipeline = new CompilerPipeline<IAssemblyCompilerStage>();
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
            get { return _architecture; }
        }

        /// <summary>
        /// Gets the assembly.
        /// </summary>
        /// <value>The assembly.</value>
        public IMetadataModule Assembly
        {
            get { return _assembly; }
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>The metadata.</value>
        public virtual IMetadataProvider Metadata
        {
            get { return _assembly.Metadata; }
        }

        /// <summary>
        /// Gets the pipeline.
        /// </summary>
        /// <value>The pipeline.</value>
        public CompilerPipeline<IAssemblyCompilerStage> Pipeline
        {
            get { return _pipeline; }
        }

		#endregion // Properties

		#region Methods

        /// <summary>
        /// Creates a method compiler
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method to compile.</param>
        /// <returns>An instance of a MethodCompilerBase for the given type/method pair.</returns>
        public abstract MethodCompilerBase CreateMethodCompiler(RuntimeType type, RuntimeMethod method);

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

            Pipeline.Execute(delegate(IAssemblyCompilerStage stage)
            {
                stage.Run(this);
            });

            EndCompile();
        }

        /// <summary>
        /// Called when compilation is about to begin.
        /// </summary>
        protected virtual void BeginCompile() { }

        /// <summary>
        /// Called when compilation has completed.
        /// </summary>
        protected virtual void EndCompile() { }

        #endregion // Methods
    }
}
