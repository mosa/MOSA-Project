// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class DelegateFixture : X86TestFixture
	{
		[Fact]
		public void DefineDelegate()
		{
			Assert.Equal(DelegateTests.DefineDelegate(), Run<bool>("Mosa.Test.Collection.DelegateTests.DefineDelegate"));
		}

		[Fact]
		public void CallDelegateVoid1()
		{
			Assert.Equal(DelegateTests.CallDelegateVoid1(), Run<int>("Mosa.Test.Collection.DelegateTests.CallDelegateVoid1"));
		}

		[Fact]
		public void CallDelegateVoid2()
		{
			Assert.Equal(DelegateTests.CallDelegateVoid2(), Run<int>("Mosa.Test.Collection.DelegateTests.CallDelegateVoid2"));
		}

		[Fact]
		public void ReassignDelegateVoid()
		{
			Assert.Equal(DelegateTests.ReassignDelegateVoid(), Run<int>("Mosa.Test.Collection.DelegateTests.ReassignDelegateVoid"));
		}

		[Theory]
		[MemberData("I4I4", DisableDiscoveryEnumeration = true)]
		public void CallDelegateParameters(int a, int b)
		{
			Assert.Equal(DelegateTests.CallDelegateParameters(a, b), Run<int>("Mosa.Test.Collection.DelegateTests.CallDelegateParameters", a, b));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void CallDelegateReturn(int a)
		{
			Assert.Equal(DelegateTests.CallDelegateReturn(a), Run<int>("Mosa.Test.Collection.DelegateTests.CallDelegateReturn", a));
		}

		[Theory]
		[MemberData("I4I4", DisableDiscoveryEnumeration = true)]
		public void CallDelegateParametersReturn(int a, int b)
		{
			Assert.Equal(DelegateTests.CallDelegateParametersReturn(a, b), Run<int>("Mosa.Test.Collection.DelegateTests.CallDelegateParametersReturn", a, b));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void CallDelegateBox(int a)
		{
			Assert.Equal(DelegateTests.CallDelegateBox(a), Run<int>("Mosa.Test.Collection.DelegateTests.CallDelegateBox", a));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void CallDelegateGenericReturn(int a)
		{
			Assert.Equal(DelegateTests.CallDelegateGenericReturn(a), Run<int>("Mosa.Test.Collection.DelegateTests.CallDelegateGenericReturn", a));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void CallDelegateGenericReturnStructA(int a)
		{
			Assert.Equal(DelegateTests.CallDelegateGenericReturnStructA(a), Run<int>("Mosa.Test.Collection.DelegateTests.CallDelegateGenericReturnStructA", a));
		}

		[Fact]
		public void CallDelegateGenericReturnStructB()
		{
			Assert.Equal(DelegateTests.CallDelegateGenericReturnStructB(), Run<bool>("Mosa.Test.Collection.DelegateTests.CallDelegateGenericReturnStructB"));
		}

		[Fact]
		public void CallInstanceDelegate()
		{
			Assert.Equal(DelegateTests.CallInstanceDelegate(), Run<int>("Mosa.Test.Collection.DelegateTests.CallInstanceDelegate"));
		}

		[Fact]
		public void CallInstanceDelegateStatic()
		{
			Assert.Equal(DelegateTests.CallInstanceDelegateStatic(), Run<int>("Mosa.Test.Collection.DelegateTests.CallInstanceDelegateStatic"));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void TestInstanceDelegate1(int a)
		{
			Assert.Equal(DelegateTests.TestInstanceDelegate1(a), Run<int>("Mosa.Test.Collection.DelegateTests.TestInstanceDelegate1", a));
		}

		[Theory]
		[MemberData("I4I4", DisableDiscoveryEnumeration = true)]
		public void TestInstanceDelegate2(int a, int b)
		{
			Assert.Equal(DelegateTests.TestInstanceDelegate2(a, b), Run<int>("Mosa.Test.Collection.DelegateTests.TestInstanceDelegate2", a, b));
		}
	}
}
