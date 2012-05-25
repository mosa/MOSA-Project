using System;
using System.Windows.Forms;

namespace Mosa.Tool.TypeExplorer
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			var main = new Main();

			if (args.Length != 0)
				main.LoadAssembly(args[0]);

			Application.Run(main);
		}
	}
}
