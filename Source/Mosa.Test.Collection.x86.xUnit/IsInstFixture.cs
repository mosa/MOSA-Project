// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class IsInstFixture : X86TestFixture
	{
		[Fact]
		public void IsInstAAToAA()
		{
			Assert.Equal(IsInstTests.IsInstAAToAA(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstAAToAA"));
		}

		[Fact]
		public void IsInstBBToAA()
		{
			Assert.Equal(IsInstTests.IsInstBBToAA(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstBBToAA"));
		}

		[Fact]
		public void IsInstCCToAA()
		{
			Assert.Equal(IsInstTests.IsInstCCToAA(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstCCToAA"));
		}

		[Fact]
		public void IsInstCCToBB()
		{
			Assert.Equal(IsInstTests.IsInstCCToBB(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstCCToBB"));
		}

		[Fact]
		public void IsInstDDToAA()
		{
			Assert.Equal(IsInstTests.IsInstDDToAA(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstDDToAA"));
		}

		[Fact]
		public void IsInstDDToBB()
		{
			Assert.Equal(IsInstTests.IsInstDDToBB(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstDDToBB"));
		}

		[Fact]
		public void IsInstDDToCC()
		{
			Assert.Equal(IsInstTests.IsInstDDToCC(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstDDToCC"));
		}

		[Fact]
		public void IsInstAAtoIAA()
		{
			Assert.Equal(IsInstTests.IsInstAAtoIAA(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstAAtoIAA"));
		}

		[Fact]
		public void IsInstBBToIAA()
		{
			Assert.Equal(IsInstTests.IsInstBBToIAA(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstBBToIAA"));
		}

		[Fact]
		public void IsInstCCToIAA()
		{
			Assert.Equal(IsInstTests.IsInstCCToIAA(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstCCToIAA"));
		}

		[Fact]
		public void IsInstCCToIBB()
		{
			Assert.Equal(IsInstTests.IsInstCCToIBB(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstCCToIBB"));
		}

		[Theory]
		[MemberData("I4", DisableDiscoveryEnumeration = true)]
		public void IsInstI4ToI4(int i)
		{
			Assert.Equal(IsInstTests.IsInstI4ToI4(i), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstI4ToI4", i));
		}

		[Fact]
		public void IsInstU4ToI4()
		{
			Assert.Equal(IsInstTests.IsInstU4ToI4(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstU4ToI4"));
		}

		[Fact]
		public void IsInstI8ToI8()
		{
			Assert.Equal(IsInstTests.IsInstI8ToI8(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstI8ToI8"));
		}

		[Fact]
		public void IsInstU8ToU8()
		{
			Assert.Equal(IsInstTests.IsInstU8ToU8(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstU8ToU8"));
		}

		[Fact]
		public void IsInstI4ToU4()
		{
			Assert.Equal(IsInstTests.IsInstI4ToU4(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstI4ToU4"));
		}

		[Fact]
		public void IsInstI1ToI1()
		{
			Assert.Equal(IsInstTests.IsInstI1ToI1(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstI1ToI1"));
		}

		[Fact]
		public void IsInstI2ToI2()
		{
			Assert.Equal(IsInstTests.IsInstI2ToI2(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstI2ToI2"));
		}

		[Fact]
		public void IsInstU1ToU1()
		{
			Assert.Equal(IsInstTests.IsInstU1ToU1(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstU1ToU1"));
		}

		[Fact]
		public void IsInstU2ToU2()
		{
			Assert.Equal(IsInstTests.IsInstU2ToU2(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstU2ToU2"));
		}

		[Fact]
		public void IsInstCToC()
		{
			Assert.Equal(IsInstTests.IsInstCToC(), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstCToC"));
		}

		[Theory]
		[MemberData("B", DisableDiscoveryEnumeration = true)]
		public void IsInstBToB(bool b)
		{
			Assert.Equal(IsInstTests.IsInstBToB(b), Run<bool>("Mosa.Test.Collection.IsInstTests.IsInstBToB", b));
		}
	}
}
