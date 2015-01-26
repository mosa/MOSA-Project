/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Kernel.Helpers;
using Mosa.Platform.Internal.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	///
	/// </summary>
	public static class Panic
	{
		/// <summary>
		/// Nows this instance.
		/// </summary>
		public static void Now()
		{
			Now(0);
		}

		/// <summary>
		/// Nows the specified error code.
		/// </summary>
		/// <param name="errorCode">The error code.</param>
		public static void Now(uint errorCode)
		{
			Screen.Column = 0;
			Screen.Row = 0;
			Screen.Color = 0x0C;

			Screen.Write('P');
			Screen.Write('A');
			Screen.Write('N');
			Screen.Write('I');
			Screen.Write('C');
			Screen.Write('!');
			Screen.Write(' ');
			Screen.Write(errorCode, 8, 8);

			while (true)
				Native.Hlt();
		}

		#region Beautiful Panic

		private static void PrepareScreen(string title)
		{
			IDT.SetInterruptHandler(null);
			Screen.BackgroundColor = Colors.LightGray;
			Screen.Clear();
			Screen.Goto(1, 1);
			Screen.Color = Colors.DarkGray;
			Screen.Write("*** " + title + " ***");
			Screen.Goto(3, 1);
		}

		public static void InvalidOperation()
		{
			Error("Invalid operation");
		}

		public static void DumpMemory(uint address)
		{
			PrepareScreen("Memory Dump");

			var a = address;
			for (var y = 0; y < 20; y++)
			{
				Screen.Row++;
				Screen.Column = 0;
				for (var x = 0; x < 6; x++)
				{
					for (var x2 = 0; x2 < 4; x2++)
					{
						WriteHex(Native.Get8(a).ToString("X"), 2);
						Screen.Write(' ');
						a++;
					}
					Screen.Write(' ');
				}
			}

			Halt();
		}

		private static void WriteHex(string hex, byte digits)
		{
			for (var i = 0; i < digits - hex.Length; i++)
				Screen.Write('0');
			Screen.Write(hex);
		}

		public static void Message(string message)
		{
			PrepareScreen("Debug Message");
			Screen.Color = Colors.Red;
			Screen.Write(message);
			Halt();
		}

		public static void Message(char message)
		{
			PrepareScreen("Debug Message");
			Screen.Color = Colors.Red;
			Screen.Write(message);
			Halt();
		}

		public static void Error(string message)
		{
			PrepareScreen("Kernel Panic");
			Screen.Color = Colors.Red;
			Screen.Write(message);
			Screen.Row += 2;
			Screen.Column = 0;
			DumpStackTrace();
			Screen.Color = Colors.LightGray;
			Halt();
		}

		private static void Halt()
		{
			Screen.Goto(Screen.Rows - 1, 0);
			while (true)
				Native.Hlt();
		}

		public unsafe static void DumpStackTrace()
		{
			uint depth = 0;

			while (true)
			{
				var methodDef = Runtime.GetMethodDefinitionFromStackFrameDepth(depth);

				if (methodDef == null)
					return;

				string caller = Runtime.GetMethodDefinitionName(methodDef);

				if (caller == null)
					return;

				Screen.Write(caller);
				Screen.Row++;
				Screen.Column = 0;

				depth++;
			}
		}

		#endregion Beautiful Panic
	}
}