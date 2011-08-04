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
	public interface IAA { }
	public interface IBB { }
	public interface ICC { }

	public class AA : IAA { }
	public class BB : AA, IBB { }
	public class CC { }
	public class DD : BB { }

	public class IsInstTest : KernelTest
	{
		public static void Test()
		{
			Screen.Color = Colors.Gray;
			Screen.Write(" Inst:");

			PrintResult(IsInstTest1());
			PrintResult(IsInstTest2());
			PrintResult(IsInstTest3());
			PrintResult(IsInstTest4());
			PrintResult(IsInstTest5());
			PrintResult(IsInstTest6());
			PrintResult(IsInstTest7());

			PrintResult(IsInstTest8());
			PrintResult(IsInstTest9());
			PrintResult(IsInstTest10());
			PrintResult(IsInstTest11());
		}

		public static bool IsInstTest1()
		{
			object o = new AA();

			return (o is AA);
		}

		public static bool IsInstTest2()
		{
			object o = new BB();

			return (o is AA);
		}

		public static bool IsInstTest3()
		{
			object o = new CC();

			return !(o is AA);
		}

		public static bool IsInstTest4()
		{
			object o = new CC();

			return !(o is BB);
		}

		public static bool IsInstTest5()
		{
			object o = new DD();

			return (o is AA);
		}

		public static bool IsInstTest6()
		{
			object o = new DD();

			return (o is BB);
		}

		public static bool IsInstTest7()
		{
			object o = new DD();

			return !(o is CC);
		}


		public static bool IsInstTest8()
		{
			object o = new AA();

			return (o is IAA);
		}

		public static bool IsInstTest9()
		{
			object o = new BB();

			return (o is IAA);
		}

		public static bool IsInstTest10()
		{
			object o = new CC();

			return !(o is IAA);
		}

		public static bool IsInstTest11()
		{
			object o = new CC();

			return !(o is IBB);
		}

	}

}
