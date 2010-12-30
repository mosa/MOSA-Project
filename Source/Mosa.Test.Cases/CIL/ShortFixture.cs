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
	public class ShortFixture : TestCompilerAdapter
	{

		public ShortFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void AddI2_I2(short a, short b)
		{
			Assert.AreEqual(ShortTests.AddI2I2(a, b), Run<int>("Mosa.Test.Collection", "ShortTests", "AddI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void SubI2_I2(short a, short b)
		{
			Assert.AreEqual(ShortTests.SubI2I2(a, b), Run<int>("Mosa.Test.Collection", "ShortTests", "SubI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void MulI2_I2(short a, short b)
		{
			Assert.AreEqual(ShortTests.MulI2I2(a, b), Run<int>("Mosa.Test.Collection", "ShortTests", "MulI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2WithoutZero")]
		public void DivI2_I2(short a, short b)
		{
			Assert.AreEqual(ShortTests.DivI2I2(a, b), Run<int>("Mosa.Test.Collection", "ShortTests", "DivI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivI2_I2DivideByZeroException(short a, short b)
		{
			Assert.AreEqual(ShortTests.DivI2I2(a, b), Run<int>("Mosa.Test.Collection", "ShortTests", "DivI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2WithoutZero")]
		public void RemI2_I2(short a, short b)
		{
			Assert.AreEqual(ShortTests.RemI2I2(a, b), Run<int>("Mosa.Test.Collection", "ShortTests", "RemI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2Zero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemI2_I2DivideByZeroException(short a, short b)
		{
			Assert.AreEqual(ShortTests.RemI2I2(a, b), Run<int>("Mosa.Test.Collection", "ShortTests", "RemI2I2", a, b));
		}

		[Test, Factory(typeof(I1), "Samples")]
		public void RetI2(short a)
		{
			Assert.AreEqual(ShortTests.RetI2(a), Run<short>("Mosa.Test.Collection", "ShortTests", "RetI2", a));
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void AndI2_I2(short a, short b)
		{
			Assert.AreEqual(ShortTests.AndI2I2(a, b), Run<int>("Mosa.Test.Collection", "ShortTests", "AndI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void OrI2_I2(short a, short b)
		{
			Assert.AreEqual(ShortTests.OrI2I2(a, b), Run<int>("Mosa.Test.Collection", "ShortTests", "OrI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void XorI2_I2(short a, short b)
		{
			Assert.AreEqual(ShortTests.XorI2I2(a, b), Run<int>("Mosa.Test.Collection", "ShortTests", "XorI2I2", a, b));
		}

		//[Test, Factory(typeof(I2), "Samples")]
		//public void CompI2(short a)
		//{
		//    Assert.AreEqual(ShortTests.CompI2(a), Run<int>("Mosa.Test.Collection", "ShortTests", "CompI2", a));
		//}

		//[Test, Factory(typeof(Variations), "I2_I2UpTo8")]
		//public void ShiftLeftI2_I2(short a, byte b)
		//{
		//    Assert.AreEqual(ShortTests.ShiftLeftI2_I2(a, b), Run<int>("Mosa.Test.Collection", "ShortTests", "ShiftLeftI2I2", a, b));
		//}

		//[Test, Factory(typeof(Variations), "I2_I2UpTo8")]
		//public void ShiftRightI2_I2(short a, byte b)
		//{
		//    Assert.AreEqual(ShortTests.ShiftRightI2_I2(a, b), Run<int>("Mosa.Test.Collection", "ShortTests", "ShiftRightI2I2", a, b));
		//}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void CeqI2_I2(short a, short b)
		{
			Assert.AreEqual(ShortTests.CeqI2I2(a, b), Run<bool>("Mosa.Test.Collection", "ShortTests", "CeqI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void CltI2_I2(short a, short b)
		{
			Assert.AreEqual(ShortTests.CltI2I2(a, b), Run<bool>("Mosa.Test.Collection", "ShortTests", "CltI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void CgtI2_I2(short a, short b)
		{
			Assert.AreEqual(ShortTests.CgtI2I2(a, b), Run<bool>("Mosa.Test.Collection", "ShortTests", "CgtI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void CleI2_I2(short a, short b)
		{
			Assert.AreEqual(ShortTests.CleI2I2(a, b), Run<bool>("Mosa.Test.Collection", "ShortTests", "CleI2I2", a, b));
		}

		[Test, Factory(typeof(Variations), "I2_I2")]
		public void CgeI2_I2(short a, short b)
		{
			Assert.AreEqual(ShortTests.CgeI2I2(a, b), Run<bool>("Mosa.Test.Collection", "ShortTests", "CgeI2I2", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ShortTests", "Newarr"));
		}

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void Ldlen(int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ShortTests", "Ldlen", length));
		}

		[Test, Factory(typeof(Variations), "ISmall_I1")]
		public void StelemI2(int index, short value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ShortTests", "Stelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_I1")]
		public void LdelemI2(int index, short value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ShortTests", "Ldelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_I1")]
		public void LdelemaI2(int index, short value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ShortTests", "Ldelema", index, value));
		}

	}
}
