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
	public class Int64Fixture : TestCompilerAdapter
	{

		public Int64Fixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void AddI8I8(long a, long b)
		{
			Assert.AreEqual(Int64Tests.AddI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "AddI8I8", a, b));
		}

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void SubI8I8(long a, long b)
		{
			Assert.AreEqual(Int64Tests.SubI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "SubI8I8", a, b));
		}

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void MulI8I8(long a, long b)
		{
			Assert.AreEqual(Int64Tests.MulI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "MulI8I8", a, b));
		}

		[Test, Factory(typeof(Variations), "I8_I8WithoutZero")]
		public void DivI8I8(long a, long b)
		{
			Assert.AreEqual(Int64Tests.DivI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "DivI8I8", a, b));
		}

		[Test, Factory(typeof(Variations), "I8_I8Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivI8I8DivideByZeroException(long a, long b)
		{
			Assert.AreEqual(Int64Tests.DivI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "DivI8I8", a, b));
		}

		[Test, Factory(typeof(Variations), "I8_I8WithoutZero")]
		public void RemI8I8(long a, long b)
		{
			Assert.AreEqual(Int64Tests.RemI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "RemI8I8", a, b));
		}

		[Test, Factory(typeof(Variations), "I8_I8Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemI8I8DivideByZeroException(long a, long b)
		{
			Assert.AreEqual(Int64Tests.RemI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "RemI8I8", a, b));
		}

		[Test, Factory(typeof(I8), "Samples")]
		public void RetI8(long a)
		{
			Assert.AreEqual(Int64Tests.RetI8(a), Run<long>("Mosa.Test.Collection", "Int64Tests", "RetI8", a));
		}

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void AndI8I8(long a, long b)
		{
			Assert.AreEqual(Int64Tests.AndI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "AndI8I8", a, b));
		}

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void OrI8I8(long a, long b)
		{
			Assert.AreEqual(Int64Tests.OrI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "OrI8I8", a, b));
		}

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void XorI8I8(long a, long b)
		{
			Assert.AreEqual(Int64Tests.XorI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "XorI8I8", a, b));
		}

		//[Test, Factory(typeof(I8), "Samples")]
		//public void CompI8(long a)
		//{
		//    Assert.AreEqual(Int64Tests.CompI8(a), Run<long>("Mosa.Test.Collection", "Int64Tests", "CompI8", a));
		//}

		//[Test, Factory(typeof(Variations), "I8_I8UpTo8")]
		//public void ShiftLeftI8I8(long a, byte b)
		//{
		//    Assert.AreEqual(Int64Tests.ShiftLeftI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "ShiftLeftI8I8", a, b));
		//}

		//[Test, Factory(typeof(Variations), "I8_I8UpTo8")]
		//public void ShiftRightI8I8(long a, byte b)
		//{
		//    Assert.AreEqual(Int64Tests.ShiftRightI8I8(a, b), Run<long>("Mosa.Test.Collection", "Int64Tests", "ShiftRightI8I8", a, b));
		//}

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void CeqI8I8(long a, long b)
		{
			Assert.AreEqual(Int64Tests.CeqI8I8(a, b), Run<bool>("Mosa.Test.Collection", "Int64Tests", "CeqI8I8", a, b));
		}

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void CltI8I8(long a, long b)
		{
			Assert.AreEqual(Int64Tests.CltI8I8(a, b), Run<bool>("Mosa.Test.Collection", "Int64Tests", "CltI8I8", a, b));
		}

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void CgtI8I8(long a, long b)
		{
			Assert.AreEqual(Int64Tests.CgtI8I8(a, b), Run<bool>("Mosa.Test.Collection", "Int64Tests", "CgtI8I8", a, b));
		}

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void CleI8I8(long a, long b)
		{
			Assert.AreEqual(Int64Tests.CleI8I8(a, b), Run<bool>("Mosa.Test.Collection", "Int64Tests", "CleI8I8", a, b));
		}

		[Test, Factory(typeof(Variations), "I8_I8")]
		public void CgeI8I8(long a, long b)
		{
			Assert.AreEqual(Int64Tests.CgeI8I8(a, b), Run<bool>("Mosa.Test.Collection", "Int64Tests", "CgeI8I8", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int64Tests", "Newarr"));
		}

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void Ldlen(int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int64Tests", "Ldlen", length));
		}

		[Test, Factory(typeof(Variations), "ISmall_I8")]
		public void StelemI8(int index, long value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int64Tests", "Stelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_I8")]
		public void LdelemI8(int index, long value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int64Tests", "Ldelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_I8")]
		public void LdelemaI8(int index, long value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int64Tests", "Ldelema", index, value));
		}

	}
}
