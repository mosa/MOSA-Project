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
	public class Int64Fixture : TestCompilerAdapter
	{

		public Int64Fixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void AddI8I8([I8]long a, [I8]long b)
		{
			Assert.AreEqual(Int64Tests.AddI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "AddI8I8", a, b));
		}

		[Test]
		public void SubI8I8([I8]long a, [I8]long b)
		{
			Assert.AreEqual(Int64Tests.SubI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "SubI8I8", a, b));
		}

		[Test]
		public void MulI8I8([I8]long a, [I8]long b)
		{
			Assert.AreEqual(Int64Tests.MulI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "MulI8I8", a, b));
		}

		[Test]
		public void DivI8I8([I8]long a, [I8NotZero]long b)
		{
			if (a == long.MinValue && b == -1)
				Assert.Inconclusive("TODO: Overflow exception not implemented");

			Assert.AreEqual(Int64Tests.DivI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "DivI8I8", a, b));
		}

		[Test]
		[Pending]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivI8I8DivideByZeroException([I8]long a)
		{
			Assert.AreEqual(Int64Tests.DivI8I8(a, (long)0), Run<long>("Mosa.Test.Collection", "Int64Tests", "DivI8I8", a, (long)0));
		}

		[Test]
		public void RemI8I8([I8]long a, [I8NotZero]long b)
		{
			if (a == long.MinValue && b == -1)
				Assert.Inconclusive("TODO: Overflow exception not implemented");
			
			Assert.AreEqual(Int64Tests.RemI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "RemI8I8", a, b));
		}

		[Test]
		[Pending]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemI8I8DivideByZeroException([I8]long a)
		{
			Assert.AreEqual(Int64Tests.RemI8I8(a, (long)0), Run<long>("Mosa.Test.Collection", "Int64Tests", "RemI8I8", a, (long)0));
		}

		[Test]
		public void RetI8([I8]long a)
		{
			Assert.AreEqual(Int64Tests.RetI8(a), Run<long>("Mosa.Test.Collection", "Int64Tests", "RetI8", a));
		}

		[Test]
		public void AndI8I8([I8]long a, [I8]long b)
		{
			Assert.AreEqual(Int64Tests.AndI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "AndI8I8", a, b));
		}

		[Test]
		public void OrI8I8([I8]long a, [I8]long b)
		{
			Assert.AreEqual(Int64Tests.OrI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "OrI8I8", a, b));
		}

		[Test]
		public void XorI8I8([I8]long a, [I8]long b)
		{
			Assert.AreEqual(Int64Tests.XorI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "XorI8I8", a, b));
		}

		[Test]
		public void CeqI8I8([I8]long a, [I8]long b)
		{
			Assert.AreEqual(Int64Tests.CeqI8I8(a, b), Run<bool>("Mosa.Test.Collection", "Int64Tests", "CeqI8I8", a, b));
		}

		[Test]
		public void CltI8I8([I8]long a, [I8]long b)
		{
			Assert.AreEqual(Int64Tests.CltI8I8(a, b), Run<bool>("Mosa.Test.Collection", "Int64Tests", "CltI8I8", a, b));
		}

		[Test]
		public void CgtI8I8([I8]long a, [I8]long b)
		{
			Assert.AreEqual(Int64Tests.CgtI8I8(a, b), Run<bool>("Mosa.Test.Collection", "Int64Tests", "CgtI8I8", a, b));
		}

		[Test]
		public void CleI8I8([I8]long a, [I8]long b)
		{
			Assert.AreEqual(Int64Tests.CleI8I8(a, b), Run<bool>("Mosa.Test.Collection", "Int64Tests", "CleI8I8", a, b));
		}

		[Test]
		public void CgeI8I8([I8]long a, [I8]long b)
		{
			Assert.AreEqual(Int64Tests.CgeI8I8(a, b), Run<bool>("Mosa.Test.Collection", "Int64Tests", "CgeI8I8", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int64Tests", "Newarr"));
		}

		[Test]
		public void Ldlen([I4Small]int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int64Tests", "Ldlen", length));
		}

		[Test]
		public void StelemI8([I4Small]int index, [I4]long value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int64Tests", "Stelem", index, value));
		}

		[Test]
		public void LdelemI8([I4Small]int index, [I4]long value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int64Tests", "Ldelem", index, value));
		}

		[Test]
		public void LdelemaI8([I4Small]int index, [I4]long value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int64Tests", "Ldelema", index, value));
		}

	}
}
