// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.x86.xUnit
{

	public class SwitchFixture : X86TestFixture
	{
	
		[Theory]
		[PropertyData("I1")]
		public void SwitchI1(sbyte a)
		{
			Assert.Equal(Mosa.Test.Collection.SwitchTests.SwitchI1(a), Run<sbyte>("Mosa.Test.Collection.SwitchTests.SwitchI1", a));
		}
	
		[Theory]
		[PropertyData("I2")]
		public void SwitchI2(short a)
		{
			Assert.Equal(Mosa.Test.Collection.SwitchTests.SwitchI2(a), Run<short>("Mosa.Test.Collection.SwitchTests.SwitchI2", a));
		}
	
		[Theory]
		[PropertyData("I4")]
		public void SwitchI4(int a)
		{
			Assert.Equal(Mosa.Test.Collection.SwitchTests.SwitchI4(a), Run<int>("Mosa.Test.Collection.SwitchTests.SwitchI4", a));
		}
	
		[Theory]
		[PropertyData("I8")]
		public void SwitchI8(long a)
		{
			Assert.Equal(Mosa.Test.Collection.SwitchTests.SwitchI8(a), Run<long>("Mosa.Test.Collection.SwitchTests.SwitchI8", a));
		}
		
		[Theory]
		[PropertyData("U1")]
		public void SwitchU1(byte a)
		{
			Assert.Equal(Mosa.Test.Collection.SwitchTests.SwitchU1(a), Run<byte>("Mosa.Test.Collection.SwitchTests.SwitchU1", a));
		}
	
		[Theory]
		[PropertyData("U2")]
		public void SwitchU2(ushort a)
		{
			Assert.Equal(Mosa.Test.Collection.SwitchTests.SwitchU2(a), Run<ushort>("Mosa.Test.Collection.SwitchTests.SwitchU2", a));
		}
	
		[Theory]
		[PropertyData("U4")]
		public void SwitchU4(uint a)
		{
			Assert.Equal(Mosa.Test.Collection.SwitchTests.SwitchU4(a), Run<uint>("Mosa.Test.Collection.SwitchTests.SwitchU4", a));
		}
	
		[Theory]
		[PropertyData("U8")]
		public void SwitchU8(ulong a)
		{
			Assert.Equal(Mosa.Test.Collection.SwitchTests.SwitchU8(a), Run<ulong>("Mosa.Test.Collection.SwitchTests.SwitchU8", a));
		}
		}
}

#endif
