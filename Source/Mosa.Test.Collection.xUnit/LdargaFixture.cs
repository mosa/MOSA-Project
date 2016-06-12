// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Test.Collection.xUnit
{
	public class Ldarga : TestFixture
	{
		private double DoubleTolerance = 0.000001d;
		private float FloatTolerance = 0.00001f;

		#region CheckValue

		[Theory]
		[MemberData("U1", DisableDiscoveryEnumeration = true)]
		public void LdargaCheckValueU1(byte a)
		{
			Assert.Equal(a, Run<byte>("Mosa.Test.Collection.LdargaTests.LdargaCheckValueU1", a));
		}

		[Theory]
		[MemberData("U2", DisableDiscoveryEnumeration = true)]
		public void LdargaCheckValueU2(ushort a)
		{
			Assert.Equal(a, Run<ushort>("Mosa.Test.Collection.LdargaTests.LdargaCheckValueU2", a));
		}

		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void LdargaCheckValueU4(uint a)
		{
			Assert.Equal(a, Run<uint>("Mosa.Test.Collection.LdargaTests.LdargaCheckValueU4", a));
		}

		[Theory]
		[MemberData("U8", DisableDiscoveryEnumeration = true)]
		public void LdargaCheckValueU8(ulong a)
		{
			Assert.Equal(a, Run<ulong>("Mosa.Test.Collection.LdargaTests.LdargaCheckValueU8", a));
		}

		[Theory]
		[MemberData("I1", DisableDiscoveryEnumeration = true)]
		public void LdargaCheckValueI1(sbyte a)
		{
			Assert.Equal(a, Run<sbyte>("Mosa.Test.Collection.LdargaTests.LdargaCheckValueI1", a));
		}

		[Theory]
		[MemberData("I2", DisableDiscoveryEnumeration = true)]
		public void LdargaCheckValueI2(short a)
		{
			Assert.Equal(a, Run<short>("Mosa.Test.Collection.LdargaTests.LdargaCheckValueI2", a));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void LdargaCheckValueI4(int a)
		{
			Assert.Equal(a, Run<int>("Mosa.Test.Collection.LdargaTests.LdargaCheckValueI4", a));
		}

		[Theory]
		[MemberData("I8", DisableDiscoveryEnumeration = true)]
		public void LdargaCheckValueI8(long a)
		{
			Assert.Equal(a, Run<long>("Mosa.Test.Collection.LdargaTests.LdargaCheckValueI8", a));
		}

		[Theory]
		[MemberData("C", DisableDiscoveryEnumeration = true)]
		public void LdargaCheckValueC(char a)
		{
			Assert.Equal(a, Run<char>("Mosa.Test.Collection.LdargaTests.LdargaCheckValueC", a));
		}

		[Theory]
		[MemberData("B", DisableDiscoveryEnumeration = true)]
		public void LdargaCheckValueB(bool a)
		{
			Assert.Equal(a, Run<bool>("Mosa.Test.Collection.LdargaTests.LdargaCheckValueB", a));
		}

		[Theory]
		[MemberData("R4", DisableDiscoveryEnumeration = true)]
		public void LdargaCheckValueR4(float a)
		{
			Assert.Equal(a, Run<float>("Mosa.Test.Collection.LdargaTests.LdargaCheckValueR4", a));
		}

		[Theory]
		[MemberData("R8", DisableDiscoveryEnumeration = true)]
		public void LdargaCheckValueR8(double a)
		{
			Assert.Equal(a, Run<double>("Mosa.Test.Collection.LdargaTests.LdargaCheckValueR8", a));
		}

		#endregion CheckValue

		#region ChangeValue

		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void LdargaChangeValueU1(byte a, byte b)
		{
			Assert.Equal(b, Run<byte>("Mosa.Test.Collection.LdargaTests.LdargaChangeValueU1", a, b));
		}

		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void LdargaChangeValueU2(ushort a, ushort b)
		{
			Assert.Equal(b, Run<ushort>("Mosa.Test.Collection.LdargaTests.LdargaChangeValueU2", a, b));
		}

		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void LdargaChangeValueU4(uint a, uint b)
		{
			Assert.Equal(b, Run<uint>("Mosa.Test.Collection.LdargaTests.LdargaChangeValueU4", a, b));
		}

		[Theory]
		[MemberData("U8U8", DisableDiscoveryEnumeration = true)]
		public void LdargaChangeValueU8(ulong a, ulong b)
		{
			Assert.Equal(b, Run<ulong>("Mosa.Test.Collection.LdargaTests.LdargaChangeValueU8", a, b));
		}

		[Theory]
		[MemberData("I1I1", DisableDiscoveryEnumeration = true)]
		public void LdargaChangeValueI1(sbyte a, sbyte b)
		{
			Assert.Equal(b, Run<sbyte>("Mosa.Test.Collection.LdargaTests.LdargaChangeValueI1", a, b));
		}

		[Theory]
		[MemberData("I2I2", DisableDiscoveryEnumeration = true)]
		public void LdargaChangeValueI2(short a, short b)
		{
			Assert.Equal(b, Run<short>("Mosa.Test.Collection.LdargaTests.LdargaChangeValueI2", a, b));
		}

		[Theory]
		[MemberData("I4I4", DisableDiscoveryEnumeration = true)]
		public void LdargaChangeValueI4(int a, int b)
		{
			Assert.Equal(b, Run<int>("Mosa.Test.Collection.LdargaTests.LdargaChangeValueI4", a, b));
		}

		[Theory]
		[MemberData("I8I8", DisableDiscoveryEnumeration = true)]
		public void LdargaChangeValueI8(long a, long b)
		{
			Assert.Equal(b, Run<long>("Mosa.Test.Collection.LdargaTests.LdargaChangeValueI8", a, b));
		}

		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void LdargaChangeValueC(char a, char b)
		{
			Assert.Equal(b, Run<char>("Mosa.Test.Collection.LdargaTests.LdargaChangeValueC", a, b));
		}

		[Theory]
		[MemberData("BB", DisableDiscoveryEnumeration = true)]
		public void LdargaChangeValueB(bool a, bool b)
		{
			Assert.Equal(b, Run<bool>("Mosa.Test.Collection.LdargaTests.LdargaChangeValueB", a, b));
		}

		[Theory]
		[MemberData("R4R4", DisableDiscoveryEnumeration = true)]
		public void LdargaChangeValueR4(float a, float b)
		{
			Assert.Equal(b, Run<float>("Mosa.Test.Collection.LdargaTests.LdargaChangeValueR4", a, b));
		}

		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void LdargaChangeValueR8(double a, double b)
		{
			Assert.Equal(b, Run<double>("Mosa.Test.Collection.LdargaTests.LdargaChangeValueR8", a, b));
		}

		#endregion ChangeValue
	}
}
