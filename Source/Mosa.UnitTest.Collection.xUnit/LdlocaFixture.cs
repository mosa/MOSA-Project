// Copyright (c) MOSA Project. Licensed under the New BSD License.


using Xunit;
using Xunit.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class Fixture_LdlocaU1 : TestFixture
	{
		[Theory]
		[MemberData(nameof(U1))]
		public void LdlocaCheckValueU1(byte a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueU1(a), Run<bool>("Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueU1", a));
		}
	}

	public class Fixture_LdlocaU2 : TestFixture
	{
		[Theory]
		[MemberData(nameof(U2))]
		public void LdlocaCheckValueU2(ushort a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueU2(a), Run<bool>("Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueU2", a));
		}
	}

	public class Fixture_LdlocaU4 : TestFixture
	{
		[Theory]
		[MemberData(nameof(U4))]
		public void LdlocaCheckValueU4(uint a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueU4(a), Run<bool>("Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueU4", a));
		}
	}

	public class Fixture_LdlocaU8 : TestFixture
	{
		[Theory]
		[MemberData(nameof(U8))]
		public void LdlocaCheckValueU8(ulong a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueU8(a), Run<bool>("Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueU8", a));
		}
	}

	public class Fixture_LdlocaI1 : TestFixture
	{
		[Theory]
		[MemberData(nameof(I1))]
		public void LdlocaCheckValueI1(sbyte a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueI1(a), Run<bool>("Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueI1", a));
		}
	}

	public class Fixture_LdlocaI2 : TestFixture
	{
		[Theory]
		[MemberData(nameof(I2))]
		public void LdlocaCheckValueI2(short a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueI2(a), Run<bool>("Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueI2", a));
		}
	}

	public class Fixture_LdlocaI4 : TestFixture
	{
		[Theory]
		[MemberData(nameof(I4))]
		public void LdlocaCheckValueI4(int a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueI4(a), Run<bool>("Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueI4", a));
		}
	}

	public class Fixture_LdlocaI8 : TestFixture
	{
		[Theory]
		[MemberData(nameof(I8))]
		public void LdlocaCheckValueI8(long a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueI8(a), Run<bool>("Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueI8", a));
		}
	}

	public class Fixture_LdlocaR4 : TestFixture
	{
		[Theory]
		[MemberData(nameof(R4))]
		public void LdlocaCheckValueR4(float a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueR4(a), Run<bool>("Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueR4", a));
		}
	}

	public class Fixture_LdlocaR8 : TestFixture
	{
		[Theory]
		[MemberData(nameof(R8))]
		public void LdlocaCheckValueR8(double a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueR8(a), Run<bool>("Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueR8", a));
		}
	}

	public class Fixture_LdlocaC : TestFixture
	{
		[Theory]
		[MemberData(nameof(C))]
		public void LdlocaCheckValueC(char a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueC(a), Run<bool>("Mosa.UnitTest.Collection.LdlocaTests.LdlocaCheckValueC", a));
		}
	}

}
