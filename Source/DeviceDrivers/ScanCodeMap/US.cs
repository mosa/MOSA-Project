/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;

namespace Mosa.DeviceDrivers.ScanCodeMap
{
	/// <summary>
	/// Implements the US Keyboard map (scan code set 2)
	/// </summary>
	public class US : IScanCodeMap
	{
		private enum KeyState { Normal, Escaped, Espaced2, EscapeBreak };

		private KeyState keyState;

		/// <summary>
		/// Initializes a new instance of the <see cref="US"/> class.
		/// </summary>
		public US()
		{
			keyState = KeyState.Normal;
		}

		/// <summary>
		/// Convert can code into a key
		/// </summary>
		/// <param name="scancode">The scancode.</param>
		/// <returns></returns>
		public KeyEvent ConvertScanCode(byte scancode)
		{
			KeyEvent key = new KeyEvent();

			if (scancode == 0)
				return key;

			if (keyState == KeyState.Normal)
			{
				if (scancode == 0xE0)
				{
					keyState = KeyState.Escaped;
					return key;
				}

				if ((scancode & 0x80) != 0)
					key.KeyPress = KeyEvent.Press.Break;
				else
					key.KeyPress = KeyEvent.Press.Make;

				key.KeyType = KeyType.RegularKey;

				switch (scancode)
				{
					case 1: key.Character = (char)27; break;
					case 2: key.Character = '1'; break;
					case 3: key.Character = '2'; break;
					case 4: key.Character = '3'; break;
					case 5: key.Character = '4'; break;
					case 6: key.Character = '5'; break;
					case 7: key.Character = '6'; break;
					case 8: key.Character = '7'; break;
					case 9: key.Character = '8'; break;
					case 10: key.Character = '9'; break;
					case 11: key.Character = '0'; break;
					case 12: key.Character = '-'; break;
					case 13: key.Character = '='; break;
					case 14: key.Character = '\b'; break;
					case 15: key.Character = '\t'; break;
					case 16: key.Character = 'q'; break;
					case 17: key.Character = 'w'; break;
					case 18: key.Character = 'e'; break;
					case 19: key.Character = 'r'; break;
					case 20: key.Character = 't'; break;
					case 21: key.Character = 'y'; break;
					case 22: key.Character = 'u'; break;
					case 23: key.Character = 'i'; break;
					case 24: key.Character = 'o'; break;
					case 25: key.Character = 'p'; break;
					case 26: key.Character = '['; break;
					case 27: key.Character = ']'; break;
					case 28: key.Character = '\n'; break;
					case 29: key.KeyType = KeyType.LeftControl; break;
					case 30: key.Character = 'a'; break;
					case 31: key.Character = 's'; break;
					case 32: key.Character = 'd'; break;
					case 33: key.Character = 'f'; break;
					case 34: key.Character = 'g'; break;
					case 35: key.Character = 'h'; break;
					case 36: key.Character = 'j'; break;
					case 37: key.Character = 'k'; break;
					case 38: key.Character = 'l'; break;
					case 39: key.Character = ';'; break;
					case 40: key.Character = '\''; break;
					case 41: key.Character = '`'; break;
					case 42: key.KeyType = KeyType.LeftShift; break;
					case 43: key.Character = '\\'; break;
					case 44: key.Character = 'z'; break;
					case 45: key.Character = 'x'; break;
					case 46: key.Character = 'c'; break;
					case 47: key.Character = 'v'; break;
					case 48: key.Character = 'b'; break;
					case 49: key.Character = 'n'; break;
					case 50: key.Character = 'm'; break;
					case 51: key.Character = ','; break;
					case 52: key.Character = '.'; break;
					case 53: key.Character = '/'; break;
					case 54: key.KeyType = KeyType.RightShift; break;
					case 55: key.Character = '*'; break;
					case 56: key.KeyType = KeyType.LeftAlt; break;
					case 57: key.Character = ' '; break;
					case 58: key.KeyType = KeyType.CapsLock; break;
					case 59: key.KeyType = KeyType.F1; break;
					case 60: key.KeyType = KeyType.F2; break;
					case 61: key.KeyType = KeyType.F3; break;
					case 62: key.KeyType = KeyType.F4; break;
					case 63: key.KeyType = KeyType.F5; break;
					case 64: key.KeyType = KeyType.F6; break;
					case 65: key.KeyType = KeyType.F7; break;
					case 66: key.KeyType = KeyType.F8; break;
					case 67: key.KeyType = KeyType.F9; break;
					case 68: key.KeyType = KeyType.F10; break;
					case 69: key.KeyType = KeyType.NumLock; break;
					case 70: key.KeyType = KeyType.ScrollLock; break;
					case 71: key.KeyType = KeyType.Home; break;
					case 72: key.KeyType = KeyType.UpArrow; break;
					case 73: key.KeyType = KeyType.PageUp; break;
					case 74: key.Character = '-'; break;
					case 75: key.KeyType = KeyType.LeftArrow; break;
					case 76: key.Character = (char)0; break;
					case 77: key.KeyType = KeyType.RightArrow; break;
					case 78: key.Character = '+'; break;
					case 79: key.KeyType = KeyType.End; break;
					case 80: key.KeyType = KeyType.DownArrow; break;
					case 81: key.KeyType = KeyType.PageDown; break;
					case 82: key.KeyType = KeyType.Insert; break;
					case 83: key.KeyType = KeyType.Delete; break;
					case 86: key.Character = '\\'; break;
					case 87: key.KeyType = KeyType.F11; break;
					case 88: key.KeyType = KeyType.F12; break;
					case 129: key.Character = (char)27; break;
					case 130: key.Character = '!'; break;
					case 131: key.Character = '@'; break;
					case 132: key.Character = '#'; break;
					case 133: key.Character = '$'; break;
					case 134: key.Character = '%'; break;
					case 135: key.Character = '^'; break;
					case 136: key.Character = '&'; break;
					case 137: key.Character = '*'; break;
					case 138: key.Character = '('; break;
					case 139: key.Character = ')'; break;
					case 140: key.Character = '_'; break;
					case 141: key.Character = '+'; break;
					case 142: key.Character = '\b'; break;
					case 143: key.Character = '\t'; break;
					case 144: key.Character = 'Q'; break;
					case 145: key.Character = 'W'; break;
					case 146: key.Character = 'E'; break;
					case 147: key.Character = 'R'; break;
					case 148: key.Character = 'T'; break;
					case 149: key.Character = 'Y'; break;
					case 150: key.Character = 'U'; break;
					case 151: key.Character = 'I'; break;
					case 152: key.Character = 'O'; break;
					case 153: key.Character = 'P'; break;
					case 154: key.Character = '{'; break;
					case 155: key.Character = '}'; break;
					case 156: key.Character = '\n'; break;
					case 157: key.KeyType = KeyType.RightControl; break;
					case 158: key.Character = 'A'; break;
					case 159: key.Character = 'S'; break;
					case 160: key.Character = 'D'; break;
					case 161: key.Character = 'F'; break;
					case 162: key.Character = 'G'; break;
					case 163: key.Character = 'H'; break;
					case 164: key.Character = 'J'; break;
					case 165: key.Character = 'K'; break;
					case 166: key.Character = 'L'; break;
					case 167: key.Character = ':'; break;
					case 168: key.Character = '"'; break;
					case 169: key.Character = '~'; break;
					case 170: key.KeyType = KeyType.LeftShift; break;
					case 171: key.Character = '|'; break;
					case 172: key.Character = 'Z'; break;
					case 173: key.Character = 'X'; break;
					case 174: key.Character = 'C'; break;
					case 175: key.Character = 'V'; break;
					case 176: key.Character = 'B'; break;
					case 177: key.Character = 'N'; break;
					case 178: key.Character = 'M'; break;
					case 179: key.Character = '<'; break;
					case 180: key.Character = '>'; break;
					case 181: key.Character = '?'; break;
					case 182: key.KeyType = KeyType.RightShift; break;
					case 183: key.Character = '*'; break;
					case 184: key.KeyType = KeyType.RightAlt; break;
					case 185: key.Character = ' '; break;
					case 186: key.KeyType = KeyType.CapsLock; break;
					case 187: key.KeyType = KeyType.F1; break;
					case 188: key.KeyType = KeyType.F2; break;
					case 189: key.KeyType = KeyType.F3; break;
					case 190: key.KeyType = KeyType.F4; break;
					case 191: key.KeyType = KeyType.F5; break;
					case 192: key.KeyType = KeyType.F6; break;
					case 193: key.KeyType = KeyType.F7; break;
					case 194: key.KeyType = KeyType.F8; break;
					case 195: key.KeyType = KeyType.F9; break;
					case 196: key.KeyType = KeyType.F10; break;
					case 197: key.KeyType = KeyType.NumLock; break;
					case 198: key.KeyType = KeyType.ScrollLock; break;
					case 199: key.KeyType = KeyType.Home; break;
					case 200: key.KeyType = KeyType.UpArrow; break;
					case 201: key.KeyType = KeyType.PageUp; break;
					case 202: key.Character = '-'; break;
					case 203: key.KeyType = KeyType.LeftArrow; break;
					case 205: key.KeyType = KeyType.RightArrow; break;
					case 206: key.Character = '+'; break;
					case 207: key.KeyType = KeyType.End; break;
					case 208: key.KeyType = KeyType.DownArrow; break;
					case 209: key.KeyType = KeyType.PageDown; break;
					case 210: key.KeyType = KeyType.Insert; break;
					case 211: key.KeyType = KeyType.Delete; break;
					case 214: key.Character = '|'; break;
					case 215: key.KeyType = KeyType.F11; break;
					case 216: key.KeyType = KeyType.F12; break;
					default: break;
				}

				keyState = KeyState.Normal;
				return key;
			}
			else if ((keyState == KeyState.Escaped) || (keyState == KeyState.EscapeBreak))
			{
				if (scancode == 0xE0)
				{

					key.KeyType = KeyType.RegularKey;

					if (((scancode & 0x80) != 0) || (keyState == KeyState.EscapeBreak))
						key.KeyPress = KeyEvent.Press.Break;
					else
						key.KeyPress = KeyEvent.Press.Make;

					if (scancode == 0xF0)
					{
						keyState = KeyState.EscapeBreak;
						return key;
					}

					switch (scancode)
					{
						case 0x1C: key.Character = '\n'; break;
						case 0x1D: key.KeyType = KeyType.LeftControl; break;
						case 0x2A: key.KeyType = KeyType.LeftShift; break;
						case 0x35: key.Character = '/'; break;
						case 0x36: key.KeyType = KeyType.RightShift; break;

						case 0x37: key.KeyType = KeyType.ControlPrintScreen; break;
						case 0x38: key.KeyType = KeyType.LeftAlt; break; // ?

						case 0x46: key.KeyType = KeyType.ScrollLock; break;
						case 0x47: key.KeyType = KeyType.Home; break;
						case 0x48: key.KeyType = KeyType.UpArrow; break;
						case 0x49: key.KeyType = KeyType.PageUp; break;
						case 0x4B: key.KeyType = KeyType.LeftArrow; break;
						case 0x4D: key.KeyType = KeyType.RightArrow; break;

						case 0x4F: key.KeyType = KeyType.End; break;
						case 0x50: key.KeyType = KeyType.DownArrow; break;
						case 0x51: key.KeyType = KeyType.PageDown; break;
						case 0x52: key.KeyType = KeyType.Insert; break;
						case 0x53: key.KeyType = KeyType.Delete; break;

						case 0x5B: key.KeyType = KeyType.LeftWindow; break;
						case 0x5C: key.KeyType = KeyType.RightWindow; break;
						case 0x5D: key.KeyType = KeyType.Menu; break;

						//case 0x37: key.KeyType = Key.Special.Power; break;
						//case 0x3F: key.KeyType = Key.Special.Sleep; break;
						//case 0x5E: key.KeyType = Key.Special.Wake; break;

						default: break;
					}

					keyState = KeyState.Normal;
					return key;
				}
				else if (keyState == KeyState.Espaced2)
				{
					keyState = KeyState.Normal;
					return key;
				}
			}

			return key;
		}
	}
}
