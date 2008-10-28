/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License
 * with restrictions to the license beneath, concering
 * the use of the CommandLine assembly.
 *
 * Authors:
 *  Kai P. Reisert (<mailto:kpreisert@googlemail.com>)
 */

using System;
using System.Collections.Generic;
using System.IO;

using NDesk.Options;

using Mosa.Platforms.x86;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Loader.PE;
using Mosa.Runtime.Metadata;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// Class containing the entry point of the program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point for the compiler.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            Compiler compiler = new Compiler();
            compiler.Run(args);
        }
    }
}
