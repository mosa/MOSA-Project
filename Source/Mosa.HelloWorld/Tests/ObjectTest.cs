/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

using Mosa.Platform.x86;
using Mosa.Kernel;
using Mosa.Kernel.x86;
using Mosa.ClassLib;

namespace Mosa.HelloWorld.Tests
{

	public class ObjectTest : KernelTest
	{
		public class AA { }
		public class BB : AA { }
		public class CC { }

		public static void Test()
		{
			Screen.Write(" Object: ");

			PrintResult(ObjectTest1());
			PrintResult(ObjectTest2());
			PrintResult(ObjectTest3());
			PrintResult(ObjectTest4());
		}

		public static bool ObjectTest1()
		{
			object b = new BB();

			if (b is AA)
				return true;

			return false;
		}

		public static bool ObjectTest2()
		{
			object b = new AA();

			if (b is AA)
				return true;

			return false;
		}

		public static bool ObjectTest3()
		{
			object b = new CC();

			if (b is AA)
				return false;

			return true;
		}

		public static bool ObjectTest4()
		{
			object b = new CC();

			if (b is BB)
				return false;

			return true;
		}


	}

}
