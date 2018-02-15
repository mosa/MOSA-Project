// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class CompilerFixture : TestFixture
	{
		[Theory]
		[MemberData(nameof(I4))]
		public void even_mod(int a)
		{
			Assert.Equal(CompilerTests.even_mod(a), Run<int>("Mosa.UnitTest.Collection.CompilerTests.even_mod", a));
		}

		[Theory]
		[MemberData(nameof(U4))]
		public void even_unsigned(uint a)
		{
			Assert.Equal(CompilerTests.even_unsigned(a), Run<uint>("Mosa.UnitTest.Collection.CompilerTests.even_unsigned", a));
		}

		[Theory]
		[MemberData(nameof(I4))]
		public void even_bit(int a)
		{
			Assert.Equal(CompilerTests.even_bit(a), Run<int>("Mosa.UnitTest.Collection.CompilerTests.even_bit", a));
		}

		[Theory]
		[MemberData(nameof(U4))]
		public void even_bool(uint a)
		{
			Assert.Equal(CompilerTests.even_bool(a), Run<bool>("Mosa.UnitTest.Collection.CompilerTests.even_bool", a));
		}

		[Theory]
		[MemberData(nameof(U4))]
		public void aligned4_bool(uint a)
		{
			Assert.Equal(CompilerTests.aligned4_bool(a), Run<bool>("Mosa.UnitTest.Collection.CompilerTests.aligned4_bool", a));
		}

		[Theory]
		[MemberData(nameof(U4))]
		public void aligned4_unsigned(uint a)
		{
			Assert.Equal(CompilerTests.aligned4_unsigned(a), Run<uint>("Mosa.UnitTest.Collection.CompilerTests.aligned4_unsigned", a));
		}
	}
}
