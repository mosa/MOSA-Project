// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal.x86;

/// <summary>
/// Screen
/// </summary>
public static class VGAConsole
{
	private enum ControlState { Normal, Escape };

	private static ControlState State = ControlState.Normal;

	private static byte BufferLength = 0;
	private static ulong BufferData = 0; // small 8 bit buffer

	private static byte FirstCharacter;
	private static byte SecondCharacter;
	private static byte FinalCharacter;

	private static int Parameter1 = 0;
	private static int Parameter2 = 0;
	private static int Parameter3 = 0;
	private static int Parameter4 = 0;
	private static int Parameter5 = 0;
	private static int Parameter6 = 0;
	private static int ParameterCount = 0;

	public static void Initialize()
	{
		ClearControlSequences();
	}

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
		switch (State)
		{
			case ControlState.Normal: { Normal(b); return; }
			case ControlState.Escape: { Escape(b); return; }
		}
	}

	private static void Normal(byte b)
	{
		if (b >= 32 && b <= 126)
		{
			VGAText.Write(b);
			return;
		}

		switch (b)
		{
			case 0x08: VGAText.Backspace(); return;     // ascii
			case 0x0A: VGAText.Newline(); return;       // ascii
			case 0x0C: VGAText.Formfeed(); return;      // ascii
			case 0x0D: VGAText.CarriageReturn(); return;// ascii
			case 0x1b: State = ControlState.Escape; return;    // vt100
			default: return;
		}
	}

	private static void Escape(byte b)
	{
		if (FirstCharacter == 0)
		{
			FirstCharacter = b;

			if (b == (byte)'c')
			{
				Evaluate();
			}
		}
		else if (IsCharacter(b))
		{
			FinalCharacter = b;

			if (BufferLength != 0)
			{
				ConvertBufferToParameter();
			}

			Evaluate();
		}
		else if (IsDigit(b))
		{
			AppendBuffer(b);
		}
		else if (b == (byte)';')
		{
			ConvertBufferToParameter();
		}
		else
		{
			SecondCharacter = b;

			if (b == (byte)'=' || b == (byte)'>' || b == (byte)'<')
			{
				Evaluate();
			}
		}
	}

	private static void Evaluate()
	{
		if (FirstCharacter == (byte)'E')
		{
			// Move to next line (ESC E)
			VGAText.Newline();
		}
		else if (FirstCharacter == (byte)'[')
		{
			if (FinalCharacter == (byte)'m')
			{
				// Display Attributes
				for (int i = 0; i < ParameterCount; i++)
				{
					var parameter = GetParameter(i + 1);

					switch (parameter)
					{
						case 30: VGAText.SetColor(VGAColor.Black); break;
						case 31: VGAText.SetColor(VGAColor.Red); break;
						case 32: VGAText.SetColor(VGAColor.Green); break;
						case 33: VGAText.SetColor(VGAColor.Yellow); break;
						case 34: VGAText.SetColor(VGAColor.Blue); break;
						case 35: VGAText.SetColor(VGAColor.Magenta); break;
						case 36: VGAText.SetColor(VGAColor.Cyan); break;
						case 37: VGAText.SetColor(VGAColor.White); break;

						case 40: VGAText.SetBackground(VGAColor.Black); break;
						case 41: VGAText.SetBackground(VGAColor.Red); break;
						case 42: VGAText.SetBackground(VGAColor.Green); break;
						case 43: VGAText.SetBackground(VGAColor.Yellow); break;
						case 44: VGAText.SetBackground(VGAColor.Blue); break;
						case 45: VGAText.SetBackground(VGAColor.Magenta); break;
						case 46: VGAText.SetBackground(VGAColor.Cyan); break;
						case 47: VGAText.SetBackground(VGAColor.White); break;

						// FUTURE:
						//0   Reset all attributes
						//1   Bright
						//2   Dim
						//4   Underscore
						//5   Blink
						//7   Reverse
						//8   Hidden

						default: break;
					}
				}
			}
			else if (FinalCharacter == (byte)'J')
			{
				var parameter = GetParameter(1);

				switch (parameter)
				{
					case 0: break; // TODO: Clear screen cursor dowon
					case 1: break; // TODO: Clear screen cursor up
					case 2: VGAText.Clear(); break;
					default: break;
				}
			}
			ClearControlSequences();
		}
	}

	private static bool IsDigit(byte b)
	{
		return b >= (byte)'0' && b <= (byte)'9';
	}

	private static bool IsCharacter(byte b)
	{
		return (b >= (byte)'a' && b <= (byte)'z')
		       || (b >= (byte)'A' && b <= (byte)'Z');
	}

	private static void ConvertBufferToParameter()
	{
		int value = 0;

		for (int i = 0; i < BufferLength; i++)
		{
			var b = GetBuffer(i);
			value = value * 10 + (b - (byte)'0');
		}

		ParameterCount++;

		if (ParameterCount == 1)
		{
			Parameter1 = value;
		}
		else if (ParameterCount == 2)
		{
			Parameter2 = value;
		}
		else if (ParameterCount == 3)
		{
			Parameter3 = value;
		}
		else if (ParameterCount == 4)
		{
			Parameter4 = value;
		}
		else if (ParameterCount == 5)
		{
			Parameter5 = value;
		}
		else if (ParameterCount == 6)
		{
			Parameter6 = value;
		}

		ClearBuffer();
	}

	private static int GetParameter(int index)
	{
		switch (index)
		{
			case 1: return Parameter1;
			case 2: return Parameter2;
			case 3: return Parameter3;
			case 4: return Parameter4;
			case 5: return Parameter5;
			case 6: return Parameter6;
			default: return 0;
		}
	}

	public static void ClearControlSequences()
	{
		State = ControlState.Normal;
		FirstCharacter = 0;
		SecondCharacter = 0;
		FinalCharacter = 0;
		ClearBuffer();
		ClearParameters();
	}

	public static void ClearParameters()
	{
		Parameter1 = 0;
		Parameter2 = 0;
		Parameter3 = 0;
		Parameter4 = 0;
		Parameter5 = 0;
		Parameter6 = 0;
		ParameterCount = 0;
	}

	private static void ClearBuffer()
	{
		BufferLength = 0;
		BufferData = 0;
	}

	private static void AppendBuffer(byte c)
	{
		BufferData = (BufferData << 8) | c;
		BufferLength++;
	}

	private static byte GetBuffer(int index)
	{
		return (byte)(BufferData >> ((BufferLength - index - 1) * 8));
	}
}
