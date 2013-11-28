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
	public class UInt16Fixture : TestFixture
	{
		[Theory]
		[PropertyData("U2U2")]
		public void AddU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.AddU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.AddU2U2", a, b));
		}

		[Theory]
		[PropertyData("U2U2")]
		public void SubU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.SubU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.SubU2U2", a, b));
		}

		[Theory]
		[PropertyData("U2U2")]
		public void MulU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.MulU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.MulU2U2", a, b));
		}

		[Theory]
		[PropertyData("U2U2")]
		public void DivU2U2(ushort a, ushort b)
		{
			if (b == 0)
			{
				return;
			}

			Assert.Equal(UInt16Tests.DivU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.DivU2U2", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void DivU2U2DivideByZeroException(ushort a)
		{
			Assert.Equal(UInt16Tests.DivU2U2(a, (ushort)0), Run<int>("Mosa.Test.Collection.UInt16Tests.DivU2U2", a, (ushort)0));
		}

		[Theory]
		[PropertyData("U2U2")]
		public void RemU2U2(ushort a, ushort b)
		{
			if (b == 0)
			{
				return;
			}

			Assert.Equal(UInt16Tests.RemU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.RemU2U2", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void RemU2U2DivideByZeroException(ushort a)
		{
			Assert.Equal(UInt16Tests.RemU2U2(a, (ushort)0), Run<int>("Mosa.Test.Collection.UInt16Tests.RemU2U2", a, (ushort)0));
		}

		[Theory]
		[PropertyData("U2")]
		public void RetU2(ushort a)
		{
			Assert.Equal(UInt16Tests.RetU2(a), Run<ushort>("Mosa.Test.Collection.UInt16Tests.RetU2", a));
		}

		[Theory]
		[PropertyData("U2U2")]
		public void AndU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.AndU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.AndU2U2", a, b));
		}

		[Theory]
		[PropertyData("U2U2")]
		public void OrU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.OrU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.OrU2U2", a, b));
		}

		[Theory]
		[PropertyData("U2U2")]
		public void XorU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.XorU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.XorU2U2", a, b));
		}

		[Theory]
		[PropertyData("U2")]
		public void CompU2(ushort a)
		{
			Assert.Equal(UInt16Tests.CompU2(a), Run<int>("Mosa.Test.Collection.UInt16Tests.CompU2", a));
		}

		[Theory]
		[PropertyData("U2U1UpTo16")]
		public void ShiftLeftU2U2(ushort a, byte b)
		{
			Assert.Equal(UInt16Tests.ShiftLeftU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.ShiftLeftU2U2", a, b));
		}

		[Theory]
		[PropertyData("U2U1UpTo16")]
		public void ShiftRightU2U2(ushort a, byte b)
		{
			Assert.Equal(UInt16Tests.ShiftRightU2U2(a, b), Run<int>("Mosa.Test.Collection.UInt16Tests.ShiftRightU2U2", a, b));
		}

		[Theory]
		[PropertyData("U2U2")]
		public void CeqU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.CeqU2U2(a, b), Run<bool>("Mosa.Test.Collection.UInt16Tests.CeqU2U2", a, b));
		}

		[Theory]
		[PropertyData("U2U2")]
		public void CltU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.CltU2U2(a, b), Run<bool>("Mosa.Test.Collection.UInt16Tests.CltU2U2", a, b));
		}

		[Theory]
		[PropertyData("U2U2")]
		public void CgtU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.CgtU2U2(a, b), Run<bool>("Mosa.Test.Collection.UInt16Tests.CgtU2U2", a, b));
		}

		[Theory]
		[PropertyData("U2U2")]
		public void CleU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.CleU2U2(a, b), Run<bool>("Mosa.Test.Collection.UInt16Tests.CleU2U2", a, b));
		}

		[Theory]
		[PropertyData("U2U2")]
		public void CgeU2U2(ushort a, ushort b)
		{
			Assert.Equal(UInt16Tests.CgeU2U2(a, b), Run<bool>("Mosa.Test.Collection.UInt16Tests.CgeU2U2", a, b));
		}

		[Fact]
		public void Newarr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt16Tests.Newarr"));
		}

		[Theory]
		[PropertyData("I4Small")]
		public void Ldlen(int length)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt16Tests.Ldlen", length));
		}

		[Theory]
		[PropertyData("I4SmallU2")]
		public void StelemU2(int index, ushort value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt16Tests.Stelem", index, value));
		}

		[Theory]
		[PropertyData("I4SmallU2")]
		public void LdelemU2(int index, ushort value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt16Tests.Ldelem", index, value));
		}

		[Theory]
		[PropertyData("I4SmallU2")]
		public void LdelemaU2(int index, ushort value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.UInt16Tests.Ldelema", index, value));
		}
	}
}