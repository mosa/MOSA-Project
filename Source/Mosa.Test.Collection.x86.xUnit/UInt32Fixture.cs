// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class UInt32Fixture : X86TestFixture
	{
		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void AddU4U4(uint a, uint b)
		{
			Assert.Equal(UInt32Tests.AddU4U4(a, b), Run<uint>("Mosa.Test.Collection.UInt32Tests.AddU4U4", a, b));
		}

		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void SubU4U4(uint a, uint b)
		{
			Assert.Equal(UInt32Tests.SubU4U4(a, b), Run<uint>("Mosa.Test.Collection.UInt32Tests.SubU4U4", a, b));
		}

		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void MulU4U4(uint a, uint b)
		{
			Assert.Equal(UInt32Tests.MulU4U4(a, b), Run<uint>("Mosa.Test.Collection.UInt32Tests.MulU4U4", a, b));
		}

		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void DivU4U4(uint a, uint b)
		{
			if (b == 0)
			{
				return;
			}

			Assert.Equal(UInt32Tests.DivU4U4(a, b), Run<uint>("Mosa.Test.Collection.UInt32Tests.DivU4U4", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void DivU4U4DivideByZeroException(uint a)
		{
			Assert.Equal(UInt32Tests.DivU4U4(a, (uint)0), Run<uint>("Mosa.Test.Collection.UInt32Tests.DivU4U4", a, (uint)0));
		}

		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void RemU4U4(uint a, uint b)
		{
			if (b == 0)
			{
				return;
			}

			Assert.Equal(UInt32Tests.RemU4U4(a, b), Run<uint>("Mosa.Test.Collection.UInt32Tests.RemU4U4", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void RemU4U4DivideByZeroException(uint a)
		{
			Assert.Equal(UInt32Tests.RemU4U4(a, (uint)0), Run<uint>("Mosa.Test.Collection.UInt32Tests.RemU4U4", a, (uint)0));
		}

		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void RetU4(uint a)
		{
			Assert.Equal(UInt32Tests.RetU4(a), Run<uint>("Mosa.Test.Collection.UInt32Tests.RetU4", a));
		}

		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void AndU4U4(uint a, uint b)
		{
			Assert.Equal(UInt32Tests.AndU4U4(a, b), Run<uint>("Mosa.Test.Collection.UInt32Tests.AndU4U4", a, b));
		}

		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void OrU4U4(uint a, uint b)
		{
			Assert.Equal(UInt32Tests.OrU4U4(a, b), Run<uint>("Mosa.Test.Collection.UInt32Tests.OrU4U4", a, b));
		}

		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void XorU4U4(uint a, uint b)
		{
			Assert.Equal(UInt32Tests.XorU4U4(a, b), Run<uint>("Mosa.Test.Collection.UInt32Tests.XorU4U4", a, b));
		}

		[Theory]
		[MemberData("U4", DisableDiscoveryEnumeration = true)]
		public void CompU4(uint a)
		{
			Assert.Equal(UInt32Tests.CompU4(a), Run<uint>("Mosa.Test.Collection.UInt32Tests.CompU4", a));
		}

		[Theory]
		[MemberData("U4U1UpTo32", DisableDiscoveryEnumeration = true)]
		public void ShiftLeftU4U1(uint a, byte b)
		{
			Assert.Equal(UInt32Tests.ShiftLeftU4U1(a, b), Run<uint>("Mosa.Test.Collection.UInt32Tests.ShiftLeftU4U1", a, b));
		}

		[Theory]
		[MemberData("U4U1UpTo32", DisableDiscoveryEnumeration = true)]
		public void ShiftRightU4U1(uint a, byte b)
		{
			Assert.Equal(UInt32Tests.ShiftRightU4U1(a, b), Run<uint>("Mosa.Test.Collection.UInt32Tests.ShiftRightU4U1", a, b));
		}

		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void CeqU4U4(uint a, uint b)
		{
			Assert.Equal(UInt32Tests.CeqU4U4(a, b), Run<bool>("Mosa.Test.Collection.UInt32Tests.CeqU4U4", a, b));
		}

		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void CltU4U4(uint a, uint b)
		{
			Assert.Equal(UInt32Tests.CltU4U4(a, b), Run<bool>("Mosa.Test.Collection.UInt32Tests.CltU4U4", a, b));
		}

		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void CgtU4U4(uint a, uint b)
		{
			Assert.Equal(UInt32Tests.CgtU4U4(a, b), Run<bool>("Mosa.Test.Collection.UInt32Tests.CgtU4U4", a, b));
		}

		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void CleU4U4(uint a, uint b)
		{
			Assert.Equal(UInt32Tests.CleU4U4(a, b), Run<bool>("Mosa.Test.Collection.UInt32Tests.CleU4U4", a, b));
		}

		[Theory]
		[MemberData("U4U4", DisableDiscoveryEnumeration = true)]
		public void CgeU4U4(uint a, uint b)
		{
			Assert.Equal(UInt32Tests.CgeU4U4(a, b), Run<bool>("Mosa.Test.Collection.UInt32Tests.CgeU4U4", a, b));
		}

		[Fact]
		public void Newarr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt32Tests.Newarr"));
		}

		[Theory]
		[MemberData("I4Small", DisableDiscoveryEnumeration = true)]
		public void Ldlen(int length)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt32Tests.Ldlen", length));
		}

		[Theory]
		[MemberData("I4SmallU4", DisableDiscoveryEnumeration = true)]
		public void StelemU4(int index, uint value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt32Tests.Stelem", index, value));
		}

		[Theory]
		[MemberData("I4SmallU4", DisableDiscoveryEnumeration = true)]
		public void LdelemU4(int index, uint value)
		{
			Assert.Equal(UInt32Tests.Ldelem(index, value), Run<uint>("Mosa.Test.Collection.UInt32Tests.Ldelem", index, value));
		}

		[Theory]
		[MemberData("I4SmallU4", DisableDiscoveryEnumeration = true)]
		public void LdelemaU4(int index, uint value)
		{
			Assert.Equal(UInt32Tests.Ldelema(index, value), Run<uint>("Mosa.Test.Collection.UInt32Tests.Ldelema", index, value));
		}
	}
}
