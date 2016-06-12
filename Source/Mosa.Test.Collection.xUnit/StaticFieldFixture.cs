// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;
using Mosa.Test.Collection;

namespace Mosa.Test.Collection.xUnit
{
	public class StaticFieldFixture : TestFixture
	{
		[Theory]
		[MemberData("U1", DisableDiscoveryEnumeration = true)]
		public void StaticFieldU1(byte a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.StaticFieldTestU1.StaticFieldU1", a));
		}

		[Theory]
		[MemberData("U2", DisableDiscoveryEnumeration = true)]
		public void StaticFieldU2(ushort a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.StaticFieldTestU2.StaticFieldU2", a));
		}

		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void StaticFieldU4(uint a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.StaticFieldTestU4.StaticFieldU4", a));
		}

		[Theory]
		[MemberData("U8", DisableDiscoveryEnumeration = true)]
		public void StaticFieldU8(ulong a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.StaticFieldTestU8.StaticFieldU8", a));
		}

		[Theory]
		[MemberData("I1", DisableDiscoveryEnumeration = true)]
		public void StaticFieldI1(sbyte a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.StaticFieldTestI1.StaticFieldI1", a));
		}

		[Theory]
		[MemberData("I2", DisableDiscoveryEnumeration = true)]
		public void StaticFieldI2(short a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.StaticFieldTestI2.StaticFieldI2", a));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void StaticFieldI4(int a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.StaticFieldTestI4.StaticFieldI4", a));
		}

		[Theory]
		[MemberData("I8", DisableDiscoveryEnumeration = true)]
		public void StaticFieldI8(long a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.StaticFieldTestI8.StaticFieldI8", a));
		}

		[Theory]
		[MemberData("R4", DisableDiscoveryEnumeration = true)]
		public void StaticFieldR4(float a)
		{
			if (float.IsNaN(a))
				return;

			Assert.True(Run<bool>("Mosa.Test.Collection.StaticFieldTestR4.StaticFieldR4", a));
		}

		[Theory]
		[MemberData("R8", DisableDiscoveryEnumeration = true)]
		public void StaticFieldR8(double a)
		{
			if (double.IsNaN(a))
				return;

			Assert.True(Run<bool>("Mosa.Test.Collection.StaticFieldTestR8.StaticFieldR8", a));
		}

		[Theory]
		[MemberData("B", DisableDiscoveryEnumeration = true)]
		public void StaticFieldB(bool a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.StaticFieldTestB.StaticFieldB", a));
		}

		[Theory]
		[MemberData("C", DisableDiscoveryEnumeration = true)]
		public void StaticFieldC(char a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.StaticFieldTestC.StaticFieldC", a));
		}

		[Theory]
		[MemberData("U1", DisableDiscoveryEnumeration = true)]
		public void StaticReturnFieldU1(byte a)
		{
			Assert.Equal(Mosa.Test.Collection.StaticFieldTestU1.StaticReturnFieldU1(a), Run<byte>("Mosa.Test.Collection.StaticFieldTestU1.StaticReturnFieldU1", a));
		}

		[Theory]
		[MemberData("U2", DisableDiscoveryEnumeration = true)]
		public void StaticReturnFieldU2(ushort a)
		{
			Assert.Equal(Mosa.Test.Collection.StaticFieldTestU2.StaticReturnFieldU2(a), Run<ushort>("Mosa.Test.Collection.StaticFieldTestU2.StaticReturnFieldU2", a));
		}

		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void StaticReturnFieldU4(uint a)
		{
			Assert.Equal(Mosa.Test.Collection.StaticFieldTestU4.StaticReturnFieldU4(a), Run<uint>("Mosa.Test.Collection.StaticFieldTestU4.StaticReturnFieldU4", a));
		}

		[Theory]
		[MemberData("U8", DisableDiscoveryEnumeration = true)]
		public void StaticReturnFieldU8(ulong a)
		{
			Assert.Equal(Mosa.Test.Collection.StaticFieldTestU8.StaticReturnFieldU8(a), Run<ulong>("Mosa.Test.Collection.StaticFieldTestU8.StaticReturnFieldU8", a));
		}

		[Theory]
		[MemberData("I1", DisableDiscoveryEnumeration = true)]
		public void StaticReturnFieldI1(sbyte a)
		{
			Assert.Equal(Mosa.Test.Collection.StaticFieldTestI1.StaticReturnFieldI1(a), Run<sbyte>("Mosa.Test.Collection.StaticFieldTestI1.StaticReturnFieldI1", a));
		}

		[Theory]
		[MemberData("I2", DisableDiscoveryEnumeration = true)]
		public void StaticReturnFieldI2(short a)
		{
			Assert.Equal(Mosa.Test.Collection.StaticFieldTestI2.StaticReturnFieldI2(a), Run<short>("Mosa.Test.Collection.StaticFieldTestI2.StaticReturnFieldI2", a));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void StaticReturnFieldI4(int a)
		{
			Assert.Equal(Mosa.Test.Collection.StaticFieldTestI4.StaticReturnFieldI4(a), Run<int>("Mosa.Test.Collection.StaticFieldTestI4.StaticReturnFieldI4", a));
		}

		[Theory]
		[MemberData("I8", DisableDiscoveryEnumeration = true)]
		public void StaticReturnFieldI8(long a)
		{
			Assert.Equal(Mosa.Test.Collection.StaticFieldTestI8.StaticReturnFieldI8(a), Run<long>("Mosa.Test.Collection.StaticFieldTestI8.StaticReturnFieldI8", a));
		}

		[Theory]
		[MemberData("R4", DisableDiscoveryEnumeration = true)]
		public void StaticReturnFieldR4(float a)
		{
			if (float.IsNaN(a))
				return;

			Assert.Equal(Mosa.Test.Collection.StaticFieldTestR4.StaticReturnFieldR4(a), Run<float>("Mosa.Test.Collection.StaticFieldTestR4.StaticReturnFieldR4", a));
		}

		[Theory]
		[MemberData("R8", DisableDiscoveryEnumeration = true)]
		public void StaticReturnFieldR8(double a)
		{
			if (double.IsNaN(a))
				return;

			Assert.Equal(Mosa.Test.Collection.StaticFieldTestR8.StaticReturnFieldR8(a), Run<double>("Mosa.Test.Collection.StaticFieldTestR8.StaticReturnFieldR8", a));
		}

		[Theory]
		[MemberData("B", DisableDiscoveryEnumeration = true)]
		public void StaticReturnFieldB(bool a)
		{
			Assert.Equal(Mosa.Test.Collection.StaticFieldTestB.StaticReturnFieldB(a), Run<bool>("Mosa.Test.Collection.StaticFieldTestB.StaticReturnFieldB", a));
		}

		[Theory]
		[MemberData("C", DisableDiscoveryEnumeration = true)]
		public void StaticReturnFieldC(char a)
		{
			Assert.Equal(Mosa.Test.Collection.StaticFieldTestC.StaticReturnFieldC(a), Run<char>("Mosa.Test.Collection.StaticFieldTestC.StaticReturnFieldC", a));
		}
	}
}
