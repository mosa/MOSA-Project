/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *
 */

using System;
using System.Collections.Generic;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Vm;

namespace Mosa.Tools.Compiler.LinkTimeCodeGeneration
{
    sealed class LinkerMethodCompiler : MethodCompilerBase
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkerMethodCompiler"/> class.
        /// </summary>
        /// <param name="compiler">The assembly compiler executing this method compiler.</param>
        /// <param name="method">The metadata of the method to compile.</param>
        /// <param name="instructions">The instructions to compile into the method.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="compiler"/>, <paramref name="method"/> or <paramref name="instructions"/> is null.</exception>
        public LinkerMethodCompiler(AssemblyCompiler compiler, RuntimeMethod method, List<LegacyInstruction> instructions) :
            base(compiler.Pipeline.Find<IAssemblyLinker>(), compiler.Architecture, compiler.Assembly, method.DeclaringType, method)
        {
            if (instructions == null)
                throw new ArgumentNullException(@"instructions");

            this.Pipeline.AddRange(new IMethodCompilerStage[] {
                new LinkerInstructionSource(instructions),
                new BasicBlockBuilderStage()
            });
            compiler.Architecture.ExtendMethodCompilerPipeline(this.Pipeline);
        }

        #endregion // Construction
    }
}
