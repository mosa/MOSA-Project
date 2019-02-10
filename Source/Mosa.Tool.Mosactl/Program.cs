// Copyright (c) MOSA Project. Licensed under the New BSD License.
using System.Collections.Generic;

namespace Mosa.Tool.Mosactl
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
			var app = new MosaCtl();
			app.Run(new List<string>(args));
		}
	}
}
