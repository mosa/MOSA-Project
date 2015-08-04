// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class DoubleFixture : X86TestFixture
	{
		//private static double Tolerance = 0.000001d;
		//private static IComparer<double> target = new ApproximateComparer(Tolerance);

		[Theory]
		[PropertyData("R8R8")]
		public void AddR8R8(double a, double b)
		{
			Assert.Equal(DoubleTests.AddR8R8(a, b), Run<double>("Mosa.Test.Collection", "DoubleTests", "AddR8R8", a, b));
		}

		[Theory]
		[PropertyData("R8R8")]
		public void SubR8R8(double a, double b)
		{
			Assert.Equal(DoubleTests.SubR8R8(a, b), Run<double>("Mosa.Test.Collection", "DoubleTests", "SubR8R8", a, b));
		}

		[Theory]
		[PropertyData("R8R8")]
		public void MulR8R8(double a, double b)
		{
			Assert.Equal(DoubleTests.MulR8R8(a, b), Run<double>("Mosa.Test.Collection", "DoubleTests", "MulR8R8", a, b));
		}

		[Theory]
		[PropertyData("R8R8")]
		public void DivR8R8(double a, double b)
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

			Assert.Equal(DoubleTests.DivR8R8(a, b), Run<double>("Mosa.Test.Collection", "DoubleTests", "DivR8R8", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void DivR8R8DivideByZeroException(double a)
		{
			Run<double>("Mosa.Test.Collection", "DoubleTests", "DivR8R8", (double)0, a, (double)0);
		}

		[Theory]
		[PropertyData("R8R8")]
		public void RemR8R8(double a, double b)
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

			Assert.Equal(DoubleTests.RemR8R8(a, b), Run<double>("Mosa.Test.Collection", "DoubleTests", "RemR8R8", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void RemI4I4DivideByZeroException(int a)
		{
			Run<double>("Mosa.Test.Collection", "DoubleTests", "RemR8R8", (double)0, a, (double)0);
		}

		[Theory]
		[PropertyData("R8SimpleR8Simple")]
		public void CeqR8R8(double a, double b)
		{
			Assert.Equal(DoubleTests.CeqR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CeqR8R8", a, b));
		}

		[Theory]
		[PropertyData("R8SimpleR8Simple")]
		public void CneqR8R8(double a, double b)
		{
			Assert.Equal(DoubleTests.CneqR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CneqR8R8", a, b));
		}

		[Theory]
		[PropertyData("R8SimpleR8Simple")]
		public void CltR8R8(double a, double b)
		{
			Assert.Equal(DoubleTests.CltR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CltR8R8", a, b));
		}

		[Theory]
		[PropertyData("R8SimpleR8Simple")]
		public void CgtR8R8(double a, double b)
		{
			Assert.Equal(DoubleTests.CgtR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CgtR8R8", a, b));
		}

		[Theory]
		[PropertyData("R8SimpleR8Simple")]
		public void CleR8R8(double a, double b)
		{
			Assert.Equal(DoubleTests.CleR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CleR8R8", a, b));
		}

		[Theory]
		[PropertyData("R8SimpleR8Simple")]
		public void CgeR8R8(double a, double b)
		{
			Assert.Equal(DoubleTests.CgeR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CgeR8R8", a, b));
		}

		[Fact]
		public void Newarr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Newarr"));
		}

		[Theory]
		[PropertyData("I4Small")]
		public void Ldlen(int length)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Ldlen", length));
		}

		[Theory]
		[PropertyData("I4SmallR8Simple")]
		public void StelemR8(int index, double value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Stelem", index, value));
		}

		[Theory]
		[PropertyData("I4SmallR8Simple")]
		public void LdelemR8(int index, double value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Ldelem", index, value));
		}

		[Theory]
		[PropertyData("I4SmallR8Simple")]
		public void LdelemaR8(int index, double value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Ldelema", index, value));
		}

		[Theory]
		[PropertyData("R8")]
		public void IsNaN(double value)
		{
			Assert.Equal(DoubleTests.IsNaN(value), Run<bool>("Mosa.Test.Collection", "DoubleTests", "IsNaN", value));
		}
	}
}
