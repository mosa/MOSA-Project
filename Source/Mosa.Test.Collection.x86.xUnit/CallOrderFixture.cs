/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class CallOrderFixture : X86TestFixture
	{
		//[Fact]
		public void CallEmpty()
		{
		}

		[Theory]
		[PropertyData("I4")]
		public void CallOrderI4(int a)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderI4(a), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderI4", a));
		}

		[Theory]
		[PropertyData("I4I4")]
		public void CallOrderI4I4(int a, int b)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderI4I4(a, b), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderI4I4", a, b));
		}

		[Theory]
		[PropertyData("I4I4")]
		public void CallOrderI4I4_2(int a, int b)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderI4I4_2(a, b), Run<int>("Mosa.Test.Collection.CallOrderTests.CallOrderI4I4_2", a, b));
		}

		[Theory]
		[PropertyData("U4U4")]
		public void CallOrderU4U4(uint a, uint b)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderU4U4(a, b), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderU4U4", a, b));
		}

		[Theory]
		[PropertyData("U4U4")]
		public void CallOrderU4U4_2(uint a, uint b)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderU4U4_2(a, b), Run<uint>("Mosa.Test.Collection.CallOrderTests.CallOrderU4U4_2", a, b));
		}

		[Theory]
		[PropertyData("I4MiniI4MiniI4Mini")]
		public void CallOrderI4I4I4(int a, int b, int c)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderI4I4I4(a, b, c), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderI4I4I4", a, b, c));
		}

		[Theory]
		[PropertyData("I4MiniI4MiniI4MiniI4Mini")]
		public void CallOrderI4I4I4I4(int a, int b, int c, int d)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderI4I4I4I4(a, b, c, d), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderI4I4I4I4", a, b, c, d));
		}

		[Theory]
		[PropertyData("I8MiniI8MiniI8MiniI8Mini")]
		public void CallOrderI8I8I8I8(long a, long b, long c, long d)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderI8I8I8I8(a, b, c, d), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderI8I8I8I8", a, b, c, d));
		}

		[Theory]
		[PropertyData("U8")]
		public void CallOrderU8(ulong a)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderU8(a), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderU8", a));
		}

		[Theory]
		[PropertyData("U8U8")]
		public void CallOrderU8U8(ulong a, ulong b)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderU8U8(a, b), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderU8U8", a, b));
		}

		[Theory]
		[PropertyData("U8MiniU8MiniU8MiniU8Mini")]
		public void CallOrderU8U8U8U8(ulong a, ulong b, ulong c, ulong d)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderU8U8U8U8(a, b, c, d), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderU8U8U8U8", a, b, c, d));
		}

		[Theory]
		[PropertyData("U4MiniU8MiniU8MiniU8Mini")]
		public void CallOrderU4U8U8U8(uint a, ulong b, ulong c, ulong d)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderU4U8U8U8(a, b, c, d), Run<bool>("Mosa.Test.Collection.CallOrderTests.CallOrderU4U8U8U8", a, b, c, d));
		}

		[Theory]
		[PropertyData("I4MiniI4MiniI4MiniI4Mini")]
		public void CallOrderI4I4I4I4_2(int a, int b, int c, int d)
		{
			Assert.Equal(Mosa.Test.Collection.CallOrderTests.CallOrderI4I4I4I4_2(a, b, c, d), Run<int>("Mosa.Test.Collection.CallOrderTests.CallOrderI4I4I4I4_2", a, b, c, d));
		}
	}
}