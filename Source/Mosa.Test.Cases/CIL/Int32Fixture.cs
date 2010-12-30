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
	public class Int32Fixture : TestCompilerAdapter
	{

		public Int32Fixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void AddI4I4(int a, int b)
		{
			Assert.AreEqual(Int32Tests.AddI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "AddI4I4", a, b));
		}

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void SubI4I4(int a, int b)
		{
			Assert.AreEqual(Int32Tests.SubI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "SubI4I4", a, b));
		}

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void MulI4I4(int a, int b)
		{
			Assert.AreEqual(Int32Tests.MulI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "MulI4I4", a, b));
		}

		[Test, Factory(typeof(Variations), "I4_I4WithoutZero")]
		public void DivI4I4(int a, int b)
		{
			Assert.AreEqual(Int32Tests.DivI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "DivI4I4", a, b));
		}

		[Test, Factory(typeof(Variations), "I4_I4Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivI4I4DivideByZeroException(int a, int b)
		{
			Assert.AreEqual(Int32Tests.DivI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "DivI4I4", a, b));
		}

		[Test, Factory(typeof(Variations), "I4_I4WithoutZero")]
		public void RemI4I4(int a, int b)
		{
			Assert.AreEqual(Int32Tests.RemI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "RemI4I4", a, b));
		}

		[Test, Factory(typeof(Variations), "I4_I4Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemI4I4DivideByZeroException(int a, int b)
		{
			Assert.AreEqual(Int32Tests.RemI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "RemI4I4", a, b));
		}

		[Test, Factory(typeof(I4), "Samples")]
		public void RetI4(int a)
		{
			Assert.AreEqual(Int32Tests.RetI4(a), Run<int>("Mosa.Test.Collection", "Int32Tests", "RetI4", a));
		}

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void AndI4I4(int a, int b)
		{
			Assert.AreEqual(Int32Tests.AndI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "AndI4I4", a, b));
		}

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void OrI4I4(int a, int b)
		{
			Assert.AreEqual(Int32Tests.OrI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "OrI4I4", a, b));
		}

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void XorI4I4(int a, int b)
		{
			Assert.AreEqual(Int32Tests.XorI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "XorI4I4", a, b));
		}

		//[Test, Factory(typeof(I4), "Samples")]
		//public void CompI4(int a)
		//{
		//    Assert.AreEqual(Int32Tests.CompI4(a), Run<int>("Mosa.Test.Collection", "Int32Tests", "CompI4", a));
		//}

		//[Test, Factory(typeof(Variations), "I4_I4UpTo8")]
		//public void ShiftLeftI4I4(int a, byte b)
		//{
		//    Assert.AreEqual(Int32Tests.ShiftLeftI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "ShiftLeftI4I4", a, b));
		//}

		//[Test, Factory(typeof(Variations), "I4_I4UpTo8")]
		//public void ShiftRightI4I4(int a, byte b)
		//{
		//    Assert.AreEqual(Int32Tests.ShiftRightI4I4(a, b), Run<int>("Mosa.Test.Collection", "Int32Tests", "ShiftRightI4I4", a, b));
		//}

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void CeqI4I4(int a, int b)
		{
			Assert.AreEqual(Int32Tests.CeqI4I4(a, b), Run<bool>("Mosa.Test.Collection", "Int32Tests", "CeqI4I4", a, b));
		}

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void CltI4I4(int a, int b)
		{
			Assert.AreEqual(Int32Tests.CltI4I4(a, b), Run<bool>("Mosa.Test.Collection", "Int32Tests", "CltI4I4", a, b));
		}

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void CgtI4I4(int a, int b)
		{
			Assert.AreEqual(Int32Tests.CgtI4I4(a, b), Run<bool>("Mosa.Test.Collection", "Int32Tests", "CgtI4I4", a, b));
		}

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void CleI4I4(int a, int b)
		{
			Assert.AreEqual(Int32Tests.CleI4I4(a, b), Run<bool>("Mosa.Test.Collection", "Int32Tests", "CleI4I4", a, b));
		}

		[Test, Factory(typeof(Variations), "I4_I4")]
		public void CgeI4I4(int a, int b)
		{
			Assert.AreEqual(Int32Tests.CgeI4I4(a, b), Run<bool>("Mosa.Test.Collection", "Int32Tests", "CgeI4I4", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int32Tests", "Newarr"));
		}

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void Ldlen(int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int32Tests", "Ldlen", length));
		}

		[Test, Factory(typeof(Variations), "ISmall_I4")]
		public void StelemI4(int index, int value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int32Tests", "Stelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_I4")]
		public void LdelemI4(int index, int value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int32Tests", "Ldelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_I4")]
		public void LdelemaI4(int index, int value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int32Tests", "Ldelema", index, value));
		}

	}
}
