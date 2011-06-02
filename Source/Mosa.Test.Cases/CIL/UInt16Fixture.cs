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

using Mosa.Test.System;
using Mosa.Test.System.Numbers;
using Mosa.Test.Collection;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	[Importance(Importance.Critical)]
	public class UInt16Fixture : TestCompilerAdapter
	{

		public UInt16Fixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void AddU2U2([U2]ushort a, [U2]ushort b)
		{
			Assert.AreEqual(UInt16Tests.AddU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "AddU2U2", a, b));
		}

		[Test]
		public void SubU2U2([U2]ushort a, [U2]ushort b)
		{
			Assert.AreEqual(UInt16Tests.SubU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "SubU2U2", a, b));
		}

		[Test]
		public void MulU2U2([U2]ushort a, [U2]ushort b)
		{
			Assert.AreEqual(UInt16Tests.MulU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "MulU2U2", a, b));
		}

		[Test]
		public void DivU2U2([U2]ushort a, [U2NotZero]ushort b)
		{
			Assert.AreEqual(UInt16Tests.DivU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "DivU2U2", a, b));
		}

		[Test]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivU2U2DivideByZeroException([U2]ushort a)
		{
			Assert.AreEqual(UInt16Tests.DivU2U2(a, (ushort)0), Run<int>("Mosa.Test.Collection", "UInt16Tests", "DivU2U2", a, (ushort)0));
		}

		[Test]
		public void RemU2U2([U2]ushort a, [U2NotZero]ushort b)
		{
			Assert.AreEqual(UInt16Tests.RemU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "RemU2U2", a, b));
		}

		[Test]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemU2U2DivideByZeroException([U2]ushort a)
		{
			Assert.AreEqual(UInt16Tests.RemU2U2(a, (ushort)0), Run<int>("Mosa.Test.Collection", "UInt16Tests", "RemU2U2", a, (ushort)0));
		}

		[Test]
		public void RetU2([U2]ushort a)
		{
			Assert.AreEqual(UInt16Tests.RetU2(a), Run<ushort>("Mosa.Test.Collection", "UInt16Tests", "RetU2", a));
		}

		[Test]
		public void AndU2U2([U2]ushort a, [U2]ushort b)
		{
			Assert.AreEqual(UInt16Tests.AndU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "AndU2U2", a, b));
		}

		[Test]
		public void OrU2U2([U2]ushort a, [U2]ushort b)
		{
			Assert.AreEqual(UInt16Tests.OrU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "OrU2U2", a, b));
		}

		[Test]
		public void XorU2U2([U2]ushort a, [U2]ushort b)
		{
			Assert.AreEqual(UInt16Tests.XorU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "XorU2U2", a, b));
		}

		[Test]
		public void CompU2([U2]ushort a)
		{
			Assert.AreEqual(UInt16Tests.CompU2(a), Run<int>("Mosa.Test.Collection", "UInt16Tests", "CompU2", a));
		}

		[Test]
		public void ShiftLeftU2U2([U2]ushort a, [I1UpTo16]byte b)
		{
			Assert.AreEqual(UInt16Tests.ShiftLeftU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "ShiftLeftU2U2", a, b));
		}

		[Test]
		public void ShiftRightU2U2([U2]ushort a, [I1UpTo16]byte b)
		{
			Assert.AreEqual(UInt16Tests.ShiftRightU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "ShiftRightU2U2", a, b));
		}

		[Test]
		public void CeqU2U2([U2]ushort a, [U2]ushort b)
		{
			Assert.AreEqual(UInt16Tests.CeqU2U2(a, b), Run<bool>("Mosa.Test.Collection", "UInt16Tests", "CeqU2U2", a, b));
		}

		[Test]
		public void CltU2U2([U2]ushort a, [U2]ushort b)
		{
			Assert.AreEqual(UInt16Tests.CltU2U2(a, b), Run<bool>("Mosa.Test.Collection", "UInt16Tests", "CltU2U2", a, b));
		}

		[Test]
		public void CgtU2U2([U2]ushort a, [U2]ushort b)
		{
			Assert.AreEqual(UInt16Tests.CgtU2U2(a, b), Run<bool>("Mosa.Test.Collection", "UInt16Tests", "CgtU2U2", a, b));
		}

		[Test]
		public void CleU2U2([U2]ushort a, [U2]ushort b)
		{
			Assert.AreEqual(UInt16Tests.CleU2U2(a, b), Run<bool>("Mosa.Test.Collection", "UInt16Tests", "CleU2U2", a, b));
		}

		[Test]
		public void CgeU2U2([U2]ushort a, [U2]ushort b)
		{
			Assert.AreEqual(UInt16Tests.CgeU2U2(a, b), Run<bool>("Mosa.Test.Collection", "UInt16Tests", "CgeU2U2", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt16Tests", "Newarr"));
		}

		[Test]
		public void Ldlen([I4Small]int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt16Tests", "Ldlen", length));
		}

		[Test]
		public void StelemU2([I4Small]int index, [U2]ushort value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt16Tests", "Stelem", index, value));
		}

		[Test]
		public void LdelemU2([I4Small]int index, [U2]ushort value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt16Tests", "Ldelem", index, value));
		}

		[Test]
		public void LdelemaU2([I4Small]int index, [U2]ushort value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt16Tests", "Ldelema", index, value));
		}

	}
}
