/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using MbUnit.Framework;
using Mosa.Test.Numbers;
using Mosa.Test.System;

namespace Mosa.Test.Collection.MbUnit
{
	[TestFixture]
	public class BoxingFixture : TestCompilerAdapter
	{
		private double DoubleTolerance = 0.000001d;
		private float FloatTolerance = 0.00001f;

		public BoxingFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void BoxU1([U1]byte value)
		{
			Assert.AreEqual(BoxingTests.BoxU1(value), Run<byte>("Mosa.Test.Collection", "BoxingTests", "BoxU1", value));
		}

		[Test]
		public void BoxU2([U2]ushort value)
		{
			Assert.AreEqual(BoxingTests.BoxU2(value), Run<ushort>("Mosa.Test.Collection", "BoxingTests", "BoxU2", value));
		}

		[Test]
		public void BoxU4([U4]uint value)
		{
			Assert.AreEqual(BoxingTests.BoxU4(value), Run<uint>("Mosa.Test.Collection", "BoxingTests", "BoxU4", value));
		}

		[Test]
		public void BoxU8([U8]ulong value)
		{
			Assert.AreEqual(BoxingTests.BoxU8(value), Run<ulong>("Mosa.Test.Collection", "BoxingTests", "BoxU8", value));
		}

		[Test]
		public void BoxI1([I1]sbyte value)
		{
			Assert.AreEqual(BoxingTests.BoxI1(value), Run<sbyte>("Mosa.Test.Collection", "BoxingTests", "BoxI1", value));
		}

		[Test]
		public void BoxI2([I2]short value)
		{
			Assert.AreEqual(BoxingTests.BoxI2(value), Run<short>("Mosa.Test.Collection", "BoxingTests", "BoxI2", value));
		}

		[Test]
		public void BoxI4([I4]int value)
		{
			Assert.AreEqual(BoxingTests.BoxI4(value), Run<int>("Mosa.Test.Collection", "BoxingTests", "BoxI4", value));
		}

		[Test]
		public void BoxI8([I8]long value)
		{
			Assert.AreEqual(BoxingTests.BoxI8(value), Run<long>("Mosa.Test.Collection", "BoxingTests", "BoxI8", value));
		}

		[Test]
		public void BoxR4([R4NumberNoExtremes]float value)
		{
			float result = Run<float>("Mosa.Test.Collection", "BoxingTests", "BoxR4", value);
			float expected = BoxingTests.BoxR4(value);

			Assert.AreApproximatelyEqual(expected, result, FloatTolerance);
			Assert.IsFalse(float.IsNaN(result), "Returned NaN");
		}

		[Test]
		public void BoxR8([R8NumberNoExtremes]double value)
		{
			double result = Run<double>("Mosa.Test.Collection", "BoxingTests", "BoxR8", value);
			double expected = BoxingTests.BoxR8(value);

			Assert.AreApproximatelyEqual(expected, result, DoubleTolerance);
			Assert.IsFalse(double.IsNaN(result), "Returned NaN");
		}

		[Test]
		public void BoxC([C]char value)
		{
			Assert.AreEqual(BoxingTests.BoxC(value), Run<char>("Mosa.Test.Collection", "BoxingTests", "BoxC", value));
		}
	}
}