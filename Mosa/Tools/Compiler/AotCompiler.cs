/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 */

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Vm;
using System.Diagnostics;

namespace Mosa.Tools.Compiler
{

    /// <summary>
    /// Implements the ahead of time compiler.
    /// </summary>
    /// <remarks>
    /// This class implements the ahead of time compiler for MOSA. The AoT uses 
    /// the compiler services offered in Mosa.Runtime.CompilerFramework in order
    /// to share as much code as possible with assembly jit compiler in MOSA. The 
    /// primary difference between the two compilers is primarily the number and
    /// quality of compilation stages used. The AoT compiler makes use of assembly lot
    /// more optimizations than the jit. The jit is tweaked for execution speed. 
    /// </remarks>
    public sealed class AotCompiler : AssemblyCompiler
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="AotCompiler"/> class.
        /// </summary>
        /// <param name="architecture">The target compilation architecture.</param>
        /// <param name="assembly">The assembly to compile.</param>
        public AotCompiler(IArchitecture architecture, IMetadataModule assembly) : 
            base(architecture, assembly)
        {
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Executes the compiler using the configured stages.
        /// </summary>
        /// <remarks>
        /// The method iterates the compilation stage chain and runs each
        /// stage on the input.
        /// </remarks>
        public void Run()
        {
            // Build the default assembly compiler pipeline
            this.Architecture.ExtendAssemblyCompilerPipeline(this.Pipeline);

            // Run the compiler
            base.Compile();
        }

        /// <summary>
        /// Creates a method compiler
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="method">The method to compile.</param>
        /// <returns>
        /// An instance of a MethodCompilerBase for the given type/method pair.
        /// </returns>
        public override MethodCompilerBase CreateMethodCompiler(RuntimeType type, RuntimeMethod method)
        {
            IArchitecture arch = this.Architecture;
            MethodCompilerBase mc = new AotMethodCompiler(
                this,
                type,
                method
            );
            arch.ExtendMethodCompilerPipeline(mc.Pipeline);
            return mc;
        }

        #endregion // Methods
    }
}
