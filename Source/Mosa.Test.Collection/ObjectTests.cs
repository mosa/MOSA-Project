/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 */

namespace Mosa.Test.Collection
{
	public class Base
	{
		public int Test()
		{
			return 5;
		}
	}

	public class Derived : Base
	{
		private readonly int int32;

		public Derived()
		{
			this.int32 = 0;
		}

		public Derived(int value)
		{
			this.int32 = value;
		}

		public Derived(int v1, int v2)
		{
			this.int32 = v1 + v2;
		}

		public Derived(int v1, int v2, int v3)
		{
			this.int32 = (v1 * v2) + v3;
		}

		public static bool NewobjTest()
		{
			Derived d = new Derived();
			return d != null;
		}

		public static bool NewobjTestWithOneArg()
		{
			Derived d = new Derived(42);
			return (d.int32 == 42);
		}

		public static bool NewobjTestWithTwoArgs()
		{
			Derived d = new Derived(42, 3);
			return (d.int32 == 45);
		}

		public static bool NewobjTestWithThreeArgs()
		{
			Derived d = new Derived(21, 2, 7);
			return (d.int32 == 49);
		}
	}

	public class VBase
	{
		public virtual int Test()
		{
			return 5;
		}
	}

	public class VDerived : VBase
	{
		public static readonly VDerived Instance = new VDerived();

		public static int STest()
		{
			return Instance.Test();
		}

		public static int STestBaseCall()
		{
			return Instance.TestBaseCall();
		}

		public override int Test()
		{
			return 7;
		}

		public int TestBaseCall()
		{
			return this.Test() + base.Test();
		}
	}
}
