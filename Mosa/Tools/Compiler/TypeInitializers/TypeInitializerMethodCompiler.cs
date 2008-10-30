/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 */

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Vm;

namespace Mosa.Tools.Compiler.TypeInitializers
{
    /// <summary>
    /// A special method compiler for type initializers.
    /// </summary>
    sealed class TypeInitializerMethodCompiler : MethodCompilerBase
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeInitializerMethodCompiler"/> class.
        /// </summary>
        /// <param name="compiler">The AoT compiler.</param>
        /// <param name="method">The method to compiler.</param>
        /// <param name="source">The instruction source.</param>
        public TypeInitializerMethodCompiler(AotCompiler compiler, RuntimeMethod method, TypeInitializerInstructionSource source) :
            base(compiler.Pipeline.Find<IAssemblyLinker>(), compiler.Architecture, compiler.Assembly, method.DeclaringType, method)
        {
            this.Pipeline.Add(source);
            compiler.Architecture.ExtendMethodCompilerPipeline(this.Pipeline);
        }

        #endregion // Construction
    }
}
