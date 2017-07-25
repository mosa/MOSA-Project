// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

namespace Mosa.DeviceDriver.ScanCodeMap
{
	/// <summary>
	/// Implements the HU Keyboard map.
	/// </summary>
	public class HU : IScanCodeMap
	{
		private bool _shifted;
		private bool _alted;

		/// <summary>
		/// Initializes a new instance of the <see cref="HU"/> class.
		/// </summary>
		public HU()
		{
			_shifted = false;
			_alted = false;
		}

		private char TransformCharacter(char c)
		{
			if (_alted)
			{
				switch (c)
				{
					case 'q': return '\\';
					case 'w': return '|';

					case 'f': return '[';
					case 'g': return ']';
					case 'é': return '$';

					case 'í': return '<';
					case 'y': return '>';
					case 'x': return '#';
					case 'c': return '&';
					case 'v': return '@';
					case 'b': return '{';
					case 'n': return '}';
					case ',': return ';';
					case '.': return '>';
					case '-': return '*';
				}
			}

			if (_shifted)
				return char.ToUpper(c);

			return c;
		}

		/// <summary>
		/// Convert can code into a key
		/// </summary>
		/// <param name="scancode">The scancode.</param>
		/// <returns></returns>
		public KeyEvent ConvertScanCode(byte scancode)
		{
			var key = new KeyEvent();

			if (scancode == 0)
				return key;

			key.KeyPress = ((scancode & 0x80) != 0) ? KeyEvent.KeyPressType.Break : key.KeyPress = KeyEvent.KeyPressType.Make;
			key.KeyType = KeyType.RegularKey;

			switch (scancode & 0x7F)
			{
				case 1: key.Character = (char)27; break;
				case 2: key.Character = (_shifted ? '\'' : '1'); break;
				case 3: key.Character = (_shifted ? '"' : '2'); break;
				case 4: key.Character = (_shifted ? '+' : '3'); break;
				case 5: key.Character = (_shifted ? '!' : '4'); break;
				case 6: key.Character = (_shifted ? '%' : '5'); break;
				case 7: key.Character = (_shifted ? '/' : '6'); break;
				case 8: key.Character = (_shifted ? '=' : '7'); break;
				case 9: key.Character = (_shifted ? '(' : '8'); break;
				case 10: key.Character = (_shifted ? ')' : '9'); break;
				case 11: key.Character = TransformCharacter('ö'); break;
				case 12: key.Character = TransformCharacter('ü'); break;
				case 13: key.Character = TransformCharacter('ó'); break;
				case 14: key.Character = '\b'; break;
				case 15: key.Character = '\t'; break;
				case 16: key.Character = TransformCharacter('q'); break;
				case 17: key.Character = TransformCharacter('w'); break;
				case 18: key.Character = TransformCharacter('e'); break;
				case 19: key.Character = TransformCharacter('r'); break;
				case 20: key.Character = TransformCharacter('t'); break;
				case 21: key.Character = TransformCharacter('z'); break;
				case 22: key.Character = TransformCharacter('u'); break;
				case 23: key.Character = TransformCharacter('i'); break;
				case 24: key.Character = TransformCharacter('o'); break;
				case 25: key.Character = TransformCharacter('p'); break;
				case 26: key.Character = TransformCharacter('ő'); break;
				case 27: key.Character = TransformCharacter('ú'); break;
				case 28: key.Character = '\n'; break;
				case 29: key.KeyType = KeyType.LeftControl; break;
				case 30: key.Character = TransformCharacter('a'); break;
				case 31: key.Character = TransformCharacter('s'); break;
				case 32: key.Character = TransformCharacter('d'); break;
				case 33: key.Character = TransformCharacter('f'); break;
				case 34: key.Character = TransformCharacter('g'); break;
				case 35: key.Character = TransformCharacter('h'); break;
				case 36: key.Character = TransformCharacter('j'); break;
				case 37: key.Character = TransformCharacter('k'); break;
				case 38: key.Character = TransformCharacter('l'); break;
				case 39: key.Character = TransformCharacter('é'); break;
				case 40: key.Character = TransformCharacter('á'); break;
				case 41: key.Character = TransformCharacter('0'); break;
				case 42: key.KeyType = KeyType.LeftShift; break;
				case 43: key.Character = TransformCharacter('ű'); break;
				case 44: key.Character = TransformCharacter('y'); break;
				case 45: key.Character = TransformCharacter('x'); break;
				case 46: key.Character = TransformCharacter('c'); break;
				case 47: key.Character = TransformCharacter('v'); break;
				case 48: key.Character = TransformCharacter('b'); break;
				case 49: key.Character = TransformCharacter('n'); break;
				case 50: key.Character = TransformCharacter('m'); break;
				case 51: key.Character = (_shifted ? '?' : ','); break;
				case 52: key.Character = (_shifted ? ':' : '.'); break;
				case 53: key.Character = (_shifted ? '_' : '-'); break;
				case 54: key.KeyType = KeyType.RightShift; break;

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

				case 77: key.KeyType = KeyType.RightArrow; break;
				case 78: key.Character = '+'; break;
				case 79: key.KeyType = KeyType.End; break;
				case 80: key.KeyType = KeyType.DownArrow; break;
				case 81: key.KeyType = KeyType.PageDown; break;
				case 82: key.KeyType = KeyType.Insert; break;
				case 83: key.KeyType = KeyType.Delete; break;

				case 86: key.Character = TransformCharacter('<'); break;
				case 87: key.KeyType = KeyType.F11; break;
				case 88: key.KeyType = KeyType.F12; break;

				default: //Unmapped buttons (which doesn't exist on a hungarian keyboard)
					return new KeyEvent();
			}

			if ((key.KeyType == KeyType.LeftShift || key.KeyType == KeyType.RightShift))
				_shifted = (key.KeyPress == KeyEvent.KeyPressType.Make);
			if (key.KeyType == KeyType.CapsLock)
				_shifted = !_shifted;
			if (key.KeyType == KeyType.LeftAlt)
				_alted = (key.KeyPress == KeyEvent.KeyPressType.Make);

			return key;
		}
	}
}
