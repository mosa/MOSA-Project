/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework
{

	/// <summary>
	/// Base class for just-in-time and ahead-of-time compilers, which use
	/// the Mosa.Runtime.CompilerFramework framework.
	/// </summary>
	public abstract class AssemblyCompiler
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
        /// Holds the current compilation type.
        /// </summary>
        private RuntimeType _currentType;

        /// <summary>
        /// The pipeline of the assembly compiler.
        /// </summary>
        private CompilerPipeline<IAssemblyCompilerStage> _pipeline;

        /// <summary>
        /// Compilation schedule.
        /// </summary>
        private Queue<MethodCompilerBase> _schedule;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new compiler instance.
		/// </summary>
        /// <param name="architecture">The compiler target architecture.</param>
        /// <param name="assembly">The assembly of this compiler.</param>
        /// <exception cref="System.ArgumentNullException">Either <paramref name="architecture"/> or <paramref name="stages"/> is null.</exception>
		protected AssemblyCompiler(IArchitecture architecture, IMetadataModule assembly)
		{
            if (null == architecture)
                throw new ArgumentNullException(@"architecture");

            _architecture = architecture;
            _assembly = assembly;
            _pipeline = new CompilerPipeline<IAssemblyCompilerStage>();
            _schedule = new Queue<MethodCompilerBase>();
		}

		#endregion // Construction

		#region Properties

        /// <summary>
        /// Returns the architecture used by the compiler.
        /// </summary>
        public IArchitecture Architecture
        {
            get { return _architecture; }
        }


        public IMetadataModule Assembly
        {
            get { return _assembly; }
        }

        public RuntimeType CurrentType
        {
            get { return _currentType; }
        }

        public virtual IMetadataProvider Metadata
        {
            get { return _assembly.Metadata; }
        }

        public CompilerPipeline<IAssemblyCompilerStage> Pipeline
        {
            get { return _pipeline; }
        }

		#endregion // Properties

		#region Methods

        /// <summary>
        /// Creates a method compiler 
        /// </summary>
        /// <param name="method">The method to compile.</param>
        /// <returns></returns>
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

            IMetadataProvider metadata = this.Metadata;
            //TokenTypes maxTypeToken = metadata.GetMaxTokenValue(TokenTypes.TypeDef);

            IMetadataModule module = this.Assembly;


            ReadOnlyRuntimeTypeListView types = RuntimeBase.Instance.TypeLoader.GetTypesFromModule(this.Assembly);
            foreach (RuntimeType type in types) 
            {
                _currentType = type;
                //_currentType = new MetadataTypeDefinition(metadata, typeToken);
                //Debug.WriteLine(String.Format("Type: {0}", _currentType));
                
                // Do not compile generic types
				if (false == _currentType.IsGeneric)
				{
                    this.Pipeline.Execute(delegate(IAssemblyCompilerStage stage) {
                        stage.Run(this);
                    });
				}
            }

            // Run all scheduled method compilers...
            while (0 != _schedule.Count)
            {
                MethodCompilerBase mc = _schedule.Dequeue();
                mc.Compile();
                mc.Dispose();
            }
        }

        #endregion // Methods

        public void ScheduleMethodForCompilation(RuntimeType type, RuntimeMethod method)
        {
            MethodCompilerBase mc = CreateMethodCompiler(type, method);
            _schedule.Enqueue(mc);
            // FIXME: CompilerScheduler.Schedule(mc);
        }
    }
}
