// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.AppSystem;

namespace Mosa.Application
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
