/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *
 */

using Mosa.Test.Collection;
using Xunit;
using Xunit.Extensions;

namespace Mosa.TinyCPUSimulator.TestSystem.xUnit
{
	public class BoxingFixture : TestFixture
	{
		private double DoubleTolerance = 0.000001d;
		private float FloatTolerance = 0.00001f;

		[Theory]
		[PropertyData("U1")]
		public void BoxU1(byte value)
		{
			Assert.Equal(BoxingTests.BoxU1(value), Run<byte>("Mosa.Test.Collection.BoxingTests.BoxU1", value));
		}

		[Theory]
		[PropertyData("U2")]
		public void BoxU2(ushort value)
		{
			Assert.Equal(BoxingTests.BoxU2(value), Run<ushort>("Mosa.Test.Collection.BoxingTests.BoxU2", value));
		}

		[Theory]
		[PropertyData("U4")]
		public void BoxU4(uint value)
		{
			Assert.Equal(BoxingTests.BoxU4(value), Run<uint>("Mosa.Test.Collection.BoxingTests.BoxU4", value));
		}

		[Theory]
		[PropertyData("U8")]
		public void BoxU8(ulong value)
		{
			Assert.Equal(BoxingTests.BoxU8(value), Run<ulong>("Mosa.Test.Collection.BoxingTests.BoxU8", value));
		}

		[Theory]
		[PropertyData("I1")]
		public void BoxI1(sbyte value)
		{
			Assert.Equal(BoxingTests.BoxI1(value), Run<sbyte>("Mosa.Test.Collection.BoxingTests.BoxI1", value));
		}

		[Theory]
		[PropertyData("I2")]
		public void BoxI2(short value)
		{
			Assert.Equal(BoxingTests.BoxI2(value), Run<short>("Mosa.Test.Collection.BoxingTests.BoxI2", value));
		}

		[Theory]
		[PropertyData("I4")]
		public void BoxI4(int value)
		{
			Assert.Equal(BoxingTests.BoxI4(value), Run<int>("Mosa.Test.Collection.BoxingTests.BoxI4", value));
		}

		[Theory]
		[PropertyData("I8")]
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
		[PropertyData("C")]
		public void BoxC(char value)
		{
			Assert.Equal(BoxingTests.BoxC(value), Run<char>("Mosa.Test.Collection.BoxingTests.BoxC", value));
		}
	}
}