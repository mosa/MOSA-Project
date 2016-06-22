// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTest.Collection;
using Xunit;
using Xunit.Extensions;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class GenericCallFixture : TestFixture
	{
		[Theory]
		[MemberData("U1", DisableDiscoveryEnumeration = true)]
		public void GenericCallU1(byte value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.GenericCallTests.GenericCallU1", value));
		}

		[Theory]
		[MemberData("U2", DisableDiscoveryEnumeration = true)]
		public void GenericCallU2(ushort value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.GenericCallTests.GenericCallU2", value));
		}

		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void GenericCallU4(uint value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.GenericCallTests.GenericCallU4", value));
		}

		[Theory]
		[MemberData("U8", DisableDiscoveryEnumeration = true)]
		public void GenericCallU8(ulong value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.GenericCallTests.GenericCallU8", value));
		}

		[Theory]
		[MemberData("I1", DisableDiscoveryEnumeration = true)]
		public void GenericCallI1(sbyte value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.GenericCallTests.GenericCallI1", value));
		}

		[Theory]
		[MemberData("I2", DisableDiscoveryEnumeration = true)]
		public void GenericCallI2(short value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.GenericCallTests.GenericCallI2", value));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void GenericCallI4(int value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.GenericCallTests.GenericCallI4", value));
		}

		[Theory]
		[MemberData("I8", DisableDiscoveryEnumeration = true)]
		public void GenericCallI8(long value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.GenericCallTests.GenericCallI8", value));
		}

		//[Theory]
		//public void GenericCallR4([R4NumberNoExtremes]float value)
		//{
		//	float result = Run<float>("Mosa.UnitTest.Collection.GenericCallingTests.BoxR4", value);
		//	float expected = GenericCallingTests.BoxR4(value);

		//	Assert.ApproximatelyEqual(expected, result, FloatTolerance);
		//	Assert.False(float.IsNaN(result), "Returned NaN");
		//}

		//[Theory]
		//public void GenericCallR8([R8NumberNoExtremes]double value)
		//{
		//	double result = Run<double>("Mosa.UnitTest.Collection.GenericCallingTests.BoxR8", value);
		//	double expected = GenericCallingTests.BoxR8(value);

		//	Assert.ApproximatelyEqual(expected, result, DoubleTolerance);
		//	Assert.False(double.IsNaN(result), "Returned NaN");
		//}

		[Theory]
		[MemberData("C", DisableDiscoveryEnumeration = true)]
		public void GenericCallC(char value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.GenericCallTests.GenericCallC", value));
		}
	}
}
