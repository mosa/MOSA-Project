// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class StructureFixture : X86TestFixture
	{
		[Theory]
		[MemberData("U1", DisableDiscoveryEnumeration = true)]
		public void StructTestSet1U1(byte one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1U1(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1U1", one));
		}

		[Theory]
		[MemberData("U1U1U1", DisableDiscoveryEnumeration = true)]
		public void StructTestSet3U1(byte one, byte two, byte three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3U1(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3U1", one, two, three));
		}
		[Theory]
		[MemberData("U2", DisableDiscoveryEnumeration = true)]
		public void StructTestSet1U2(ushort one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1U2(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1U2", one));
		}

		[Theory]
		[MemberData("U2U2U2", DisableDiscoveryEnumeration = true)]
		public void StructTestSet3U2(ushort one, ushort two, ushort three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3U2(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3U2", one, two, three));
		}
		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void StructTestSet1U4(uint one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1U4(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1U4", one));
		}

		[Theory]
		[MemberData("U4MiniU4MiniU4Mini", DisableDiscoveryEnumeration = true)]
		public void StructTestSet3U4(uint one, uint two, uint three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3U4(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3U4", one, two, three));
		}
		[Theory]
		[MemberData("U8", DisableDiscoveryEnumeration = true)]
		public void StructTestSet1U8(ulong one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1U8(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1U8", one));
		}

		[Theory]
		[MemberData("U8MiniU8MiniU8Mini", DisableDiscoveryEnumeration = true)]
		public void StructTestSet3U8(ulong one, ulong two, ulong three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3U8(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3U8", one, two, three));
		}
		[Theory]
		[MemberData("I1", DisableDiscoveryEnumeration = true)]
		public void StructTestSet1I1(sbyte one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1I1(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1I1", one));
		}

		[Theory]
		[MemberData("I1I1I1", DisableDiscoveryEnumeration = true)]
		public void StructTestSet3I1(sbyte one, sbyte two, sbyte three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3I1(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3I1", one, two, three));
		}
		[Theory]
		[MemberData("I2", DisableDiscoveryEnumeration = true)]
		public void StructTestSet1I2(short one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1I2(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1I2", one));
		}

		[Theory]
		[MemberData("I2I2I2", DisableDiscoveryEnumeration = true)]
		public void StructTestSet3I2(short one, short two, short three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3I2(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3I2", one, two, three));
		}
		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void StructTestSet1I4(int one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1I4(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1I4", one));
		}

		[Theory]
		[MemberData("I4MiniI4MiniI4Mini", DisableDiscoveryEnumeration = true)]
		public void StructTestSet3I4(int one, int two, int three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3I4(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3I4", one, two, three));
		}
		[Theory]
		[MemberData("I8", DisableDiscoveryEnumeration = true)]
		public void StructTestSet1I8(long one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1I8(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1I8", one));
		}

		[Theory]
		[MemberData("I8MiniI8MiniI8Mini", DisableDiscoveryEnumeration = true)]
		public void StructTestSet3I8(long one, long two, long three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3I8(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3I8", one, two, three));
		}
		[Theory]
		[MemberData("R4", DisableDiscoveryEnumeration = true)]
		public void StructTestSet1R4(float one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1R4(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1R4", one));
		}

		[Theory]
		[MemberData("R4MiniR4MiniR4Mini", DisableDiscoveryEnumeration = true)]
		public void StructTestSet3R4(float one, float two, float three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3R4(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3R4", one, two, three));
		}
		[Theory]
		[MemberData("R8", DisableDiscoveryEnumeration = true)]
		public void StructTestSet1R8(double one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1R8(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1R8", one));
		}

		[Theory]
		[MemberData("R8MiniR8MiniR8Mini", DisableDiscoveryEnumeration = true)]
		public void StructTestSet3R8(double one, double two, double three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3R8(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3R8", one, two, three));
		}
		[Theory]
		[MemberData("C", DisableDiscoveryEnumeration = true)]
		public void StructTestSet1C(char one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1C(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1C", one));
		}

		[Theory]
		[MemberData("CCC", DisableDiscoveryEnumeration = true)]
		public void StructTestSet3C(char one, char two, char three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3C(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3C", one, two, three));
		}
	}
}
