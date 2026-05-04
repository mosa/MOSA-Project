// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.Configuration;

namespace Mosa.Utility.UnitTestBisector.Supervisor;

internal static class Program
{
	private static int Main(string[] args)
	{
		try
		{
			var mosaSettings = new MosaSettings();
			mosaSettings.LoadArguments(args);

			var supervisor = new ProcessSupervisor(mosaSettings, args);
			return supervisor.Run();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"ERROR: {ex.Message}");
			return 1;
		}
	}
}
