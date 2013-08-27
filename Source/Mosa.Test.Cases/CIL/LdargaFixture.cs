using MbUnit.Framework;
using Mosa.Test.System;
using Mosa.Test.System.Numbers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	public class Ldarga : TestCompilerAdapter
	{
		private double DoubleTolerance = 0.000001d;
		private float FloatTolerance = 0.00001f;

		public Ldarga()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		#region CheckValue

		[Test]
		public void LdargaCheckValueU1([U1]byte a)
		{
			Assert.AreEqual(a, Run<byte>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueU1", a));
		}

		[Test]
		public void LdargaCheckValueU2([U2]ushort a)
		{
			Assert.AreEqual(a, Run<ushort>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueU2", a));
		}

		[Test]
		public void LdargaCheckValueU4([U4]uint a)
		{
			Assert.AreEqual(a, Run<uint>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueU4", a));
		}

		[Test]
		public void LdargaCheckValueU8([U8]ulong a)
		{
			Assert.AreEqual(a, Run<ulong>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueU8", a));
		}

		[Test]
		public void LdargaCheckValueI1([I1]sbyte a)
		{
			Assert.AreEqual(a, Run<sbyte>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueI1", a));
		}

		[Test]
		public void LdargaCheckValueI2([I2]short a)
		{
			Assert.AreEqual(a, Run<short>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueI2", a));
		}

		[Test]
		public void LdargaCheckValueI4([I4]int a)
		{
			Assert.AreEqual(a, Run<int>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueI4", a));
		}

		[Test]
		public void LdargaCheckValueI8([I8]long a)
		{
			Assert.AreEqual(a, Run<long>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueI8", a));
		}

		[Test]
		public void LdargaCheckValueC([C]char a)
		{
			Assert.AreEqual(a, Run<char>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueC", a));
		}

		[Test]
		public void LdargaCheckValueB([B]bool a)
		{
			Assert.AreEqual(a, Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueB", a));
		}

		[Test]
		public void LdargaCheckValueR4([R4NumberNoExtremes]float a)
		{
			Assert.AreApproximatelyEqual(a, (float)Run<float>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueR4", a), FloatTolerance);
		}

		[Test]
		public void LdargaCheckValueR8([R8NumberNoExtremes]double a)
		{
			Assert.AreApproximatelyEqual(a, Run<double>("Mosa.Test.Collection", "LdargaTests", "LdargaCheckValueR8", a), DoubleTolerance);
		}

		#endregion CheckValue

		#region ChangeValue

		[Test]
		public void LdargaChangeValueU1([U1]byte a, [U1]byte b)
		{
			Assert.AreEqual(b, Run<byte>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueU1", a, b));
		}

		[Test]
		public void LdargaChangeValueU2([U2]ushort a, [U2]ushort b)
		{
			Assert.AreEqual(b, Run<ushort>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueU2", a, b));
		}

		[Test]
		public void LdargaChangeValueU4([U4]uint a, [U4]uint b)
		{
			Assert.AreEqual(b, Run<uint>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueU4", a, b));
		}

		[Test]
		public void LdargaChangeValueU8([U8]ulong a, [U8]ulong b)
		{
			Assert.AreEqual(b, Run<ulong>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueU8", a, b));
		}

		[Test]
		public void LdargaChangeValueI1([I1]sbyte a, [I1]sbyte b)
		{
			Assert.AreEqual(b, Run<sbyte>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueI1", a, b));
		}

		[Test]
		public void LdargaChangeValueI2([I2]short a, [I2]short b)
		{
			Assert.AreEqual(b, Run<short>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueI2", a, b));
		}

		[Test]
		public void LdargaChangeValueI4([I4]int a, [I4]int b)
		{
			Assert.AreEqual(b, Run<int>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueI4", a, b));
		}

		[Test]
		public void LdargaChangeValueI8([I8]long a, [I8]long b)
		{
			Assert.AreEqual(b, Run<long>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueI8", a, b));
		}

		[Test]
		public void LdargaChangeValueC([C]char a, [C]char b)
		{
			Assert.AreEqual(b, Run<char>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueC", a, b));
		}

		[Test]
		public void LdargaChangeValueB([B]bool a, [B]bool b)
		{
			Assert.AreEqual(b, Run<bool>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueB", a, b));
		}

		[Test]
		public void LdargaChangeValueR4([R4NumberNoExtremes]float a, [R4NumberNoExtremes]float b)
		{
			Assert.AreApproximatelyEqual(b, Run<float>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueR4", a, b), FloatTolerance);
		}

		[Test]
		public void LdargaChangeValueR8([R8NumberNoExtremes]double a, [R8NumberNoExtremes]double b)
		{
			Assert.AreApproximatelyEqual(b, Run<double>("Mosa.Test.Collection", "LdargaTests", "LdargaChangeValueR8", a, b), DoubleTolerance);
		}

		#endregion ChangeValue
	}
}