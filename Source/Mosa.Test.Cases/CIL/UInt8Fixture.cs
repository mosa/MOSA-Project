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
	public class UInt8Fixture : TestCompilerAdapter
	{
		public UInt8Fixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void AddU1U1([U1]byte a, [U1]byte b)
		{
			Assert.AreEqual(UInt8Tests.AddU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "AddU1U1", a, b));
		}

		[Test]
		public void SubU1U1([U1]byte a, [U1]byte b)
		{
			Assert.AreEqual(UInt8Tests.SubU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "SubU1U1", a, b));
		}

		[Test]
		public void MulU1U1([U1]byte a, [U1]byte b)
		{
			Assert.AreEqual(UInt8Tests.MulU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "MulU1U1", a, b));
		}

		[Test]
		public void DivU1U1([U1]byte a, [U1NotZero]byte b)
		{
			Assert.AreEqual(UInt8Tests.DivU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "DivU1U1", a, b));
		}

		[Test]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivU1U1DivideByZeroException([U1]byte a)
		{
			Assert.AreEqual(UInt8Tests.DivU1U1(a, (byte)0), Run<int>("Mosa.Test.Collection", "UInt8Tests", "DivU1U1", a, (byte)0));
		}

		[Test]
		public void RemU1U1([U1]byte a, [U1NotZero]byte b)
		{
			Assert.AreEqual(UInt8Tests.RemU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "RemU1U1", a, b));
		}

		[Test]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemU1U1DivideByZeroException([U1]byte a)
		{
			Assert.AreEqual(UInt8Tests.RemU1U1(a, (byte)0), Run<int>("Mosa.Test.Collection", "UInt8Tests", "RemU1U1", a, (byte)0));
		}

		[Test]
		public void RetU1([U1]byte a)
		{
			Assert.AreEqual(UInt8Tests.RetU1(a), Run<byte>("Mosa.Test.Collection", "UInt8Tests", "RetU1", a));
		}

		[Test]
		public void AndU1U1([U1]byte a, [U1]byte b)
		{
			Assert.AreEqual(UInt8Tests.AndU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "AndU1U1", a, b));
		}

		[Test]
		public void OrU1U1([U1]byte a, [U1]byte b)
		{
			Assert.AreEqual(UInt8Tests.OrU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "OrU1U1", a, b));
		}

		[Test]
		public void XorU1U1([U1]byte a, [U1]byte b)
		{
			Assert.AreEqual(UInt8Tests.XorU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "XorU1U1", a, b));
		}

		[Test]
		public void CompU1([U1]byte a)
		{
			Assert.AreEqual(UInt8Tests.CompU1(a), Run<int>("Mosa.Test.Collection", "UInt8Tests", "CompU1", a));
		}

		[Test]
		public void ShiftLeftU1U1([U1]byte a, [I1UpTo8]byte b)
		{
			Assert.AreEqual(UInt8Tests.ShiftLeftU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "ShiftLeftU1U1", a, b));
		}

		[Test]
		public void ShiftRightU1U1([U1]byte a, [I1UpTo8]byte b)
		{
			Assert.AreEqual(UInt8Tests.ShiftRightU1U1(a, b), Run<int>("Mosa.Test.Collection", "UInt8Tests", "ShiftRightU1U1", a, b));
		}

		[Test]
		public void CeqU1U1([U1]byte a, [U1]byte b)
		{
			Assert.AreEqual(UInt8Tests.CeqU1U1(a, b), Run<bool>("Mosa.Test.Collection", "UInt8Tests", "CeqU1U1", a, b));
		}

		[Test]
		public void CltU1U1([U1]byte a, [U1]byte b)
		{
			Assert.AreEqual(UInt8Tests.CltU1U1(a, b), Run<bool>("Mosa.Test.Collection", "UInt8Tests", "CltU1U1", a, b));
		}

		[Test]
		public void CgtU1U1([U1]byte a, [U1]byte b)
		{
			Assert.AreEqual(UInt8Tests.CgtU1U1(a, b), Run<bool>("Mosa.Test.Collection", "UInt8Tests", "CgtU1U1", a, b));
		}

		[Test]
		public void CleU1U1([U1]byte a, [U1]byte b)
		{
			Assert.AreEqual(UInt8Tests.CleU1U1(a, b), Run<bool>("Mosa.Test.Collection", "UInt8Tests", "CleU1U1", a, b));
		}

		[Test]
		public void CgeU1U1([U1]byte a, [U1]byte b)
		{
			Assert.AreEqual(UInt8Tests.CgeU1U1(a, b), Run<bool>("Mosa.Test.Collection", "UInt8Tests", "CgeU1U1", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt8Tests", "Newarr"));
		}

		[Test]
		public void Ldlen([I4Small]int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt8Tests", "Ldlen", length));
		}

		[Test]
		public void StelemU1([I4Small]int index, [U1]byte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt8Tests", "Stelem", index, value));
		}

		[Test]
		public void LdelemU1([I4Small]int index, [U1]byte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt8Tests", "Ldelem", index, value));
		}

		[Test]
		public void LdelemaU1([I4Small]int index, [U1]byte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt8Tests", "Ldelema", index, value));
		}
	}
}