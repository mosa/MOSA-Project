/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.Test.Collection;
using Xunit;
using Xunit.Extensions;

namespace Mosa.TinyCPUSimulator.TestSystem.xUnit
{
	public class ArrayLayoutFixture : TestFixture
	{
		[Fact]
		public void ArrayB()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.ArrayLayoutTests.B"));
		}

		[Fact]
		public void ArrayC()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.ArrayLayoutTests.C"));
		}

		[Fact]
		public void ArrayU1()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.ArrayLayoutTests.U1"));
		}

		[Fact]
		public void ArrayU2()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.ArrayLayoutTests.U2"));
		}

		[Fact]
		public void ArrayU4()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.ArrayLayoutTests.U4"));
		}

		[Fact]
		public void ArrayU8()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.ArrayLayoutTests.U8"));
		}

		[Fact]
		public void ArrayR4()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.ArrayLayoutTests.R4"));
		}

		[Fact]
		public void ArrayR8()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.ArrayLayoutTests.R8"));
		}
	}
}