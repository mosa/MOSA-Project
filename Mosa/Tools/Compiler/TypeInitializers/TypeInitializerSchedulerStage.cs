/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Vm;
using Mosa.Tools.Compiler.LinkTimeCodeGeneration;
using Mosa.Runtime.CompilerFramework.IR;
using System.Collections.Generic;
using Mosa.Runtime.Linker;

namespace Mosa.Tools.Compiler.TypeInitializers
{
    /// <summary>
    /// Schedules type initializers and creates a hidden mosacl_main method,
    /// which runs all type initializers in sequence.
    /// </summary>
    /// <remarks>
    /// Dependencies are not resolved, it is hoped that dependencies are resolved
    /// by the high-level language compiler by placing cctors in some order in
    /// metadata.
    /// </remarks>
    public sealed class TypeInitializerSchedulerStage : IAssemblyCompilerStage
    {
        #region Data Members

        /// <summary>
        /// Holds a list of cctors call instructions.
        /// </summary>
        private List<LegacyInstruction> instructions;

		/// <summary>
		/// Holds the method for the type initalizer
		/// </summary>
    	private CompilerGeneratedMethod _method;

        #endregion // Data Members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeInitializerSchedulerStage"/> class.
        /// </summary>
        public TypeInitializerSchedulerStage()
        {
            this.instructions = new List<LegacyInstruction>();
            this.instructions.Add(new PrologueInstruction(0));
        }

        #endregion // Construction
		
		#region Properties

		/// <summary>
		/// Gets the intializer method.
		/// </summary>
		/// <value>The method.</value>
    	public CompilerGeneratedMethod Method
    	{
    		get
			{
				return _method;
			}
    	}

		#endregion

		#region Methods

		/// <summary>
        /// Schedules the specified method for invocation in the main.
        /// </summary>
        /// <param name="method">The method.</param>
        public void Schedule(RuntimeMethod method)
        {
            this.instructions.Add(new CallInstruction(method));
        }

        #endregion // Methods

        #region IAssemblyCompilerStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public string Name
        {
            get { return @"Type Initializer Scheduler"; }
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public void Run(AssemblyCompiler compiler)
        {
            // Any initializers to run?
            if (this.instructions.Count > 0)
            {
                // Add a call to the current entry point to the scheduler
                this.instructions.AddRange(new LegacyInstruction[] {
                    new CallInstruction(compiler.Assembly.EntryPoint),
                    new EpilogueInstruction(0)
                });

				_method = LinkTimeCodeGenerator.Compile(compiler, @"AssemblyInit", this.instructions);

            }
        }

        #endregion // IAssemblyCompilerStage Members
    }
}
