/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.TestWorld.x86.Tests
{
	public interface IAA { }

	public interface IBB { }

	public interface ICC { }

	public class AA : IAA { }

	public class BB : AA, IBB { }

	public class CC { }

	public class DD : BB { }

	public class IsInstanceTest : KernelTest
	{
		public IsInstanceTest()
			: base("IsInstance")
		{
			testMethods.Add(IsInstTest1);
			testMethods.Add(IsInstTest2);
			testMethods.Add(IsInstTest3);
			testMethods.Add(IsInstTest4);
			testMethods.Add(IsInstTest5);
			testMethods.Add(IsInstTest6);
			testMethods.Add(IsInstTest7);
			testMethods.Add(IsInstTest8);
			testMethods.Add(IsInstTest9);
			testMethods.Add(IsInstTest10);
			testMethods.Add(IsInstTest11);
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