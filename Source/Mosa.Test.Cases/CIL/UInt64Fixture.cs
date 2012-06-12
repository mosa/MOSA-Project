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
	public class UInt64Fixture : TestCompilerAdapter
	{

		public UInt64Fixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void AddU8U8([U8]ulong a, [U8]ulong b)
		{
			Assert.AreEqual(UInt64Tests.AddU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "AddU8U8", a, b));
		}

		[Test]
		public void SubU8U8([U8]ulong a, [U8]ulong b)
		{
			Assert.AreEqual(UInt64Tests.SubU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "SubU8U8", a, b));
		}

		[Test]
		public void MulU8U8([U8]ulong a, [U8]ulong b)
		{
			Assert.AreEqual(UInt64Tests.MulU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "MulU8U8", a, b));
		}

		[Test]
		public void DivU8U8([U8]ulong a, [U8NotZero]ulong b)
		{
			Assert.AreEqual(UInt64Tests.DivU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "DivU8U8", a, b));
		}

		[Test]
		[Pending]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivU8U8DivideByZeroException([U8]ulong a)
		{
			Assert.AreEqual(UInt64Tests.DivU8U8(a, (ulong)0), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "DivU8U8", a, (ulong)0));
		}

		[Test]
		public void RemU8U8([U8]ulong a, [U8NotZero]ulong b)
		{
			//[Row(UInt64.MaxValue - 1, UInt64.MaxValue)] // BUG: Crashes test runner
			if ((a == UInt64.MaxValue - 1) && (b == UInt64.MaxValue))
				Assert.Inconclusive("TODO: Overflow exception not implemented");

			Assert.AreEqual(UInt64Tests.RemU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "RemU8U8", a, b));
		}

		[Test]
		[Pending]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemU8U8DivideByZeroException([U8]ulong a)
		{
			Assert.AreEqual(UInt64Tests.RemU8U8(a, (ulong)0), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "RemU8U8", a, (ulong)0));
		}

		[Test]
		public void RetU8([U8]ulong a)
		{
			Assert.AreEqual(UInt64Tests.RetU8(a), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "RetU8", a));
		}

		[Test]
		public void AndU8U8([U8]ulong a, [U8]ulong b)
		{
			Assert.AreEqual(UInt64Tests.AndU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "AndU8U8", a, b));
		}

		[Test]
		public void OrU8U8([U8]ulong a, [U8]ulong b)
		{
			Assert.AreEqual(UInt64Tests.OrU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "OrU8U8", a, b));
		}

		[Test]
		public void XorU8U8([U8]ulong a, [U8]ulong b)
		{
			Assert.AreEqual(UInt64Tests.XorU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "XorU8U8", a, b));
		}

		[Test]
		public void CompU8([U8]ulong a)
		{
			Assert.AreEqual(UInt64Tests.CompU8(a), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "CompU8", a));
		}

		[Test]
		public void ShiftLeftU8U8([U8]ulong a, [I1UpTo32]byte b)
		{
			Assert.AreEqual(UInt64Tests.ShiftLeftU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "ShiftLeftU8U8", a, b));
		}

		[Test]
		[Pending]
		public void ShiftRightU8U8([U8]ulong a, [I1UpTo32]byte b)
		{
			Assert.AreEqual(UInt64Tests.ShiftRightU8U8(a, b), Run<ulong>("Mosa.Test.Collection", "UInt64Tests", "ShiftRightU8U8", a, b));
		}

		[Test]
		public void CeqU8U8([U8]ulong a, [U8]ulong b)
		{
			Assert.AreEqual(UInt64Tests.CeqU8U8(a, b), Run<bool>("Mosa.Test.Collection", "UInt64Tests", "CeqU8U8", a, b));
		}

		[Test]
		public void CltU8U8([U8]ulong a, [U8]ulong b)
		{
			Assert.AreEqual(UInt64Tests.CltU8U8(a, b), Run<bool>("Mosa.Test.Collection", "UInt64Tests", "CltU8U8", a, b));
		}

		[Test]
		public void CgtU8U8([U8]ulong a, [U8]ulong b)
		{
			Assert.AreEqual(UInt64Tests.CgtU8U8(a, b), Run<bool>("Mosa.Test.Collection", "UInt64Tests", "CgtU8U8", a, b));
		}

		[Test]
		public void CleU8U8([U8]ulong a, [U8]ulong b)
		{
			Assert.AreEqual(UInt64Tests.CleU8U8(a, b), Run<bool>("Mosa.Test.Collection", "UInt64Tests", "CleU8U8", a, b));
		}

		[Test]
		public void CgeU8U8([U8]ulong a, [U8]ulong b)
		{
			Assert.AreEqual(UInt64Tests.CgeU8U8(a, b), Run<bool>("Mosa.Test.Collection", "UInt64Tests", "CgeU8U8", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt64Tests", "Newarr"));
		}

		[Test]
		public void Ldlen([I4Small]int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt64Tests", "Ldlen", length));
		}

		[Test]
		public void StelemU8([I4Small]int index, [U8]ulong value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt64Tests", "Stelem", index, value));
		}

		[Test]
		public void LdelemU8([I4Small]int index, [U8]ulong value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt64Tests", "Ldelem", index, value));
		}

		[Test]
		public void LdelemaU8([I4Small]int index, [U8]ulong value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt64Tests", "Ldelema", index, value));
		}

	}
}
