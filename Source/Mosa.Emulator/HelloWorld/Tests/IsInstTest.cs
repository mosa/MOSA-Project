/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.HelloWorld.x86.Tests
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
		public IsInstTest()
			: base("Inst")
		{
			testMethods.AddLast(IsInstTest1);
			testMethods.AddLast(IsInstTest2);
			testMethods.AddLast(IsInstTest3);
			testMethods.AddLast(IsInstTest4);
			testMethods.AddLast(IsInstTest5);
			testMethods.AddLast(IsInstTest6);
			testMethods.AddLast(IsInstTest7);
			testMethods.AddLast(IsInstTest8);
			testMethods.AddLast(IsInstTest9);
			testMethods.AddLast(IsInstTest10);
			testMethods.AddLast(IsInstTest11);
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