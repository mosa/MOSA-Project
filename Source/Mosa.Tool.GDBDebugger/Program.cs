// Copyright (c) MOSA Project. Licensed under the New BSD License.

using CommandLine;
using System;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger
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

			main.LoadArguments(args);

			Application.Run(main);
		}
	}
}
