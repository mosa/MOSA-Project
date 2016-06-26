// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTest.Collection
{
	public class VirtualBase
	{
		public virtual int Test()
		{
			return 5;
		}
	}

	public class VirtualDerived : VirtualBase
	{
		public static VirtualDerived Instance;// = new VirtualDerived();

		public static int TestVirtualCall()
		{
			Instance = new VirtualDerived();
			return Instance.Test();
		}

		public static int TestBaseCall()
		{
			Instance = new VirtualDerived();
			return Instance.TestBase();
		}

		public override int Test()
		{
			return 7;
		}

		public int TestBase()
		{
			return Test() + base.Test();
		}
	}
}
