// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.BareMetal.CoolWorld.Console.Apps;

namespace Mosa.BareMetal.CoolWorld.Console;

public static class AppManager
{
	public static readonly IApp[] Applications =
	{
		new BootInfo(),
		new Clear(),
		new Credits(),
		new Help(),
		new Mem(),
		new Reboot(),
		new Shell(),
		new ShowDisks(),
		new ShowFS(),
		new ShowISA(),
		new ShowPCI(),
		new Shutdown(),
		new Time()
	};

	public static bool Execute(string name)
	{
		foreach (var app in Applications)
		{
			if (app.Name.ToLower() == name.ToLower())
			{
				app.Execute();
				return true;
			}
		}

		return false;
	}
}
