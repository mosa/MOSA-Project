/*
* (c) 2009 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.IO;
using System.Collections.Generic;

namespace Mosa.Tools.Mono.UpdateProject
{
	/// <summary>
	/// Program class for Mosa.Tools.Mono.UpdateProject
	/// </summary>
	internal class Program
	{
		private static int Main(string[] args)
		{
			Console.WriteLine("UpdateProject v0.1 [www.mosa-project.org]");
			Console.WriteLine("Copyright 2009 by the MOSA Project. Licensed under the New BSD License.");
			Console.WriteLine("Written by Philipp Garcia (phil@thinkedge.com)");
			Console.WriteLine();
			Console.WriteLine("Usage: UpdateProject <project file>");
			Console.WriteLine();

			if (args.Length < 1) {
				Console.Error.WriteLine("ERROR: Missing arguments");
				return -1;
			}

			try {
				Transform.Process(args[0]);
				Add.Process(args[0]);
			}
			catch (Exception e) {
				Console.Error.WriteLine("Error: " + e);
				return -1;
			}

			return 0;
		}

	}
}