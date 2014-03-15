using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class StructureFixture : X86TestFixture
	{
		[Theory]
		[PropertyData("U1")]
		public void StructTestSet1U1(byte one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1U1(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1U1", one));
		}
		
		[Theory]
		[PropertyData("U1U1U1")]
		public void StructTestSet3U1(byte one, byte two, byte three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3U1(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3U1", one, two, three));
		}
			[Theory]
		[PropertyData("U2")]
		public void StructTestSet1U2(ushort one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1U2(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1U2", one));
		}
		
		[Theory]
		[PropertyData("U2U2U2")]
		public void StructTestSet3U2(ushort one, ushort two, ushort three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3U2(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3U2", one, two, three));
		}
			[Theory]
		[PropertyData("U4")]
		public void StructTestSet1U4(uint one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1U4(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1U4", one));
		}
		
		[Theory]
		[PropertyData("U4MiniU4MiniU4Mini")]
		public void StructTestSet3U4(uint one, uint two, uint three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3U4(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3U4", one, two, three));
		}
			[Theory]
		[PropertyData("U8")]
		public void StructTestSet1U8(ulong one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1U8(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1U8", one));
		}
		
		[Theory]
		[PropertyData("U8MiniU8MiniU8Mini")]
		public void StructTestSet3U8(ulong one, ulong two, ulong three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3U8(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3U8", one, two, three));
		}
			[Theory]
		[PropertyData("I1")]
		public void StructTestSet1I1(sbyte one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1I1(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1I1", one));
		}
		
		[Theory]
		[PropertyData("I1I1I1")]
		public void StructTestSet3I1(sbyte one, sbyte two, sbyte three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3I1(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3I1", one, two, three));
		}
			[Theory]
		[PropertyData("I2")]
		public void StructTestSet1I2(short one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1I2(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1I2", one));
		}
		
		[Theory]
		[PropertyData("I2I2I2")]
		public void StructTestSet3I2(short one, short two, short three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3I2(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3I2", one, two, three));
		}
			[Theory]
		[PropertyData("I4")]
		public void StructTestSet1I4(int one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1I4(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1I4", one));
		}
		
		[Theory]
		[PropertyData("I4MiniI4MiniI4Mini")]
		public void StructTestSet3I4(int one, int two, int three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3I4(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3I4", one, two, three));
		}
			[Theory]
		[PropertyData("I8")]
		public void StructTestSet1I8(long one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1I8(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1I8", one));
		}
		
		[Theory]
		[PropertyData("I8MiniI8MiniI8Mini")]
		public void StructTestSet3I8(long one, long two, long three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3I8(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3I8", one, two, three));
		}
			[Theory]
		[PropertyData("R4")]
		public void StructTestSet1R4(float one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1R4(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1R4", one));
		}
		
		[Theory]
		[PropertyData("R4MiniR4MiniR4Mini")]
		public void StructTestSet3R4(float one, float two, float three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3R4(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3R4", one, two, three));
		}
			[Theory]
		[PropertyData("R8")]
		public void StructTestSet1R8(double one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1R8(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1R8", one));
		}
		
		[Theory]
		[PropertyData("R8MiniR8MiniR8Mini")]
		public void StructTestSet3R8(double one, double two, double three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3R8(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3R8", one, two, three));
		}
			[Theory]
		[PropertyData("C")]
		public void StructTestSet1C(char one)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet1C(one), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet1C", one));
		}
		
		[Theory]
		[PropertyData("CCC")]
		public void StructTestSet3C(char one, char two, char three)
		{
			Assert.Equal(Mosa.Test.Collection.StructTests.StructTestSet3C(one, two, three), Run<bool>("Mosa.Test.Collection.StructTests.StructTestSet3C", one, two, three));
		}
		}
}
