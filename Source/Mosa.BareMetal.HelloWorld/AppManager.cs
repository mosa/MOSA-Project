// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.BareMetal.HelloWorld.Apps;

namespace Mosa.BareMetal.HelloWorld;

public static class AppManager
{
	private static readonly IApp[] Applications =
	{
		new Shell()
	};

	public static bool Execute(string name)
	{
		foreach (var app in Applications)
			if (app.Name.ToLower() == name.ToLower())
			{
				app.Execute();
				return true;
			}

		return false;
	}
}
