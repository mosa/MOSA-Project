// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Demo.AppSystem;

namespace Mosa.Demo.Application
{
	/// <summary>
	/// Credits
	/// </summary>
	public class Clear : BaseApplication, IConsoleApp
	{
		public override int Start(string parameters)
		{
			Console.ClearScreen();

			return 0;
		}
	}
}
