using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Tools.Permutations
{
	public static class Constants
	{
		public static IList<string> SubstituteWithConstants(IList<string> values, bool isChar)
		{
			IList<string> newvalues = new List<string>();

			foreach (string value in values)
				if (isChar)
					newvalues.Add(SubstituteConstant2(value));
				else
					newvalues.Add(SubstituteConstant(value));

			return newvalues;
		}

		public static string SubstituteConstant(string value)
		{
			int comma = value.IndexOf(',');
			string exception = string.Empty;

			if (comma > 0)
			{
				exception = value.Substring(comma);
				value = value.Substring(0, comma);
			}

			if (value == UInt64.MaxValue.ToString()) value = "UInt64.MaxValue";
			else if (value == Int64.MaxValue.ToString()) value = "Int64.MaxValue";
			else if (value == Int64.MinValue.ToString()) value = "Int64.MinValue";
			else if (value == UInt32.MaxValue.ToString()) value = "UInt32.MaxValue";
			else if (value == Int32.MaxValue.ToString()) value = "Int32.MaxValue";
			else if (value == Int32.MinValue.ToString()) value = "Int32.MinValue";
			else if (value == UInt16.MaxValue.ToString()) value = "UInt16.MaxValue";
			else if (value == Int16.MaxValue.ToString()) value = "Int16.MaxValue";
			else if (value == Int16.MinValue.ToString()) value = "Int16.MinValue";
			else if (value == byte.MaxValue.ToString()) value = "byte.MaxValue";
			else if (value == sbyte.MaxValue.ToString()) value = "sbyte.MaxValue";
			else if (value == sbyte.MinValue.ToString()) value = "sbyte.MinValue";

			else if (value == (UInt64.MaxValue - 1).ToString()) value = "UInt64.MaxValue - 1";
			else if (value == (Int64.MaxValue - 1).ToString()) value = "Int64.MaxValue - 1";
			else if (value == (Int64.MinValue + 1).ToString()) value = "Int64.MinValue + 1";
			else if (value == (UInt32.MaxValue - 1).ToString()) value = "UInt32.MaxValue - 1";
			else if (value == (Int32.MaxValue - 1).ToString()) value = "Int32.MaxValue - 1";
			else if (value == (Int32.MinValue + 1).ToString()) value = "Int32.MinValue + 1";
			else if (value == (UInt16.MaxValue - 1).ToString()) value = "UInt16.MaxValue - 1";
			else if (value == (Int16.MaxValue - 1).ToString()) value = "Int16.MaxValue - 1";
			else if (value == (Int16.MinValue + 1).ToString()) value = "Int16.MinValue + 1";
			else if (value == (byte.MaxValue - 1).ToString()) value = "byte.MaxValue - 1";
			else if (value == (sbyte.MaxValue - 1).ToString()) value = "sbyte.MaxValue - 1";
			else if (value == (sbyte.MinValue + 1).ToString()) value = "sbyte.MinValue + 1";

			else if (value == (UInt64.MaxValue - 2).ToString()) value = "UInt64.MaxValue - 2";
			else if (value == (Int64.MaxValue - 2).ToString()) value = "Int64.MaxValue - 2";
			else if (value == (Int64.MinValue + 2).ToString()) value = "Int64.MinValue + 2";
			else if (value == (UInt32.MaxValue - 2).ToString()) value = "UInt32.MaxValue - 2";
			else if (value == (Int32.MaxValue - 2).ToString()) value = "Int32.MaxValue - 2";
			else if (value == (Int32.MinValue + 2).ToString()) value = "Int32.MinValue + 2";
			else if (value == (UInt16.MaxValue - 2).ToString()) value = "UInt16.MaxValue - 2";
			else if (value == (Int16.MaxValue - 2).ToString()) value = "Int16.MaxValue - 2";
			else if (value == (Int16.MinValue + 2).ToString()) value = "Int16.MinValue + 2";
			else if (value == (byte.MaxValue - 2).ToString()) value = "byte.MaxValue - 2";
			else if (value == (sbyte.MaxValue - 2).ToString()) value = "sbyte.MaxValue - 2";
			else if (value == (sbyte.MinValue + 2).ToString()) value = "sbyte.MinValue + 2";
			else if (value == double.MaxValue.ToString()) value = "double.MaxValue";
			else if (value == double.MinValue.ToString()) value = "double.MinValue";
			//else if (value == double.NaN.ToString()) value = "double.NaN";
			//else if (value == double.NegativeInfinity.ToString()) value = "double.NegativeInfinity";
			//else if (value == double.PositiveInfinity.ToString()) value = "double.PositiveInfinity";
			//else if (value == float.NaN.ToString()) value = "float.NaN";
			//else if (value == float.NegativeInfinity.ToString()) value = "float.NegativeInfinity";
			//else if (value == float.PositiveInfinity.ToString()) value = "float.PositiveInfinity";

			return value + exception;
		}

		public static string SubstituteConstant2(string value)
		{
			int comma = value.IndexOf(',');
			string exception = string.Empty;

			if (comma > 0)
			{
				exception = value.Substring(comma);
				value = value.Substring(0, comma);
			}

			if (value == ((int)char.MaxValue).ToString()) value = "Char.MaxValue";
			else if (value == ((int)char.MinValue).ToString()) value = "Char.MinValue";

			return value + exception;
		}

	}
}
