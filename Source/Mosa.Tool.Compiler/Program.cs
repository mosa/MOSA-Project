/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Tool.Compiler
{
    /// <summary>
    /// Class containing the entry point of the program.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Main entry point for the compiler.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        internal static void Main(string[] args)
        {
            Compiler compiler = new Compiler();
            compiler.Run(args);
        }
    }
}

