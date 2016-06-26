// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class RegisterAllocatorFixture : TestFixture
	{
		[Fact]
		public void Pressure8()
		{
			Assert.Equal(Mosa.UnitTest.Collection.RegisterAllocatorTests.Pressure8(), Run<int>("Mosa.UnitTest.Collection.RegisterAllocatorTests.Pressure8"));
		}

		[Theory]
		[MemberData("I4MiniI4MiniI4MiniI4Mini", DisableDiscoveryEnumeration = true)]
		public void Pressure7(int a, int b, int c, int d)
		{
			Assert.Equal(Mosa.UnitTest.Collection.RegisterAllocatorTests.Pressure7(a, b, 7, c, 9, d, 10), Run<int>("Mosa.UnitTest.Collection.RegisterAllocatorTests.Pressure7", a, b, 7, c, 9, d, 10));
		}

		[Theory]
		[MemberData("I4MiniI4MiniI4MiniI4Mini", DisableDiscoveryEnumeration = true)]
		public void Pressure9(int a, int b, int c, int d)
		{
			Assert.Equal(Mosa.UnitTest.Collection.RegisterAllocatorTests.Pressure9(a, b, c, 7, d, 3, 9), Run<int>("Mosa.UnitTest.Collection.RegisterAllocatorTests.Pressure9", a, b, c, 7, d, 3, 9));
		}
	}
}
