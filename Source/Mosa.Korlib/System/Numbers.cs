// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	internal static class Numbers
	{
		public static string UInt8ToString(byte value, string format = null)
		{
			// TODO: Actually formats
			return CreateString(value, false, format);
		}

		public static string Int8ToString(sbyte value, string format = null)
		{
			// TODO: Actually formats
			return CreateString(value, true, format);
		}

		public static string Int16ToString(short value, string format = null)
		{
			// TODO: Actually formats
			return CreateString(value, true, format);
		}

		public static string UInt16ToString(ushort value, string format = null)
		{
			// TODO: Actually formats
			return CreateString(value, true, format);
		}

		public static string Int32ToString(int value, string format = null)
		{
			// TODO: Actually formats
			return CreateString(value, true, format);
		}

		public static string UInt32ToString(uint value, string format = null)
		{
			// TODO: Actually formats
			return CreateString(value, format);
		}

		/// Used for all times except uint.
		unsafe static string CreateString(int value, bool signed, string format)
		{
			bool hex = format != null ? true : false;

			int offset = 0;

			uint uvalue = (uint)value;
			var divisor = hex ? 16u : 10;
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
			var result = String.InternalAllocateString(length);
		
			char* chars = result.first_char;


			if (negative)
			{
				*(chars + offset) = '-';
				offset++;
				count--;
			}

			for (var i = 0; i < count; i++)
			{
				var remainder = uvalue % divisor;

				if (remainder < 10)
					*(chars + offset + count - 1 - i) = (char)('0' + remainder);
				else
					*(chars + offset + count - 1 - i) = (char)('A' + remainder - 10);

				uvalue /= divisor;
			}

			return result;
		}

		/// Used for uints only atm.
		unsafe static string CreateString(uint value, string format)
		{
			bool hex = format != null ? true : false;

			int offset = 0;

			uint uvalue = (uint)value;
			var divisor = hex ? 16u : 10;
			int length = 0;
			int count = 0;
			uint temp;

			temp = uvalue;

			do
			{
				temp /= divisor;
				count++;
			}
			while (temp != 0);

			length = count;
			var result = String.InternalAllocateString(length);
			var chars = result.first_char;

			for (var i = 0; i < count; i++)
			{
				var remainder = uvalue % divisor;

				if (remainder < 10)
					*(chars + offset + count - 1 - i) = (char)('0' + remainder);
				else
					*(chars + offset + count - 1 - i) = (char)('A' + remainder - 10);
				uvalue /= divisor;
			}

			return result;
		}
	}
}
