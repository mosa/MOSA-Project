/*
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

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	[Importance(Importance.Critical)]
	public class DoubleFixture : TestCompilerAdapter
	{

		public DoubleFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void AddR8R8([R8Number]double a, [R8Number]double b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "AddR8R8", DoubleTests.AddR8R8(a, b), a, b));
		}

		[Test]
		public void SubR8R8([R8Number]double a, [R8Number]double b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "SubR8R8", DoubleTests.SubR8R8(a, b), a, b));
		}

		[Test]
		public void MulR8R8([R8Number]double a, [R8Number]double b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "MulR8R8", DoubleTests.MulR8R8(a, b), a, b));
		}

		[Test]
		public void DivR8R8([R8Number]double a, [R8NumberNotZero]double b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "DivR8R8", DoubleTests.DivR8R8(a, b), a, b));
		}

		[Test]
		[Pending]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivR8R8DivideByZeroException([R8Number]double a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "DivR8R8", DoubleTests.DivR8R8(a, (double)0), a, (double)0));
		}

		[Test]
		[Pending]
		public void RemR8R8([R8Number]double a, [R8NumberNotZero]double b)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "RemR8R8", DoubleTests.RemR8R8(a, b), a, b));
		}

		[Test]
		[Pending]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemR8R8DivideByZeroException([R8Number]double a)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "RemR8R8", DoubleTests.RemR8R8(a, (double)0), a, (double)0));
		}

		[Test]
		public void CeqR8R8([R8Number]double a, [R8Number]double b)
		{
			Assert.AreEqual(DoubleTests.CeqR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CeqR8R8", a, b));
		}

		[Test]
		public void CltR8R8([R8Number]double a, [R8Number]double b)
		{
			Assert.AreEqual(DoubleTests.CltR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CltR8R8", a, b));
		}

		[Test]
		public void CgtR8R8([R8Number]double a, [R8Number]double b)
		{
			Assert.AreEqual(DoubleTests.CgtR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CgtR8R8", a, b));
		}

		[Test]
		public void CleR8R8([R8Number]double a, [R8Number]double b)
		{
			Assert.AreEqual(DoubleTests.CleR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CleR8R8", a, b));
		}

		[Test]
		public void CgeR8R8([R8Number]double a, [R8Number]double b)
		{
			Assert.AreEqual(DoubleTests.CgeR8R8(a, b), Run<bool>("Mosa.Test.Collection", "DoubleTests", "CgeR8R8", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Newarr"));
		}

		[Test]
		public void Ldlen([I4Small]int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Ldlen", length));
		}

		[Test]
		public void StelemR8([I4Small]int index, [R8Number]double value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Stelem", index, value));
		}

		[Test]
		public void LdelemR8([I4Small]int index, [R8Number]double value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Ldelem", index, value));
		}

		[Test]
		public void LdelemaR8([I4Small]int index, [R8Number]double value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "DoubleTests", "Ldelema", index, value));
		}

	}
}

