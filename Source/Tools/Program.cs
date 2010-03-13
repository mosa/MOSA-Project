/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Kai P. Reisert <kpreisert@googlemail.com>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;

namespace Mosa.Tools.LoaderTool
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
            Console.Write("Initializing loader tool runtime...");
            var runtime = new LoaderToolRuntime();
            Console.WriteLine(" done.");

            Console.Write("Loading mscorlib.dll...");
            IMetadataModule module = runtime.AssemblyLoader.Load(@"/Library/Frameworks/Mono.framework/Versions/2.6.1/lib/mono/2.0/mscorlib.dll");
            Console.WriteLine(" done.");
			
			Console.WriteLine("Running new TypeLoader...");
			TypeLoader loader = new TypeLoader(module);
			foreach (RuntimeType type in loader.LoadTypes())
			{
				try
				{
					Console.WriteLine("\t{0}", type.FullName);
				}
				catch (Exception e)
				{
					Console.Write("\tException processing ");
					try
					{
						Console.Write(type.FullName);
					}
					catch
					{
						Console.WriteLine("(unknown)");
					}
					
					Console.WriteLine(e);
					break; 
				}
			}
			Console.WriteLine("Done.");
        }
    }
    
    
}

