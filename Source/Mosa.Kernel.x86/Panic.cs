// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

		public static void Error(Mosa.Internal.StringBuffer message)
		{
			BeginError();
			Screen.Write(message);
			EndError();
		}

		public static void Error(uint error)
		{
			Error(new Mosa.Internal.StringBuffer(error));
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

		public static void DumpStackTrace()
		{
			DumpStackTrace(0);
		}

		public static void DumpStackTrace(uint depth)
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
