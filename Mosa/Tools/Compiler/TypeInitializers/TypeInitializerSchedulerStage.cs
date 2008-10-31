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
        /// Holds a list of cctors scheduled for inclusion.
        /// </summary>
        private TypeInitializerInstructionSource source;

        #endregion // Data Members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeInitializerSchedulerStage"/> class.
        /// </summary>
        public TypeInitializerSchedulerStage()
        {
            this.source = new TypeInitializerInstructionSource();
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Schedules the specified method for invocation in the main.
        /// </summary>
        /// <param name="method">The method.</param>
        public void Schedule(RuntimeMethod method)
        {
            this.source.Schedule(method);
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
            if (this.source.Instructions.Count > 0)
            {
                // FIXME: Add a call to the current entry point to the scheduler
                //this.source.Schedule(compiler.Assembly.EntryPoint);

                CompilerGeneratedType initType = new CompilerGeneratedType(compiler.Assembly, @"Mosa.Tools.Compiler", @"AssemblyInitializer");
                CompilerGeneratedMethod initMethod = new CompilerGeneratedMethod(compiler.Assembly, @"AssemblyInit", initType);
                initType.Methods.Add(initMethod);

                // FIXME: Add the compiler generated type/method to the compilation assembly

                // FIXME: Set the assembly initializer method as the entry point
                //compiler.Assembly.EntryPoint = initMethod;

                TypeInitializerMethodCompiler timc = new TypeInitializerMethodCompiler((AotCompiler)compiler, initMethod, this.source);
                timc.Compile();
            }
        }

        #endregion // IAssemblyCompilerStage Members
    }
}
