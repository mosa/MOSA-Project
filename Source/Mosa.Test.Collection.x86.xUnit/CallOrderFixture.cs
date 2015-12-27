// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class CallOrderFixture : X86TestFixture
	{
		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void CallOrderI4(int a)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderI4(a), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderI4", a));
		}

		[Theory]
		[MemberData("I4I4", DisableDiscoveryEnumeration = true)]
		public void CallOrderI4I4(int a, int b)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderI4I4(a, b), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderI4I4", a, b));
		}

		[Theory]
		[MemberData("I4I4", DisableDiscoveryEnumeration = true)]
		public void CallOrderI4I4_2(int a, int b)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderI4I4_2(a, b), Run<int>("Mosa.Test.Collection.CallOrderTests.CallOrderI4I4_2", a, b));
		}

		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void CallOrderU4U4(uint a, uint b)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderU4U4(a, b), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderU4U4", a, b));
		}

		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void CallOrderU4U4_2(uint a, uint b)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderU4U4_2(a, b), Run<uint>("Mosa.Test.Collection.CallOrderTests.CallOrderU4U4_2", a, b));
		}

		[Theory]
		[MemberData("I4MiniI4MiniI4Mini", DisableDiscoveryEnumeration = true)]
		public void CallOrderI4I4I4(int a, int b, int c)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderI4I4I4(a, b, c), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderI4I4I4", a, b, c));
		}

		[Theory]
		[MemberData("I4MiniI4MiniI4MiniI4Mini", DisableDiscoveryEnumeration = true)]
		public void CallOrderI4I4I4I4(int a, int b, int c, int d)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderI4I4I4I4(a, b, c, d), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderI4I4I4I4", a, b, c, d));
		}

		[Theory]
		[MemberData("I8MiniI8MiniI8MiniI8Mini", DisableDiscoveryEnumeration = true)]
		public void CallOrderI8I8I8I8(long a, long b, long c, long d)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderI8I8I8I8(a, b, c, d), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderI8I8I8I8", a, b, c, d));
		}

		[Theory]
		[MemberData("U8", DisableDiscoveryEnumeration = true)]
		public void CallOrderU8(ulong a)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderU8(a), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderU8", a));
		}

		[Theory]
		[MemberData("U8U8", DisableDiscoveryEnumeration = true)]
		public void CallOrderU8U8(ulong a, ulong b)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderU8U8(a, b), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderU8U8", a, b));
		}

		[Theory]
		[MemberData("U8MiniU8MiniU8MiniU8Mini", DisableDiscoveryEnumeration = true)]
		public void CallOrderU8U8U8U8(ulong a, ulong b, ulong c, ulong d)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderU8U8U8U8(a, b, c, d), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderU8U8U8U8", a, b, c, d));
		}

		[Theory]
		[MemberData("U4MiniU8MiniU8MiniU8Mini", DisableDiscoveryEnumeration = true)]
		public void CallOrderU4U8U8U8(uint a, ulong b, ulong c, ulong d)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderU4U8U8U8(a, b, c, d), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderU4U8U8U8", a, b, c, d));
		}

		[Theory]
		[MemberData("I4MiniI4MiniI4MiniI4Mini", DisableDiscoveryEnumeration = true)]
		public void CallOrderI4I4I4I4_2(int a, int b, int c, int d)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderI4I4I4I4_2(a, b, c, d), Run<int>("Mosa.Test.Collection.CallOrderTests.CallOrderI4I4I4I4_2", a, b, c, d));
		}
	}
}
