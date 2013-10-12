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
	public class UInt64Fixture : TestFixture
	{
		[Theory]
		[PropertyData("U8U8")]
		public void AddU8U8(ulong a, ulong b)
		{
			Assert.Equal(UInt64Tests.AddU8U8(a, b), Run<ulong>("Mosa.Test.Collection.UInt64Tests.AddU8U8", a, b));
		}

		[Theory]
		[PropertyData("U8U8")]
		public void SubU8U8(ulong a, ulong b)
		{
			Assert.Equal(UInt64Tests.SubU8U8(a, b), Run<ulong>("Mosa.Test.Collection.UInt64Tests.SubU8U8", a, b));
		}

		[Theory]
		[PropertyData("U8U8")]
		public void MulU8U8(ulong a, ulong b)
		{
			Assert.Equal(UInt64Tests.MulU8U8(a, b), Run<ulong>("Mosa.Test.Collection.UInt64Tests.MulU8U8", a, b));
		}

		[Theory]
		[PropertyData("U8U8")]
		public void DivU8U8(ulong a, ulong b)
		{
			if (b == 0)
			{
				return;
			}

			Assert.Equal(UInt64Tests.DivU8U8(a, b), Run<ulong>("Mosa.Test.Collection.UInt64Tests.DivU8U8", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void DivU8U8DivideByZeroException(ulong a)
		{
			Assert.Equal(UInt64Tests.DivU8U8(a, (ulong)0), Run<ulong>("Mosa.Test.Collection.UInt64Tests.DivU8U8", a, (ulong)0));
		}

		[Theory]
		[PropertyData("U8U8")]
		public void RemU8U8(ulong a, ulong b)
		{
			if (b == 0)
			{
				return;
			}

			Assert.Equal(UInt64Tests.RemU8U8(a, b), Run<ulong>("Mosa.Test.Collection.UInt64Tests.RemU8U8", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void RemU8U8DivideByZeroException(ulong a)
		{
			Assert.Equal(UInt64Tests.RemU8U8(a, (ulong)0), Run<ulong>("Mosa.Test.Collection.UInt64Tests.RemU8U8", a, (ulong)0));
		}

		[Theory]
		[PropertyData("U8")]
		public void RetU8(ulong a)
		{
			Assert.Equal(UInt64Tests.RetU8(a), Run<ulong>("Mosa.Test.Collection.UInt64Tests.RetU8", a));
		}

		[Theory]
		[PropertyData("U8U8")]
		public void AndU8U8(ulong a, ulong b)
		{
			Assert.Equal(UInt64Tests.AndU8U8(a, b), Run<ulong>("Mosa.Test.Collection.UInt64Tests.AndU8U8", a, b));
		}

		[Theory]
		[PropertyData("U8U8")]
		public void OrU8U8(ulong a, ulong b)
		{
			Assert.Equal(UInt64Tests.OrU8U8(a, b), Run<ulong>("Mosa.Test.Collection.UInt64Tests.OrU8U8", a, b));
		}

		[Theory]
		[PropertyData("U8U8")]
		public void XorU8U8(ulong a, ulong b)
		{
			Assert.Equal(UInt64Tests.XorU8U8(a, b), Run<ulong>("Mosa.Test.Collection.UInt64Tests.XorU8U8", a, b));
		}

		[Theory]
		[PropertyData("U8")]
		public void CompU8(ulong a)
		{
			Assert.Equal(UInt64Tests.CompU8(a), Run<ulong>("Mosa.Test.Collection.UInt64Tests.CompU8", a));
		}

		[Theory]
		[PropertyData("U8U1UpTo32")]
		public void ShiftLeftU8U1(ulong a, byte b)
		{
			Assert.Equal(UInt64Tests.ShiftLeftU8U1(a, b), Run<ulong>("Mosa.Test.Collection.UInt64Tests.ShiftLeftU8U1", a, b));
		}

		[Theory]
		[PropertyData("U8U1UpTo32")]
		public void ShiftRightU8U1(ulong a, byte b)
		{
			Assert.Equal(UInt64Tests.ShiftRightU8U1(a, b), Run<ulong>("Mosa.Test.Collection.UInt64Tests.ShiftRightU8U1", a, b));
		}

		[Theory]
		[PropertyData("U8U8")]
		public void CeqU8U8(ulong a, ulong b)
		{
			Assert.Equal(UInt64Tests.CeqU8U8(a, b), Run<bool>("Mosa.Test.Collection.UInt64Tests.CeqU8U8", a, b));
		}

		[Theory]
		[PropertyData("U8U8")]
		public void CltU8U8(ulong a, ulong b)
		{
			Assert.Equal(UInt64Tests.CltU8U8(a, b), Run<bool>("Mosa.Test.Collection.UInt64Tests.CltU8U8", a, b));
		}

		[Theory]
		[PropertyData("U8U8")]
		public void CgtU8U8(ulong a, ulong b)
		{
			Assert.Equal(UInt64Tests.CgtU8U8(a, b), Run<bool>("Mosa.Test.Collection.UInt64Tests.CgtU8U8", a, b));
		}

		[Theory]
		[PropertyData("U8U8")]
		public void CleU8U8(ulong a, ulong b)
		{
			Assert.Equal(UInt64Tests.CleU8U8(a, b), Run<bool>("Mosa.Test.Collection.UInt64Tests.CleU8U8", a, b));
		}

		[Theory]
		[PropertyData("U8U8")]
		public void CgeU8U8(ulong a, ulong b)
		{
			Assert.Equal(UInt64Tests.CgeU8U8(a, b), Run<bool>("Mosa.Test.Collection.UInt64Tests.CgeU8U8", a, b));
		}

		[Fact]
		public void Newarr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt64Tests.Newarr"));
		}

		[Theory]
		[PropertyData("I4Small")]
		public void Ldlen(int length)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt64Tests.Ldlen", length));
		}

		[Theory]
		[PropertyData("I4SmallU8")]
		public void StelemU8(int index, ulong value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt64Tests.Stelem", index, value));
		}

		[Theory]
		[PropertyData("I4SmallU8")]
		public void LdelemU8(int index, ulong value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt64Tests.Ldelem", index, value));
		}

		[Theory]
		[PropertyData("I4SmallU8")]
		public void LdelemaU8(int index, ulong value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt64Tests.Ldelema", index, value));
		}
	}
}