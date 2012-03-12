/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Text;

using Mosa.Internal.Plug;

namespace Mosa.HelloWorld.x86.Tests
{
	class PlugTestCase
	{
		public static int Double(int a) { return 0; } // incomplete implementation
		public static int AddOne(int a) { return 0; } // incomplete implementation
		public int AddZZZ(int z) { return 0; } // incomplete implementation
	}

	[PlugType("Mosa.HelloWorld.x86.Tests.PlugTestCase")]
	static class PlugTestImplementation
	{
		public static int AddOne(int a) { return a + 1; }

		[PlugMethod("Mosa.HelloWorld.x86.Tests.PlugTestCase.Double")]
		public static int Double(int a) { return a + a; }

		public static int AddZZZ(ref PlugTestCase plugTestCase, int z) { return z; }
	}
}
