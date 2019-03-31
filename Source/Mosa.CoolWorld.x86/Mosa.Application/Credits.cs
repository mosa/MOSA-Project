// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.AppSystem;

namespace Mosa.Application
{
	/// <summary>
	/// Credits
	/// </summary>
	public class Credits : BaseApplication, IConsoleApp
	{
		public override int Start(string parameters)
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

			return 0;
		}
	}
}
