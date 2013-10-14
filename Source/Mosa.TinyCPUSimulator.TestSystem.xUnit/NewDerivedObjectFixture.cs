/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *
 */

using Xunit;

namespace Mosa.TinyCPUSimulator.TestSystem.xUnit
{
	public class NewDerivedObjectFixture : TestFixture
	{
		[Fact]
		public void NewDerivedObjectWithoutArgs()
		{
			bool result = Run<bool>("Mosa.Test.Collection.DerivedNewObjectTests.WithoutArgs");
			Assert.True(result);
		}

		[Fact]
		public void NewDerivedObjectWithOneArg()
		{
			bool result = Run<bool>("Mosa.Test.Collection.DerivedNewObjectTests.WithOneArg");
			Assert.True(result);
		}

		[Fact]
		public void NewDerivedObjectjWithTwoArgs()
		{
			bool result = Run<bool>("Mosa.Test.Collection.DerivedNewObjectTests.WithTwoArgs");
			Assert.True(result);
		}

		[Fact]
		public void NewDerivedObjectWithThreeArgs()
		{
			bool result = Run<bool>("Mosa.Test.Collection", @"DerivedNewObjectTests", @"WithThreeArgs");
			Assert.True(result);
		}
	}
}