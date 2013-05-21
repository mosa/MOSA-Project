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
	public class UInt32Fixture : TestCompilerAdapter
	{
		public UInt32Fixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void AddU4U4([U4]uint a, [U4]uint b)
		{
			Assert.AreEqual(UInt32Tests.AddU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "AddU4U4", a, b));
		}

		[Test]
		public void SubU4U4([U4]uint a, [U4]uint b)
		{
			Assert.AreEqual(UInt32Tests.SubU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "SubU4U4", a, b));
		}

		[Test]
		public void MulU4U4([U4]uint a, [U4]uint b)
		{
			Assert.AreEqual(UInt32Tests.MulU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "MulU4U4", a, b));
		}

		[Test]
		public void DivU4U4([U4]uint a, [U4NotZero]uint b)
		{
			Assert.AreEqual(UInt32Tests.DivU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "DivU4U4", a, b));
		}

		[Test]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivU4U4DivideByZeroException([U4]uint a)
		{
			Assert.AreEqual(UInt32Tests.DivU4U4(a, (uint)0), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "DivU4U4", a, (uint)0));
		}

		[Test]
		public void RemU4U4([U4]uint a, [U4NotZero]uint b)
		{
			Assert.AreEqual(UInt32Tests.RemU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "RemU4U4", a, b));
		}

		[Test]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemU4U4DivideByZeroException([U4]uint a)
		{
			Assert.AreEqual(UInt32Tests.RemU4U4(a, (uint)0), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "RemU4U4", a, (uint)0));
		}

		[Test]
		public void RetU4([U4]uint a)
		{
			Assert.AreEqual(UInt32Tests.RetU4(a), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "RetU4", a));
		}

		[Test]
		public void AndU4U4([U4]uint a, [U4]uint b)
		{
			Assert.AreEqual(UInt32Tests.AndU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "AndU4U4", a, b));
		}

		[Test]
		public void OrU4U4([U4]uint a, [U4]uint b)
		{
			Assert.AreEqual(UInt32Tests.OrU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "OrU4U4", a, b));
		}

		[Test]
		public void XorU4U4([U4]uint a, [U4]uint b)
		{
			Assert.AreEqual(UInt32Tests.XorU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "XorU4U4", a, b));
		}

		[Test]
		public void CompU4([U4]uint a)
		{
			Assert.AreEqual(UInt32Tests.CompU4(a), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "CompU4", a));
		}

		[Test]
		public void ShiftLeftU4U4([U4]uint a, [I1UpTo32]byte b)
		{
			Assert.AreEqual(UInt32Tests.ShiftLeftU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "ShiftLeftU4U4", a, b));
		}

		[Test]
		public void ShiftRightU4U4([U4]uint a, [I1UpTo32]byte b)
		{
			Assert.AreEqual(UInt32Tests.ShiftRightU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "ShiftRightU4U4", a, b));
		}

		[Test]
		public void CeqU4U4([U4]uint a, [U4]uint b)
		{
			Assert.AreEqual(UInt32Tests.CeqU4U4(a, b), Run<bool>("Mosa.Test.Collection", "UInt32Tests", "CeqU4U4", a, b));
		}

		[Test]
		public void CltU4U4([U4]uint a, [U4]uint b)
		{
			Assert.AreEqual(UInt32Tests.CltU4U4(a, b), Run<bool>("Mosa.Test.Collection", "UInt32Tests", "CltU4U4", a, b));
		}

		[Test]
		public void CgtU4U4([U4]uint a, [U4]uint b)
		{
			Assert.AreEqual(UInt32Tests.CgtU4U4(a, b), Run<bool>("Mosa.Test.Collection", "UInt32Tests", "CgtU4U4", a, b));
		}

		[Test]
		public void CleU4U4([U4]uint a, [U4]uint b)
		{
			Assert.AreEqual(UInt32Tests.CleU4U4(a, b), Run<bool>("Mosa.Test.Collection", "UInt32Tests", "CleU4U4", a, b));
		}

		[Test]
		public void CgeU4U4([U4]uint a, [U4]uint b)
		{
			Assert.AreEqual(UInt32Tests.CgeU4U4(a, b), Run<bool>("Mosa.Test.Collection", "UInt32Tests", "CgeU4U4", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt32Tests", "Newarr"));
		}

		[Test]
		public void Ldlen([I4Small]int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt32Tests", "Ldlen", length));
		}

		[Test]
		public void StelemU4([I4Small]int index, [U4]uint value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt32Tests", "Stelem", index, value));
		}

		[Test]
		public void LdelemU4([I4Small]int index, [U4]uint value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt32Tests", "Ldelem", index, value));
		}

		[Test]
		public void LdelemaU4([I4Small]int index, [U4]uint value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt32Tests", "Ldelema", index, value));
		}
	}
}