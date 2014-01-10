/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.Platform.Internal.x86;

namespace Mosa.Test.Application
{
	/// <summary>
	///
	/// </summary>
	public static class Boot
	{

		public static int Test1()
		{
			Holder<int> holderInt = new Holder<int>(10);

			return holderInt.value;
		}

		public static int Test2()
		{
			Parent<int> parent = new Parent<int>();

			return parent.value;
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