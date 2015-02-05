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

		private static bool firstError = true;

		private static void PrepareScreen(string title)
		{
			IDT.SetInterruptHandler(null);
			Screen.BackgroundColor = Colors.Black;
			Screen.Clear();
			Screen.Goto(1, 1);
			Screen.Color = Colors.LightGray;
			Screen.Write("*** ");
			Screen.Write(title);

			if (firstError)
				firstError = false;
			else
				Screen.Write(" (multiple)");

			Screen.Write(" ***");
			Screen.Goto(3, 1);
		}

		public static void InvalidOperation()
		{
			Error("Invalid operation");
		}

		#region DumpMemory

		public static void DumpMemory(uint address)
		{
			PrepareScreen("Memory Dump");
			Screen.Column = 0;
			Screen.Write("ADDRESS  ");
			Screen.Color = Colors.Brown;
			Screen.Write("03 02 01 00  07 06 05 04   11 10 09 08  15 14 13 12   ASCII");

			var word = address;
			var rowAddress = address;
			uint rows = 21;
			for (uint y = 0; y < rows; y++)
			{
				Screen.Row++;
				Screen.Column = 0;

				WriteHex(word, 8, Colors.Brown);
				Screen.Write("  ");

				const uint dwordsPerRow = 4;
				for (uint x = 0; x < dwordsPerRow; x++)
				{
					for (uint x2 = 0; x2 < 4; x2++)
					{
						var number = Native.Get8(word + ((4 - 1) - x2));
						WriteHex(number, 2, Colors.LightGray);
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

		private static void WriteHex(uint num, byte color)
		{
			WriteHex(num, 0, color);
		}

		private static void WriteHex(uint num, byte digits, byte color)
		{
			var oldColor = Screen.Color;
			Screen.Color = color;

			if (num == 0)
				Screen.Color = Colors.LightGray;

			var hex = new StringBuffer(num, "X");

			for (var i = 0; i < digits - hex.Length; i++)
				Screen.Write('0');
			Screen.Write(hex);

			Screen.Color = oldColor;
		}

		#endregion DumpMemory

		#region Message

		public static void BeginMessage()
		{
			PrepareScreen("Debug Message");
			Screen.Color = Colors.Red;
		}

		public static void Message(string message)
		{
			BeginMessage();
			Screen.Write(message);
			Halt();
		}

		public static void Message(char message)
		{
			BeginMessage();
			Screen.Write(message);
			Halt();
		}

		public static void Message(uint message)
		{
			BeginMessage();
			Screen.Write(" Number: 0x");
			Screen.Write(message, "X");
			Halt();
		}

		#endregion Message

		#region Error

		private static void BeginError()
		{
			PrepareScreen("Kernel Panic");
			Screen.Color = Colors.Red;
		}

		public static void Error(string message)
		{
			BeginError();
			Screen.Write(message);
			EndError();
		}

		public static void Error(StringBuffer message)
		{
			BeginError();
			Screen.Write(message);
			EndError();
		}

		public static void Error(uint error)
		{
			Error(new StringBuffer(error));
		}

		private static void EndError()
		{
			Screen.Row += 2;
			Screen.Column = 0;
			DumpStackTrace();
			Screen.Color = Colors.LightGray;
			Halt();
		}

		#endregion Error

		private static void Halt()
		{
			Screen.Goto(Screen.Rows - 1, 0);
			while (true)
				Native.Hlt();
		}

		#region DumpStackTrace

		private static uint ebp = 0;
		private static uint eip = 0;

		internal static void SetStackPointer(uint ebp, uint eip)
		{
			Panic.ebp = ebp;
			Panic.eip = eip;
		}

		public unsafe static void DumpStackTrace()
		{
			DumpStackTrace(0);
		}

		public unsafe static void DumpStackTrace(uint depth)
		{
			while (true)
			{
				var entry = Runtime.GetStackTraceEntry(depth, ebp, eip);
				if (!entry.Valid) return;

				if (!entry.Skip)
				{
					Screen.Write(entry.ToStringBuffer());
					Screen.Row++;
					Screen.Column = 0;
				}

				depth++;
			}
		}

		#endregion DumpStackTrace
	}
}