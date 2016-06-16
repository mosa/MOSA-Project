// Copyright (c) MOSA Project. Licensed under the New BSD License.


using Xunit;
using Xunit.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Test.Collection.xUnit
{
	public class Ldloca : TestFixture
	{
		
		[Theory]
		[MemberData("U1", DisableDiscoveryEnumeration = true)]
		public void LdlocaCheckValueU1(byte a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.LdlocaTests.LdlocaCheckValueU1", a, a));
		}
		
		[Theory]
		[MemberData("U2", DisableDiscoveryEnumeration = true)]
		public void LdlocaCheckValueU2(ushort a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.LdlocaTests.LdlocaCheckValueU2", a, a));
		}
		
		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void LdlocaCheckValueU4(uint a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.LdlocaTests.LdlocaCheckValueU4", a, a));
		}
		
		[Theory]
		[MemberData("U8", DisableDiscoveryEnumeration = true)]
		public void LdlocaCheckValueU8(ulong a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.LdlocaTests.LdlocaCheckValueU8", a, a));
		}
		
		[Theory]
		[MemberData("I1", DisableDiscoveryEnumeration = true)]
		public void LdlocaCheckValueI1(sbyte a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.LdlocaTests.LdlocaCheckValueI1", a, a));
		}
		
		[Theory]
		[MemberData("I2", DisableDiscoveryEnumeration = true)]
		public void LdlocaCheckValueI2(short a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.LdlocaTests.LdlocaCheckValueI2", a, a));
		}
		
		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void LdlocaCheckValueI4(int a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.LdlocaTests.LdlocaCheckValueI4", a, a));
		}
		
		[Theory]
		[MemberData("I8", DisableDiscoveryEnumeration = true)]
		public void LdlocaCheckValueI8(long a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.LdlocaTests.LdlocaCheckValueI8", a, a));
		}
		
		[Theory]
		[MemberData("R4", DisableDiscoveryEnumeration = true)]
		public void LdlocaCheckValueR4(float a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.LdlocaTests.LdlocaCheckValueR4", a, a));
		}
		
		[Theory]
		[MemberData("R8", DisableDiscoveryEnumeration = true)]
		public void LdlocaCheckValueR8(double a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.LdlocaTests.LdlocaCheckValueR8", a, a));
		}
		
		[Theory]
		[MemberData("C", DisableDiscoveryEnumeration = true)]
		public void LdlocaCheckValueC(char a)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.LdlocaTests.LdlocaCheckValueC", a, a));
		}
			}
}
