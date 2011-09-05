/*
* (c) 2009 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;

namespace Mosa.Tools.Mono.UpdateProject
{
	/// <summary>
	/// Program class for Mosa.Tools.Mono.UpdateProject
	/// </summary>
	internal class Program
	{
		private static int Main(string[] args)
		{
			Console.WriteLine();
			Console.WriteLine("UpdateProject v0.2 [www.mosa-project.org]");
			Console.WriteLine("Copyright 2010. New BSD License.");
			Console.WriteLine("Written by Philipp Garcia (phil@thinkedge.com)");
			Console.WriteLine();

			if (args.Length < 1)
			{
				Console.WriteLine("Usage: UpdateProject <options>");
				Console.Error.WriteLine("ERROR: Missing arguments");
				return -1;
			}

			try
			{
				Options options = new Options(args);

				foreach (string file in options.Files)
					Transform.Process(options, file);

				if (options.UpdateProjectFiles)
					foreach (string project in options.Projects)
						Add.Process(project);

			}
			catch (Exception e)
			{
				Console.Error.WriteLine("Error: " + e);
				return -1;
			}

			return 0;
		}

	}
}