// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class BooleanFixture : TestFixture
	{
		[Theory]
		[MemberData("B", DisableDiscoveryEnumeration = true)]
		public void RetB(bool a)
		{
			Assert.Equal(BooleanTests.RetB(a), Run<bool>("Mosa.UnitTest.Collection.BooleanTests.RetB", a));
		}

		[Theory]
		[MemberData("BB", DisableDiscoveryEnumeration = true)]
		public void AndBB(bool a, bool b)
		{
			Assert.Equal(BooleanTests.AndBB(a, b), Run<bool>("Mosa.UnitTest.Collection.BooleanTests.AndBB", a, b));
		}

		[Theory]
		[MemberData("BB", DisableDiscoveryEnumeration = true)]
		public void OrBB(bool a, bool b)
		{
			Assert.Equal(BooleanTests.OrBB(a, b), Run<bool>("Mosa.UnitTest.Collection.BooleanTests.OrBB", a, b));
		}

		[Theory]
		[MemberData("BB", DisableDiscoveryEnumeration = true)]
		public void XorBB(bool a, bool b)
		{
			Assert.Equal(BooleanTests.XorBB(a, b), Run<bool>("Mosa.UnitTest.Collection.BooleanTests.XorBB", a, b));
		}

		[Theory]
		[MemberData("B", DisableDiscoveryEnumeration = true)]
		public void NotB(bool a)
		{
			Assert.Equal(BooleanTests.NotB(a), Run<bool>("Mosa.UnitTest.Collection.BooleanTests.NotB", a));
		}

		[Fact]
		public void Newarr()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.BooleanTests.Newarr"));
		}

		[Theory]
		[MemberData("I4Small", DisableDiscoveryEnumeration = true)]
		public void Ldlen(int length)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.BooleanTests.Ldlen", length));
		}

		[Theory]
		[MemberData("I4SmallB", DisableDiscoveryEnumeration = true)]
		public void StelemB(int index, bool value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.BooleanTests.Stelem", index, value));
		}

		[Theory]
		[MemberData("I4SmallB", DisableDiscoveryEnumeration = true)]
		public void LdelemB(int index, bool value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.BooleanTests.Ldelem", index, value));
		}

		[Theory]
		[MemberData("I4SmallB", DisableDiscoveryEnumeration = true)]
		public void LdelemaB(int index, bool value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.BooleanTests.Ldelema", index, value));
		}
	}
}
