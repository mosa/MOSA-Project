﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using MbUnit.Framework;
using Mosa.Test.Collection;
using Mosa.Test.System;

namespace Mosa.Test.Collection.MbUnit
{
	[TestFixture]
	[Importance(Importance.Critical)]
	public class SingleFixture : TestCompilerAdapter
	{
		private float Tolerance = 0.000001f;

		public SingleFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void AddR4R4([R4Number]float a, [R4Number]float b)
		{
			Assert.AreApproximatelyEqual(SingleTests.AddR4R4(a, b), Run<float>("Mosa.Test.Collection", "SingleTests", "AddR4R4", a, b), Tolerance);
		}
		
		[Test]
		public void SubR4R4([R4Number]float a, [R4Number]float b)
		{
			Assert.AreApproximatelyEqual(SingleTests.SubR4R4(a, b), Run<float>("Mosa.Test.Collection", "SingleTests", "SubR4R4", a, b), Tolerance);
		}

		[Test]
		public void MulR4R4([R4Number]float a, [R4Number]float b)
		{
			Assert.AreApproximatelyEqual(SingleTests.MulR4R4(a, b), Run<float>("Mosa.Test.Collection", "SingleTests", "MulR4R4", a, b), Tolerance);
		}

		[Test]
		public void DivR4R4([R4Number]float a, [R4NumberNotZero]float b)
		{
			Assert.AreApproximatelyEqual(SingleTests.DivR4R4(a, b), Run<float>("Mosa.Test.Collection", "SingleTests", "DivR4R4", a, b), Tolerance);
		}

		[Test]
		[Pending]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivR4R4DivideByZeroException([R4Number]float a)
		{
			Run<float>("Mosa.Test.Collection", "SingleTests", "DivR4R4", (float)0, a, (float)0);
		}

		[Test]
		public void RemR4R4([R4NumberNoExtremes]float a, [R4NumberNoExtremesOrZero] float b)
		{
			Assert.AreApproximatelyEqual(SingleTests.RemR4R4(a, b), Run<float>("Mosa.Test.Collection", "SingleTests", "RemR4R4", a, b), Tolerance);
		}

		[Test]
		[Pending]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemR4R4DivideByZeroException([R4Number]float a)
		{
			Run<float>("Mosa.Test.Collection", "SingleTests", "RemR4R4", (float)0, a, (float)0);
		}

		[Test]
		public void CeqR4R4([R4Number]float a, [R4Number]float b)
		{
			Assert.AreEqual(SingleTests.CeqR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CeqR4R4", a, b));
		}

		[Test]
		public void CneqR4R4([R4Number]float a, [R4Number]float b)
		{
			Assert.AreEqual(SingleTests.CneqR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CneqR4R4", a, b));
		}

		[Test]
		public void CltR4R4([R4Number]float a, [R4Number]float b)
		{
			Assert.AreEqual(SingleTests.CltR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CltR4R4", a, b));
		}

		[Test]
		public void CgtR4R4([R4Number]float a, [R4Number]float b)
		{
			Assert.AreEqual(SingleTests.CgtR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CgtR4R4", a, b));
		}

		[Test]
		public void CleR4R4([R4Number]float a, [R4Number]float b)
		{
			Assert.AreEqual(SingleTests.CleR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CleR4R4", a, b));
		}

		[Test]
		public void CgeR4R4([R4Number]float a, [R4Number]float b)
		{
			Assert.AreEqual(SingleTests.CgeR4R4(a, b), Run<bool>("Mosa.Test.Collection", "SingleTests", "CgeR4R4", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "SingleTests", "Newarr"));
		}

		[Test]
		public void Ldlen([I4Small]int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "SingleTests", "Ldlen", length));
		}

		[Test]
		public void StelemR4([I4Small]int index, [R4Number]float value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "SingleTests", "Stelem", index, value));
		}

		[Test]
		public void LdelemR4([I4Small]int index, [R4Number]float value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "SingleTests", "Ldelem", index, value));
		}

		[Test]
		public void LdelemaR4([I4Small]int index, [R4Number]float value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "SingleTests", "Ldelema", index, value));
		}

		[Test]
		public void IsNaN([R4]float value)
		{
			Assert.AreEqual(SingleTests.IsNaN(value), Run<bool>("Mosa.Test.Collection", "SingleTests", "IsNaN", value));
		}
	}
}