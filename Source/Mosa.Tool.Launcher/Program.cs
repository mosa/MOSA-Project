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

			foreach (var arg in args)
			{
				if (arg == "-q")
				{
					main.ExitOnLaunch = true;
					continue;
				}
				else if (arg == "-a")
				{
					main.AutoLaunch = true;
					continue;
				}

				if (arg.IndexOf(Path.DirectorySeparatorChar) >= 0)
				{
					main.SourceFile = arg;
				}
				else
				{
					main.SourceFile = Path.Combine(Directory.GetCurrentDirectory(), arg);
				}
			}

			Application.Run(main);
		}
	}
}