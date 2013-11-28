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
using Mosa.Test.Numbers;

namespace Mosa.Test.Collection.MbUnit
{
	[TestFixture]
	[Importance(Importance.Critical)]
	public class Int16Fixture : TestCompilerAdapter
	{
		public Int16Fixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void AddI2I2([I2]short a, [I2]short b)
		{
			Assert.AreEqual(Int16Tests.AddI2I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "AddI2I2", a, b));
		}

		[Test]
		public void SubI2I2([I2]short a, [I2]short b)
		{
			Assert.AreEqual(Int16Tests.SubI2I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "SubI2I2", a, b));
		}

		[Test]
		public void MulI2I2([I2]short a, [I2]short b)
		{
			Assert.AreEqual(Int16Tests.MulI2I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "MulI2I2", a, b));
		}

		[Test]
		public void DivI2I2([I2]short a, [I2NotZero]short b)
		{
			if (a == short.MinValue && b == -1)
				Assert.Inconclusive("TODO: Overflow exception not implemented");

			Assert.AreEqual(Int16Tests.DivI2I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "DivI2I2", a, b));
		}

		[Test]
		[Pending]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivI2I2DivideByZeroException([I2]short a)
		{
			Assert.AreEqual(Int16Tests.DivI2I2(a, (short)0), Run<int>("Mosa.Test.Collection", "Int16Tests", "DivI2I2", a, (short)0));
		}

		[Test]
		public void RemI2I2([I2]short a, [I2NotZero]short b)
		{
			Assert.AreEqual(Int16Tests.RemI2I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "RemI2I2", a, b));
		}

		[Test]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemI2I2DivideByZeroException([I2]short a)
		{
			Assert.AreEqual(Int16Tests.RemI2I2(a, (short)0), Run<int>("Mosa.Test.Collection", "Int16Tests", "RemI2I2", a, (short)0));
		}

		[Test]
		public void RetI2([I2]short a)
		{
			Assert.AreEqual(Int16Tests.RetI2(a), Run<short>("Mosa.Test.Collection", "Int16Tests", "RetI2", a));
		}

		[Test]
		public void AndI2I2([I2]short a, [I2]short b)
		{
			Assert.AreEqual(Int16Tests.AndI2I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "AndI2I2", a, b));
		}

		[Test]
		public void OrI2I2([I2]short a, [I2]short b)
		{
			Assert.AreEqual(Int16Tests.OrI2I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "OrI2I2", a, b));
		}

		[Test]
		public void XorI2I2([I2]short a, [I2]short b)
		{
			Assert.AreEqual(Int16Tests.XorI2I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "XorI2I2", a, b));
		}

		[Test]
		public void CeqI2I2([I2]short a, [I2]short b)
		{
			Assert.AreEqual(Int16Tests.CeqI2I2(a, b), Run<bool>("Mosa.Test.Collection", "Int16Tests", "CeqI2I2", a, b));
		}

		[Test]
		public void CltI2I2([I2]short a, [I2]short b)
		{
			Assert.AreEqual(Int16Tests.CltI2I2(a, b), Run<bool>("Mosa.Test.Collection", "Int16Tests", "CltI2I2", a, b));
		}

		[Test]
		public void CgtI2I2([I2]short a, [I2]short b)
		{
			Assert.AreEqual(Int16Tests.CgtI2I2(a, b), Run<bool>("Mosa.Test.Collection", "Int16Tests", "CgtI2I2", a, b));
		}

		[Test]
		public void CleI2I2([I2]short a, [I2]short b)
		{
			Assert.AreEqual(Int16Tests.CleI2I2(a, b), Run<bool>("Mosa.Test.Collection", "Int16Tests", "CleI2I2", a, b));
		}

		[Test]
		public void CgeI2I2([I2]short a, [I2]short b)
		{
			Assert.AreEqual(Int16Tests.CgeI2I2(a, b), Run<bool>("Mosa.Test.Collection", "Int16Tests", "CgeI2I2", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int16Tests", "Newarr"));
		}

		[Test]
		public void Ldlen([I4Small]int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int16Tests", "Ldlen", length));
		}

		[Test]
		public void StelemI2([I4Small]int index, [I2]short value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int16Tests", "Stelem", index, value));
		}

		[Test]
		public void LdelemI2([I4Small]int index, [I2]short value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int16Tests", "Ldelem", index, value));
		}

		[Test]
		public void LdelemaI2([I4Small]int index, [I2]short value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int16Tests", "Ldelema", index, value));
		}
	}
}