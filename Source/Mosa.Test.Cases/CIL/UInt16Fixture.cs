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

using Mosa.Test.Runtime.CompilerFramework;
using Mosa.Test.Runtime.CompilerFramework.Numbers;
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

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void AddU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UInt16Tests.AddU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "AddU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void SubU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UInt16Tests.SubU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "SubU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void MulU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UInt16Tests.MulU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "MulU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2WithoutZero")]
		public void DivU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UInt16Tests.DivU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "DivU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivU2U2DivideByZeroException(ushort a, ushort b)
		{
			Assert.AreEqual(UInt16Tests.DivU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "DivU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2WithoutZero")]
		public void RemU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UInt16Tests.RemU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "RemU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemU2U2DivideByZeroException(ushort a, ushort b)
		{
			Assert.AreEqual(UInt16Tests.RemU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "RemU2U2", a, b));
		}

		[Test, Factory(typeof(U2), "Samples")]
		public void RetU2(ushort a)
		{
			Assert.AreEqual(UInt16Tests.RetU2(a), Run<ushort>("Mosa.Test.Collection", "UInt16Tests", "RetU2", a));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void AndU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UInt16Tests.AndU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "AndU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void OrU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UInt16Tests.OrU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "OrU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void XorU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UInt16Tests.XorU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "XorU2U2", a, b));
		}

		[Test, Factory(typeof(U2), "Samples")]
		public void CompU2(ushort a)
		{
			Assert.AreEqual(UInt16Tests.CompU2(a), Run<int>("Mosa.Test.Collection", "UInt16Tests", "CompU2", a));
		}

		[Test, Factory(typeof(Variations), "U2_I1UpTo16")]
		public void ShiftLeftU2U2(ushort a, byte b)
		{
			Assert.AreEqual(UInt16Tests.ShiftLeftU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "ShiftLeftU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_I1UpTo16")]
		public void ShiftRightU2U2(ushort a, byte b)
		{
			Assert.AreEqual(UInt16Tests.ShiftRightU2U2(a, b), Run<int>("Mosa.Test.Collection", "UInt16Tests", "ShiftRightU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void CeqU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UInt16Tests.CeqU2U2(a, b), Run<bool>("Mosa.Test.Collection", "UInt16Tests", "CeqU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void CltU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UInt16Tests.CltU2U2(a, b), Run<bool>("Mosa.Test.Collection", "UInt16Tests", "CltU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void CgtU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UInt16Tests.CgtU2U2(a, b), Run<bool>("Mosa.Test.Collection", "UInt16Tests", "CgtU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void CleU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UInt16Tests.CleU2U2(a, b), Run<bool>("Mosa.Test.Collection", "UInt16Tests", "CleU2U2", a, b));
		}

		[Test, Factory(typeof(Variations), "U2_U2")]
		public void CgeU2U2(ushort a, ushort b)
		{
			Assert.AreEqual(UInt16Tests.CgeU2U2(a, b), Run<bool>("Mosa.Test.Collection", "UInt16Tests", "CgeU2U2", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt16Tests", "Newarr"));
		}

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void Ldlen(int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt16Tests", "Ldlen", length));
		}

		[Test, Factory(typeof(Variations), "ISmall_U2")]
		public void StelemU2(int index, ushort value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt16Tests", "Stelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_U2")]
		public void LdelemU2(int index, ushort value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt16Tests", "Ldelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_U2")]
		public void LdelemaU2(int index, ushort value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt16Tests", "Ldelema", index, value));
		}

	}
}
