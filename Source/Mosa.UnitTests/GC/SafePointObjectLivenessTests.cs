// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.GC;

public class SafePointObjectLivenessTests
{
	private class SimmpleObject
	{
		public int Value;

		public SimmpleObject(int value)
		{
			Value = value;
		}
	}

	[MosaUnitTest]
	public static int ObjectLiveRange()
	{
		int sum = 0;

		var o1 = new SimmpleObject(1);
		var o2 = new SimmpleObject(2);
		var o3 = new SimmpleObject(3);
		var o4 = new SimmpleObject(4);
		var o5 = new SimmpleObject(5);

		for (int i = 0; i < 8; i++)
		{
			sum++;
		}

		return o1.Value + o2.Value;
	}
}
