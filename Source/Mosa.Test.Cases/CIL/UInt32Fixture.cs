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
	public class UInt32Fixture : TestCompilerAdapter
	{

		public UInt32Fixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void AddU4U4(uint a, uint b)
		{
			Assert.AreEqual(UInt32Tests.AddU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "AddU4U4", a, b));
		}

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void SubU4U4(uint a, uint b)
		{
			Assert.AreEqual(UInt32Tests.SubU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "SubU4U4", a, b));
		}

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void MulU4U4(uint a, uint b)
		{
			Assert.AreEqual(UInt32Tests.MulU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "MulU4U4", a, b));
		}

		[Test, Factory(typeof(Variations), "U4_U4WithoutZero")]
		public void DivU4U4(uint a, uint b)
		{
			Assert.AreEqual(UInt32Tests.DivU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "DivU4U4", a, b));
		}

		[Test, Factory(typeof(Variations), "U4_U4Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivU4U4DivideByZeroException(uint a, uint b)
		{
			Assert.AreEqual(UInt32Tests.DivU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "DivU4U4", a, b));
		}

		[Test, Factory(typeof(Variations), "U4_U4WithoutZero")]
		public void RemU4U4(uint a, uint b)
		{
			Assert.AreEqual(UInt32Tests.RemU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "RemU4U4", a, b));
		}

		[Test, Factory(typeof(Variations), "U4_U4Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemU4U4DivideByZeroException(uint a, uint b)
		{
			Assert.AreEqual(UInt32Tests.RemU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "RemU4U4", a, b));
		}

		[Test, Factory(typeof(U4), "Samples")]
		public void RetU4(uint a)
		{
			Assert.AreEqual(UInt32Tests.RetU4(a), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "RetU4", a));
		}

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void AndU4U4(uint a, uint b)
		{
			Assert.AreEqual(UInt32Tests.AndU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "AndU4U4", a, b));
		}

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void OrU4U4(uint a, uint b)
		{
			Assert.AreEqual(UInt32Tests.OrU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "OrU4U4", a, b));
		}

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void XorU4U4(uint a, uint b)
		{
			Assert.AreEqual(UInt32Tests.XorU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "XorU4U4", a, b));
		}

		[Test, Factory(typeof(U4), "Samples")]
		public void CompU4(uint a)
		{
			Assert.AreEqual(UInt32Tests.CompU4(a), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "CompU4", a));
		}

		[Test, Factory(typeof(Variations), "U4_U1UpTo32")]
		public void ShiftLeftU4U4(uint a, byte b)
		{
			Assert.AreEqual(UInt32Tests.ShiftLeftU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "ShiftLeftU4U4", a, b));
		}

		[Test, Factory(typeof(Variations), "U4_U1UpTo32")]
		public void ShiftRightU4U4(uint a, byte b)
		{
			Assert.AreEqual(UInt32Tests.ShiftRightU4U4(a, b), Run<uint>("Mosa.Test.Collection", "UInt32Tests", "ShiftRightU4U4", a, b));
		}

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void CeqU4U4(uint a, uint b)
		{
			Assert.AreEqual(UInt32Tests.CeqU4U4(a, b), Run<bool>("Mosa.Test.Collection", "UInt32Tests", "CeqU4U4", a, b));
		}

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void CltU4U4(uint a, uint b)
		{
			Assert.AreEqual(UInt32Tests.CltU4U4(a, b), Run<bool>("Mosa.Test.Collection", "UInt32Tests", "CltU4U4", a, b));
		}

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void CgtU4U4(uint a, uint b)
		{
			Assert.AreEqual(UInt32Tests.CgtU4U4(a, b), Run<bool>("Mosa.Test.Collection", "UInt32Tests", "CgtU4U4", a, b));
		}

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void CleU4U4(uint a, uint b)
		{
			Assert.AreEqual(UInt32Tests.CleU4U4(a, b), Run<bool>("Mosa.Test.Collection", "UInt32Tests", "CleU4U4", a, b));
		}

		[Test, Factory(typeof(Variations), "U4_U4")]
		public void CgeU4U4(uint a, uint b)
		{
			Assert.AreEqual(UInt32Tests.CgeU4U4(a, b), Run<bool>("Mosa.Test.Collection", "UInt32Tests", "CgeU4U4", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt32Tests", "Newarr"));
		}

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void Ldlen(int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt32Tests", "Ldlen", length));
		}

		[Test, Factory(typeof(Variations), "ISmall_U4")]
		public void StelemU4(int index, uint value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt32Tests", "Stelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_U4")]
		public void LdelemU4(int index, uint value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt32Tests", "Ldelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_U4")]
		public void LdelemaU4(int index, uint value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "UInt32Tests", "Ldelema", index, value));
		}

	}
}
