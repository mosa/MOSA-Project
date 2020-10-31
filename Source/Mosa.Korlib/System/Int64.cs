// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	///
	/// </summary>
	public struct Int64
	{
		public const long MaxValue = 0x7fffffffffffffff;
		public const long MinValue = -9223372036854775808;

		internal long _value;

		public int CompareTo(long value)
		{
			if (_value < value) return -1;
			else if (_value > value) return 1;
			return 0;
		}

		public bool Equals(long obj)
		{
			return Equals((object)obj);
		}

		public override bool Equals(object obj)
		{
			return ((long)obj) == _value;
		}

		public override int GetHashCode()
		{
			return (int)_value;
		}

		public static bool TryParse(string s, out long result)
		{
			try
			{
				result = Parse(s);
				return true;
			}
			catch
			{
				result = 0;
				return false;
			}
		}

		public static long Parse(string s)
		{
			const string digits = "0123456789";
			long result = 0;

			int z = 0;
			bool neg = false;

			if (s.Length >= 1)
			{
				if (s[0] == '+') z = 1;
				if (s[0] == '-')
				{
					z = 1;
					neg = true;
				}
			}

			for (int i = z; i < s.Length; i++)
			{
				int ind = digits.IndexOf(s[i]);
				if (ind == -1)
				{
					throw new Exception("Format is incorrect");
				}
				result = (result * 10) + ind;
			}

			if (neg) result *= -1;

			return result;
		}
	}
}
