﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Test.Collection
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
			return this.Test() + base.Test();
		}
	}
}