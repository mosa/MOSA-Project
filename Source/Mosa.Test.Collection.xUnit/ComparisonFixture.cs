// Copyright (c) MOSA Project. Licensed under the New BSD License.


// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.xUnit
{
	public class ComparisonFixture : TestFixture
	{
				
		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void CompareEqualU1(byte a, byte b)
		{
			Assert.Equal(ComparisonTests.CompareEqualU1(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareEqualU1", a, b));
		}
				
		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void CompareNotEqualU1(byte a, byte b)
		{
			Assert.Equal(ComparisonTests.CompareNotEqualU1(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareNotEqualU1", a, b));
		}
				
		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void CompareGreaterThanU1(byte a, byte b)
		{
			Assert.Equal(ComparisonTests.CompareGreaterThanU1(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareGreaterThanU1", a, b));
		}
				
		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void CompareLessThanU1(byte a, byte b)
		{
			Assert.Equal(ComparisonTests.CompareLessThanU1(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareLessThanU1", a, b));
		}
				
		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void CompareGreaterThanOrEqualU1(byte a, byte b)
		{
			Assert.Equal(ComparisonTests.CompareGreaterThanOrEqualU1(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareGreaterThanOrEqualU1", a, b));
		}
				
		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void CompareLessThanOrEqualU1(byte a, byte b)
		{
			Assert.Equal(ComparisonTests.CompareLessThanOrEqualU1(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareLessThanOrEqualU1", a, b));
		}
				
		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void CompareEqualU2(ushort a, ushort b)
		{
			Assert.Equal(ComparisonTests.CompareEqualU2(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareEqualU2", a, b));
		}
				
		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void CompareNotEqualU2(ushort a, ushort b)
		{
			Assert.Equal(ComparisonTests.CompareNotEqualU2(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareNotEqualU2", a, b));
		}
				
		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void CompareGreaterThanU2(ushort a, ushort b)
		{
			Assert.Equal(ComparisonTests.CompareGreaterThanU2(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareGreaterThanU2", a, b));
		}
				
		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void CompareLessThanU2(ushort a, ushort b)
		{
			Assert.Equal(ComparisonTests.CompareLessThanU2(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareLessThanU2", a, b));
		}
				
		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void CompareGreaterThanOrEqualU2(ushort a, ushort b)
		{
			Assert.Equal(ComparisonTests.CompareGreaterThanOrEqualU2(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareGreaterThanOrEqualU2", a, b));
		}
				
		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void CompareLessThanOrEqualU2(ushort a, ushort b)
		{
			Assert.Equal(ComparisonTests.CompareLessThanOrEqualU2(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareLessThanOrEqualU2", a, b));
		}
				
		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void CompareEqualU4(uint a, uint b)
		{
			Assert.Equal(ComparisonTests.CompareEqualU4(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareEqualU4", a, b));
		}
				
		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void CompareNotEqualU4(uint a, uint b)
		{
			Assert.Equal(ComparisonTests.CompareNotEqualU4(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareNotEqualU4", a, b));
		}
				
		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void CompareGreaterThanU4(uint a, uint b)
		{
			Assert.Equal(ComparisonTests.CompareGreaterThanU4(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareGreaterThanU4", a, b));
		}
				
		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void CompareLessThanU4(uint a, uint b)
		{
			Assert.Equal(ComparisonTests.CompareLessThanU4(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareLessThanU4", a, b));
		}
				
		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void CompareGreaterThanOrEqualU4(uint a, uint b)
		{
			Assert.Equal(ComparisonTests.CompareGreaterThanOrEqualU4(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareGreaterThanOrEqualU4", a, b));
		}
				
		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void CompareLessThanOrEqualU4(uint a, uint b)
		{
			Assert.Equal(ComparisonTests.CompareLessThanOrEqualU4(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareLessThanOrEqualU4", a, b));
		}
				
		[Theory]
		[MemberData("U8U8", DisableDiscoveryEnumeration = true)]
		public void CompareEqualU8(ulong a, ulong b)
		{
			Assert.Equal(ComparisonTests.CompareEqualU8(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareEqualU8", a, b));
		}
				
		[Theory]
		[MemberData("U8U8", DisableDiscoveryEnumeration = true)]
		public void CompareNotEqualU8(ulong a, ulong b)
		{
			Assert.Equal(ComparisonTests.CompareNotEqualU8(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareNotEqualU8", a, b));
		}
				
		[Theory]
		[MemberData("U8U8", DisableDiscoveryEnumeration = true)]
		public void CompareGreaterThanU8(ulong a, ulong b)
		{
			Assert.Equal(ComparisonTests.CompareGreaterThanU8(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareGreaterThanU8", a, b));
		}
				
		[Theory]
		[MemberData("U8U8", DisableDiscoveryEnumeration = true)]
		public void CompareLessThanU8(ulong a, ulong b)
		{
			Assert.Equal(ComparisonTests.CompareLessThanU8(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareLessThanU8", a, b));
		}
				
		[Theory]
		[MemberData("U8U8", DisableDiscoveryEnumeration = true)]
		public void CompareGreaterThanOrEqualU8(ulong a, ulong b)
		{
			Assert.Equal(ComparisonTests.CompareGreaterThanOrEqualU8(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareGreaterThanOrEqualU8", a, b));
		}
				
		[Theory]
		[MemberData("U8U8", DisableDiscoveryEnumeration = true)]
		public void CompareLessThanOrEqualU8(ulong a, ulong b)
		{
			Assert.Equal(ComparisonTests.CompareLessThanOrEqualU8(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareLessThanOrEqualU8", a, b));
		}
				
		[Theory]
		[MemberData("I1I1", DisableDiscoveryEnumeration = true)]
		public void CompareEqualI1(sbyte a, sbyte b)
		{
			Assert.Equal(ComparisonTests.CompareEqualI1(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareEqualI1", a, b));
		}
				
		[Theory]
		[MemberData("I1I1", DisableDiscoveryEnumeration = true)]
		public void CompareNotEqualI1(sbyte a, sbyte b)
		{
			Assert.Equal(ComparisonTests.CompareNotEqualI1(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareNotEqualI1", a, b));
		}
				
		[Theory]
		[MemberData("I1I1", DisableDiscoveryEnumeration = true)]
		public void CompareGreaterThanI1(sbyte a, sbyte b)
		{
			Assert.Equal(ComparisonTests.CompareGreaterThanI1(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareGreaterThanI1", a, b));
		}
				
		[Theory]
		[MemberData("I1I1", DisableDiscoveryEnumeration = true)]
		public void CompareLessThanI1(sbyte a, sbyte b)
		{
			Assert.Equal(ComparisonTests.CompareLessThanI1(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareLessThanI1", a, b));
		}
				
		[Theory]
		[MemberData("I1I1", DisableDiscoveryEnumeration = true)]
		public void CompareGreaterThanOrEqualI1(sbyte a, sbyte b)
		{
			Assert.Equal(ComparisonTests.CompareGreaterThanOrEqualI1(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareGreaterThanOrEqualI1", a, b));
		}
				
		[Theory]
		[MemberData("I1I1", DisableDiscoveryEnumeration = true)]
		public void CompareLessThanOrEqualI1(sbyte a, sbyte b)
		{
			Assert.Equal(ComparisonTests.CompareLessThanOrEqualI1(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareLessThanOrEqualI1", a, b));
		}
				
		[Theory]
		[MemberData("I2I2", DisableDiscoveryEnumeration = true)]
		public void CompareEqualI2(short a, short b)
		{
			Assert.Equal(ComparisonTests.CompareEqualI2(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareEqualI2", a, b));
		}
				
		[Theory]
		[MemberData("I2I2", DisableDiscoveryEnumeration = true)]
		public void CompareNotEqualI2(short a, short b)
		{
			Assert.Equal(ComparisonTests.CompareNotEqualI2(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareNotEqualI2", a, b));
		}
				
		[Theory]
		[MemberData("I2I2", DisableDiscoveryEnumeration = true)]
		public void CompareGreaterThanI2(short a, short b)
		{
			Assert.Equal(ComparisonTests.CompareGreaterThanI2(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareGreaterThanI2", a, b));
		}
				
		[Theory]
		[MemberData("I2I2", DisableDiscoveryEnumeration = true)]
		public void CompareLessThanI2(short a, short b)
		{
			Assert.Equal(ComparisonTests.CompareLessThanI2(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareLessThanI2", a, b));
		}
				
		[Theory]
		[MemberData("I2I2", DisableDiscoveryEnumeration = true)]
		public void CompareGreaterThanOrEqualI2(short a, short b)
		{
			Assert.Equal(ComparisonTests.CompareGreaterThanOrEqualI2(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareGreaterThanOrEqualI2", a, b));
		}
				
		[Theory]
		[MemberData("I2I2", DisableDiscoveryEnumeration = true)]
		public void CompareLessThanOrEqualI2(short a, short b)
		{
			Assert.Equal(ComparisonTests.CompareLessThanOrEqualI2(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareLessThanOrEqualI2", a, b));
		}
				
		[Theory]
		[MemberData("I4I4", DisableDiscoveryEnumeration = true)]
		public void CompareEqualI4(int a, int b)
		{
			Assert.Equal(ComparisonTests.CompareEqualI4(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareEqualI4", a, b));
		}
				
		[Theory]
		[MemberData("I4I4", DisableDiscoveryEnumeration = true)]
		public void CompareNotEqualI4(int a, int b)
		{
			Assert.Equal(ComparisonTests.CompareNotEqualI4(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareNotEqualI4", a, b));
		}
				
		[Theory]
		[MemberData("I4I4", DisableDiscoveryEnumeration = true)]
		public void CompareGreaterThanI4(int a, int b)
		{
			Assert.Equal(ComparisonTests.CompareGreaterThanI4(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareGreaterThanI4", a, b));
		}
				
		[Theory]
		[MemberData("I4I4", DisableDiscoveryEnumeration = true)]
		public void CompareLessThanI4(int a, int b)
		{
			Assert.Equal(ComparisonTests.CompareLessThanI4(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareLessThanI4", a, b));
		}
				
		[Theory]
		[MemberData("I4I4", DisableDiscoveryEnumeration = true)]
		public void CompareGreaterThanOrEqualI4(int a, int b)
		{
			Assert.Equal(ComparisonTests.CompareGreaterThanOrEqualI4(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareGreaterThanOrEqualI4", a, b));
		}
				
		[Theory]
		[MemberData("I4I4", DisableDiscoveryEnumeration = true)]
		public void CompareLessThanOrEqualI4(int a, int b)
		{
			Assert.Equal(ComparisonTests.CompareLessThanOrEqualI4(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareLessThanOrEqualI4", a, b));
		}
				
		[Theory]
		[MemberData("I8I8", DisableDiscoveryEnumeration = true)]
		public void CompareEqualI8(long a, long b)
		{
			Assert.Equal(ComparisonTests.CompareEqualI8(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareEqualI8", a, b));
		}
				
		[Theory]
		[MemberData("I8I8", DisableDiscoveryEnumeration = true)]
		public void CompareNotEqualI8(long a, long b)
		{
			Assert.Equal(ComparisonTests.CompareNotEqualI8(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareNotEqualI8", a, b));
		}
				
		[Theory]
		[MemberData("I8I8", DisableDiscoveryEnumeration = true)]
		public void CompareGreaterThanI8(long a, long b)
		{
			Assert.Equal(ComparisonTests.CompareGreaterThanI8(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareGreaterThanI8", a, b));
		}
				
		[Theory]
		[MemberData("I8I8", DisableDiscoveryEnumeration = true)]
		public void CompareLessThanI8(long a, long b)
		{
			Assert.Equal(ComparisonTests.CompareLessThanI8(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareLessThanI8", a, b));
		}
				
		[Theory]
		[MemberData("I8I8", DisableDiscoveryEnumeration = true)]
		public void CompareGreaterThanOrEqualI8(long a, long b)
		{
			Assert.Equal(ComparisonTests.CompareGreaterThanOrEqualI8(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareGreaterThanOrEqualI8", a, b));
		}
				
		[Theory]
		[MemberData("I8I8", DisableDiscoveryEnumeration = true)]
		public void CompareLessThanOrEqualI8(long a, long b)
		{
			Assert.Equal(ComparisonTests.CompareLessThanOrEqualI8(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareLessThanOrEqualI8", a, b));
		}
				
		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void CompareEqualC(char a, char b)
		{
			Assert.Equal(ComparisonTests.CompareEqualC(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareEqualC", a, b));
		}
				
		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void CompareNotEqualC(char a, char b)
		{
			Assert.Equal(ComparisonTests.CompareNotEqualC(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareNotEqualC", a, b));
		}
				
		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void CompareGreaterThanC(char a, char b)
		{
			Assert.Equal(ComparisonTests.CompareGreaterThanC(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareGreaterThanC", a, b));
		}
				
		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void CompareLessThanC(char a, char b)
		{
			Assert.Equal(ComparisonTests.CompareLessThanC(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareLessThanC", a, b));
		}
				
		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void CompareGreaterThanOrEqualC(char a, char b)
		{
			Assert.Equal(ComparisonTests.CompareGreaterThanOrEqualC(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareGreaterThanOrEqualC", a, b));
		}
				
		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void CompareLessThanOrEqualC(char a, char b)
		{
			Assert.Equal(ComparisonTests.CompareLessThanOrEqualC(a, b), Run<bool>("Mosa.Test.Collection.ComparisonTests.CompareLessThanOrEqualC", a, b));
		}
		
	}
}
