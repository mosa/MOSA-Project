// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Common
{
	public static class StringExtension
	{
		public static uint ParseHexOrDecimal(this string value)
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
	}
}
