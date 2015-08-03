// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Windows.Forms;

namespace Mosa.Tool.TinySimulator
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
				main.CompileOnLaunch = args[0];
			}

			Application.Run(main);
		}
	}
}