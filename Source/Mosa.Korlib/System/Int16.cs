// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System
{
	/// <summary>
	///
	/// </summary>
	public struct Int16
	{
		public const short MaxValue = 32767;
		public const short MinValue = -32768;

		internal short _value;

		public int CompareTo(short value)
		{
			if (_value < value) return -1;
			else if (_value > value) return 1;
			return 0;
		}

		public bool Equals(short obj)
		{
			return Equals((object)obj);
		}

		public override bool Equals(object obj)
		{
			return ((short)obj) == _value;
		}

		public override string ToString()
		{
			return int.CreateString((uint)_value, true, false);
		}

		public override int GetHashCode()
		{
			return _value;
		}

		public static bool TryParse(string s, out short result)
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

		public static short Parse(string s)
		{
			const string digits = "0123456789";
			short result = 0;

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
				result = (short)((result * 10) + ind);
			}

			if (neg) result *= -1;

			return result;
		}
	}
}
