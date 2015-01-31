/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Platform.Internal.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	///
	/// </summary>
	public static class Panic
	{
		public static void Setup()
		{
			//Mosa.Kernel.Helpers.InternalPanic.ExceptionHandler = Error;
		}

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
			Screen.BackgroundColor = Colors.Black;
			Screen.Clear();
			Screen.Goto(1, 1);
			Screen.Color = Colors.LightGray;
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
			Screen.Column = 0;
			//Screen.Write("Address   dword        dword         dword       dword      ascii");
			Screen.Write("ADDRESS  ");
			Screen.Color = Colors.Brown;
			Screen.Write("03 02 01 00  07 06 05 04   11 10 09 08  15 14 13 12   ASCII");
			//Screen.Write(address.ToString("X"));

			var word = address;
			var rowAddress = address;
			uint rows = 21;
			for (uint y = 0; y < rows; y++)
			{
				Screen.Row++;
				Screen.Column = 0;

				WriteHex(word.ToString("X"), Colors.Brown);
				Screen.Write("  ");

				const uint dwordsPerRow = 4;
				for (uint x = 0; x < dwordsPerRow; x++)
				{
					for (uint x2 = 0; x2 < 4; x2++)
					{
						var number = Native.Get8(word + ((4 - 1) - x2));
						//var number = Native.Get8(word + x2);
						WriteHex(number.ToString("X"), 2, number == 0);
						Screen.Write(' ');
					}
					if (x == 1 || x == 3)
						Screen.Write(' ');
					Screen.Write(' ');
					word += 4;
				}

				for (uint x = 0; x < dwordsPerRow * 4; x++)
				{
					var num = Native.Get8(rowAddress + x);
					if (num == 0)
						Screen.Color = Colors.DarkGray;
					else
						Screen.Color = Colors.LightGray;

					byte byteMin = 32;
					byte byteMax = 128;

					#region COMPILER_BUG

					//if (num >= 32 && num < 128) //COMPILER_BUG: This conditinal expression will not resolved correctly!
					//{
					//	Screen.Color = Colors.LightGray;
					//	Screen.Write((char)num);
					//}
					//else
					//{
					//	if (num == 0)
					//		Screen.Color = Colors.DarkGray;
					//	else
					//		Screen.Color = Colors.LightGray;
					//	Screen.Write('.');
					//}

					#endregion COMPILER_BUG

					//workarround: separate method
					WriteHexChar(num);
				}
				Screen.Color = Colors.LightGray;

				//avoid empty line, when line before was fully filled
				if (Screen.Column == 0)
					Screen.Row--;

				rowAddress += (dwordsPerRow * 4);
			}

			Halt();
		}

		private static void WriteHexChar(byte num)
		{
			if (num >= 32 && num < 128)
			{
				Screen.Color = Colors.LightGray;
				Screen.Write((char)num);
			}
			else
			{
				if (num == 0)
					Screen.Color = Colors.DarkGray;
				else
					Screen.Color = Colors.LightGray;
				Screen.Write('.');
			}
		}

		private static void WriteHex(string hex, byte digits, bool zero)
		{
			if (!zero)
				Screen.Color = Colors.LightGray;

			for (var i = 0; i < digits - hex.Length; i++)
				Screen.Write('0');
			Screen.Write(hex);

			Screen.Color = Colors.DarkGray;
		}

		private static void WriteHex(string hex, byte color)
		{
			var oldColor = Screen.Color;
			Screen.Color = color;
			Screen.Write(hex);
			Screen.Color = oldColor;
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

		public static void Message(uint message)
		{
			PrepareScreen("Debug Message");
			Screen.Color = Colors.Red;
			Screen.Write(" Number: 0x");
			Screen.Write(message.ToString("X"));
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

		public static void Error(uint error)
		{
			Error(error.ToString());
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