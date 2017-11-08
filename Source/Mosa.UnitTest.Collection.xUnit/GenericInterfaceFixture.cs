// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class GenericInterfaceFixture : TestFixture
	{
		[Theory]
		[MemberData(nameof(I4), DisableDiscoveryEnumeration = true)]
		public void InterfaceTest1(int i)
		{
			Assert.Equal(GenericInterfaceTests.InterfaceTest1(i), Run<int>("Mosa.UnitTest.Collection.GenericInterfaceTests.InterfaceTest1", i));
		}

		[Theory]
		[MemberData(nameof(I4), DisableDiscoveryEnumeration = true)]
		public void InterfaceTest2(int i)
		{
			Assert.Equal(GenericInterfaceTests.InterfaceTest2(i), Run<int>("Mosa.UnitTest.Collection.GenericInterfaceTests.InterfaceTest2", i));
		}

		[Theory]
		[MemberData(nameof(I4), DisableDiscoveryEnumeration = true)]
		public void InterfaceTest3(int i)
		{
			Assert.Equal(GenericInterfaceTests.InterfaceTest3(i), Run<int>("Mosa.UnitTest.Collection.GenericInterfaceTests.InterfaceTest3", i));
		}
	}
}
