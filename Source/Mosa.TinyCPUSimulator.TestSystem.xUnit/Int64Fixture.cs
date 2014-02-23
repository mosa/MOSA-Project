/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Test.Collection;
using Xunit;
using Xunit.Extensions;

namespace Mosa.TinyCPUSimulator.TestSystem.xUnit
{
	public class Int64Fixture : TestFixture
	{
		[Theory]
		//[PropertyData("I8I8")]
		[InlineData((long)1, (long)2)]
		public void AddI8I8(long a, long b)
		{
			Assert.Equal(Int64Tests.AddI8I8(a, b), Run<long>("Mosa.Test.Collection.Int64Tests.AddI8I8", a, b));
		}

		[Theory]
		[PropertyData("I8I8")]
		public void SubI8I8(long a, long b)
		{
			Assert.Equal(Int64Tests.SubI8I8(a, b), Run<long>("Mosa.Test.Collection.Int64Tests.SubI8I8", a, b));
		}

		[Theory]
		[PropertyData("I8I8")]
		public void MulI8I8(long a, long b)
		{
			Assert.Equal(Int64Tests.MulI8I8(a, b), Run<long>("Mosa.Test.Collection.Int64Tests.MulI8I8", a, b));
		}

		[Theory]
		[PropertyData("I8I8")]
		public void DivI8I8(long a, long b)
		{
			if (b == 0)
			{
				return;
			}

			Assert.Equal(Int64Tests.DivI8I8(a, b), Run<long>("Mosa.Test.Collection.Int64Tests.DivI8I8", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void DivI8I8DivideByZeroException(long a)
		{
			Assert.Equal(Int64Tests.DivI8I8(a, (long)0), Run<long>("Mosa.Test.Collection.Int64Tests.DivI8I8", a, (long)0));
		}

		[Theory]
		[PropertyData("I8I8")]
		public void RemI8I8(long a, long b)
		{
			if (b == 0)
			{
				return;
			}

			Assert.Equal(Int64Tests.RemI8I8(a, b), Run<long>("Mosa.Test.Collection.Int64Tests.RemI8I8", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void RemI8I8DivideByZeroException(long a)
		{
			Assert.Equal(Int64Tests.RemI8I8(a, (long)0), Run<long>("Mosa.Test.Collection.Int64Tests.RemI8I8", a, (long)0));
		}

		[Theory]
		[PropertyData("I8")]
		public void RetI8(long a)
		{
			Assert.Equal(Int64Tests.RetI8(a), Run<long>("Mosa.Test.Collection.Int64Tests.RetI8", a));
		}

		[Theory]
		[PropertyData("I8I8")]
		public void AndI8I8(long a, long b)
		{
			Assert.Equal(Int64Tests.AndI8I8(a, b), Run<long>("Mosa.Test.Collection.Int64Tests.AndI8I8", a, b));
		}

		[Theory]
		[PropertyData("I8I8")]
		public void OrI8I8(long a, long b)
		{
			Assert.Equal(Int64Tests.OrI8I8(a, b), Run<long>("Mosa.Test.Collection.Int64Tests.OrI8I8", a, b));
		}

		[Theory]
		[PropertyData("I8I8")]
		public void XorI8I8(long a, long b)
		{
			Assert.Equal(Int64Tests.XorI8I8(a, b), Run<long>("Mosa.Test.Collection.Int64Tests.XorI8I8", a, b));
		}

		[Theory]
		[PropertyData("I8")]
		public void CompI8(long a)
		{
			Assert.Equal(Int64Tests.CompI8(a), Run<long>("Mosa.Test.Collection.Int64Tests.CompI8", a));
		}

		[Theory]
		[PropertyData("I8U1UpTo32")]
		public void ShiftLeftI8I8(long a, byte b)
		{
			Assert.Equal(Int64Tests.ShiftLeftI8U1(a, b), Run<long>("Mosa.Test.Collection.Int64Tests.ShiftLeftI8U1", a, b));
		}

		[Theory]
		[PropertyData("I8U1UpTo32")]
		public void ShiftRightI8I8(long a, byte b)
		{
			Assert.Equal(Int64Tests.ShiftRightI8U1(a, b), Run<long>("Mosa.Test.Collection.Int64Tests.ShiftRightI8U1", a, b));
		}

		[Theory]
		[PropertyData("I8I8")]
		public void CeqI8I8(long a, long b)
		{
			Assert.Equal(Int64Tests.CeqI8I8(a, b), Run<bool>("Mosa.Test.Collection.Int64Tests.CeqI8I8", a, b));
		}

		[Theory]
		[PropertyData("I8I8")]
		public void CltI8I8(long a, long b)
		{
			Assert.Equal(Int64Tests.CltI8I8(a, b), Run<bool>("Mosa.Test.Collection.Int64Tests.CltI8I8", a, b));
		}

		[Theory]
		[PropertyData("I8I8")]
		public void CgtI8I8(long a, long b)
		{
			Assert.Equal(Int64Tests.CgtI8I8(a, b), Run<bool>("Mosa.Test.Collection.Int64Tests.CgtI8I8", a, b));
		}

		[Theory]
		[PropertyData("I8I8")]
		public void CleI8I8(long a, long b)
		{
			Assert.Equal(Int64Tests.CleI8I8(a, b), Run<bool>("Mosa.Test.Collection.Int64Tests.CleI8I8", a, b));
		}

		[Theory]
		[PropertyData("I8I8")]
		public void CgeI8I8(long a, long b)
		{
			Assert.Equal(Int64Tests.CgeI8I8(a, b), Run<bool>("Mosa.Test.Collection.Int64Tests.CgeI8I8", a, b));
		}

		[Fact]
		public void Newarr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.Int64Tests.Newarr"));
		}

		[Theory]
		[PropertyData("I4Small")]
		public void Ldlen(int length)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.Int64Tests.Ldlen", length));
		}

		[Theory]
		[PropertyData("I4SmallI8")]
		public void StelemI8(int index, long value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.Int64Tests.Stelem", index, value));
		}

		[Theory]
		[PropertyData("I4SmallI8")]
		public void LdelemI8(int index, long value)
		{
			Assert.Equal(Int64Tests.Ldelem(index, value), Run<long>("Mosa.Test.Collection.Int64Tests.Ldelem", index, value));
		}

		[Theory]
		[PropertyData("I4SmallI8")]
		public void LdelemaI8(int index, long value)
		{
			Assert.Equal(Int64Tests.Ldelema(index, value), Run<long>("Mosa.Test.Collection.Int64Tests.Ldelema", index, value));
		}
	}
}