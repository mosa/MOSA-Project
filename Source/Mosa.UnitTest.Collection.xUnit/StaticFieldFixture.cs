// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class StaticFieldFixture : TestFixture
	{
		[Theory]
		[MemberData(nameof(U1))]
		public void StaticFieldU1(byte a)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.StaticFieldTestU1.StaticFieldU1", a));
		}

		[Theory]
		[MemberData(nameof(U2))]
		public void StaticFieldU2(ushort a)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.StaticFieldTestU2.StaticFieldU2", a));
		}

		[Theory]
		[MemberData(nameof(U4))]
		public void StaticFieldU4(uint a)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.StaticFieldTestU4.StaticFieldU4", a));
		}

		[Theory]
		[MemberData(nameof(U8))]
		public void StaticFieldU8(ulong a)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.StaticFieldTestU8.StaticFieldU8", a));
		}

		[Theory]
		[MemberData(nameof(I1))]
		public void StaticFieldI1(sbyte a)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.StaticFieldTestI1.StaticFieldI1", a));
		}

		[Theory]
		[MemberData(nameof(I2))]
		public void StaticFieldI2(short a)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.StaticFieldTestI2.StaticFieldI2", a));
		}

		[Theory]
		[MemberData(nameof(I4))]
		public void StaticFieldI4(int a)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.StaticFieldTestI4.StaticFieldI4", a));
		}

		[Theory]
		[MemberData(nameof(I8))]
		public void StaticFieldI8(long a)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.StaticFieldTestI8.StaticFieldI8", a));
		}

		[Theory]
		[MemberData(nameof(R4))]
		public void StaticFieldR4(float a)
		{
			if (float.IsNaN(a))
				return;

			Assert.True(Run<bool>("Mosa.UnitTest.Collection.StaticFieldTestR4.StaticFieldR4", a));
		}

		[Theory]
		[MemberData(nameof(R8))]
		public void StaticFieldR8(double a)
		{
			if (double.IsNaN(a))
				return;

			Assert.True(Run<bool>("Mosa.UnitTest.Collection.StaticFieldTestR8.StaticFieldR8", a));
		}

		[Theory]
		[MemberData(nameof(B))]
		public void StaticFieldB(bool a)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.StaticFieldTestB.StaticFieldB", a));
		}

		[Theory]
		[MemberData(nameof(C))]
		public void StaticFieldC(char a)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.StaticFieldTestC.StaticFieldC", a));
		}

		[Theory]
		[MemberData(nameof(U1))]
		public void StaticReturnFieldU1(byte a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.StaticFieldTestU1.StaticReturnFieldU1(a), Run<byte>("Mosa.UnitTest.Collection.StaticFieldTestU1.StaticReturnFieldU1", a));
		}

		[Theory]
		[MemberData(nameof(U2))]
		public void StaticReturnFieldU2(ushort a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.StaticFieldTestU2.StaticReturnFieldU2(a), Run<ushort>("Mosa.UnitTest.Collection.StaticFieldTestU2.StaticReturnFieldU2", a));
		}

		[Theory]
		[MemberData(nameof(U4))]
		public void StaticReturnFieldU4(uint a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.StaticFieldTestU4.StaticReturnFieldU4(a), Run<uint>("Mosa.UnitTest.Collection.StaticFieldTestU4.StaticReturnFieldU4", a));
		}

		[Theory]
		[MemberData(nameof(U8))]
		public void StaticReturnFieldU8(ulong a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.StaticFieldTestU8.StaticReturnFieldU8(a), Run<ulong>("Mosa.UnitTest.Collection.StaticFieldTestU8.StaticReturnFieldU8", a));
		}

		[Theory]
		[MemberData(nameof(I1))]
		public void StaticReturnFieldI1(sbyte a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.StaticFieldTestI1.StaticReturnFieldI1(a), Run<sbyte>("Mosa.UnitTest.Collection.StaticFieldTestI1.StaticReturnFieldI1", a));
		}

		[Theory]
		[MemberData(nameof(I2))]
		public void StaticReturnFieldI2(short a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.StaticFieldTestI2.StaticReturnFieldI2(a), Run<short>("Mosa.UnitTest.Collection.StaticFieldTestI2.StaticReturnFieldI2", a));
		}

		[Theory]
		[MemberData(nameof(I4))]
		public void StaticReturnFieldI4(int a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.StaticFieldTestI4.StaticReturnFieldI4(a), Run<int>("Mosa.UnitTest.Collection.StaticFieldTestI4.StaticReturnFieldI4", a));
		}

		[Theory]
		[MemberData(nameof(I8))]
		public void StaticReturnFieldI8(long a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.StaticFieldTestI8.StaticReturnFieldI8(a), Run<long>("Mosa.UnitTest.Collection.StaticFieldTestI8.StaticReturnFieldI8", a));
		}

		[Theory]
		[MemberData(nameof(R4))]
		public void StaticReturnFieldR4(float a)
		{
			if (float.IsNaN(a))
				return;

			Assert.Equal(Mosa.UnitTest.Collection.StaticFieldTestR4.StaticReturnFieldR4(a), Run<float>("Mosa.UnitTest.Collection.StaticFieldTestR4.StaticReturnFieldR4", a));
		}

		[Theory]
		[MemberData(nameof(R8))]
		public void StaticReturnFieldR8(double a)
		{
			if (double.IsNaN(a))
				return;

			Assert.Equal(Mosa.UnitTest.Collection.StaticFieldTestR8.StaticReturnFieldR8(a), Run<double>("Mosa.UnitTest.Collection.StaticFieldTestR8.StaticReturnFieldR8", a));
		}

		[Theory]
		[MemberData(nameof(B))]
		public void StaticReturnFieldB(bool a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.StaticFieldTestB.StaticReturnFieldB(a), Run<bool>("Mosa.UnitTest.Collection.StaticFieldTestB.StaticReturnFieldB", a));
		}

		[Theory]
		[MemberData(nameof(C))]
		public void StaticReturnFieldC(char a)
		{
			Assert.Equal(Mosa.UnitTest.Collection.StaticFieldTestC.StaticReturnFieldC(a), Run<char>("Mosa.UnitTest.Collection.StaticFieldTestC.StaticReturnFieldC", a));
		}
	}
}
