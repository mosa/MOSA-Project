/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
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

		private static uint line = 0;
		private static uint totallines = 3;
		private static uint startline = 18;

		public static uint Count = 0;

		public static void Trace(string location)
		{
			// Save Position
			uint x = Screen.Column;
			uint y = Screen.row;
			byte c = Screen.Color;
			byte b = Screen.BackgroundColor;

			Screen.Column = 0;
			Screen.Row = startline + line;
			Screen.Color = 0x0C;
			Screen.BackgroundColor = 0;

			Count++;
			Screen.Write("#");
			Screen.Write(Count, 10, 8);
			Screen.Write(" - Trace: ");
			Screen.Write(location);

			for (int left = 80 - (location.Length + 19); left > 0; left--)
				Screen.Write(' ');

			if (++line > totallines) line = 0;

			// Restore Position
			Screen.Column = x;
			Screen.Row = y;
			Screen.Color = c;
			Screen.BackgroundColor = b;
		}
	}
}
