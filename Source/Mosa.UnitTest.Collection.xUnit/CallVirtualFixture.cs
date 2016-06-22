// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class CallVirtualFixture : TestFixture
	{
		[Fact]
		public void TestVirtualCall()
		{
			Assert.Equal(Mosa.UnitTest.Collection.VirtualDerived.TestVirtualCall(), Run<int>("Mosa.UnitTest.Collection.VirtualDerived.TestVirtualCall"));
		}

		[Fact]
		public void TestBaseCall()
		{
			Assert.Equal(Mosa.UnitTest.Collection.VirtualDerived.TestBaseCall(), Run<int>("Mosa.UnitTest.Collection.VirtualDerived.TestBaseCall"));
		}
	}
}
