// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TestWorld.x86.Tests
{
	public class ValueTypeTests
	{
		public static string Test1()
		{
			int i = 10;

			string s = i.ToString();

			return s;
		}

		public static int Test2()
		{
			ValueType vt = new ValueType(2);

			int i = vt.GetValue();

			return i;
		}
	}

	public struct ValueType
	{
		private int z;

		public ValueType(int x)
		{
			z = x;
		}

		public int GetValue()
		{
			return z;
		}
	}
}
