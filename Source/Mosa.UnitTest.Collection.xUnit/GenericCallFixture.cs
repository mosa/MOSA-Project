// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class GenericCallFixture : TestFixture
	{
		[Theory]
		[MemberData(nameof(U1))]
		public void GenericCallU1(byte value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.GenericCallTests.GenericCallU1", value));
		}

		[Theory]
		[MemberData(nameof(U2))]
		public void GenericCallU2(ushort value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.GenericCallTests.GenericCallU2", value));
		}

		[Theory]
		[MemberData(nameof(U4))]
		public void GenericCallU4(uint value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.GenericCallTests.GenericCallU4", value));
		}

		[Theory]
		[MemberData(nameof(U8))]
		public void GenericCallU8(ulong value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.GenericCallTests.GenericCallU8", value));
		}

		[Theory]
		[MemberData(nameof(I1))]
		public void GenericCallI1(sbyte value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.GenericCallTests.GenericCallI1", value));
		}

		[Theory]
		[MemberData(nameof(I2))]
		public void GenericCallI2(short value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.GenericCallTests.GenericCallI2", value));
		}

		[Theory]
		[MemberData(nameof(I4))]
		public void GenericCallI4(int value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.GenericCallTests.GenericCallI4", value));
		}

		[Theory]
		[MemberData(nameof(I8))]
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
		[MemberData(nameof(C))]
		public void GenericCallC(char value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.GenericCallTests.GenericCallC", value));
		}
	}
}
