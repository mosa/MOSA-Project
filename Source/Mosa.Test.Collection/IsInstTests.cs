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
	public interface IAA { }

	public interface IBB { }

	public interface ICC { }

	public class AA : IAA { }

	public class BB : AA, IBB { }

	public class CC { }

	public class DD : BB { }

	public static class IsInstTests
	{
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

			return (o is AA);
		}

		public static bool IsInstTest4()
		{
			object o = new CC();

			return (o is BB);
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

			return (o is CC);
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

			return (o is IAA);
		}

		public static bool IsInstTest11()
		{
			object o = new CC();

			return (o is IBB);
		}

		public static bool IsInstTest12()
		{
			object o = (int)1;
			return (o is int);
		}

		public static bool IsInstTest13()
		{
			object o = (uint)1;
			return (o is uint);
		}

		public static bool IsInstTest14()
		{
			object o = (long)1;
			return (o is long);
		}

		public static bool IsInstTest15()
		{
			object o = (ulong)1;
			return (o is ulong);
		}

		public static bool IsInstTest16()
		{
			object o = (int)1;
			return (o is uint);
		}

		public static bool IsInstTest17()
		{
			object o = (uint)1;
			return (o is int);
		}

		//public static bool IsInstTest16()
		//{
		//    object o = (char)'A';
		//    return (o is char);
		//}
	}
}