// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.BareMetal.HelloWorld.Apps;

public class Credits : IApp
{
	public string Name => "Credits";

	public string Description => "Shows everyone who contributed to the project.";

	public void Execute()
	{
		Console.WriteLine("*** MOSA Credits ****");
		Console.WriteLine(" Phil Garcia");
		Console.WriteLine(" Simon Wollwage");
		Console.WriteLine(" Michael Frohlich");
		Console.WriteLine(" Stefan Andres Charsley");
		Console.WriteLine(" Chin Ki Yuen");
		Console.WriteLine(" Patrick Reisert");
		Console.WriteLine(" Phillip Webster");
		Console.WriteLine(" Kevin Thompson");
		Console.WriteLine(" Kai P.Reisert");
		Console.WriteLine(" M.de Bruijn");
		Console.WriteLine(" Royce Mitchell III");
		Console.WriteLine(" Sebastian Loncar");
		Console.WriteLine(" AnErrupTion");
	}
}
