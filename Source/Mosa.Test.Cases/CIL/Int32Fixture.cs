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
using Mosa.Test.System.Numbers;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	[Importance(Importance.Critical)]
	public class Int32Fixture : TestCompilerAdapter
	{

		public Int32Fixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void AddI4I4([I4]int a, [I4]int b)
		{
			Assert.AreEqual(Int32Tests.AddI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "AddI4I4", a, b));
		}

		[Test]
		public void SubI4I4([I4]int a, [I4]int b)
		{
			Assert.AreEqual(Int32Tests.SubI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "SubI4I4", a, b));
		}

		[Test]
		public void MulI4I4([I4]int a, [I4]int b)
		{
			Assert.AreEqual(Int32Tests.MulI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "MulI4I4", a, b));
		}

		[Test]
		public void DivI4I4([I4]int a, [I4NotZero]int b)
		{
			if (a == int.MinValue && b == -1)
				Assert.Inconclusive("TODO: Overflow exception not implemented");

			Assert.AreEqual(Int32Tests.DivI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "DivI4I4", a, b));
		}

		[Test]
		[Pending]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivI4I4DivideByZeroException([I4]int a)
		{
			Assert.AreEqual(Int32Tests.DivI4I4(a, (int)0), Run<int>("Mosa.Test.Collection", "Int32Tests", "DivI4I4", a, (int)0));
		}

		[Test]
		public void RemI4I4([I4]int a, [I4NotZero]int b)
		{
			if (a == int.MinValue && b == -1)
				Assert.Inconclusive("TODO: Overflow exception not implemented");
			
			Assert.AreEqual(Int32Tests.RemI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "RemI4I4", a, b));
		}

		[Test]
		[Pending]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemI4I4DivideByZeroException([I4]int a)
		{
			Assert.AreEqual(Int32Tests.RemI4I4(a, (int)0), Run<int>("Mosa.Test.Collection", "Int32Tests", "RemI4I4", a, (int)0));
		}

		[Test]
		public void RetI4([I4]int a)
		{
			Assert.AreEqual(Int32Tests.RetI4(a), Run<int>("Mosa.Test.Collection", "Int32Tests", "RetI4", a));
		}

		[Test]
		public void AndI4I4([I4]int a, [I4]int b)
		{
			Assert.AreEqual(Int32Tests.AndI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "AndI4I4", a, b));
		}

		[Test]
		public void OrI4I4([I4]int a, [I4]int b)
		{
			Assert.AreEqual(Int32Tests.OrI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "OrI4I4", a, b));
		}

		[Test]
		public void XorI4I4([I4]int a, [I4]int b)
		{
			Assert.AreEqual(Int32Tests.XorI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "XorI4I4", a, b));
		}

		//TODO: Shifts

		[Test]
		public void CeqI4I4([I4]int a, [I4]int b)
		{
			Assert.AreEqual(Int32Tests.CeqI4I4(a, b), Run<bool>("Mosa.Test.Collection", "Int32Tests", "CeqI4I4", a, b));
		}

		[Test]
		public void CltI4I4([I4]int a, [I4]int b)
		{
			Assert.AreEqual(Int32Tests.CltI4I4(a, b), Run<bool>("Mosa.Test.Collection", "Int32Tests", "CltI4I4", a, b));
		}

		[Test]
		public void CgtI4I4([I4]int a, [I4]int b)
		{
			Assert.AreEqual(Int32Tests.CgtI4I4(a, b), Run<bool>("Mosa.Test.Collection", "Int32Tests", "CgtI4I4", a, b));
		}

		[Test]
		public void CleI4I4([I4]int a, [I4]int b)
		{
			Assert.AreEqual(Int32Tests.CleI4I4(a, b), Run<bool>("Mosa.Test.Collection", "Int32Tests", "CleI4I4", a, b));
		}

		[Test]
		public void CgeI4I4([I4]int a, [I4]int b)
		{
			Assert.AreEqual(Int32Tests.CgeI4I4(a, b), Run<bool>("Mosa.Test.Collection", "Int32Tests", "CgeI4I4", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int32Tests", "Newarr"));
		}

		[Test]
		public void Ldlen([I4Small]int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int32Tests", "Ldlen", length));
		}

		[Test]
		public void StelemI4([I4Small]int index, [I4]int value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int32Tests", "Stelem", index, value));
		}

		[Test]
		public void LdelemI4([I4Small]int index, [I4]int value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int32Tests", "Ldelem", index, value));
		}

		[Test]
		public void LdelemaI4([I4Small]int index, [I4]int value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int32Tests", "Ldelema", index, value));
		}

	}
}
