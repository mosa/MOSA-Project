// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class UInt8Fixture : TestFixture
	{
		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void AddU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.AddU1U1(a, b), Run<int>("Mosa.UnitTest.Collection.UInt8Tests.AddU1U1", a, b));
		}

		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void SubU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.SubU1U1(a, b), Run<int>("Mosa.UnitTest.Collection.UInt8Tests.SubU1U1", a, b));
		}

		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void MulU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.MulU1U1(a, b), Run<int>("Mosa.UnitTest.Collection.UInt8Tests.MulU1U1", a, b));
		}

		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void DivU1U1(byte a, byte b)
		{
			if (b == 0)
			{
				return;
			}

			Assert.Equal(UInt8Tests.DivU1U1(a, b), Run<int>("Mosa.UnitTest.Collection.UInt8Tests.DivU1U1", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void DivU1U1DivideByZeroException(byte a)
		{
			Assert.Equal(UInt8Tests.DivU1U1(a, 0), Run<int>("Mosa.UnitTest.Collection.UInt8Tests.DivU1U1", a, (byte)0));
		}

		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void RemU1U1(byte a, byte b)
		{
			if (b == 0)
			{
				return;
			}

			Assert.Equal(UInt8Tests.RemU1U1(a, b), Run<int>("Mosa.UnitTest.Collection.UInt8Tests.RemU1U1", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void RemU1U1DivideByZeroException(byte a)
		{
			Assert.Equal(UInt8Tests.RemU1U1(a, 0), Run<int>("Mosa.UnitTest.Collection.UInt8Tests.RemU1U1", a, (byte)0));
		}

		[Theory]
		[MemberData("U1", DisableDiscoveryEnumeration = true)]
		public void RetU1(byte a)
		{
			Assert.Equal(UInt8Tests.RetU1(a), Run<byte>("Mosa.UnitTest.Collection.UInt8Tests.RetU1", a));
		}

		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void AndU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.AndU1U1(a, b), Run<int>("Mosa.UnitTest.Collection.UInt8Tests.AndU1U1", a, b));
		}

		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void OrU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.OrU1U1(a, b), Run<int>("Mosa.UnitTest.Collection.UInt8Tests.OrU1U1", a, b));
		}

		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void XorU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.XorU1U1(a, b), Run<int>("Mosa.UnitTest.Collection.UInt8Tests.XorU1U1", a, b));
		}

		[Theory]
		[MemberData("U1", DisableDiscoveryEnumeration = true)]
		public void CompU1(byte a)
		{
			Assert.Equal(UInt8Tests.CompU1(a), Run<int>("Mosa.UnitTest.Collection.UInt8Tests.CompU1", a));
		}

		[Theory]
		[MemberData("U1U1UpTo16", DisableDiscoveryEnumeration = true)]
		public void ShiftLeftU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.ShiftLeftU1U1(a, b), Run<int>("Mosa.UnitTest.Collection.UInt8Tests.ShiftLeftU1U1", a, b));
		}

		[Theory]
		[MemberData("U1U1UpTo16", DisableDiscoveryEnumeration = true)]
		public void ShiftRightU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.ShiftRightU1U1(a, b), Run<int>("Mosa.UnitTest.Collection.UInt8Tests.ShiftRightU1U1", a, b));
		}

		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void CeqU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.CeqU1U1(a, b), Run<bool>("Mosa.UnitTest.Collection.UInt8Tests.CeqU1U1", a, b));
		}

		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void CltU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.CltU1U1(a, b), Run<bool>("Mosa.UnitTest.Collection.UInt8Tests.CltU1U1", a, b));
		}

		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void CgtU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.CgtU1U1(a, b), Run<bool>("Mosa.UnitTest.Collection.UInt8Tests.CgtU1U1", a, b));
		}

		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void CleU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.CleU1U1(a, b), Run<bool>("Mosa.UnitTest.Collection.UInt8Tests.CleU1U1", a, b));
		}

		[Theory]
		[MemberData("U1U1", DisableDiscoveryEnumeration = true)]
		public void CgeU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.CgeU1U1(a, b), Run<bool>("Mosa.UnitTest.Collection.UInt8Tests.CgeU1U1", a, b));
		}

		[Fact]
		public void Newarr()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.UInt8Tests.Newarr"));
		}

		[Theory]
		[MemberData("I4Small", DisableDiscoveryEnumeration = true)]
		public void Ldlen(int length)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.UInt8Tests.Ldlen", length));
		}

		[Theory]
		[MemberData("I4SmallU1", DisableDiscoveryEnumeration = true)]
		public void StelemU1(int index, byte value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection.UInt8Tests.Stelem", index, value));
		}

		[Theory]
		[MemberData("I4SmallU1", DisableDiscoveryEnumeration = true)]
		public void LdelemU1(int index, byte value)
		{
			Assert.Equal(UInt8Tests.Ldelem(index, value), Run<byte>("Mosa.UnitTest.Collection.UInt8Tests.Ldelem", index, value));
		}

		[Theory]
		[MemberData("I4SmallU1", DisableDiscoveryEnumeration = true)]
		public void LdelemaU1(int index, byte value)
		{
			Assert.Equal(UInt8Tests.Ldelema(index, value), Run<byte>("Mosa.UnitTest.Collection.UInt8Tests.Ldelema", index, value));
		}
	}
}
