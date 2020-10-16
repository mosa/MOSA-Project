// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	/// Implementation of the "Convert" class.
	/// </summary>
	public static class Convert
	{
		public static bool ToBoolean(bool value)
		{
			return value;
		}

		public static bool ToBoolean(byte value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(double value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(float value)
		{
			return (value != 0f);
		}

		public static bool ToBoolean(int value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(long value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(sbyte value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(short value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(uint value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(ulong value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(ushort value)
		{
			return (value != 0);
		}

		public static int ToInt32(bool value)
		{
			return value ? 1 : 0;
		}

		public static int ToInt32(byte value)
		{
			return value;
		}

		public static int ToInt32(char value)
		{
			return value;
		}
	}
}
