// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;
using Xunit.Extensions;

namespace Mosa.Test.Collection.xUnit
{
	public class SingleFixture : TestFixture
	{
		//private static float Tolerance = 0.000001f;
		//private static IComparer<float> target = new ApproximateComparer(Tolerance);

		[Theory]
		[MemberData("R4R4", DisableDiscoveryEnumeration = true)]
		public void AddR4R4(float a, float b)
		{
			Assert.Equal(SingleTests.AddR4R4(a, b), Run<float>("Mosa.Test.Collection", "SingleTests", "AddR4R4", a, b));
		}

		[Theory]
		[MemberData("R4R4", DisableDiscoveryEnumeration = true)]
		public void SubR4R4(float a, float b)
		{
			Assert.Equal(SingleTests.SubR4R4(a, b), Run<float>("Mosa.Test.Collection", "SingleTests", "SubR4R4", a, b));
		}

		[Theory]
		[MemberData("R4R4", DisableDiscoveryEnumeration = true)]
		public void MulR4R4(float a, float b)
		{
			Assert.Equal(SingleTests.MulR4R4(a, b), Run<float>("Mosa.Test.Collection", "SingleTests", "MulR4R4", a, b));
		}

		[Theory]
		[MemberData("R4R4", DisableDiscoveryEnumeration = true)]
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
		//[MemberData("R4R4", DisableDiscoveryEnumeration = true)]
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
		[MemberData("R4SimpleR4Simple", DisableDiscoveryEnumeration = true)]
		public void CeqR4R4(float a, float b)
		{
			Assert.Equal(SingleTests.CeqR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CeqR4R4", a, b));
		}

		[Theory]
		[MemberData("R4SimpleR4Simple", DisableDiscoveryEnumeration = true)]
		public void CneqR4R4(float a, float b)
		{
			Assert.Equal(SingleTests.CneqR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CneqR4R4", a, b));
		}

		[Theory]
		[MemberData("R4SimpleR4Simple", DisableDiscoveryEnumeration = true)]
		public void CltR4R4(float a, float b)
		{
			Assert.Equal(SingleTests.CltR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CltR4R4", a, b));
		}

		[Theory]
		[MemberData("R4SimpleR4Simple", DisableDiscoveryEnumeration = true)]
		public void CgtR4R4(float a, float b)
		{
			Assert.Equal(SingleTests.CgtR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CgtR4R4", a, b));
		}

		[Theory]
		[MemberData("R4SimpleR4Simple", DisableDiscoveryEnumeration = true)]
		public void CleR4R4(float a, float b)
		{
			Assert.Equal(SingleTests.CleR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CleR4R4", a, b));
		}

		[Theory]
		[MemberData("R4SimpleR4Simple", DisableDiscoveryEnumeration = true)]
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
		[MemberData("I4Small", DisableDiscoveryEnumeration = true)]
		public void Ldlen(int length)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection", "SingleTests", "Ldlen", length));
		}

		[Theory]
		[MemberData("I4SmallR4Simple", DisableDiscoveryEnumeration = true)]
		public void StelemR4(int index, float value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection", "SingleTests", "Stelem", index, value));
		}

		[Theory]
		[MemberData("I4SmallR4Simple", DisableDiscoveryEnumeration = true)]
		public void LdelemR4(int index, float value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection", "SingleTests", "Ldelem", index, value));
		}

		[Theory]
		[MemberData("I4SmallR4Simple", DisableDiscoveryEnumeration = true)]
		public void LdelemaR4(int index, float value)
		{
			Assert.True(Run<bool>("Mosa.Test.Collection", "SingleTests", "Ldelema", index, value));
		}

		//[Theory]
		//[MemberData("R4", DisableDiscoveryEnumeration = true)]
		public void IsNaN(float value)
		{
			Assert.Equal(SingleTests.IsNaN(value), Run<bool>("Mosa.Test.Collection", "SingleTests", "IsNaN", value));
		}
	}
}
