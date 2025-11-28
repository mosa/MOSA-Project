// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.BareMetal.CoolWorld.Console.Apps;

public class Help : IApp
{
	public string Name => "Help";

	public string Description => "Shows information about all commands.";

	public void Execute()
	{
		foreach (var app in AppManager.Applications)
			System.Console.WriteLine(app.Name + " - " + app.Description);
	}
}
