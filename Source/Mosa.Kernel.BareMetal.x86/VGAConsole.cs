// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal.x86;

/// <summary>
/// Screen
/// </summary>
public static class VGAConsole
{
	// VT100 Info:
	// https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences

	private enum ControlState
	{ Normal, Escape };

	private static ControlState State = ControlState.Normal;

	private static byte BufferLength;
	private static ulong BufferData; // small 8 bit buffer

	private static byte FirstCharacter;
	private static byte SecondCharacter;
	private static byte FinalCharacter;

	private static int Parameter1;
	private static int Parameter2;
	private static int Parameter3;
	private static int Parameter4;
	private static int Parameter5;
	private static int Parameter6;
	private static int ParameterCount;

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

			if (b is (byte)'=' or (byte)'>' or (byte)'<')
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
			switch (FinalCharacter)
			{
				case (byte)'m':
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
								case 37: VGAText.SetColor(VGAColor.LightGray); break;

								case 90: VGAText.SetColor(VGAColor.DarkGray); break;
								case 91: VGAText.SetColor(VGAColor.LightRed); break;
								case 92: VGAText.SetColor(VGAColor.LightGreen); break;
								case 93: VGAText.SetColor(VGAColor.Yellow); break;
								case 94: VGAText.SetColor(VGAColor.LightBlue); break;
								case 95: VGAText.SetColor(VGAColor.LightMagenta); break;
								case 96: VGAText.SetColor(VGAColor.LightCyan); break;
								case 97: VGAText.SetColor(VGAColor.White); break;

								case 40: VGAText.SetBackground(VGAColor.Black); break;
								case 41: VGAText.SetBackground(VGAColor.Red); break;
								case 42: VGAText.SetBackground(VGAColor.Green); break;
								case 43: VGAText.SetBackground(VGAColor.Yellow); break;
								case 44: VGAText.SetBackground(VGAColor.Blue); break;
								case 45: VGAText.SetBackground(VGAColor.Magenta); break;
								case 46: VGAText.SetBackground(VGAColor.Cyan); break;
								case 47: VGAText.SetBackground(VGAColor.White); break;

								case 100: VGAText.SetBackground(VGAColor.DarkGray); break;
								case 101: VGAText.SetBackground(VGAColor.LightRed); break;
								case 102: VGAText.SetBackground(VGAColor.LightGreen); break;
								case 103: VGAText.SetBackground(VGAColor.Yellow); break;
								case 104: VGAText.SetBackground(VGAColor.LightBlue); break;
								case 105: VGAText.SetBackground(VGAColor.LightMagenta); break;
								case 106: VGAText.SetBackground(VGAColor.LightCyan); break;
								case 107: VGAText.SetBackground(VGAColor.White); break;

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

						break;
					}

				case (byte)'J':
					{
						var parameter = GetParameter(1);

						switch (parameter)
						{
							case 0: break; // TODO: Clear screen cursor dowon
							case 1: break; // TODO: Clear screen cursor up
							case 2: VGAText.Clear(); break;
							default: break;
						}

						break;
					}

				case (byte)'H':
					{
						var x = GetParameter(1);
						var y = GetParameter(2);

						VGAText.Goto(x, y);
						break;
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
		return index switch
		{
			1 => Parameter1,
			2 => Parameter2,
			3 => Parameter3,
			4 => Parameter4,
			5 => Parameter5,
			6 => Parameter6,
			_ => 0
		};
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
