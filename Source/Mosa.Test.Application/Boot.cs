/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.Platform.Internal.x86;
using Mosa.ClassLib;

namespace Mosa.Test.Application
{
	/// <summary>
	///
	/// </summary>
	public static class Boot
	{
		public static void Test1()
		{
			Holder<int> holderInt = new Holder<int>(10);
		}

		public static void Test2()
		{
			Parent<int> parent = new Parent<int>();

			int p = parent.child.value;
		}

		public static void Test3()
		{
			LinkedList<object> list = new LinkedList<object>();

			object o = new object();

			list.Add(o);
		}

		/// <summary>
		/// Mains this instance.
		/// </summary>
		public static int Main()
		{
			Native.Nop();

			return 0;
		}
	}
}