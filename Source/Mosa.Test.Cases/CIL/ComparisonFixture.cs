 

using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

using Mosa.Test.System;
using Mosa.Test.System.Numbers;
using Mosa.Test.Collection;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	public class ComparisonFixture : TestCompilerAdapter
	{
		public ComparisonFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}
				
		[Test]
		public void CompareEqualU1U1([U1]byte a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU1U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU1U1", a, b));
		}
				
		[Test]
		public void CompareNotEqualU1U1([U1]byte a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU1U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU1U1", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU1U1([U1]byte a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU1U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU1U1", a, b));
		}
				
		[Test]
		public void CompareLessThanU1U1([U1]byte a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU1U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU1U1", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU1U1([U1]byte a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU1U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU1U1", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU1U1([U1]byte a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU1U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU1U1", a, b));
		}
				
		[Test]
		public void CompareEqualU1U2([U1]byte a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU1U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU1U2", a, b));
		}
				
		[Test]
		public void CompareNotEqualU1U2([U1]byte a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU1U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU1U2", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU1U2([U1]byte a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU1U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU1U2", a, b));
		}
				
		[Test]
		public void CompareLessThanU1U2([U1]byte a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU1U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU1U2", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU1U2([U1]byte a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU1U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU1U2", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU1U2([U1]byte a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU1U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU1U2", a, b));
		}
				
		[Test]
		public void CompareEqualU1U4([U1]byte a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU1U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU1U4", a, b));
		}
				
		[Test]
		public void CompareNotEqualU1U4([U1]byte a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU1U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU1U4", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU1U4([U1]byte a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU1U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU1U4", a, b));
		}
				
		[Test]
		public void CompareLessThanU1U4([U1]byte a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU1U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU1U4", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU1U4([U1]byte a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU1U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU1U4", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU1U4([U1]byte a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU1U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU1U4", a, b));
		}
				
		[Test]
		public void CompareEqualU1U8([U1]byte a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU1U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU1U8", a, b));
		}
				
		[Test]
		public void CompareNotEqualU1U8([U1]byte a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU1U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU1U8", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU1U8([U1]byte a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU1U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU1U8", a, b));
		}
				
		[Test]
		public void CompareLessThanU1U8([U1]byte a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU1U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU1U8", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU1U8([U1]byte a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU1U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU1U8", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU1U8([U1]byte a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU1U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU1U8", a, b));
		}
				
		[Test]
		public void CompareEqualU1C([U1]byte a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU1C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU1C", a, b));
		}
				
		[Test]
		public void CompareNotEqualU1C([U1]byte a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU1C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU1C", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU1C([U1]byte a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU1C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU1C", a, b));
		}
				
		[Test]
		public void CompareLessThanU1C([U1]byte a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU1C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU1C", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU1C([U1]byte a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU1C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU1C", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU1C([U1]byte a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU1C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU1C", a, b));
		}
				
		[Test]
		public void CompareEqualU2U1([U2]ushort a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU2U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU2U1", a, b));
		}
				
		[Test]
		public void CompareNotEqualU2U1([U2]ushort a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU2U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU2U1", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU2U1([U2]ushort a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU2U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU2U1", a, b));
		}
				
		[Test]
		public void CompareLessThanU2U1([U2]ushort a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU2U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU2U1", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU2U1([U2]ushort a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU2U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU2U1", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU2U1([U2]ushort a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU2U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU2U1", a, b));
		}
				
		[Test]
		public void CompareEqualU2U2([U2]ushort a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU2U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU2U2", a, b));
		}
				
		[Test]
		public void CompareNotEqualU2U2([U2]ushort a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU2U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU2U2", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU2U2([U2]ushort a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU2U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU2U2", a, b));
		}
				
		[Test]
		public void CompareLessThanU2U2([U2]ushort a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU2U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU2U2", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU2U2([U2]ushort a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU2U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU2U2", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU2U2([U2]ushort a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU2U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU2U2", a, b));
		}
				
		[Test]
		public void CompareEqualU2U4([U2]ushort a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU2U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU2U4", a, b));
		}
				
		[Test]
		public void CompareNotEqualU2U4([U2]ushort a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU2U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU2U4", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU2U4([U2]ushort a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU2U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU2U4", a, b));
		}
				
		[Test]
		public void CompareLessThanU2U4([U2]ushort a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU2U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU2U4", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU2U4([U2]ushort a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU2U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU2U4", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU2U4([U2]ushort a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU2U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU2U4", a, b));
		}
				
		[Test]
		public void CompareEqualU2U8([U2]ushort a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU2U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU2U8", a, b));
		}
				
		[Test]
		public void CompareNotEqualU2U8([U2]ushort a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU2U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU2U8", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU2U8([U2]ushort a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU2U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU2U8", a, b));
		}
				
		[Test]
		public void CompareLessThanU2U8([U2]ushort a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU2U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU2U8", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU2U8([U2]ushort a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU2U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU2U8", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU2U8([U2]ushort a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU2U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU2U8", a, b));
		}
				
		[Test]
		public void CompareEqualU2C([U2]ushort a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU2C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU2C", a, b));
		}
				
		[Test]
		public void CompareNotEqualU2C([U2]ushort a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU2C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU2C", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU2C([U2]ushort a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU2C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU2C", a, b));
		}
				
		[Test]
		public void CompareLessThanU2C([U2]ushort a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU2C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU2C", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU2C([U2]ushort a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU2C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU2C", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU2C([U2]ushort a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU2C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU2C", a, b));
		}
				
		[Test]
		public void CompareEqualU4U1([U4]uint a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU4U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU4U1", a, b));
		}
				
		[Test]
		public void CompareNotEqualU4U1([U4]uint a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU4U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU4U1", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU4U1([U4]uint a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU4U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU4U1", a, b));
		}
				
		[Test]
		public void CompareLessThanU4U1([U4]uint a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU4U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU4U1", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU4U1([U4]uint a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU4U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU4U1", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU4U1([U4]uint a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU4U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU4U1", a, b));
		}
				
		[Test]
		public void CompareEqualU4U2([U4]uint a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU4U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU4U2", a, b));
		}
				
		[Test]
		public void CompareNotEqualU4U2([U4]uint a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU4U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU4U2", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU4U2([U4]uint a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU4U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU4U2", a, b));
		}
				
		[Test]
		public void CompareLessThanU4U2([U4]uint a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU4U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU4U2", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU4U2([U4]uint a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU4U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU4U2", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU4U2([U4]uint a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU4U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU4U2", a, b));
		}
				
		[Test]
		public void CompareEqualU4U4([U4]uint a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU4U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU4U4", a, b));
		}
				
		[Test]
		public void CompareNotEqualU4U4([U4]uint a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU4U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU4U4", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU4U4([U4]uint a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU4U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU4U4", a, b));
		}
				
		[Test]
		public void CompareLessThanU4U4([U4]uint a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU4U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU4U4", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU4U4([U4]uint a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU4U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU4U4", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU4U4([U4]uint a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU4U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU4U4", a, b));
		}
				
		[Test]
		public void CompareEqualU4U8([U4]uint a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU4U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU4U8", a, b));
		}
				
		[Test]
		public void CompareNotEqualU4U8([U4]uint a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU4U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU4U8", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU4U8([U4]uint a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU4U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU4U8", a, b));
		}
				
		[Test]
		public void CompareLessThanU4U8([U4]uint a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU4U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU4U8", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU4U8([U4]uint a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU4U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU4U8", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU4U8([U4]uint a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU4U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU4U8", a, b));
		}
				
		[Test]
		public void CompareEqualU4C([U4]uint a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU4C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU4C", a, b));
		}
				
		[Test]
		public void CompareNotEqualU4C([U4]uint a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU4C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU4C", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU4C([U4]uint a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU4C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU4C", a, b));
		}
				
		[Test]
		public void CompareLessThanU4C([U4]uint a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU4C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU4C", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU4C([U4]uint a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU4C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU4C", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU4C([U4]uint a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU4C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU4C", a, b));
		}
				
		[Test]
		public void CompareEqualU8U1([U8]ulong a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU8U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU8U1", a, b));
		}
				
		[Test]
		public void CompareNotEqualU8U1([U8]ulong a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU8U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU8U1", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU8U1([U8]ulong a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU8U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU8U1", a, b));
		}
				
		[Test]
		public void CompareLessThanU8U1([U8]ulong a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU8U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU8U1", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU8U1([U8]ulong a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU8U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU8U1", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU8U1([U8]ulong a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU8U1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU8U1", a, b));
		}
				
		[Test]
		public void CompareEqualU8U2([U8]ulong a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU8U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU8U2", a, b));
		}
				
		[Test]
		public void CompareNotEqualU8U2([U8]ulong a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU8U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU8U2", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU8U2([U8]ulong a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU8U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU8U2", a, b));
		}
				
		[Test]
		public void CompareLessThanU8U2([U8]ulong a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU8U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU8U2", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU8U2([U8]ulong a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU8U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU8U2", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU8U2([U8]ulong a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU8U2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU8U2", a, b));
		}
				
		[Test]
		public void CompareEqualU8U4([U8]ulong a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU8U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU8U4", a, b));
		}
				
		[Test]
		public void CompareNotEqualU8U4([U8]ulong a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU8U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU8U4", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU8U4([U8]ulong a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU8U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU8U4", a, b));
		}
				
		[Test]
		public void CompareLessThanU8U4([U8]ulong a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU8U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU8U4", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU8U4([U8]ulong a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU8U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU8U4", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU8U4([U8]ulong a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU8U4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU8U4", a, b));
		}
				
		[Test]
		public void CompareEqualU8U8([U8]ulong a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU8U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU8U8", a, b));
		}
				
		[Test]
		public void CompareNotEqualU8U8([U8]ulong a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU8U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU8U8", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU8U8([U8]ulong a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU8U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU8U8", a, b));
		}
				
		[Test]
		public void CompareLessThanU8U8([U8]ulong a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU8U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU8U8", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU8U8([U8]ulong a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU8U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU8U8", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU8U8([U8]ulong a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU8U8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU8U8", a, b));
		}
				
		[Test]
		public void CompareEqualU8C([U8]ulong a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualU8C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualU8C", a, b));
		}
				
		[Test]
		public void CompareNotEqualU8C([U8]ulong a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualU8C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualU8C", a, b));
		}
				
		[Test]
		public void CompareGreaterThanU8C([U8]ulong a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanU8C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanU8C", a, b));
		}
				
		[Test]
		public void CompareLessThanU8C([U8]ulong a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanU8C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanU8C", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualU8C([U8]ulong a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualU8C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualU8C", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualU8C([U8]ulong a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualU8C(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualU8C", a, b));
		}
				
		[Test]
		public void CompareEqualCU1([C]char a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualCU1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualCU1", a, b));
		}
				
		[Test]
		public void CompareNotEqualCU1([C]char a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualCU1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualCU1", a, b));
		}
				
		[Test]
		public void CompareGreaterThanCU1([C]char a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanCU1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanCU1", a, b));
		}
				
		[Test]
		public void CompareLessThanCU1([C]char a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanCU1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanCU1", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualCU1([C]char a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualCU1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualCU1", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualCU1([C]char a, [U1]byte b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualCU1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualCU1", a, b));
		}
				
		[Test]
		public void CompareEqualCU2([C]char a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualCU2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualCU2", a, b));
		}
				
		[Test]
		public void CompareNotEqualCU2([C]char a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualCU2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualCU2", a, b));
		}
				
		[Test]
		public void CompareGreaterThanCU2([C]char a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanCU2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanCU2", a, b));
		}
				
		[Test]
		public void CompareLessThanCU2([C]char a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanCU2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanCU2", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualCU2([C]char a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualCU2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualCU2", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualCU2([C]char a, [U2]ushort b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualCU2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualCU2", a, b));
		}
				
		[Test]
		public void CompareEqualCU4([C]char a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualCU4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualCU4", a, b));
		}
				
		[Test]
		public void CompareNotEqualCU4([C]char a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualCU4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualCU4", a, b));
		}
				
		[Test]
		public void CompareGreaterThanCU4([C]char a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanCU4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanCU4", a, b));
		}
				
		[Test]
		public void CompareLessThanCU4([C]char a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanCU4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanCU4", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualCU4([C]char a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualCU4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualCU4", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualCU4([C]char a, [U4]uint b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualCU4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualCU4", a, b));
		}
				
		[Test]
		public void CompareEqualCU8([C]char a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualCU8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualCU8", a, b));
		}
				
		[Test]
		public void CompareNotEqualCU8([C]char a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualCU8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualCU8", a, b));
		}
				
		[Test]
		public void CompareGreaterThanCU8([C]char a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanCU8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanCU8", a, b));
		}
				
		[Test]
		public void CompareLessThanCU8([C]char a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanCU8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanCU8", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualCU8([C]char a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualCU8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualCU8", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualCU8([C]char a, [U8]ulong b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualCU8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualCU8", a, b));
		}
				
		[Test]
		public void CompareEqualCC([C]char a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualCC(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualCC", a, b));
		}
				
		[Test]
		public void CompareNotEqualCC([C]char a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualCC(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualCC", a, b));
		}
				
		[Test]
		public void CompareGreaterThanCC([C]char a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanCC(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanCC", a, b));
		}
				
		[Test]
		public void CompareLessThanCC([C]char a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanCC(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanCC", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualCC([C]char a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualCC(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualCC", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualCC([C]char a, [C]char b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualCC(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualCC", a, b));
		}
				
				
		[Test]
		public void CompareEqualI1I1([I1]sbyte a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualI1I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualI1I1", a, b));
		}
				
		[Test]
		public void CompareNotEqualI1I1([I1]sbyte a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualI1I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualI1I1", a, b));
		}
				
		[Test]
		public void CompareGreaterThanI1I1([I1]sbyte a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanI1I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanI1I1", a, b));
		}
				
		[Test]
		public void CompareLessThanI1I1([I1]sbyte a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanI1I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanI1I1", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI1I1([I1]sbyte a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualI1I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualI1I1", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualI1I1([I1]sbyte a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualI1I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualI1I1", a, b));
		}
				
		[Test]
		public void CompareEqualI1I2([I1]sbyte a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualI1I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualI1I2", a, b));
		}
				
		[Test]
		public void CompareNotEqualI1I2([I1]sbyte a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualI1I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualI1I2", a, b));
		}
				
		[Test]
		public void CompareGreaterThanI1I2([I1]sbyte a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanI1I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanI1I2", a, b));
		}
				
		[Test]
		public void CompareLessThanI1I2([I1]sbyte a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanI1I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanI1I2", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI1I2([I1]sbyte a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualI1I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualI1I2", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualI1I2([I1]sbyte a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualI1I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualI1I2", a, b));
		}
				
		[Test]
		public void CompareEqualI1I4([I1]sbyte a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualI1I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualI1I4", a, b));
		}
				
		[Test]
		public void CompareNotEqualI1I4([I1]sbyte a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualI1I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualI1I4", a, b));
		}
				
		[Test]
		public void CompareGreaterThanI1I4([I1]sbyte a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanI1I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanI1I4", a, b));
		}
				
		[Test]
		public void CompareLessThanI1I4([I1]sbyte a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanI1I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanI1I4", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI1I4([I1]sbyte a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualI1I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualI1I4", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualI1I4([I1]sbyte a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualI1I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualI1I4", a, b));
		}
				
		[Test]
		public void CompareEqualI1I8([I1]sbyte a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualI1I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualI1I8", a, b));
		}
				
		[Test]
		public void CompareNotEqualI1I8([I1]sbyte a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualI1I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualI1I8", a, b));
		}
				
		[Test]
		public void CompareGreaterThanI1I8([I1]sbyte a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanI1I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanI1I8", a, b));
		}
				
		[Test]
		public void CompareLessThanI1I8([I1]sbyte a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanI1I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanI1I8", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI1I8([I1]sbyte a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualI1I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualI1I8", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualI1I8([I1]sbyte a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualI1I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualI1I8", a, b));
		}
				
		[Test]
		public void CompareEqualI2I1([I2]short a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualI2I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualI2I1", a, b));
		}
				
		[Test]
		public void CompareNotEqualI2I1([I2]short a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualI2I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualI2I1", a, b));
		}
				
		[Test]
		public void CompareGreaterThanI2I1([I2]short a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanI2I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanI2I1", a, b));
		}
				
		[Test]
		public void CompareLessThanI2I1([I2]short a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanI2I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanI2I1", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI2I1([I2]short a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualI2I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualI2I1", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualI2I1([I2]short a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualI2I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualI2I1", a, b));
		}
				
		[Test]
		public void CompareEqualI2I2([I2]short a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualI2I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualI2I2", a, b));
		}
				
		[Test]
		public void CompareNotEqualI2I2([I2]short a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualI2I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualI2I2", a, b));
		}
				
		[Test]
		public void CompareGreaterThanI2I2([I2]short a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanI2I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanI2I2", a, b));
		}
				
		[Test]
		public void CompareLessThanI2I2([I2]short a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanI2I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanI2I2", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI2I2([I2]short a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualI2I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualI2I2", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualI2I2([I2]short a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualI2I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualI2I2", a, b));
		}
				
		[Test]
		public void CompareEqualI2I4([I2]short a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualI2I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualI2I4", a, b));
		}
				
		[Test]
		public void CompareNotEqualI2I4([I2]short a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualI2I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualI2I4", a, b));
		}
				
		[Test]
		public void CompareGreaterThanI2I4([I2]short a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanI2I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanI2I4", a, b));
		}
				
		[Test]
		public void CompareLessThanI2I4([I2]short a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanI2I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanI2I4", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI2I4([I2]short a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualI2I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualI2I4", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualI2I4([I2]short a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualI2I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualI2I4", a, b));
		}
				
		[Test]
		public void CompareEqualI2I8([I2]short a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualI2I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualI2I8", a, b));
		}
				
		[Test]
		public void CompareNotEqualI2I8([I2]short a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualI2I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualI2I8", a, b));
		}
				
		[Test]
		public void CompareGreaterThanI2I8([I2]short a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanI2I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanI2I8", a, b));
		}
				
		[Test]
		public void CompareLessThanI2I8([I2]short a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanI2I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanI2I8", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI2I8([I2]short a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualI2I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualI2I8", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualI2I8([I2]short a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualI2I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualI2I8", a, b));
		}
				
		[Test]
		public void CompareEqualI4I1([I4]int a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualI4I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualI4I1", a, b));
		}
				
		[Test]
		public void CompareNotEqualI4I1([I4]int a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualI4I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualI4I1", a, b));
		}
				
		[Test]
		public void CompareGreaterThanI4I1([I4]int a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanI4I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanI4I1", a, b));
		}
				
		[Test]
		public void CompareLessThanI4I1([I4]int a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanI4I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanI4I1", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI4I1([I4]int a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualI4I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualI4I1", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualI4I1([I4]int a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualI4I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualI4I1", a, b));
		}
				
		[Test]
		public void CompareEqualI4I2([I4]int a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualI4I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualI4I2", a, b));
		}
				
		[Test]
		public void CompareNotEqualI4I2([I4]int a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualI4I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualI4I2", a, b));
		}
				
		[Test]
		public void CompareGreaterThanI4I2([I4]int a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanI4I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanI4I2", a, b));
		}
				
		[Test]
		public void CompareLessThanI4I2([I4]int a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanI4I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanI4I2", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI4I2([I4]int a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualI4I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualI4I2", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualI4I2([I4]int a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualI4I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualI4I2", a, b));
		}
				
		[Test]
		public void CompareEqualI4I4([I4]int a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualI4I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualI4I4", a, b));
		}
				
		[Test]
		public void CompareNotEqualI4I4([I4]int a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualI4I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualI4I4", a, b));
		}
				
		[Test]
		public void CompareGreaterThanI4I4([I4]int a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanI4I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanI4I4", a, b));
		}
				
		[Test]
		public void CompareLessThanI4I4([I4]int a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanI4I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanI4I4", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI4I4([I4]int a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualI4I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualI4I4", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualI4I4([I4]int a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualI4I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualI4I4", a, b));
		}
				
		[Test]
		public void CompareEqualI4I8([I4]int a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualI4I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualI4I8", a, b));
		}
				
		[Test]
		public void CompareNotEqualI4I8([I4]int a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualI4I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualI4I8", a, b));
		}
				
		[Test]
		public void CompareGreaterThanI4I8([I4]int a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanI4I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanI4I8", a, b));
		}
				
		[Test]
		public void CompareLessThanI4I8([I4]int a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanI4I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanI4I8", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI4I8([I4]int a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualI4I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualI4I8", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualI4I8([I4]int a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualI4I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualI4I8", a, b));
		}
				
		[Test]
		public void CompareEqualI8I1([I8]long a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualI8I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualI8I1", a, b));
		}
				
		[Test]
		public void CompareNotEqualI8I1([I8]long a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualI8I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualI8I1", a, b));
		}
				
		[Test]
		public void CompareGreaterThanI8I1([I8]long a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanI8I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanI8I1", a, b));
		}
				
		[Test]
		public void CompareLessThanI8I1([I8]long a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanI8I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanI8I1", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI8I1([I8]long a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualI8I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualI8I1", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualI8I1([I8]long a, [I1]sbyte b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualI8I1(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualI8I1", a, b));
		}
				
		[Test]
		public void CompareEqualI8I2([I8]long a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualI8I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualI8I2", a, b));
		}
				
		[Test]
		public void CompareNotEqualI8I2([I8]long a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualI8I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualI8I2", a, b));
		}
				
		[Test]
		public void CompareGreaterThanI8I2([I8]long a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanI8I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanI8I2", a, b));
		}
				
		[Test]
		public void CompareLessThanI8I2([I8]long a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanI8I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanI8I2", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI8I2([I8]long a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualI8I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualI8I2", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualI8I2([I8]long a, [I2]short b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualI8I2(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualI8I2", a, b));
		}
				
		[Test]
		public void CompareEqualI8I4([I8]long a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualI8I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualI8I4", a, b));
		}
				
		[Test]
		public void CompareNotEqualI8I4([I8]long a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualI8I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualI8I4", a, b));
		}
				
		[Test]
		public void CompareGreaterThanI8I4([I8]long a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanI8I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanI8I4", a, b));
		}
				
		[Test]
		public void CompareLessThanI8I4([I8]long a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanI8I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanI8I4", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI8I4([I8]long a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualI8I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualI8I4", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualI8I4([I8]long a, [I4]int b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualI8I4(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualI8I4", a, b));
		}
				
		[Test]
		public void CompareEqualI8I8([I8]long a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareEqualI8I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareEqualI8I8", a, b));
		}
				
		[Test]
		public void CompareNotEqualI8I8([I8]long a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareNotEqualI8I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareNotEqualI8I8", a, b));
		}
				
		[Test]
		public void CompareGreaterThanI8I8([I8]long a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanI8I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanI8I8", a, b));
		}
				
		[Test]
		public void CompareLessThanI8I8([I8]long a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanI8I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanI8I8", a, b));
		}
				
		[Test]
		public void CompareGreaterThanOrEqualI8I8([I8]long a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareGreaterThanOrEqualI8I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareGreaterThanOrEqualI8I8", a, b));
		}
				
		[Test]
		public void CompareLessThanOrEqualI8I8([I8]long a, [I8]long b)
		{
			Assert.AreEqual(ComparisonTests.CompareLessThanOrEqualI8I8(a, b), Run<bool>("Mosa.Test.Collection", "ComparisonTests", "CompareLessThanOrEqualI8I8", a, b));
		}
			}
}
