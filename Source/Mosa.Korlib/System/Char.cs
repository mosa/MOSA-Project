// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System
{
	/// <summary>
	/// Char
	/// </summary>
	[Serializable]
	public struct Char: IComparable, IComparable<char>
	{
		public const char MaxValue = (char)0xffff;
		public const char MinValue = (char)0;

		internal char m_value;

		public override bool Equals(object obj)
		{
			if (this.GetType() == obj.GetType() && obj is Char)
			{
				return (this.m_value == ((Char)obj).m_value);
			}
			else
			{
				return false;
			}

		}

		public bool Equals(char obj)
		{
			return m_value == obj;
		}

		public int CompareTo(object value)
		{
			if (value == null) return 1;

			if (!(value is char)) throw new ArgumentException("Argument Type Must Be Char", "value");

			if (m_value < ((char)value).m_value) return -1;

			if (m_value > ((char)value).m_value) return 1;

			return 0;
		}

		public int CompareTo(char value)
		{
			if (m_value < value) return -1;

			if (m_value > value) return 1;

			return 0;
		}

		public static bool IsUpper(char c)
		{
			/*plug
			 * IsAscii
			 * IsLatin1
			 */
			return c >= 'A' && c <= 'Z';
		}

		public static bool IsUpper(string s, int index)
		{
			return IsUpper(s[index]);
		}

		public static bool IsLower(char c)
		{
			/*plug
			 * IsAscii
			 * IsLatin1
			 */
			return c >= 'a' && c <= 'z';
		}

		public static bool IsLower(string s, int index)
		{
			return IsLower(s[index]);
		}

		public override string ToString()
		{
			return new String(m_value, 1);
		}

		public override int GetHashCode()
		{
			return (int)m_value | ((int)m_value << 16);
		}

		private static bool IsWhiteSpaceLatin1(char c)
		{
			// There are characters which belong to UnicodeCategory.Control but are considered as white spaces.
			// We use code point comparisons for these characters here as a temporary fix.

			// U+0009 = <control> HORIZONTAL TAB
			// U+000a = <control> LINE FEED
			// U+000b = <control> VERTICAL TAB
			// U+000c = <contorl> FORM FEED
			// U+000d = <control> CARRIAGE RETURN
			// U+0085 = <control> NEXT LINE
			// U+00a0 = NO-BREAK SPACE
			if ((c == ' ') || (c >= '\x0009' && c <= '\x000d') || c == '\x00a0' || c == '\x0085')
			{
				return (true);
			}
			return (false);
		}

		private static bool IsLatin1(char ch) => ch <= 'ÿ';

		private static bool IsAscii(char ch) => ch <= '\u007F';

		public static bool IsWhiteSpace(char c)
		{
			return (IsWhiteSpaceLatin1(c));
		}

		public static bool IsLetter(char c)
		{
			/*plug
			 * IsAscii
			 * IsLatin1
			 */
			c |= ' ';
			return c >= 'a' && c <= 'z';
		}

		public static bool IsDigit(char c)
		{
			return c >= '0' && c <= '9';
		}

		public static bool IsLetterOrDigit(char c)
		{
			return (IsLetter(c) || IsDigit(c));
		}

		public static char ToUpper(char c)
		{
			if (IsUpper(c) || !IsLetter(c))
				return c;

			return (char)(c - 32);
		}

		public static char ToLower(char c)
		{
			if (IsLower(c) || !IsLetter(c))
				return c;

			return (char)(c + 32);
		}

		public static bool IsSurrogate(char c)
		{
			return c >= 0x00d800 && c <= 0x00dfff;
		}

		public static bool IsSurrogate(string s, int index)
		{
			if (s == null)
				throw new ArgumentNullException(nameof(s));

			if (((uint)index) >= ((uint)s.Length))
				throw new ArgumentOutOfRangeException(nameof(index));

			return IsSurrogate(s[index]);
		}

		public static bool IsHighSurrogate(char c)
		{
			return (c >= '\ud800') && (c <= '\udbff');
		}

		public static bool IsHighSurrogate(string s, int index)
		{
			if (s == null)
				throw new ArgumentNullException(nameof(s));

			if (index < 0 || index >= s.Length)
				throw new ArgumentOutOfRangeException(nameof(index));

			return IsHighSurrogate(s[index]);
		}

		public static bool IsLowSurrogate(char c)
		{
			return (c >= '\udc00') && (c <= '\udfff');
		}

		public static bool IsLowSurrogate(string s, int index)
		{
			if (s == null)
				throw new ArgumentNullException(nameof(s));

			if (index < 0 || index >= s.Length)
				throw new ArgumentOutOfRangeException(nameof(index));

			return IsLowSurrogate(s[index]);
		}
	}
}
