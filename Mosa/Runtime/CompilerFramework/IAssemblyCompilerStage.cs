/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// This interface represents a stage of compilation of an assembly.
    /// </summary>
    public interface IAssemblyCompilerStage
    {
        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        void Run(AssemblyCompiler compiler);
    }
}
