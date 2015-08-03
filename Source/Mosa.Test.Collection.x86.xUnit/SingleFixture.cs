// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.x86.xUnit
{
	public class SingleFixture : X86TestFixture
	{
		//private static float Tolerance = 0.000001f;
		//private static IComparer<float> target = new ApproximateComparer(Tolerance);

		[Theory]
		[PropertyData("R4R4")]
		public void AddR4R4(float a, float b)
		{
			Assert.Equal(SingleTests.AddR4R4(a, b), Run<float>("Mosa.Test.Collection", "SingleTests", "AddR4R4", a, b));
		}

		[Theory]
		[PropertyData("R4R4")]
		public void SubR4R4(float a, float b)
		{
			Assert.Equal(SingleTests.SubR4R4(a, b), Run<float>("Mosa.Test.Collection", "SingleTests", "SubR4R4", a, b));
		}

		[Theory]
		[PropertyData("R4R4")]
		public void MulR4R4(float a, float b)
		{
			Assert.Equal(SingleTests.MulR4R4(a, b), Run<float>("Mosa.Test.Collection", "SingleTests", "MulR4R4", a, b));
		}

		[Theory]
		[PropertyData("R4R4")]
		public void DivR4R4(float a, float b)
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

			Assert.Equal(SingleTests.DivR4R4(a, b), Run<float>("Mosa.Test.Collection", "SingleTests", "DivR4R4", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void DivR4R4DivideByZeroException(float a)
		{
			Run<float>("Mosa.Test.Collection", "SingleTests", "DivR4R4", (float)0, a, (float)0);
		}

		// TinySimulator can't simulate this.
		//[Theory]
		//[PropertyData("R4R4")]
		public void RemR4R4(float a, float b)
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

			Assert.Equal(SingleTests.RemR4R4(a, b), Run<float>("Mosa.Test.Collection", "SingleTests", "RemR4R4", a, b));
		}

		//[Theory]
		//[ExpectedException(typeof(DivideByZeroException))]
		public void RemI4I4DivideByZeroException(int a)
		{
			Run<float>("Mosa.Test.Collection", "SingleTests", "RemR4R4", (float)0, a, (float)0);
		}

		[Theory]
		[PropertyData("R4SimpleR4Simple")]
		public void CeqR4R4(float a, float b)
		{
			Assert.Equal(SingleTests.CeqR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CeqR4R4", a, b));
		}

		[Theory]
		[PropertyData("R4SimpleR4Simple")]
		public void CneqR4R4(float a, float b)
		{
			Assert.Equal(SingleTests.CneqR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CneqR4R4", a, b));
		}

		[Theory]
		[PropertyData("R4SimpleR4Simple")]
		public void CltR4R4(float a, float b)
		{
			Assert.Equal(SingleTests.CltR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CltR4R4", a, b));
		}

		[Theory]
		[PropertyData("R4SimpleR4Simple")]
		public void CgtR4R4(float a, float b)
		{
			Assert.Equal(SingleTests.CgtR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CgtR4R4", a, b));
		}

		[Theory]
		[PropertyData("R4SimpleR4Simple")]
		public void CleR4R4(float a, float b)
		{
			Assert.Equal(SingleTests.CleR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CleR4R4", a, b));
		}

		[Theory]
		[PropertyData("R4SimpleR4Simple")]
		public void CgeR4R4(float a, float b)
		{
			Assert.Equal(SingleTests.CgeR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CgeR4R4", a, b));
		}

		[Fact]
		public void Newarr()
		{
			Assert.True(Run<bool>("Mosa.Test.Collection", "SingleTests", "Newarr"));
		}

		[Theory]
		[PropertyData("I4Small")]
		public void Ldlen(int length)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection", "SingleTests", "Ldlen", length));
		}

		[Theory]
		[PropertyData("I4SmallR4Simple")]
		public void StelemR4(int index, float value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection", "SingleTests", "Stelem", index, value));
		}

		[Theory]
		[PropertyData("I4SmallR4Simple")]
		public void LdelemR4(int index, float value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection", "SingleTests", "Ldelem", index, value));
		}

		[Theory]
		[PropertyData("I4SmallR4Simple")]
		public void LdelemaR4(int index, float value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection", "SingleTests", "Ldelema", index, value));
		}

		//[Theory]
		//[PropertyData("R4")]
		public void IsNaN(float value)
		{
			Assert.Equal(SingleTests.IsNaN(value), Run<bool>("Mosa.Test.Collection", "SingleTests", "IsNaN", value));
		}
	}
}