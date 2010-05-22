/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System
{
	/// <summary>
	/// 
	/// </summary>
	public struct Int32
	{
		public const int MaxValue = 0x7fffffff;
		public const int MinValue = -2147483648;

		internal int m_value;

		//public override string ToString()
		//{
		//    return CreateString((uint)m_value, true, false);
		//}

		unsafe static string CreateString(uint value, bool signed, bool hex)
		{
			int offset = 0;

			uint uvalue = (uint)value;
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

			fixed (int* len = &result.length)
			{
				char* chars = (char*)(len + 1);

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
			}

			return result;
		}
	}
}
