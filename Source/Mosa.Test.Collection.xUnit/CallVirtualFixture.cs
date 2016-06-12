// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.xUnit
{
	public class CallVirtualFixture : TestFixture
	{
		[Fact]
		public void TestVirtualCall()
		{
			Assert.Equal(Mosa.Test.Collection.VirtualDerived.TestVirtualCall(), Run<int>("Mosa.Test.Collection.VirtualDerived.TestVirtualCall"));
		}

		[Fact]
		public void TestBaseCall()
		{
			Assert.Equal(Mosa.Test.Collection.VirtualDerived.TestBaseCall(), Run<int>("Mosa.Test.Collection.VirtualDerived.TestBaseCall"));
		}
	}
}
