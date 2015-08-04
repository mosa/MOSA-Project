// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class Int32Fixture : X86TestFixture
	{
		[Theory]
		[PropertyData("I4I4")]
		public void AddI4I4(int a, int b)
		{
			Assert.Equal(Int32Tests.AddI4I4(a, b), Run<int>("Mosa.Test.Collection.Int32Tests.AddI4I4", a, b));
		}

		[Theory]
		[PropertyData("I4I4")]
		public void SubI4I4(int a, int b)
		{
			Assert.Equal(Int32Tests.SubI4I4(a, b), Run<int>("Mosa.Test.Collection.Int32Tests.SubI4I4", a, b));
		}

		[Theory]
		[PropertyData("I4I4")]
		public void MulI4I4(int a, int b)
		{
			Assert.Equal(Int32Tests.MulI4I4(a, b), Run<int>("Mosa.Test.Collection.Int32Tests.MulI4I4", a, b));
		}

		[Theory]
		[PropertyData("I4I4")]
		public void DivI4I4(int a, int b)
		{
			if (a == int.MinValue && b == -1)
			{
				//	Assert.Inconclusive("TODO: Overflow exception not implemented");
				return;
			}

			if (b == 0)
			{
				return;
			}

			Assert.Equal(Int32Tests.DivI4I4(a, b), Run<int>("Mosa.Test.Collection.Int32Tests.DivI4I4", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void DivI4I4DivideByZeroException(int a)
		{
			Assert.Equal(Int32Tests.DivI4I4(a, (int)0), Run<int>("Mosa.Test.Collection.Int32Tests.DivI4I4", a, (int)0));
		}

		[Theory]
		[PropertyData("I4I4")]
		public void RemI4I4(int a, int b)
		{
			if (a == int.MinValue && b == -1)
			{
				//Assert.Inconclusive("TODO: Overflow exception not implemented");
				return;
			}

			if (b == 0)
			{
				return;
			}

			Assert.Equal(Int32Tests.RemI4I4(a, b), Run<int>("Mosa.Test.Collection.Int32Tests.RemI4I4", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void RemI4I4DivideByZeroException(int a)
		{
			Assert.Equal(Int32Tests.RemI4I4(a, (int)0), Run<int>("Mosa.Test.Collection.Int32Tests.RemI4I4", a, (int)0));
		}

		[Theory]
		[PropertyData("I4")]
		public void RetI4(int a)
		{
			Assert.Equal(Int32Tests.RetI4(a), Run<int>("Mosa.Test.Collection.Int32Tests.RetI4", a));
		}

		[Theory]
		[PropertyData("I4I4")]
		public void AndI4I4(int a, int b)
		{
			Assert.Equal(Int32Tests.AndI4I4(a, b), Run<int>("Mosa.Test.Collection.Int32Tests.AndI4I4", a, b));
		}

		[Theory]
		[PropertyData("I4I4")]
		public void OrI4I4(int a, int b)
		{
			Assert.Equal(Int32Tests.OrI4I4(a, b), Run<int>("Mosa.Test.Collection.Int32Tests.OrI4I4", a, b));
		}

		[Theory]
		[PropertyData("I4I4")]
		public void XorI4I4(int a, int b)
		{
			Assert.Equal(Int32Tests.XorI4I4(a, b), Run<int>("Mosa.Test.Collection.Int32Tests.XorI4I4", a, b));
		}

		//TODO: Shifts

		[Theory]
		[PropertyData("I4I4")]
		public void CeqI4I4(int a, int b)
		{
			Assert.Equal(Int32Tests.CeqI4I4(a, b), Run<bool>("Mosa.Test.Collection.Int32Tests.CeqI4I4", a, b));
		}

		[Theory]
		[PropertyData("I4I4")]
		public void CltI4I4(int a, int b)
		{
			Assert.Equal(Int32Tests.CltI4I4(a, b), Run<bool>("Mosa.Test.Collection.Int32Tests.CltI4I4", a, b));
		}

		[Theory]
		[PropertyData("I4I4")]
		public void CgtI4I4(int a, int b)
		{
			Assert.Equal(Int32Tests.CgtI4I4(a, b), Run<bool>("Mosa.Test.Collection.Int32Tests.CgtI4I4", a, b));
		}

		[Theory]
		[PropertyData("I4I4")]
		public void CleI4I4(int a, int b)
		{
			Assert.Equal(Int32Tests.CleI4I4(a, b), Run<bool>("Mosa.Test.Collection.Int32Tests.CleI4I4", a, b));
		}

		[Theory]
		[PropertyData("I4I4")]
		public void CgeI4I4(int a, int b)
		{
			Assert.Equal(Int32Tests.CgeI4I4(a, b), Run<bool>("Mosa.Test.Collection.Int32Tests.CgeI4I4", a, b));
		}

		[Fact]
		public void Newarr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.Int32Tests.Newarr"));
		}

		[Theory]
		[PropertyData("I4Small")]
		public void Ldlen(int length)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.Int32Tests.Ldlen", length));
		}

		[Theory]
		[PropertyData("I4SmallI4")]
		public void StelemI4(int index, int value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection.Int32Tests.Stelem", index, value));
		}

		[Theory]
		[PropertyData("I4SmallI4")]
		public void LdelemI4(int index, int value)
		{
			Assert.Equal(Int32Tests.Ldelem(index, value), Run<int>("Mosa.Test.Collection.Int32Tests.Ldelem", index, value));
		}

		[Theory]
		[PropertyData("I4SmallI4")]
		public void LdelemaI4(int index, int value)
		{
			Assert.Equal(Int32Tests.Ldelema(index, value), Run<int>("Mosa.Test.Collection.Int32Tests.Ldelema", index, value));
		}
	}
}
