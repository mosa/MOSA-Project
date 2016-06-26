// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class RemFixture : TestFixture
	{
		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void RemI4_C1(int a)
		{
			Assert.Equal(RemTests.RemI4_C1(a), Run<int>("Mosa.UnitTest.Collection.RemTests.RemI4_C1", a));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void RemI4_C2(int a)
		{
			Assert.Equal(RemTests.RemI4_C2(a), Run<int>("Mosa.UnitTest.Collection.RemTests.RemI4_C2", a));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void RemI4_C4(int a)
		{
			Assert.Equal(RemTests.RemI4_C4(a), Run<int>("Mosa.UnitTest.Collection.RemTests.RemI4_C4", a));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void RemI4_C8(int a)
		{
			Assert.Equal(RemTests.RemI4_C8(a), Run<int>("Mosa.UnitTest.Collection.RemTests.RemI4_C8", a));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void RemI4_C16(int a)
		{
			Assert.Equal(RemTests.RemI4_C16(a), Run<int>("Mosa.UnitTest.Collection.RemTests.RemI4_C16", a));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void RemI4_C32(int a)
		{
			Assert.Equal(RemTests.RemI4_C32(a), Run<int>("Mosa.UnitTest.Collection.RemTests.RemI4_C32", a));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void RemI4_C64(int a)
		{
			Assert.Equal(RemTests.RemI4_C64(a), Run<int>("Mosa.UnitTest.Collection.RemTests.RemI4_C64", a));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void RemI4_C128(int a)
		{
			Assert.Equal(RemTests.RemI4_C128(a), Run<int>("Mosa.UnitTest.Collection.RemTests.RemI4_C128", a));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void RemI4_C256(int a)
		{
			Assert.Equal(RemTests.RemI4_C256(a), Run<int>("Mosa.UnitTest.Collection.RemTests.RemI4_C256", a));
		}

		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void RemU4_C1(uint a)
		{
			Assert.Equal(RemTests.RemU4_C1(a), Run<uint>("Mosa.UnitTest.Collection.RemTests.RemU4_C1", a));
		}

		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void RemU4_C2(uint a)
		{
			Assert.Equal(RemTests.RemU4_C2(a), Run<uint>("Mosa.UnitTest.Collection.RemTests.RemU4_C2", a));
		}

		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void RemU4_C4(uint a)
		{
			Assert.Equal(RemTests.RemU4_C4(a), Run<uint>("Mosa.UnitTest.Collection.RemTests.RemU4_C4", a));
		}

		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void RemU4_C8(uint a)
		{
			Assert.Equal(RemTests.RemU4_C8(a), Run<uint>("Mosa.UnitTest.Collection.RemTests.RemU4_C8", a));
		}

		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void RemU4_C16(uint a)
		{
			Assert.Equal(RemTests.RemU4_C16(a), Run<uint>("Mosa.UnitTest.Collection.RemTests.RemU4_C16", a));
		}

		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void RemU4_C32(uint a)
		{
			Assert.Equal(RemTests.RemU4_C32(a), Run<uint>("Mosa.UnitTest.Collection.RemTests.RemU4_C32", a));
		}

		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void RemU4_C64(uint a)
		{
			Assert.Equal(RemTests.RemU4_C64(a), Run<uint>("Mosa.UnitTest.Collection.RemTests.RemU4_C64", a));
		}

		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void RemU4_C128(uint a)
		{
			Assert.Equal(RemTests.RemU4_C128(a), Run<uint>("Mosa.UnitTest.Collection.RemTests.RemU4_C128", a));
		}

		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void RemU4_C256(uint a)
		{
			Assert.Equal(RemTests.RemU4_C256(a), Run<uint>("Mosa.UnitTest.Collection.RemTests.RemU4_C256", a));
		}
	}
}
