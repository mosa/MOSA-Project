// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.UnitTest.Collection.xUnit
{
	public class DoubleFixture : TestFixture
	{
		//private static double Tolerance = 0.000001d;
		//private static IComparer<double> target = new ApproximateComparer(Tolerance);

		[Theory]
		[MemberData("R8R8", DisableDiscoveryEnumeration = true)]
		public void AddR8R8(double a, double b)
		{
			Assert.Equal(DoubleTests.AddR8R8(a, b), Run<double>("Mosa.UnitTest.Collection", "DoubleTests", "AddR8R8", a, b));
		}

		[Theory]
		[MemberData("R8R8", DisableDiscoveryEnumeration = true)]
		public void SubR8R8(double a, double b)
		{
			Assert.Equal(DoubleTests.SubR8R8(a, b), Run<double>("Mosa.UnitTest.Collection", "DoubleTests", "SubR8R8", a, b));
		}

		[Theory]
		[MemberData("R8R8", DisableDiscoveryEnumeration = true)]
		public void MulR8R8(double a, double b)
		{
			Assert.Equal(DoubleTests.MulR8R8(a, b), Run<double>("Mosa.UnitTest.Collection", "DoubleTests", "MulR8R8", a, b));
		}

		[Theory]
		[MemberData("R8R8", DisableDiscoveryEnumeration = true)]
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

			Assert.Equal(DoubleTests.DivR8R8(a, b), Run<double>("Mosa.UnitTest.Collection", "DoubleTests", "DivR8R8", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void DivR8R8DivideByZeroException(double a)
		{
			Run<double>("Mosa.UnitTest.Collection", "DoubleTests", "DivR8R8", (double)0, a, (double)0);
		}

		[Theory]
		[MemberData("R8R8", DisableDiscoveryEnumeration = true)]
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

			Assert.Equal(DoubleTests.RemR8R8(a, b), Run<double>("Mosa.UnitTest.Collection", "DoubleTests", "RemR8R8", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void RemI4I4DivideByZeroException(int a)
		{
			Run<double>("Mosa.UnitTest.Collection", "DoubleTests", "RemR8R8", (double)0, a, (double)0);
		}

		[Theory]
		[MemberData("R8SimpleR8Simple", DisableDiscoveryEnumeration = true)]
		public void CeqR8R8(double a, double b)
		{
			Assert.Equal(DoubleTests.CeqR8R8(a, b), Run<bool>("Mosa.UnitTest.Collection", "DoubleTests", "CeqR8R8", a, b));
		}

		[Theory]
		[MemberData("R8SimpleR8Simple", DisableDiscoveryEnumeration = true)]
		public void CneqR8R8(double a, double b)
		{
			Assert.Equal(DoubleTests.CneqR8R8(a, b), Run<bool>("Mosa.UnitTest.Collection", "DoubleTests", "CneqR8R8", a, b));
		}

		[Theory]
		[MemberData("R8SimpleR8Simple", DisableDiscoveryEnumeration = true)]
		public void CltR8R8(double a, double b)
		{
			Assert.Equal(DoubleTests.CltR8R8(a, b), Run<bool>("Mosa.UnitTest.Collection", "DoubleTests", "CltR8R8", a, b));
		}

		[Theory]
		[MemberData("R8SimpleR8Simple", DisableDiscoveryEnumeration = true)]
		public void CgtR8R8(double a, double b)
		{
			Assert.Equal(DoubleTests.CgtR8R8(a, b), Run<bool>("Mosa.UnitTest.Collection", "DoubleTests", "CgtR8R8", a, b));
		}

		[Theory]
		[MemberData("R8SimpleR8Simple", DisableDiscoveryEnumeration = true)]
		public void CleR8R8(double a, double b)
		{
			Assert.Equal(DoubleTests.CleR8R8(a, b), Run<bool>("Mosa.UnitTest.Collection", "DoubleTests", "CleR8R8", a, b));
		}

		[Theory]
		[MemberData("R8SimpleR8Simple", DisableDiscoveryEnumeration = true)]
		public void CgeR8R8(double a, double b)
		{
			Assert.Equal(DoubleTests.CgeR8R8(a, b), Run<bool>("Mosa.UnitTest.Collection", "DoubleTests", "CgeR8R8", a, b));
		}

		[Fact]
		public void Newarr()
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection", "DoubleTests", "Newarr"));
		}

		[Theory]
		[MemberData("I4Small", DisableDiscoveryEnumeration = true)]
		public void Ldlen(int length)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection", "DoubleTests", "Ldlen", length));
		}

		[Theory]
		[MemberData("I4SmallR8Simple", DisableDiscoveryEnumeration = true)]
		public void StelemR8(int index, double value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection", "DoubleTests", "Stelem", index, value));
		}

		[Theory]
		[MemberData("I4SmallR8Simple", DisableDiscoveryEnumeration = true)]
		public void LdelemR8(int index, double value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection", "DoubleTests", "Ldelem", index, value));
		}

		[Theory]
		[MemberData("I4SmallR8Simple", DisableDiscoveryEnumeration = true)]
		public void LdelemaR8(int index, double value)
		{
			Assert.True(Run<bool>("Mosa.UnitTest.Collection", "DoubleTests", "Ldelema", index, value));
		}

		[Theory]
		[MemberData("R8", DisableDiscoveryEnumeration = true)]
		public void IsNaN(double value)
		{
			Assert.Equal(DoubleTests.IsNaN(value), Run<bool>("Mosa.UnitTest.Collection", "DoubleTests", "IsNaN", value));
		}
	}
}
