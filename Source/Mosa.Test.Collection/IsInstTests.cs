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
	public class AA { }
	public class BB : AA { }
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
	}

}
