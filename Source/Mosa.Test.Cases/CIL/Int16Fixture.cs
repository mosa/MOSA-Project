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
	public class Int16Fixture : TestCompilerAdapter
	{

		public Int16Fixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void AddI2_I2(short a, short b)
		{
			Assert.AreEqual(Int16Tests.AddI2I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "AddI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void SubI2_I2(short a, short b)
		{
			Assert.AreEqual(Int16Tests.SubI2I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "SubI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void MulI2_I2(short a, short b)
		{
			Assert.AreEqual(Int16Tests.MulI2I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "MulI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2WithoutZero")]
		public void DivI2_I2(short a, short b)
		{
			Assert.AreEqual(Int16Tests.DivI2I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "DivI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivI2_I2DivideByZeroException(short a, short b)
		{
			Assert.AreEqual(Int16Tests.DivI2I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "DivI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2WithoutZero")]
		public void RemI2_I2(short a, short b)
		{
			Assert.AreEqual(Int16Tests.RemI2I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "RemI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemI2_I2DivideByZeroException(short a, short b)
		{
			Assert.AreEqual(Int16Tests.RemI2I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "RemI2I2", a, b));
		}

		[Test, Factory(typeof(I1), "Samples")]
		public void RetI2(short a)
		{
			Assert.AreEqual(Int16Tests.RetI2(a), Run<short>("Mosa.Test.Collection", "Int16Tests", "RetI2", a));
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void AndI2_I2(short a, short b)
		{
			Assert.AreEqual(Int16Tests.AndI2I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "AndI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void OrI2_I2(short a, short b)
		{
			Assert.AreEqual(Int16Tests.OrI2I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "OrI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void XorI2_I2(short a, short b)
		{
			Assert.AreEqual(Int16Tests.XorI2I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "XorI2I2", a, b));
		}

		//[Test, Factory(typeof(I2), "Samples")]
		//public void CompI2(short a)
		//{
		//    Assert.AreEqual(Int16Tests.CompI2(a), Run<int>("Mosa.Test.Collection", "Int16Tests", "CompI2", a));
		//}

		//[Test, Factory(typeof(Variations), "I2_I2UpTo8")]
		//public void ShiftLeftI2_I2(short a, byte b)
		//{
		//    Assert.AreEqual(Int16Tests.ShiftLeftI2_I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "ShiftLeftI2I2", a, b));
		//}

		//[Test, Factory(typeof(Variations), "I2_I2UpTo8")]
		//public void ShiftRightI2_I2(short a, byte b)
		//{
		//    Assert.AreEqual(Int16Tests.ShiftRightI2_I2(a, b), Run<int>("Mosa.Test.Collection", "Int16Tests", "ShiftRightI2I2", a, b));
		//}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void CeqI2_I2(short a, short b)
		{
			Assert.AreEqual(Int16Tests.CeqI2I2(a, b), Run<bool>("Mosa.Test.Collection", "Int16Tests", "CeqI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void CltI2_I2(short a, short b)
		{
			Assert.AreEqual(Int16Tests.CltI2I2(a, b), Run<bool>("Mosa.Test.Collection", "Int16Tests", "CltI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void CgtI2_I2(short a, short b)
		{
			Assert.AreEqual(Int16Tests.CgtI2I2(a, b), Run<bool>("Mosa.Test.Collection", "Int16Tests", "CgtI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void CleI2_I2(short a, short b)
		{
			Assert.AreEqual(Int16Tests.CleI2I2(a, b), Run<bool>("Mosa.Test.Collection", "Int16Tests", "CleI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void CgeI2_I2(short a, short b)
		{
			Assert.AreEqual(Int16Tests.CgeI2I2(a, b), Run<bool>("Mosa.Test.Collection", "Int16Tests", "CgeI2I2", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int16Tests", "Newarr"));
		}

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void Ldlen(int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int16Tests", "Ldlen", length));
		}

		[Test, Factory(typeof(Variations), "ISmall_I1")]
		public void StelemI2(int index, short value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int16Tests", "Stelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_I1")]
		public void LdelemI2(int index, short value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int16Tests", "Ldelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_I1")]
		public void LdelemaI2(int index, short value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "Int16Tests", "Ldelema", index, value));
		}

	}
}
