/*
* (c) 2009 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Tool.Mono.UpdateSource
{
	/// <summary>
	/// Program class for Mono.CreateProject
	/// </summary>
	internal class Program
	{
		/// <summary>
		/// Main method
		/// </summary>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		private static int Main(string[] args)
		{
			Console.WriteLine();
			Console.WriteLine("UpdateSource v0.2 [www.mosa-project.org]");
			Console.WriteLine("Copyright 2010. New BSD License.");
			Console.WriteLine("Written by Philipp Garcia (phil@thinkedge.com)");
			Console.WriteLine();

			if (args.Length < 1)
			{
				Console.WriteLine("Usage: UpdateSource [source files]");
				Console.Error.WriteLine("ERROR: Missing argument");
				return -1;
			}

			try
			{
				foreach (string source in args)
				{
					string root = Path.GetDirectoryName(source);

					List<string> lines = new List<string>();

					foreach (string file in System.IO.File.ReadAllLines(source))
					{
						if (file.Contains(".Internal.cs")) continue;

						lines.Add(file);

						if (!file.EndsWith(".cs")) continue;
						if (file.Length < 3) continue;

						string other = file.Insert(file.Length - 3, ".Internal");

						if (File.Exists(Path.Combine(root, other)))
						{
							Console.WriteLine(other);
							lines.Add(other);
						}
					}

					System.IO.File.WriteAllLines(source, lines.ToArray());
				}

			}
			catch (Exception e)
			{
				Console.Error.WriteLine("Error: " + e.ToString());
				return -1;
			}

			return 0;
		}

	}
}