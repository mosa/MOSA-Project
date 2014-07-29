/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Windows.Forms;
using System.IO;

namespace Mosa.Tool.Launcher
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var main = new MainForm();

			if (args.Length != 0)
			{
				if (args[0].IndexOf(Path.DirectorySeparatorChar) >= 0)
				{
					main.SetSource(args[0]);
				}
				else
				{
					main.SetSource(Path.Combine(Directory.GetCurrentDirectory(), args[0]));
				}
			}

			Application.Run(main);
		}
	}
}