// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Demo.CoolWorld.x86.Mosa.AppSystem;

namespace Mosa.Demo.CoolWorld.x86.Mosa.Application
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
