// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Windows.Forms;

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
			main.Options.LoadFile(main.ConfigFile);

			//commannd line arguments will overwrite config file settings
			main.Options.LoadArguments(args);

			Application.Run(main);
		}
	}
}
