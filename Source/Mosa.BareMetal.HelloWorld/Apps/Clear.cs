// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.BareMetal.HelloWorld.Apps;

public class Clear : IApp
{
	public string Name => "Clear";

	public string Description => "Clears the screen.";

	public void Execute()
	{
		Console.Clear();
	}
}
