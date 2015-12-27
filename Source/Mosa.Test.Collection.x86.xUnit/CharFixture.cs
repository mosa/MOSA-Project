// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class CharFixture : X86TestFixture
	{
		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void AddCC(char a, char b)
		{
			Assert.Equal(CharTests.AddCC(a, b), Run<int>("Mosa.Test.Collection.CharTests.AddCC", a, b));
		}

		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void SubCC(char a, char b)
		{
			Assert.Equal(CharTests.SubCC(a, b), Run<int>("Mosa.Test.Collection.CharTests.SubCC", a, b));
		}

		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void MulCC(char a, char b)
		{
			Assert.Equal(CharTests.MulCC(a, b), Run<int>("Mosa.Test.Collection.CharTests.MulCC", a, b));
		}

		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void DivCC(char a, char b)
		{
			if (b == 0)
			{
				return;
			}

			Assert.Equal(CharTests.DivCC(a, b), Run<int>("Mosa.Test.Collection.CharTests.DivCC", a, b));
		}

		//[Theory]
		public void DivCCDivideByZeroException(char a)
		{
			Assert.Equal(CharTests.DivCC(a, (char)0), Run<int>("Mosa.Test.Collection.CharTests.DivCC", a, (char)0));
		}

		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void RemCC(char a, char b)
		{
			if (b == 0)
			{
				return;
			}

			Assert.Equal(CharTests.RemCC(a, b), Run<int>("Mosa.Test.Collection.CharTests.RemCC", a, b));
		}

		//[Theory]
		public void RemCCDivideByZeroException(char a, char b)
		{
			Assert.Equal(CharTests.RemCC(a, (char)0), Run<int>("Mosa.Test.Collection.CharTests.RemCC", a, (char)0));
		}

		[Theory]
		[MemberData("C", DisableDiscoveryEnumeration = true)]
		public void RetC(char a)
		{
			Assert.Equal(CharTests.RetC(a), Run<char>("Mosa.Test.Collection.CharTests.RetC", a));
		}

		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void AndCC(char a, char b)
		{
			Assert.Equal(CharTests.AndCC(a, b), Run<int>("Mosa.Test.Collection.CharTests.AndCC", a, b));
		}

		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void OrCC(char a, char b)
		{
			Assert.Equal(CharTests.OrCC(a, b), Run<int>("Mosa.Test.Collection.CharTests.OrCC", a, b));
		}

		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void XorCC(char a, char b)
		{
			Assert.Equal(CharTests.XorCC(a, b), Run<int>("Mosa.Test.Collection.CharTests.XorCC", a, b));
		}

		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void CeqCC(char a, char b)
		{
			Assert.Equal(CharTests.CeqCC(a, b), Run<bool>("Mosa.Test.Collection.CharTests.CeqCC", a, b));
		}

		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void CltCC(char a, char b)
		{
			Assert.Equal(CharTests.CltCC(a, b), Run<bool>("Mosa.Test.Collection.CharTests.CltCC", a, b));
		}

		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void CgtCC(char a, char b)
		{
			Assert.Equal(CharTests.CgtCC(a, b), Run<bool>("Mosa.Test.Collection.CharTests.CgtCC", a, b));
		}

		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void CleCC(char a, char b)
		{
			Assert.Equal(CharTests.CleCC(a, b), Run<bool>("Mosa.Test.Collection.CharTests.CleCC", a, b));
		}

		[Theory]
		[MemberData("CC", DisableDiscoveryEnumeration = true)]
		public void CgeCC(char a, char b)
		{
			Assert.Equal(CharTests.CgeCC(a, b), Run<bool>("Mosa.Test.Collection.CharTests.CgeCC", a, b));
		}

		[Fact]
		public void Newarr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.CharTests.Newarr"));
		}

		[Theory]
		[MemberData("I4Small", DisableDiscoveryEnumeration = true)]
		public void Ldlen(int length)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.CharTests.Ldlen", length));
		}

		[Theory]
		[MemberData("I4SmallC", DisableDiscoveryEnumeration = true)]
		public void StelemC(int index, char value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.CharTests.Stelem", index, value));
		}

		[Theory]
		[MemberData("I4SmallC", DisableDiscoveryEnumeration = true)]
		public void LdelemC(int index, char value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.CharTests.Ldelem", index, value));
		}

		[Theory]
		[MemberData("I4SmallC", DisableDiscoveryEnumeration = true)]
		public void LdelemaC(int index, char value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.CharTests.Ldelema", index, value));
		}
	}
}
