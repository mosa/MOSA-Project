// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Test.Collection.xUnit
{
	public class UInt16Fixture : TestFixture
	{
		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void AddU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.AddU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.AddU2U2", a, b));
		}

		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void SubU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.SubU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.SubU2U2", a, b));
		}

		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void MulU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.MulU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.MulU2U2", a, b));
		}

		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void DivU2U2(ushort a, ushort b)
		{
			if (b == 0)
			{
				return;
			}

			Assert.Equal(UInt16Tests.DivU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.DivU2U2", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void DivU2U2DivideByZeroException(ushort a)
		{
			Assert.Equal(UInt16Tests.DivU2U2(a, 0), Run<int>("Mosa.Test.Collection.UInt16Tests.DivU2U2", a, (ushort)0));
		}

		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void RemU2U2(ushort a, ushort b)
		{
			if (b == 0)
			{
				return;
			}

			Assert.Equal(UInt16Tests.RemU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.RemU2U2", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void RemU2U2DivideByZeroException(ushort a)
		{
			Assert.Equal(UInt16Tests.RemU2U2(a, 0), Run<int>("Mosa.Test.Collection.UInt16Tests.RemU2U2", a, (ushort)0));
		}

		[Theory]
		[MemberData("U2", DisableDiscoveryEnumeration = true)]
		public void RetU2(ushort a)
		{
			Assert.Equal(UInt16Tests.RetU2(a), Run<ushort>("Mosa.Test.Collection.UInt16Tests.RetU2", a));
		}

		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void AndU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.AndU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.AndU2U2", a, b));
		}

		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void OrU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.OrU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.OrU2U2", a, b));
		}

		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void XorU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.XorU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.XorU2U2", a, b));
		}

		[Theory]
		[MemberData("U2", DisableDiscoveryEnumeration = true)]
		public void CompU2(ushort a)
		{
			Assert.Equal(UInt16Tests.CompU2(a), Run<int>("Mosa.Test.Collection.UInt16Tests.CompU2", a));
		}

		[Theory]
		[MemberData("U2U1UpTo16", DisableDiscoveryEnumeration = true)]
		public void ShiftLeftU2U2(ushort a, byte b)
		{
			Assert.Equal(UInt16Tests.ShiftLeftU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.ShiftLeftU2U2", a, b));
		}

		[Theory]
		[MemberData("U2U1UpTo16", DisableDiscoveryEnumeration = true)]
		public void ShiftRightU2U2(ushort a, byte b)
		{
			Assert.Equal(UInt16Tests.ShiftRightU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.ShiftRightU2U2", a, b));
		}

		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void CeqU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.CeqU2U2(a, b), Run<bool>("Mosa.Test.Collection.UInt16Tests.CeqU2U2", a, b));
		}

		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void CltU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.CltU2U2(a, b), Run<bool>("Mosa.Test.Collection.UInt16Tests.CltU2U2", a, b));
		}

		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void CgtU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.CgtU2U2(a, b), Run<bool>("Mosa.Test.Collection.UInt16Tests.CgtU2U2", a, b));
		}

		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void CleU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.CleU2U2(a, b), Run<bool>("Mosa.Test.Collection.UInt16Tests.CleU2U2", a, b));
		}

		[Theory]
		[MemberData("U2U2", DisableDiscoveryEnumeration = true)]
		public void CgeU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.CgeU2U2(a, b), Run<bool>("Mosa.Test.Collection.UInt16Tests.CgeU2U2", a, b));
		}

		[Fact]
		public void Newarr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt16Tests.Newarr"));
		}

		[Theory]
		[MemberData("I4Small", DisableDiscoveryEnumeration = true)]
		public void Ldlen(int length)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt16Tests.Ldlen", length));
		}

		[Theory]
		[MemberData("I4SmallU2", DisableDiscoveryEnumeration = true)]
		public void StelemU2(int index, ushort value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt16Tests.Stelem", index, value));
		}

		[Theory]
		[MemberData("I4SmallU2", DisableDiscoveryEnumeration = true)]
		public void LdelemU2(int index, ushort value)
		{
			Assert.Equal(UInt16Tests.Ldelem(index, value), Run<ushort>("Mosa.Test.Collection.UInt16Tests.Ldelem", index, value));
		}

		[Theory]
		[MemberData("I4SmallU2", DisableDiscoveryEnumeration = true)]
		public void LdelemaU2(int index, ushort value)
		{
			Assert.Equal(UInt16Tests.Ldelema(index, value), Run<ushort>("Mosa.Test.Collection.UInt16Tests.Ldelema", index, value));
		}
	}
}
