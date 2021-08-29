// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.UnitTests
{
	public static class LockTests
	{
		[MosaUnitTest]
		public static bool Test1()
		{
			object o = new object();

			lock (o)
			{
			}

			return true;
		}

		[MosaUnitTest]
		public static bool Test2()
		{
			object o = new object();

			lock (o)
			{
				return false;
			}

			return true;
		}
	}
}
