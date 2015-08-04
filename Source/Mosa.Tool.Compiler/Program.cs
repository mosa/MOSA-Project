// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
