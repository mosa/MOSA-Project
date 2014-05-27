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

namespace Mosa.Test.Collection.x86.xUnit
{
	public class NewObjectFixture : X86TestFixture
	{
		[Fact]
		public void Create()
		{
			Assert.Equal(Mosa.Test.Collection.NewObjectTests.Create(), Run<bool>("Mosa.Test.Collection.NewObjectTests.Create"));
		}

		[Fact]
		public void CreateAndCallMethod()
		{
			Assert.Equal(Mosa.Test.Collection.NewObjectTests.CreateAndCallMethod(), Run<bool>("Mosa.Test.Collection.NewObjectTests.CreateAndCallMethod"));
		}
	}
}