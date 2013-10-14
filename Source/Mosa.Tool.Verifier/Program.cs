/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Compiler.Verifier;

namespace Mosa.Tool.Verifier
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
		internal static int Main(string[] args)
		{
			Console.WriteLine();
			Console.WriteLine("Verifier v0.1 [www.mosa-project.org]");
			Console.WriteLine("Copyright 2011. New BSD License.");
			Console.WriteLine("Written by Philipp Garcia (phil@thinkedge.com)");
			Console.WriteLine();

			VerificationOptions options = new VerificationOptions();

			bool valid = ParseOptions.Parse(options, args);

			if (!valid)
			{
				Console.WriteLine("Usage: Verifier <options: /il /ml> <assembly>");
				Console.WriteLine("ERROR: Incorrect arguments");
				return -1;
			}

			Console.WriteLine("Verifying assembly...");

			Verify verify = new Verify(options);
			verify.Run();

			if (!verify.HasError)
				return 0;

			Console.WriteLine();
			Console.WriteLine("Errors:");
			Console.WriteLine();

			foreach (VerificationEntry entry in verify.Entries)
				Console.WriteLine(entry.ToString());

			return -1;
		}
	}
}