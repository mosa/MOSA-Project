// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.UnitTest.Collection.xUnit
{

	public class Fixture_CallU1 : TestFixture
	{
		[Theory]
		[MemberData(nameof(U1), DisableDiscoveryEnumeration = true)]
		public void CallU1(byte a)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.CallTests.CallU1", a));
		}
	}

	public class Fixture_CallU2 : TestFixture
	{
		[Theory]
		[MemberData(nameof(U2), DisableDiscoveryEnumeration = true)]
		public void CallU2(ushort a)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.CallTests.CallU2", a));
		}
	}

	public class Fixture_CallU4 : TestFixture
	{
		[Theory]
		[MemberData(nameof(U4), DisableDiscoveryEnumeration = true)]
		public void CallU4(uint a)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.CallTests.CallU4", a));
		}
	}

	public class Fixture_CallU8 : TestFixture
	{
		[Theory]
		[MemberData(nameof(U8), DisableDiscoveryEnumeration = true)]
		public void CallU8(ulong a)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.CallTests.CallU8", a));
		}
	}

	public class Fixture_CallI1 : TestFixture
	{
		[Theory]
		[MemberData(nameof(I1), DisableDiscoveryEnumeration = true)]
		public void CallI1(sbyte a)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.CallTests.CallI1", a));
		}
	}

	public class Fixture_CallI2 : TestFixture
	{
		[Theory]
		[MemberData(nameof(I2), DisableDiscoveryEnumeration = true)]
		public void CallI2(short a)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.CallTests.CallI2", a));
		}
	}

	public class Fixture_CallI4 : TestFixture
	{
		[Theory]
		[MemberData(nameof(I4), DisableDiscoveryEnumeration = true)]
		public void CallI4(int a)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.CallTests.CallI4", a));
		}
	}

	public class Fixture_CallI8 : TestFixture
	{
		[Theory]
		[MemberData(nameof(I8), DisableDiscoveryEnumeration = true)]
		public void CallI8(long a)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.CallTests.CallI8", a));
		}
	}

	public class Fixture_CallC : TestFixture
	{
		[Theory]
		[MemberData(nameof(C), DisableDiscoveryEnumeration = true)]
		public void CallC(char a)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.CallTests.CallC", a));
		}
	}
}
