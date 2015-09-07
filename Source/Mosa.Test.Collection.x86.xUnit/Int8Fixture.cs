// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class Int8Fixture : X86TestFixture
	{
		[Theory]
		[InlineData((sbyte)-2, (sbyte)-4)]
		public void AddI1I1(sbyte a, sbyte b)
		{
			Assert.Equal(Int8Tests.AddI1I1(a, b), Run<int>("Mosa.Test.Collection.Int8Tests.AddI1I1", a, b));
		}

		[Theory]
		[MemberData("I1I1", DisableDiscoveryEnumeration = true)]
		public void SubI1I1(sbyte a, sbyte b)
		{
			Assert.Equal(Int8Tests.SubI1I1(a, b), Run<int>("Mosa.Test.Collection.Int8Tests.SubI1I1", a, b));
		}

		[Theory]
		[MemberData("I1I1", DisableDiscoveryEnumeration = true)]
		public void MulI1I1(sbyte a, sbyte b)
		{
			Assert.Equal(Int8Tests.MulI1I1(a, b), Run<int>("Mosa.Test.Collection.Int8Tests.MulI1I1", a, b));
		}

		[Theory]
		[MemberData("I1I1", DisableDiscoveryEnumeration = true)]
		public void DivI1I1(sbyte a, sbyte b)
		{
			if (b == 0)
			{
				return;
			}

			Assert.Equal(Int8Tests.DivI1I1(a, b), Run<int>("Mosa.Test.Collection.Int8Tests.DivI1I1", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void DivI1I1DivideByZeroException(sbyte a)
		{
			Assert.Equal(Int8Tests.DivI1I1(a, (sbyte)0), Run<int>("Mosa.Test.Collection.Int8Tests.DivI1I1", a, (sbyte)0));
		}

		[Theory]
		[MemberData("I1I1", DisableDiscoveryEnumeration = true)]
		public void RemI1I1(sbyte a, sbyte b)
		{
			if (b == 0)
			{
				return;
			}

			Assert.Equal(Int8Tests.RemI1I1(a, b), Run<int>("Mosa.Test.Collection.Int8Tests.RemI1I1", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void RemI1I1DivideByZeroException(sbyte a)
		{
			Assert.Equal(Int8Tests.RemI1I1(a, (sbyte)0), Run<int>("Mosa.Test.Collection.Int8Tests.RemI1I1", a, (sbyte)0));
		}

		[Theory]
		[MemberData("I1", DisableDiscoveryEnumeration = true)]
		public void RetI1(sbyte a)
		{
			Assert.Equal(Int8Tests.RetI1(a), Run<sbyte>("Mosa.Test.Collection.Int8Tests.RetI1", a));
		}

		[Theory]
		[MemberData("I1I1", DisableDiscoveryEnumeration = true)]
		public void AndI1I1(sbyte a, sbyte b)
		{
			Assert.Equal(Int8Tests.AndI1I1(a, b), Run<int>("Mosa.Test.Collection.Int8Tests.AndI1I1", a, b));
		}

		[Theory]
		[MemberData("I1I1", DisableDiscoveryEnumeration = true)]
		public void OrI1I1(sbyte a, sbyte b)
		{
			Assert.Equal(Int8Tests.OrI1I1(a, b), Run<int>("Mosa.Test.Collection.Int8Tests.OrI1I1", a, b));
		}

		[Theory]
		[MemberData("I1I1", DisableDiscoveryEnumeration = true)]
		public void XorI1I1(sbyte a, sbyte b)
		{
			Assert.Equal(Int8Tests.XorI1I1(a, b), Run<int>("Mosa.Test.Collection.Int8Tests.XorI1I1", a, b));
		}

		[Theory]
		[MemberData("I1", DisableDiscoveryEnumeration = true)]
		public void CompI1(sbyte a)
		{
			Assert.Equal(Int8Tests.CompI1(a), Run<int>("Mosa.Test.Collection.Int8Tests.CompI1", a));
		}

		[Theory]
		[MemberData("I1U1UpTo16", DisableDiscoveryEnumeration = true)]
		public void ShiftLeftI1I1(sbyte a, byte b)
		{
			Assert.Equal(Int8Tests.ShiftLeftI1I1(a, b), Run<int>("Mosa.Test.Collection.Int8Tests.ShiftLeftI1I1", a, b));
		}

		[Theory]
		[MemberData("I1U1UpTo16", DisableDiscoveryEnumeration = true)]
		public void ShiftRightI1I1(sbyte a, byte b)
		{
			Assert.Equal(Int8Tests.ShiftRightI1I1(a, b), Run<int>("Mosa.Test.Collection.Int8Tests.ShiftRightI1I1", a, b));
		}

		[Theory]
		[MemberData("I1I1", DisableDiscoveryEnumeration = true)]
		public void CeqI1I1(sbyte a, sbyte b)
		{
			Assert.Equal(Int8Tests.CeqI1I1(a, b), Run<bool>("Mosa.Test.Collection.Int8Tests.CeqI1I1", a, b));
		}

		[Theory]
		[MemberData("I1I1", DisableDiscoveryEnumeration = true)]
		public void CltI1I1(sbyte a, sbyte b)
		{
			Assert.Equal(Int8Tests.CltI1I1(a, b), Run<bool>("Mosa.Test.Collection.Int8Tests.CltI1I1", a, b));
		}

		[Theory]
		[MemberData("I1I1", DisableDiscoveryEnumeration = true)]
		public void CgtI1I1(sbyte a, sbyte b)
		{
			Assert.Equal(Int8Tests.CgtI1I1(a, b), Run<bool>("Mosa.Test.Collection.Int8Tests.CgtI1I1", a, b));
		}

		[Theory]
		[MemberData("I1I1", DisableDiscoveryEnumeration = true)]
		public void CleI1I1(sbyte a, sbyte b)
		{
			Assert.Equal(Int8Tests.CleI1I1(a, b), Run<bool>("Mosa.Test.Collection.Int8Tests.CleI1I1", a, b));
		}

		[Theory]
		[MemberData("I1I1", DisableDiscoveryEnumeration = true)]
		public void CgeI1I1(sbyte a, sbyte b)
		{
			Assert.Equal(Int8Tests.CgeI1I1(a, b), Run<bool>("Mosa.Test.Collection.Int8Tests.CgeI1I1", a, b));
		}

		[Fact]
		public void Newarr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.Int8Tests.Newarr"));
		}

		[Theory]
		[MemberData("I4Small", DisableDiscoveryEnumeration = true)]
		public void Ldlen(int length)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.Int8Tests.Ldlen", length));
		}

		[Theory]
		[MemberData("I4SmallI1", DisableDiscoveryEnumeration = true)]
		public void StelemI1(int index, sbyte value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.Int8Tests.Stelem", index, value));
		}

		[Theory]
		[MemberData("I4SmallI1", DisableDiscoveryEnumeration = true)]
		public void LdelemI1(int index, sbyte value)
		{
			Assert.Equal(Int8Tests.Ldelem(index, value), Run<sbyte>("Mosa.Test.Collection.Int8Tests.Ldelem", index, value));
		}

		[Theory]
		[MemberData("I4SmallI1", DisableDiscoveryEnumeration = true)]
		public void LdelemaI1(int index, sbyte value)
		{
			Assert.Equal(Int8Tests.Ldelema(index, value), Run<sbyte>("Mosa.Test.Collection.Int8Tests.Ldelema", index, value));
		}
	}
}
