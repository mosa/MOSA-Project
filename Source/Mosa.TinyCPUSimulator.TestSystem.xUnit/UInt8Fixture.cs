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
	public class UInt8Fixture : TestFixture
	{
		[Theory]
		[PropertyData("U1U1")]
		public void AddU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.AddU1U1(a, b), Run<int>("Mosa.Test.Collection.UInt8Tests.AddU1U1", a, b));
		}

		[Theory]
		[PropertyData("U1U1")]
		public void SubU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.SubU1U1(a, b), Run<int>("Mosa.Test.Collection.UInt8Tests.SubU1U1", a, b));
		}

		[Theory]
		[PropertyData("U1U1")]
		public void MulU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.MulU1U1(a, b), Run<int>("Mosa.Test.Collection.UInt8Tests.MulU1U1", a, b));
		}

		[Theory]
		[PropertyData("U1U1")]
		public void DivU1U1(byte a, byte b)
		{
			if (b == 0)
			{
				return;
			}

			Assert.Equal(UInt8Tests.DivU1U1(a, b), Run<int>("Mosa.Test.Collection.UInt8Tests.DivU1U1", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void DivU1U1DivideByZeroException(byte a)
		{
			Assert.Equal(UInt8Tests.DivU1U1(a, (byte)0), Run<int>("Mosa.Test.Collection.UInt8Tests.DivU1U1", a, (byte)0));
		}

		[Theory]
		[PropertyData("U1U1")]
		public void RemU1U1(byte a, byte b)
		{
			if (b == 0)
			{
				return;
			}

			Assert.Equal(UInt8Tests.RemU1U1(a, b), Run<int>("Mosa.Test.Collection.UInt8Tests.RemU1U1", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void RemU1U1DivideByZeroException(byte a)
		{
			Assert.Equal(UInt8Tests.RemU1U1(a, (byte)0), Run<int>("Mosa.Test.Collection.UInt8Tests.RemU1U1", a, (byte)0));
		}

		[Theory]
		[PropertyData("U1")]
		public void RetU1(byte a)
		{
			Assert.Equal(UInt8Tests.RetU1(a), Run<byte>("Mosa.Test.Collection.UInt8Tests.RetU1", a));
		}

		[Theory]
		[PropertyData("U1U1")]
		public void AndU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.AndU1U1(a, b), Run<int>("Mosa.Test.Collection.UInt8Tests.AndU1U1", a, b));
		}

		[Theory]
		[PropertyData("U1U1")]
		public void OrU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.OrU1U1(a, b), Run<int>("Mosa.Test.Collection.UInt8Tests.OrU1U1", a, b));
		}

		[Theory]
		[PropertyData("U1U1")]
		public void XorU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.XorU1U1(a, b), Run<int>("Mosa.Test.Collection.UInt8Tests.XorU1U1", a, b));
		}

		[Theory]
		[PropertyData("U1")]
		public void CompU1(byte a)
		{
			Assert.Equal(UInt8Tests.CompU1(a), Run<int>("Mosa.Test.Collection.UInt8Tests.CompU1", a));
		}

		[Theory]
		[PropertyData("U1U1UpTo16")]
		public void ShiftLeftU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.ShiftLeftU1U1(a, b), Run<int>("Mosa.Test.Collection.UInt8Tests.ShiftLeftU1U1", a, b));
		}

		[Theory]
		[PropertyData("U1U1UpTo16")]
		public void ShiftRightU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.ShiftRightU1U1(a, b), Run<int>("Mosa.Test.Collection.UInt8Tests.ShiftRightU1U1", a, b));
		}

		[Theory]
		[PropertyData("U1U1")]
		public void CeqU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.CeqU1U1(a, b), Run<bool>("Mosa.Test.Collection.UInt8Tests.CeqU1U1", a, b));
		}

		[Theory]
		[PropertyData("U1U1")]
		public void CltU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.CltU1U1(a, b), Run<bool>("Mosa.Test.Collection.UInt8Tests.CltU1U1", a, b));
		}

		[Theory]
		[PropertyData("U1U1")]
		public void CgtU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.CgtU1U1(a, b), Run<bool>("Mosa.Test.Collection.UInt8Tests.CgtU1U1", a, b));
		}

		[Theory]
		[PropertyData("U1U1")]
		public void CleU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.CleU1U1(a, b), Run<bool>("Mosa.Test.Collection.UInt8Tests.CleU1U1", a, b));
		}

		[Theory]
		[PropertyData("U1U1")]
		public void CgeU1U1(byte a, byte b)
		{
			Assert.Equal(UInt8Tests.CgeU1U1(a, b), Run<bool>("Mosa.Test.Collection.UInt8Tests.CgeU1U1", a, b));
		}

		[Fact]
		public void Newarr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt8Tests.Newarr"));
		}

		[Theory]
		[PropertyData("I4Small")]
		public void Ldlen(int length)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt8Tests.Ldlen", length));
		}

		[Theory]
		[PropertyData("I4SmallU1")]
		public void StelemU1(int index, byte value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt8Tests.Stelem", index, value));
		}

		[Theory]
		[PropertyData("I4SmallU1")]
		public void LdelemU1(int index, byte value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt8Tests.Ldelem", index, value));
		}

		[Theory]
		[PropertyData("I4SmallU1")]
		public void LdelemaU1(int index, byte value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt8Tests.Ldelema", index, value));
		}
	}
}