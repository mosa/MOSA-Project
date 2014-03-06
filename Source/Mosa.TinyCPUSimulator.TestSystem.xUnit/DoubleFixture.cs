/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Xunit;
using Xunit.Extensions;
using System.Collections.Generic;
using Mosa.TinyCPUSimulator.TestSystem;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class DoubleFixture : X86TestFixture
	{
		private static double Tolerance = 0.000001d;
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

		//[Theory]
		//[PropertyData("R8R8")]
		//public void DivR8R8(double a, [R8NumberNotZero]double b)
		//{
		//	Assert.Equal(DoubleTests.DivR8R8(a, b), Run<double>("Mosa.Test.Collection", "DoubleTests", "DivR8R8", a, b));
		//}

		//[Theory]
		//[Pending]
		//[ExpectedException(typeof(DivideByZeroException))]
		//public void DivR8R8DivideByZeroException(double a)
		//{
		//	Run<double>("Mosa.Test.Collection", "DoubleTests", "DivR8R8", (double)0, a, (double)0);
		//}

		//[Theory]
		//[PropertyData("R8R8")]
		//public void RemR8R8([R8NumberNoExtremes]double a, [R8NumberNoExtremesOrZero] double b)
		//{
		//	Assert.AreApproximatelyEqual(DoubleTests.RemR8R8(a, b), Run<double>("Mosa.Test.Collection", "DoubleTests", "RemR8R8", a, b), Tolerance);
		//}

		//[Theory]
		//[Pending]
		//[ExpectedException(typeof(DivideByZeroException))]
		//public void RemR8R8DivideByZeroException(double a)
		//{
		//	Run<double>("Mosa.Test.Collection", "DoubleTests", "RemR8R8", (double)0, a, (double)0);
		//}

		//[Theory]
		//[PropertyData("R8R8")]
		public void CeqR8R8(double a, double b)
		{
			Assert.Equal(DoubleTests.CeqR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CeqR8R8", a, b));
		}

		//[Theory]
		//[PropertyData("R8R8")]
		//public void CneqR8R8(double a, double b)
		//{
		//	Assert.Equal(DoubleTests.CneqR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CneqR8R8", a, b));
		//}

		//[Theory]
		//[PropertyData("R8R8")]
		//public void CltR8R8(double a, double b)
		//{
		//	Assert.Equal(DoubleTests.CltR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CltR8R8", a, b));
		//}

		//[Theory]
		//[PropertyData("R8R8")]
		//public void CgtR8R8(double a, double b)
		//{
		//	Assert.Equal(DoubleTests.CgtR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CgtR8R8", a, b));
		//}

		//[Theory]
		//[PropertyData("R8R8")]
		//public void CleR8R8(double a, double b)
		//{
		//	Assert.Equal(DoubleTests.CleR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CleR8R8", a, b));
		//}

		//[Theory]
		//[PropertyData("R8R8")]
		//public void CgeR8R8(double a, double b)
		//{
		//	Assert.Equal(DoubleTests.CgeR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CgeR8R8", a, b));
		//}

		//[Theory]
		//public void Newarr()
		//{
		//	Assert.True(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Newarr"));
		//}

		//[Theory]
		//public void Ldlen([I4Small]int length)
		//{
		//	Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Ldlen", length));
		//}

		//[Theory]
		//public void StelemR8([I4Small]int index, double value)
		//{
		//	Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Stelem", index, value));
		//}

		//[Theory]
		//public void LdelemR8([I4Small]int index, double value)
		//{
		//	Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Ldelem", index, value));
		//}

		//[Theory]
		//public void LdelemaR8([I4Small]int index, double value)
		//{
		//	Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Ldelema", index, value));
		//}

		//[Theory]
		//[PropertyData("R8")]
		//public void IsNaN([R8]double value)
		//{
		//	Assert.Equal(DoubleTests.IsNaN(value), Run<bool>("Mosa.Test.Collection", "DoubleTests", "IsNaN", value));
		//}
	}
}