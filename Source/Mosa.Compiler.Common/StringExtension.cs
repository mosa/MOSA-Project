// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Common
{
	public static class StringExtension
	{
		public static ulong ParseHexOrInteger(this string value)
		{
			string nbr = value.ToUpper().Trim();
			int digits = 10;
			int where = nbr.IndexOf('X');

			if (where >= 0)
			{
				digits = 16;
				nbr = nbr.Substring(where + 1);
			}

			return Convert.ToUInt32(nbr, digits);
		}

		public static ulong ParseHex(this string value)
		{
			string nbr = value.ToUpper().Trim();
			int where = nbr.IndexOf('X');

			if (where >= 0)
			{
				nbr = nbr.Substring(where + 1);
			}

			return Convert.ToUInt64(value, 16);
		}

		public static int ToInt32(this string value)
		{
			return Convert.ToInt32(value);
		}

		public static uint ToUInt32(this string value)
		{
			return Convert.ToUInt32(value);
		}

		public static long ToInt64(this string value)
		{
			return Convert.ToInt64(value);
		}

		public static ulong ToUInt64(this string value)
		{
			return Convert.ToUInt64(value);
		}
	}
}
