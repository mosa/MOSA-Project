// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class BooleanFixture : X86TestFixture
	{
		[Theory]
		[MemberData("B", DisableDiscoveryEnumeration = true)]
		public void RetB(bool a)
		{
			Assert.Equal(BooleanTests.RetB(a), Run<bool>("Mosa.Test.Collection.BooleanTests.RetB", a));
		}

		[Theory]
		[MemberData("BB", DisableDiscoveryEnumeration = true)]
		public void AndBB(bool a, bool b)
		{
			Assert.Equal(BooleanTests.AndBB(a, b), Run<bool>("Mosa.Test.Collection.BooleanTests.AndBB", a, b));
		}

		[Theory]
		[MemberData("BB", DisableDiscoveryEnumeration = true)]
		public void OrBB(bool a, bool b)
		{
			Assert.Equal(BooleanTests.OrBB(a, b), Run<bool>("Mosa.Test.Collection.BooleanTests.OrBB", a, b));
		}

		[Theory]
		[MemberData("BB", DisableDiscoveryEnumeration = true)]
		public void XorBB(bool a, bool b)
		{
			Assert.Equal(BooleanTests.XorBB(a, b), Run<bool>("Mosa.Test.Collection.BooleanTests.XorBB", a, b));
		}

		[Theory]
		[MemberData("B", DisableDiscoveryEnumeration = true)]
		public void NotB(bool a)
		{
			Assert.Equal(BooleanTests.NotB(a), Run<bool>("Mosa.Test.Collection.BooleanTests.NotB", a));
		}

		[Fact]
		public void Newarr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.BooleanTests.Newarr"));
		}

		[Theory]
		[MemberData("I4Small", DisableDiscoveryEnumeration = true)]
		public void Ldlen(int length)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.BooleanTests.Ldlen", length));
		}

		[Theory]
		[MemberData("I4SmallB", DisableDiscoveryEnumeration = true)]
		public void StelemB(int index, bool value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.BooleanTests.Stelem", index, value));
		}

		[Theory]
		[MemberData("I4SmallB", DisableDiscoveryEnumeration = true)]
		public void LdelemB(int index, bool value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.BooleanTests.Ldelem", index, value));
		}

		[Theory]
		[MemberData("I4SmallB", DisableDiscoveryEnumeration = true)]
		public void LdelemaB(int index, bool value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.BooleanTests.Ldelema", index, value));
		}
	}
}
