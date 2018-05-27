// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	/// Int32
	/// </summary>
	public struct Int32
	{
		public const int MaxValue = 0x7fffffff;
		public const int MinValue = -2147483648;

		internal int _value;

		public int CompareTo(int value)
		{
			if (_value < value) return -1;
			else if (_value > value) return 1;
			return 0;
		}

		public bool Equals(int obj)
		{
			return Equals((object)obj);
		}

		public override bool Equals(object obj)
		{
			return ((int)obj) == _value;
		}

		public override string ToString()
		{
			return CreateString((uint)_value, true, false);
		}

		public string ToString(string format)
		{
			return CreateString((uint)_value, false, true);
		}

		unsafe internal static string CreateString(uint value, bool signed, bool hex)
		{
			int offset = 0;

			uint uvalue = value;
			ushort divisor = hex ? (ushort)16 : (ushort)10;
			int length = 0;
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

			length = count;
			String result = String.InternalAllocateString(length);

			char* chars = result.first_char;

			if (negative)
			{
				*(chars + offset) = '-';
				offset++;
				count--;
			}

			for (int i = 0; i < count; i++)
			{
				uint remainder = uvalue % divisor;

				if (remainder < 10)
					*(chars + offset + count - 1 - i) = (char)('0' + remainder);
				else
					*(chars + offset + count - 1 - i) = (char)('A' + remainder - 10);

				uvalue /= divisor;
			}

			return result;
		}

		public override int GetHashCode()
		{
			return _value;
		}
	}
}
