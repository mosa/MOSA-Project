// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.xUnit
{

	public class SwitchFixture : TestFixture
	{
	
		[Theory]
		[MemberData("I1", DisableDiscoveryEnumeration = true)]
		public void SwitchI1(sbyte a)
		{
			Assert.Equal(Mosa.Test.Collection.SwitchTests.SwitchI1(a), Run<sbyte>("Mosa.Test.Collection.SwitchTests.SwitchI1", a));
		}
	
		[Theory]
		[MemberData("I2", DisableDiscoveryEnumeration = true)]
		public void SwitchI2(short a)
		{
			Assert.Equal(Mosa.Test.Collection.SwitchTests.SwitchI2(a), Run<short>("Mosa.Test.Collection.SwitchTests.SwitchI2", a));
		}
	
		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void SwitchI4(int a)
		{
			Assert.Equal(Mosa.Test.Collection.SwitchTests.SwitchI4(a), Run<int>("Mosa.Test.Collection.SwitchTests.SwitchI4", a));
		}
	
		[Theory]
		[MemberData("I8", DisableDiscoveryEnumeration = true)]
		public void SwitchI8(long a)
		{
			Assert.Equal(Mosa.Test.Collection.SwitchTests.SwitchI8(a), Run<long>("Mosa.Test.Collection.SwitchTests.SwitchI8", a));
		}
		
		[Theory]
		[MemberData("U1", DisableDiscoveryEnumeration = true)]
		public void SwitchU1(byte a)
		{
			Assert.Equal(Mosa.Test.Collection.SwitchTests.SwitchU1(a), Run<byte>("Mosa.Test.Collection.SwitchTests.SwitchU1", a));
		}
	
		[Theory]
		[MemberData("U2", DisableDiscoveryEnumeration = true)]
		public void SwitchU2(ushort a)
		{
			Assert.Equal(Mosa.Test.Collection.SwitchTests.SwitchU2(a), Run<ushort>("Mosa.Test.Collection.SwitchTests.SwitchU2", a));
		}
	
		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void SwitchU4(uint a)
		{
			Assert.Equal(Mosa.Test.Collection.SwitchTests.SwitchU4(a), Run<uint>("Mosa.Test.Collection.SwitchTests.SwitchU4", a));
		}
	
		[Theory]
		[MemberData("U8", DisableDiscoveryEnumeration = true)]
		public void SwitchU8(ulong a)
		{
			Assert.Equal(Mosa.Test.Collection.SwitchTests.SwitchU8(a), Run<ulong>("Mosa.Test.Collection.SwitchTests.SwitchU8", a));
		}
		}
}
