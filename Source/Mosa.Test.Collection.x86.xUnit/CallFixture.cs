// Copyright (c) MOSA Project. Licensed under the New BSD License.


// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class CallFixture : X86TestFixture
	{

		
		[Theory]
		[MemberData("U1", DisableDiscoveryEnumeration = true)]

		public void CallU1(byte a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.CallTests.CallU1", a));
		}
		
		[Theory]
		[MemberData("U2", DisableDiscoveryEnumeration = true)]

		public void CallU2(ushort a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.CallTests.CallU2", a));
		}
		
		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]

		public void CallU4(uint a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.CallTests.CallU4", a));
		}
		
		[Theory]
		[MemberData("U8", DisableDiscoveryEnumeration = true)]

		public void CallU8(ulong a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.CallTests.CallU8", a));
		}
		
		[Theory]
		[MemberData("I1", DisableDiscoveryEnumeration = true)]

		public void CallI1(sbyte a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.CallTests.CallI1", a));
		}
		
		[Theory]
		[MemberData("I2", DisableDiscoveryEnumeration = true)]

		public void CallI2(short a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.CallTests.CallI2", a));
		}
		
		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]

		public void CallI4(int a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.CallTests.CallI4", a));
		}
		
		[Theory]
		[MemberData("I8", DisableDiscoveryEnumeration = true)]

		public void CallI8(long a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.CallTests.CallI8", a));
		}
		
		[Theory]
		[MemberData("C", DisableDiscoveryEnumeration = true)]

		public void CallC(char a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.CallTests.CallC", a));
		}
		
	}
}
