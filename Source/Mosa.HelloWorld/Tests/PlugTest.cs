using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mosa.Internal;

namespace Mosa.HelloWorld.Tests
{
	class PlugTestCase
	{
		public int Double(int a) { return 0; } // incomplete implementation
	}

	[PlugType("Mosa.HelloWorld.Tests.PlugTestCase")]
	class PlugTestImplementation
	{
		[PlugMethod("Mosa.HelloWorld.Tests.PlugTestCase.Double")]
		public int Double(int a) { return a + a; }
	}
}
