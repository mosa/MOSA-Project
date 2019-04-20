// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.Extension;
using Mosa.Runtime;
using Mosa.Runtime.x86;
using System;

namespace Mosa.Kernel.BareMetal.x86
{
	/// <summary>
	/// Screen
	/// </summary>
	public static class Console
	{
		private enum State { Normal, Color, Background };

		private static State state = State.Normal;

		public static void Write(string s)
		{
			foreach (var c in s)
			{
				Write((byte)c);
			}
		}

		public static void Write(char c)
		{
			Write((byte)c);
		}

		public static void Write(byte b)
		{
			switch (state)
			{
				case State.Normal: { Normal(b); return; }
				case State.Color: { Screen.SetColor(b); state = State.Normal; return; }
				case State.Background: { Screen.SetBackground(b); state = State.Normal; ; return; }
			}
		}

		private static void Normal(byte b)
		{
			if ((b >= 33 && b <= 126))
			{
				Screen.Write(b);
				return;
			}

			switch (b)
			{
				case 0x08: Screen.Backspace(); return;
				case 0x0A: Screen.Newline(); return;
				case 0x0C: Screen.Formfeed(); return;
				case 0x0D: Screen.CarriageReturn(); return;
				case 0x9E: state = State.Color; return;
				case 0x9F: state = State.Background; return;
				default: return;
			}
		}
	}
}
