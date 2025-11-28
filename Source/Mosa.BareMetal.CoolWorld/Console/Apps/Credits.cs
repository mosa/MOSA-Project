// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.BareMetal.CoolWorld.Console.Apps;

public class Credits : IApp
{
	public string Name => "Credits";

	public string Description => "Shows everyone who contributed to the project.";

	public void Execute()
	{
		System.Console.WriteLine("*** MOSA Credits ****");
		System.Console.WriteLine(" Phil Garcia");
		System.Console.WriteLine(" Simon Wollwage");
		System.Console.WriteLine(" Michael Frohlich");
		System.Console.WriteLine(" Stefan Andres Charsley");
		System.Console.WriteLine(" Chin Ki Yuen");
		System.Console.WriteLine(" Patrick Reisert");
		System.Console.WriteLine(" Phillip Webster");
		System.Console.WriteLine(" Kevin Thompson");
		System.Console.WriteLine(" Kai P.Reisert");
		System.Console.WriteLine(" M.de Bruijn");
		System.Console.WriteLine(" Royce Mitchell III");
		System.Console.WriteLine(" Sebastian Loncar");
		System.Console.WriteLine(" AnErrupTion");
	}
}
