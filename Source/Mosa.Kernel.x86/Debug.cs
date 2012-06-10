/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Platform.x86.Intrinsic;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// 
	/// </summary>
	public static class Debug
	{

		private static uint Count = 0;

		public static void Trace(string location)
		{
			ConsoleSession Console = ConsoleManager.Controller.Debug;

			Console.Color = 0x0C;
			Console.BackgroundColor = 0;

			Count++;
			Console.Write("#");
			Console.Write(Count, 10, 8);
			Console.Write(" - Trace: ");
			Console.Write(location);
			Console.WriteLine();

		}
	}
}
