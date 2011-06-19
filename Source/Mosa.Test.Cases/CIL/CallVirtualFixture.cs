/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  
 */

using MbUnit.Framework;

using Mosa.Test.System;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	[Importance(Importance.Critical)]
	//[Category(@"Method calls")]
	//[Description(@"Tests proper method table building and virtual call support.")]
	public class CallVirtualFixture : TestCompilerAdapter
	{
		public CallVirtualFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void TestVirtualCall()
		{
			int result = Run<int>("Mosa.Test.Collection", @"VirtualDerived", @"TestVirtualCall");
			Assert.AreEqual(7, result);
		}

		[Test]
		public void TestBaseCall()
		{
			int result = Run<int>("Mosa.Test.Collection", @"VirtualDerived", @"TestBaseCall");
			Assert.AreEqual(12, result);
		}

	}
}
