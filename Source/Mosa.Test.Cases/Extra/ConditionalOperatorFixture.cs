 

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

using Mosa.Test.System;
using Mosa.Test.System.Numbers;
using Mosa.Test.Collection;

namespace Mosa.Test.Cases.Extra
{
	[TestFixture]
	public class ConditionalOperatorFixture : TestCompilerAdapter
	{
		public ConditionalOperatorFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}
				
		[Test]
		public void CompareEqualU1([U1]byte a, [U1]byte b, [U1]byte c, [U1]byte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualU1(a, b, c, d), Run<byte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualU1", a, b, c, d));
		}
				
		[Test]
		public void CompareNotEqualU1([U1]byte a, [U1]byte b, [U1]byte c, [U1]byte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualU1(a, b, c, d), Run<byte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualU1", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanU1([U1]byte a, [U1]byte b, [U1]byte c, [U1]byte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanU1(a, b, c, d), Run<byte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanU1", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanU1([U1]byte a, [U1]byte b, [U1]byte c, [U1]byte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanU1(a, b, c, d), Run<byte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanU1", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU1([U1]byte a, [U1]byte b, [U1]byte c, [U1]byte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualU1(a, b, c, d), Run<byte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualU1", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanOrEqualU1([U1]byte a, [U1]byte b, [U1]byte c, [U1]byte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualU1(a, b, c, d), Run<byte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualU1", a, b, c, d));
		}
				
		[Test]
		public void CompareEqualU2([U2]ushort a, [U2]ushort b, [U2]ushort c, [U2]ushort d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualU2(a, b, c, d), Run<ushort>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualU2", a, b, c, d));
		}
				
		[Test]
		public void CompareNotEqualU2([U2]ushort a, [U2]ushort b, [U2]ushort c, [U2]ushort d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualU2(a, b, c, d), Run<ushort>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualU2", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanU2([U2]ushort a, [U2]ushort b, [U2]ushort c, [U2]ushort d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanU2(a, b, c, d), Run<ushort>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanU2", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanU2([U2]ushort a, [U2]ushort b, [U2]ushort c, [U2]ushort d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanU2(a, b, c, d), Run<ushort>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanU2", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU2([U2]ushort a, [U2]ushort b, [U2]ushort c, [U2]ushort d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualU2(a, b, c, d), Run<ushort>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualU2", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanOrEqualU2([U2]ushort a, [U2]ushort b, [U2]ushort c, [U2]ushort d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualU2(a, b, c, d), Run<ushort>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualU2", a, b, c, d));
		}
				
		[Test]
		public void CompareEqualU4([U4]uint a, [U4]uint b, [U4]uint c, [U4]uint d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualU4(a, b, c, d), Run<uint>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualU4", a, b, c, d));
		}
				
		[Test]
		public void CompareNotEqualU4([U4]uint a, [U4]uint b, [U4]uint c, [U4]uint d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualU4(a, b, c, d), Run<uint>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualU4", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanU4([U4]uint a, [U4]uint b, [U4]uint c, [U4]uint d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanU4(a, b, c, d), Run<uint>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanU4", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanU4([U4]uint a, [U4]uint b, [U4]uint c, [U4]uint d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanU4(a, b, c, d), Run<uint>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanU4", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU4([U4]uint a, [U4]uint b, [U4]uint c, [U4]uint d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualU4(a, b, c, d), Run<uint>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualU4", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanOrEqualU4([U4]uint a, [U4]uint b, [U4]uint c, [U4]uint d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualU4(a, b, c, d), Run<uint>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualU4", a, b, c, d));
		}
				
		[Test]
		public void CompareEqualU8([U8]ulong a, [U8]ulong b, [U8]ulong c, [U8]ulong d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualU8(a, b, c, d), Run<ulong>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualU8", a, b, c, d));
		}
				
		[Test]
		public void CompareNotEqualU8([U8]ulong a, [U8]ulong b, [U8]ulong c, [U8]ulong d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualU8(a, b, c, d), Run<ulong>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualU8", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanU8([U8]ulong a, [U8]ulong b, [U8]ulong c, [U8]ulong d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanU8(a, b, c, d), Run<ulong>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanU8", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanU8([U8]ulong a, [U8]ulong b, [U8]ulong c, [U8]ulong d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanU8(a, b, c, d), Run<ulong>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanU8", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU8([U8]ulong a, [U8]ulong b, [U8]ulong c, [U8]ulong d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualU8(a, b, c, d), Run<ulong>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualU8", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanOrEqualU8([U8]ulong a, [U8]ulong b, [U8]ulong c, [U8]ulong d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualU8(a, b, c, d), Run<ulong>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualU8", a, b, c, d));
		}
				
		[Test]
		public void CompareEqualI1([I1]sbyte a, [I1]sbyte b, [I1]sbyte c, [I1]sbyte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualI1(a, b, c, d), Run<sbyte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualI1", a, b, c, d));
		}
				
		[Test]
		public void CompareNotEqualI1([I1]sbyte a, [I1]sbyte b, [I1]sbyte c, [I1]sbyte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualI1(a, b, c, d), Run<sbyte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualI1", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanI1([I1]sbyte a, [I1]sbyte b, [I1]sbyte c, [I1]sbyte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanI1(a, b, c, d), Run<sbyte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanI1", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanI1([I1]sbyte a, [I1]sbyte b, [I1]sbyte c, [I1]sbyte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanI1(a, b, c, d), Run<sbyte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanI1", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI1([I1]sbyte a, [I1]sbyte b, [I1]sbyte c, [I1]sbyte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualI1(a, b, c, d), Run<sbyte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualI1", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanOrEqualI1([I1]sbyte a, [I1]sbyte b, [I1]sbyte c, [I1]sbyte d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualI1(a, b, c, d), Run<sbyte>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualI1", a, b, c, d));
		}
				
		[Test]
		public void CompareEqualI2([I2]short a, [I2]short b, [I2]short c, [I2]short d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualI2(a, b, c, d), Run<short>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualI2", a, b, c, d));
		}
				
		[Test]
		public void CompareNotEqualI2([I2]short a, [I2]short b, [I2]short c, [I2]short d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualI2(a, b, c, d), Run<short>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualI2", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanI2([I2]short a, [I2]short b, [I2]short c, [I2]short d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanI2(a, b, c, d), Run<short>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanI2", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanI2([I2]short a, [I2]short b, [I2]short c, [I2]short d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanI2(a, b, c, d), Run<short>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanI2", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI2([I2]short a, [I2]short b, [I2]short c, [I2]short d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualI2(a, b, c, d), Run<short>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualI2", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanOrEqualI2([I2]short a, [I2]short b, [I2]short c, [I2]short d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualI2(a, b, c, d), Run<short>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualI2", a, b, c, d));
		}
				
		[Test]
		public void CompareEqualI4([I4]int a, [I4]int b, [I4]int c, [I4]int d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualI4(a, b, c, d), Run<int>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualI4", a, b, c, d));
		}
				
		[Test]
		public void CompareNotEqualI4([I4]int a, [I4]int b, [I4]int c, [I4]int d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualI4(a, b, c, d), Run<int>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualI4", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanI4([I4]int a, [I4]int b, [I4]int c, [I4]int d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanI4(a, b, c, d), Run<int>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanI4", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanI4([I4]int a, [I4]int b, [I4]int c, [I4]int d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanI4(a, b, c, d), Run<int>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanI4", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI4([I4]int a, [I4]int b, [I4]int c, [I4]int d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualI4(a, b, c, d), Run<int>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualI4", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanOrEqualI4([I4]int a, [I4]int b, [I4]int c, [I4]int d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualI4(a, b, c, d), Run<int>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualI4", a, b, c, d));
		}
				
		[Test]
		public void CompareEqualI8([I8]long a, [I8]long b, [I8]long c, [I8]long d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualI8(a, b, c, d), Run<long>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualI8", a, b, c, d));
		}
				
		[Test]
		public void CompareNotEqualI8([I8]long a, [I8]long b, [I8]long c, [I8]long d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualI8(a, b, c, d), Run<long>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualI8", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanI8([I8]long a, [I8]long b, [I8]long c, [I8]long d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanI8(a, b, c, d), Run<long>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanI8", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanI8([I8]long a, [I8]long b, [I8]long c, [I8]long d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanI8(a, b, c, d), Run<long>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanI8", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI8([I8]long a, [I8]long b, [I8]long c, [I8]long d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualI8(a, b, c, d), Run<long>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualI8", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanOrEqualI8([I8]long a, [I8]long b, [I8]long c, [I8]long d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualI8(a, b, c, d), Run<long>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualI8", a, b, c, d));
		}
				
		[Test]
		public void CompareEqualR4([R4]float a, [R4]float b, [R4]float c, [R4]float d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualR4(a, b, c, d), Run<float>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualR4", a, b, c, d));
		}
				
		[Test]
		public void CompareNotEqualR4([R4]float a, [R4]float b, [R4]float c, [R4]float d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualR4(a, b, c, d), Run<float>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualR4", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanR4([R4]float a, [R4]float b, [R4]float c, [R4]float d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanR4(a, b, c, d), Run<float>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanR4", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanR4([R4]float a, [R4]float b, [R4]float c, [R4]float d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanR4(a, b, c, d), Run<float>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanR4", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualR4([R4]float a, [R4]float b, [R4]float c, [R4]float d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualR4(a, b, c, d), Run<float>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualR4", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanOrEqualR4([R4]float a, [R4]float b, [R4]float c, [R4]float d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualR4(a, b, c, d), Run<float>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualR4", a, b, c, d));
		}
				
		[Test]
		public void CompareEqualR8([R8]double a, [R8]double b, [R8]double c, [R8]double d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareEqualR8(a, b, c, d), Run<double>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareEqualR8", a, b, c, d));
		}
				
		[Test]
		public void CompareNotEqualR8([R8]double a, [R8]double b, [R8]double c, [R8]double d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareNotEqualR8(a, b, c, d), Run<double>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareNotEqualR8", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanR8([R8]double a, [R8]double b, [R8]double c, [R8]double d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanR8(a, b, c, d), Run<double>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanR8", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanR8([R8]double a, [R8]double b, [R8]double c, [R8]double d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanR8(a, b, c, d), Run<double>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanR8", a, b, c, d));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualR8([R8]double a, [R8]double b, [R8]double c, [R8]double d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareGreaterThanOrEqualR8(a, b, c, d), Run<double>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareGreaterThanOrEqualR8", a, b, c, d));
		}
				
		[Test]
		public void CompareLessThanOrEqualR8([R8]double a, [R8]double b, [R8]double c, [R8]double d)
		{
			Assert.AreEqual(ConditionalOperatorTests.CompareLessThanOrEqualR8(a, b, c, d), Run<double>("Mosa.Test.Collection", "ConditionalOperatorTests", "CompareLessThanOrEqualR8", a, b, c, d));
		}
		
	}
}
