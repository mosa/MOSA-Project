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
	public class Int8Fixture : TestCompilerAdapter
	{

		public Int8Fixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void AddI1I1([I1]sbyte a, [I1]sbyte b)
		{
			Assert.AreEqual(Int8Tests.AddI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "AddI1I1", a, b));
		}

		[Test]
		public void SubI1I1([I1]sbyte a, [I1]sbyte b)
		{
			Assert.AreEqual(Int8Tests.SubI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "SubI1I1", a, b));
		}

		[Test]
		public void MulI1I1([I1]sbyte a, [I1]sbyte b)
		{
			Assert.AreEqual(Int8Tests.MulI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "MulI1I1", a, b));
		}

		[Test]
		public void DivI1I1([I1]sbyte a, [I1NotZero]sbyte b)
		{
			if (a == sbyte.MinValue && b == -1)
				Assert.Inconclusive("TODO: Overflow exception not implemented"); 
			
			Assert.AreEqual(Int8Tests.DivI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "DivI1I1", a, b));
		}

		[Test]
		[Pending]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivI1I1DivideByZeroException([I1]sbyte a)
		{
			Assert.AreEqual(Int8Tests.DivI1I1(a, (sbyte)0), Run<int>("Mosa.Test.Collection", "Int8Tests", "DivI1I1", a, (sbyte)0));
		}

		[Test]
		public void RemI1I1([I1]sbyte a, [I1NotZero]sbyte b)
		{
			Assert.AreEqual(Int8Tests.RemI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "RemI1I1", a, b));
		}

		[Test]
		[Pending]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemI1I1DivideByZeroException([I1]sbyte a)
		{
			Assert.AreEqual(Int8Tests.RemI1I1(a, (sbyte)0), Run<int>("Mosa.Test.Collection", "Int8Tests", "RemI1I1", a, (sbyte)0));
		}

		[Test]
		public void RetI1([I1]sbyte a)
		{
			Assert.AreEqual(Int8Tests.RetI1(a), Run<sbyte>("Mosa.Test.Collection", "Int8Tests", "RetI1", a));
		}

		[Test]
		public void AndI1I1([I1]sbyte a, [I1]sbyte b)
		{
			Assert.AreEqual(Int8Tests.AndI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "AndI1I1", a, b));
		}

		[Test]
		public void OrI1I1([I1]sbyte a, [I1]sbyte b)
		{
			Assert.AreEqual(Int8Tests.OrI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "OrI1I1", a, b));
		}

		[Test]
		public void XorI1I1([I1]sbyte a, [I1]sbyte b)
		{
			Assert.AreEqual(Int8Tests.XorI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "XorI1I1", a, b));
		}

		[Test]
		public void CeqI1I1([I1]sbyte a, [I1]sbyte b)
		{
			Assert.AreEqual(Int8Tests.CeqI1I1(a, b), Run<bool>("Mosa.Test.Collection", "Int8Tests", "CeqI1I1", a, b));
		}

		[Test]
		public void CltI1I1([I1]sbyte a, [I1]sbyte b)
		{
			Assert.AreEqual(Int8Tests.CltI1I1(a, b), Run<bool>("Mosa.Test.Collection", "Int8Tests", "CltI1I1", a, b));
		}

		[Test]
		public void CgtI1I1([I1]sbyte a, [I1]sbyte b)
		{
			Assert.AreEqual(Int8Tests.CgtI1I1(a, b), Run<bool>("Mosa.Test.Collection", "Int8Tests", "CgtI1I1", a, b));
		}

		[Test]
		public void CleI1I1([I1]sbyte a, [I1]sbyte b)
		{
			Assert.AreEqual(Int8Tests.CleI1I1(a, b), Run<bool>("Mosa.Test.Collection", "Int8Tests", "CleI1I1", a, b));
		}

		[Test]
		public void CgeI1I1([I1]sbyte a, [I1]sbyte b)
		{
			Assert.AreEqual(Int8Tests.CgeI1I1(a, b), Run<bool>("Mosa.Test.Collection", "Int8Tests", "CgeI1I1", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int8Tests", "Newarr"));
		}

		[Test]
		public void Ldlen([I4Small]int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int8Tests", "Ldlen", length));
		}

		[Test]
		public void StelemI1([I4Small]int index, [I1]sbyte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int8Tests", "Stelem", index, value));
		}

		[Test]
		public void LdelemI1([I4Small]int index, [I1]sbyte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int8Tests", "Ldelem", index, value));
		}

		[Test]
		public void LdelemaI1([I4Small]int index, [I1]sbyte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int8Tests", "Ldelema", index, value));
		}

	}
}
