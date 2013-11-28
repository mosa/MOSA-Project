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
	public class NewObjectFixture : TestFixture
	{
		[Fact]
		public void NewObjectWithoutArgs()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.NewObjectTests.WithoutArgs"));
		}
	}
}