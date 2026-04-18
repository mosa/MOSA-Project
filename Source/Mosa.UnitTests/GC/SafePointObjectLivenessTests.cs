// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.GC;

public class SafePointObjectLivenessTests
{
	private class SimpleObject
	{
		public int Value;

		public SimpleObject(int value)
		{
			Value = value;
		}
	}

	[MosaUnitTest]
	public static int ObjectLiveRange()
	{
		int sum = 0;

		var o1 = new SimpleObject(1);
		var o2 = new SimpleObject(2);
		var o3 = new SimpleObject(3);
		var o4 = new SimpleObject(4);
		var o5 = new SimpleObject(5);

		for (int i = 0; i < 8; i++)
		{
			sum++;
		}

		return o1.Value + o2.Value;
	}
}
