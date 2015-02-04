/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Sebastian Loncar (Arakis) <sebastian.loncar@gmail.com>
 */

using System.Runtime.InteropServices;

namespace Mosa.Platform.Internal.x86
{
	[StructLayout(LayoutKind.Explicit, Size = 204)]
	public struct StringBuffer
	{
		[FieldOffset(0)]
		private int length;

		public const int MaxLength = 100;
		public const int EntrySize = 204;

		[FieldOffset(4)]
		unsafe private char* chars;

		unsafe public char this[int index]
		{
			get { return chars[index]; }
			set { chars[index] = value; }
		}

		public void Clear()
		{
			length = 0;
		}

		public void Set(string value)
		{
			Clear();
			Append(value);
		}

		#region Append

		public void Append(string value)
		{
			if (value == null)
				return;

			for (var i = 0; i < value.Length; i++)
				Append(value[i]);
		}

		public void Append(string value, int start)
		{
			if (value == null) return;
			Append(value, start, value.Length - start);
		}

		public void Append(string value, int start, int length)
		{
			if (value == null) return;
			for (var i = 0; i < length; i++)
				Append(value[i + start]);
		}

		public void Append(StringBuffer value)
		{
			if (value.length == 0)
				return;

			for (var i = 0; i < value.Length; i++)
				Append(value[i]);
		}

		public void Append(StringBuffer value, int start)
		{
			if (value.length == 0) return;
			Append(value, start, value.Length - start);
		}

		public void Append(StringBuffer value, int start, int length)
		{
			if (value.length == 0) return;
			for (var i = 0; i < length; i++)
				Append(value[i + start]);
		}

		unsafe public void Append(char value)
		{
			if (length + 1 >= MaxLength)
			{
				//TODO: Error
				return;
			}
			length++;
			this[length - 1] = value;
		}

		public void Append(uint value)
		{
			Append(value, false, false);
		}

		public void Append(int value)
		{
			Append((uint)value, true, false);
		}

		public void Append(uint value, string format)
		{
			Append(value, false, true);
		}

		public void Append(int value, string format)
		{
			Append((uint)value, true, true);
		}

		unsafe internal void Append(uint value, bool signed, bool hex)
		{
			int offset = 0;

			uint uvalue = (uint)value;
			ushort divisor = hex ? (ushort)16 : (ushort)10;
			int len = 0;
			int count = 0;
			uint temp;
			bool negative = false;

			if (value < 0 && !hex && signed)
			{
				count++;
				uvalue = (uint)-value;
				negative = true;
			}

			temp = uvalue;

			do
			{
				temp /= divisor;
				count++;
			}
			while (temp != 0);

			var firstChar = (this.chars + this.length);

			len = count;
			Length += len;

			if (negative)
			{
				*(firstChar + offset) = '-';
				offset++;
				count--;
			}

			for (int i = 0; i < count; i++)
			{
				uint remainder = uvalue % divisor;

				if (remainder < 10)
					*(firstChar + offset + count - 1 - i) = (char)('0' + remainder);
				else
					*(firstChar + offset + count - 1 - i) = (char)('A' + remainder - 10);

				uvalue /= divisor;
			}
		}

		#endregion Append

		public void SetLength(int length)
		{
			Length = length;
		}

		public int Length
		{
			get { return length; }
			private set
			{
				if (value > MaxLength)
				{
					//TODO: Error
					value = MaxLength;
				}
				length = value;
			}
		}

		public int IndexOf(string value)
		{
			if (this.length == 0)
				return -1;

			return IndexOfImpl(value, 0, this.length);
		}

		private int IndexOfImpl(string value, int startIndex, int count)
		{
			for (int i = startIndex; i < count; i++)
			{
				bool found = true;
				for (int n = 0; n < value.Length; n++)
				{
					if (this[i] != value[n])
					{
						found = false;
						break;
					}
				}
				if (found)
					return i;
			}

			return -1;
		}
	}
}