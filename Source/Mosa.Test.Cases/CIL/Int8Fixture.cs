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
	public class Int8Fixture : TestCompilerAdapter
	{

		public Int8Fixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void AddI1I1(sbyte a, sbyte b)
		{
			Assert.AreEqual(Int8Tests.AddI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "AddI1I1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void SubI1I1(sbyte a, sbyte b)
		{
			Assert.AreEqual(Int8Tests.SubI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "SubI1I1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void MulI1I1(sbyte a, sbyte b)
		{
			Assert.AreEqual(Int8Tests.MulI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "MulI1I1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1WithoutZero")]
		public void DivI1I1(sbyte a, sbyte b)
		{
			Assert.AreEqual(Int8Tests.DivI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "DivI1I1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivI1I1DivideByZeroException(sbyte a, sbyte b)
		{
			Assert.AreEqual(Int8Tests.DivI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "DivI1I1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1WithoutZero")]
		public void RemI1I1(sbyte a, sbyte b)
		{
			Assert.AreEqual(Int8Tests.RemI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "RemI1I1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemI1I1DivideByZeroException(sbyte a, sbyte b)
		{
			Assert.AreEqual(Int8Tests.RemI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "RemI1I1", a, b));
		}

		[Test, Factory(typeof(I1), "Samples")]
		public void RetI1(sbyte a)
		{
			Assert.AreEqual(Int8Tests.RetI1(a), Run<sbyte>("Mosa.Test.Collection", "Int8Tests", "RetI1", a));
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void AndI1I1(sbyte a, sbyte b)
		{
			Assert.AreEqual(Int8Tests.AndI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "AndI1I1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void OrI1I1(sbyte a, sbyte b)
		{
			Assert.AreEqual(Int8Tests.OrI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "OrI1I1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void XorI1I1(sbyte a, sbyte b)
		{
			Assert.AreEqual(Int8Tests.XorI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "XorI1I1", a, b));
		}

		//[Test, Factory(typeof(I1), "Samples")]
		//public void CompI1(sbyte a)
		//{
		//    Assert.AreEqual(Int8Tests.CompI1(a), Run<int>("Mosa.Test.Collection", "Int8Tests", "CompI1", a));
		//}

		//[Test, Factory(typeof(Variations), "I1_I1UpTo8")]
		//public void ShiftLeftI1I1(sbyte a, byte b)
		//{
		//    Assert.AreEqual(Int8Tests.ShiftLeftI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "ShiftLeftI1I1", a, b));
		//}

		//[Test, Factory(typeof(Variations), "I1_I1UpTo8")]
		//public void ShiftRightI1I1(sbyte a, byte b)
		//{
		//    Assert.AreEqual(Int8Tests.ShiftRightI1I1(a, b), Run<int>("Mosa.Test.Collection", "Int8Tests", "ShiftRightI1I1", a, b));
		//}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void CeqI1I1(sbyte a, sbyte b)
		{
			Assert.AreEqual(Int8Tests.CeqI1I1(a, b), Run<bool>("Mosa.Test.Collection", "Int8Tests", "CeqI1I1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void CltI1I1(sbyte a, sbyte b)
		{
			Assert.AreEqual(Int8Tests.CltI1I1(a, b), Run<bool>("Mosa.Test.Collection", "Int8Tests", "CltI1I1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void CgtI1I1(sbyte a, sbyte b)
		{
			Assert.AreEqual(Int8Tests.CgtI1I1(a, b), Run<bool>("Mosa.Test.Collection", "Int8Tests", "CgtI1I1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void CleI1I1(sbyte a, sbyte b)
		{
			Assert.AreEqual(Int8Tests.CleI1I1(a, b), Run<bool>("Mosa.Test.Collection", "Int8Tests", "CleI1I1", a, b));
		}

		[Test, Factory(typeof(Variations), "I1_I1")]
		public void CgeI1I1(sbyte a, sbyte b)
		{
			Assert.AreEqual(Int8Tests.CgeI1I1(a, b), Run<bool>("Mosa.Test.Collection", "Int8Tests", "CgeI1I1", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int8Tests", "Newarr"));
		}

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void Ldlen(int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int8Tests", "Ldlen", length));
		}

		[Test, Factory(typeof(Variations), "ISmall_I1")]
		public void StelemI1(int index, sbyte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int8Tests", "Stelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_I1")]
		public void LdelemI1(int index, sbyte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int8Tests", "Ldelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_I1")]
		public void LdelemaI1(int index, sbyte value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int8Tests", "Ldelema", index, value));
		}

	}
}
