/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Ki (kiootic) <kiootic@gmail.com>
 */

using Mosa.Test.Collection;
using Xunit;
using Xunit.Extensions;

namespace Mosa.TinyCPUSimulator.TestSystem.xUnit
{
	public class GenericFixture : TestFixture
	{
		[Theory]
		[PropertyData("U1")]
		public void GenericCallU1(byte value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.GenericCallTests.GenericCallU1", value));
		}

		[Theory]
		[PropertyData("U2")]
		public void GenericCallU2(ushort value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.GenericCallTests.GenericCallU2", value));
		}

		[Theory]
		[PropertyData("U4")]
		public void GenericCallU4(uint value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.GenericCallTests.GenericCallU4", value));
		}

		[Theory]
		[PropertyData("U8")]
		public void GenericCallU8(ulong value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.GenericCallTests.GenericCallU8", value));
		}

		[Theory]
		[PropertyData("I1")]
		public void GenericCallI1(sbyte value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.GenericCallTests.GenericCallI1", value));
		}

		[Theory]
		[PropertyData("I2")]
		public void GenericCallI2(short value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.GenericCallTests.GenericCallI2", value));
		}

		[Theory]
		[PropertyData("I4")]
		public void GenericCallI4(int value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.GenericCallTests.GenericCallI4", value));
		}

		[Theory]
		[PropertyData("I8")]
		public void GenericCallI8(long value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.GenericCallTests.GenericCallI8", value));
		}

		//[Theory]
		//public void GenericCallR4([R4NumberNoExtremes]float value)
		//{
		//	float result = Run<float>("Mosa.Test.Collection.GenericCallingTests.BoxR4", value);
		//	float expected = GenericCallingTests.BoxR4(value);

		//	Assert.ApproximatelyEqual(expected, result, FloatTolerance);
		//	Assert.False(float.IsNaN(result), "Returned NaN");
		//}

		//[Theory]
		//public void GenericCallR8([R8NumberNoExtremes]double value)
		//{
		//	double result = Run<double>("Mosa.Test.Collection.GenericCallingTests.BoxR8", value);
		//	double expected = GenericCallingTests.BoxR8(value);

		//	Assert.ApproximatelyEqual(expected, result, DoubleTolerance);
		//	Assert.False(double.IsNaN(result), "Returned NaN");
		//}

		[Theory]
		[PropertyData("C")]
		public void GenericCallC(char value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.GenericCallTests.GenericCallC", value));
		}

		[Fact]
		public void GenericMixed()
		{
			Assert.Equal(0x7fffu, Run<uint>("Mosa.Test.Collection.GenericMixedTest.GenericMixed"));
		}
	}
}