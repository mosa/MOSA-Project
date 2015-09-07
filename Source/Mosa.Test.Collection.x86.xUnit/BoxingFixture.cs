// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class BoxingFixture : X86TestFixture
	{
		//private double DoubleTolerance = 0.000001d;
		//private float FloatTolerance = 0.00001f;

		[Theory]
		[MemberData("U1", DisableDiscoveryEnumeration = true)]
		public void BoxU1(byte value)
		{
			Assert.Equal(BoxingTests.BoxU1(value), Run<byte>("Mosa.Test.Collection.BoxingTests.BoxU1", value));
		}

		[Theory]
		[MemberData("U2", DisableDiscoveryEnumeration = true)]
		public void BoxU2(ushort value)
		{
			Assert.Equal(BoxingTests.BoxU2(value), Run<ushort>("Mosa.Test.Collection.BoxingTests.BoxU2", value));
		}

		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void BoxU4(uint value)
		{
			Assert.Equal(BoxingTests.BoxU4(value), Run<uint>("Mosa.Test.Collection.BoxingTests.BoxU4", value));
		}

		[Theory]
		[MemberData("U8", DisableDiscoveryEnumeration = true)]
		public void BoxU8(ulong value)
		{
			Assert.Equal(BoxingTests.BoxU8(value), Run<ulong>("Mosa.Test.Collection.BoxingTests.BoxU8", value));
		}

		[Theory]
		[MemberData("I1", DisableDiscoveryEnumeration = true)]
		public void BoxI1(sbyte value)
		{
			Assert.Equal(BoxingTests.BoxI1(value), Run<sbyte>("Mosa.Test.Collection.BoxingTests.BoxI1", value));
		}

		[Theory]
		[MemberData("I2", DisableDiscoveryEnumeration = true)]
		public void BoxI2(short value)
		{
			Assert.Equal(BoxingTests.BoxI2(value), Run<short>("Mosa.Test.Collection.BoxingTests.BoxI2", value));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void BoxI4(int value)
		{
			Assert.Equal(BoxingTests.BoxI4(value), Run<int>("Mosa.Test.Collection.BoxingTests.BoxI4", value));
		}

		[Theory]
		[MemberData("I8", DisableDiscoveryEnumeration = true)]
		public void BoxI8(long value)
		{
			Assert.Equal(BoxingTests.BoxI8(value), Run<long>("Mosa.Test.Collection.BoxingTests.BoxI8", value));
		}

		//[Theory]
		//public void BoxR4([R4NumberNoExtremes]float value)
		//{
		//	float result = Run<float>("Mosa.Test.Collection.BoxingTests.BoxR4", value);
		//	float expected = BoxingTests.BoxR4(value);

		//	Assert.ApproximatelyEqual(expected, result, FloatTolerance);
		//	Assert.False(float.IsNaN(result), "Returned NaN");
		//}

		//[Theory]
		//public void BoxR8([R8NumberNoExtremes]double value)
		//{
		//	double result = Run<double>("Mosa.Test.Collection.BoxingTests.BoxR8", value);
		//	double expected = BoxingTests.BoxR8(value);

		//	Assert.ApproximatelyEqual(expected, result, DoubleTolerance);
		//	Assert.False(double.IsNaN(result), "Returned NaN");
		//}

		[Theory]
		[MemberData("C", DisableDiscoveryEnumeration = true)]
		public void BoxC(char value)
		{
			Assert.Equal(BoxingTests.BoxC(value), Run<char>("Mosa.Test.Collection.BoxingTests.BoxC", value));
		}

		[Theory]
		[MemberData("U1", DisableDiscoveryEnumeration = true)]
		public void EqualsU1(byte value)
		{
			Assert.Equal(Mosa.Test.Collection.BoxingTests.EqualsU1(value), Run<bool>("Mosa.Test.Collection.BoxingTests.EqualsU1", value));
		}

		[Theory]
		[MemberData("U2", DisableDiscoveryEnumeration = true)]
		public void EqualsU2(ushort value)
		{
			Assert.Equal(Mosa.Test.Collection.BoxingTests.EqualsU2(value), Run<bool>("Mosa.Test.Collection.BoxingTests.EqualsU2", value));
		}

		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void EqualsU4(uint value)
		{
			Assert.Equal(Mosa.Test.Collection.BoxingTests.EqualsU4(value), Run<bool>("Mosa.Test.Collection.BoxingTests.EqualsU4", value));
		}

		[Theory]
		[MemberData("U8", DisableDiscoveryEnumeration = true)]
		public void EqualsU8(ulong value)
		{
			Assert.Equal(Mosa.Test.Collection.BoxingTests.EqualsU8(value), Run<bool>("Mosa.Test.Collection.BoxingTests.EqualsU8", value));
		}

		[Theory]
		[MemberData("I1", DisableDiscoveryEnumeration = true)]
		public void EqualsI1(sbyte value)
		{
			Assert.Equal(Mosa.Test.Collection.BoxingTests.EqualsI1(value), Run<bool>("Mosa.Test.Collection.BoxingTests.EqualsI1", value));
		}

		[Theory]
		[MemberData("I2", DisableDiscoveryEnumeration = true)]
		public void EqualsI2(short value)
		{
			Assert.Equal(Mosa.Test.Collection.BoxingTests.EqualsI2(value), Run<bool>("Mosa.Test.Collection.BoxingTests.EqualsI2", value));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void EqualsI4(int value)
		{
			Assert.Equal(Mosa.Test.Collection.BoxingTests.EqualsI4(value), Run<bool>("Mosa.Test.Collection.BoxingTests.EqualsI4", value));
		}

		[Theory]
		[MemberData("I8", DisableDiscoveryEnumeration = true)]
		public void EqualsI8(long value)
		{
			Assert.Equal(Mosa.Test.Collection.BoxingTests.EqualsI8(value), Run<bool>("Mosa.Test.Collection.BoxingTests.EqualsI8", value));
		}

		[Theory]
		[MemberData("R4NotNaN", DisableDiscoveryEnumeration = true)]
		public void EqualsR4(float value)
		{
			Assert.Equal(Mosa.Test.Collection.BoxingTests.EqualsR4(value), Run<bool>("Mosa.Test.Collection.BoxingTests.EqualsR4", value));
		}

		[Theory]
		[MemberData("R8NotNaN", DisableDiscoveryEnumeration = true)]
		public void EqualsR8(double value)
		{
			Assert.Equal(Mosa.Test.Collection.BoxingTests.EqualsR8(value), Run<bool>("Mosa.Test.Collection.BoxingTests.EqualsR8", value));
		}

		[Theory]
		[MemberData("C", DisableDiscoveryEnumeration = true)]
		public void EqualsC(char value)
		{
			Assert.Equal(Mosa.Test.Collection.BoxingTests.EqualsC(value), Run<bool>("Mosa.Test.Collection.BoxingTests.EqualsC", value));
		}
	}
}
