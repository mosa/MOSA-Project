// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.BareMetal.CoolWorld.Console.Apps;

public class Clear : IApp
{
	public string Name => "Clear";

	public string Description => "Clears the screen.";

	public void Execute()
	{
		System.Console.Clear();
	}
}
