// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.Text
{
	public class StringBuilder
	{
		private char[] Characters = new char[int.MaxValue];

		private int Length = 0;

		public StringBuilder Append(char c)
		{
			if (Length >= Characters.Length)
			{
				char[] chars = new char[Length + 100];
				Characters.CopyTo(chars, 0);
				Characters = chars;
			}

			Characters[Length] = c;
			Length++;

			return this;
		}

		public StringBuilder AppendLine(string s)
		{
			for (int i = 0; i < s.length; i++)
				Append(s[i]);

			return this;
		}

		public char[] ToCharArray()
		{
			return Characters;
		}

		public override string ToString()
		{
			return new string(Characters, 0, Length);
		}
	}
}