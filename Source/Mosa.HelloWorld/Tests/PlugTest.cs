using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mosa.Internal;

namespace Mosa.HelloWorld.Tests
{
	class PlugTestCase
	{
		public int Double(int a) { return 0; } // imcomplete implementation
	}

	[PlugType(Target = "Mosa.HelloWorld.Tests.PlugTestCase")]
	class PlugTestImplementation
	{
		[PlugMethod(Target = "Mosa.HelloWorld.Tests.PlugTestCase.Double")]
		public int Double(int a) { return a + a; }
	}
}
