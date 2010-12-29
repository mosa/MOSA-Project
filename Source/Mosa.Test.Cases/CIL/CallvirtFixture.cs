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

using Mosa.Test.Runtime.CompilerFramework;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	[Importance(Importance.Critical)]
	[Category(@"Method calls")]
	[Description(@"Tests proper method table building and virtual call support.")]
	public class CallvirtFixture : TestCompilerAdapter
	{
		public CallvirtFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void TestVirtualCall()
		{
			int result = Run<int>("Mosa.Test.Collection", @"VDerived", @"STest");
			Assert.AreEqual(7, result);
		}

		[Test]
		public void TestBaseCall()
		{
			int result = Run<int>("Mosa.Test.Collection", @"VDerived", @"STestBaseCall");
			Assert.AreEqual(12, result);
		}

	}
}
