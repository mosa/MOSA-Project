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
	[Category(@"Object support")]
	[Description(@"Tests new operator, type checking and virtual method calls.")]
	public class ObjectFixture : TestCompilerAdapter
	{
		public ObjectFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void TestNewobjWithoutArgs()
		{
			bool result = Run<bool>("Mosa.Test.Collection", @"Derived", @"NewobjTest");
			Assert.IsTrue(result);
		}

		[Test]
		public void TestNewobjWithOneArg()
		{
			bool result = Run<bool>("Mosa.Test.Collection", @"Derived", @"NewobjTestWithOneArg");
			Assert.IsTrue(result);
		}

		[Test]
		public void TestNewobjWithTwoArgs()
		{
			bool result = Run<bool>("Mosa.Test.Collection", @"Derived", @"NewobjTestWithTwoArgs");
			Assert.IsTrue(result);
		}

		[Test]
		public void TestNewobjWithThreeArgs()
		{
			bool result = Run<bool>("Mosa.Test.Collection", @"Derived", @"NewobjTestWithThreeArgs");
			Assert.IsTrue(result);
		}

	}
}
