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
	public class CharFixture : TestCompilerAdapter
	{

		public CharFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test, Factory(typeof(Variations), "C_C")]
		public void AddCC(char a, char b)
		{
			Assert.AreEqual(CharTests.AddCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "AddCC", a, b));
		}

		[Test, Factory(typeof(Variations), "C_C")]
		public void SubCC(char a, char b)
		{
			Assert.AreEqual(CharTests.SubCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "SubCC", a, b));
		}

		[Test, Factory(typeof(Variations), "C_C")]
		public void MulCC(char a, char b)
		{
			Assert.AreEqual(CharTests.MulCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "MulCC", a, b));
		}

		[Test, Factory(typeof(Variations), "C_CWithoutZero")]
		public void DivCC(char a, char b)
		{
			Assert.AreEqual(CharTests.DivCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "DivCC", a, b));
		}

		[Test, Factory(typeof(Variations), "C_CZero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivCCDivideByZeroException(char a, char b)
		{
			Assert.AreEqual(CharTests.DivCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "DivCC", a, b));
		}

		[Test, Factory(typeof(Variations), "C_CWithoutZero")]
		public void RemCC(char a, char b)
		{
			Assert.AreEqual(CharTests.RemCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "RemCC", a, b));
		}

		[Test, Factory(typeof(Variations), "C_CZero")]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemCCDivideByZeroException(char a, char b)
		{
			Assert.AreEqual(CharTests.RemCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "RemCC", a, b));
		}

		[Test, Factory(typeof(C), "Samples")]
		public void RetC(char a)
		{
			Assert.AreEqual(CharTests.RetC(a), Run<char>("Mosa.Test.Collection", "CharTests", "RetC", a));
		}

		[Test, Factory(typeof(Variations), "C_C")]
		public void AndCC(char a, char b)
		{
			Assert.AreEqual(CharTests.AndCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "AndCC", a, b));
		}

		[Test, Factory(typeof(Variations), "C_C")]
		public void OrCC(char a, char b)
		{
			Assert.AreEqual(CharTests.OrCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "OrCC", a, b));
		}

		[Test, Factory(typeof(Variations), "C_C")]
		public void XorCC(char a, char b)
		{
			Assert.AreEqual(CharTests.XorCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "XorCC", a, b));
		}

		//[Test, Factory(typeof(C), "Samples")]
		//public void CompC(char a)
		//{
		//    Assert.AreEqual(CharTests.CompC(a), Run<int>("Mosa.Test.Collection", "CharTests", "CompC", a));
		//}

		//[Test, Factory(typeof(Variations), "C_CUpTo8")]
		//public void ShiftLeftCC(char a, byte b)
		//{
		//    Assert.AreEqual(CharTests.ShiftLeftCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "ShiftLeftCC", a, b));
		//}

		//[Test, Factory(typeof(Variations), "C_CUpTo8")]
		//public void ShiftRightCC(char a, byte b)
		//{
		//    Assert.AreEqual(CharTests.ShiftRightCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "ShiftRightCC", a, b));
		//}

		[Test, Factory(typeof(Variations), "C_C")]
		public void CeqCC(char a, char b)
		{
			Assert.AreEqual(CharTests.CeqCC(a, b), Run<bool>("Mosa.Test.Collection", "CharTests", "CeqCC", a, b));
		}

		[Test, Factory(typeof(Variations), "C_C")]
		public void CltCC(char a, char b)
		{
			Assert.AreEqual(CharTests.CltCC(a, b), Run<bool>("Mosa.Test.Collection", "CharTests", "CltCC", a, b));
		}

		[Test, Factory(typeof(Variations), "C_C")]
		public void CgtCC(char a, char b)
		{
			Assert.AreEqual(CharTests.CgtCC(a, b), Run<bool>("Mosa.Test.Collection", "CharTests", "CgtCC", a, b));
		}

		[Test, Factory(typeof(Variations), "C_C")]
		public void CleCC(char a, char b)
		{
			Assert.AreEqual(CharTests.CleCC(a, b), Run<bool>("Mosa.Test.Collection", "CharTests", "CleCC", a, b));
		}

		[Test, Factory(typeof(Variations), "C_C")]
		public void CgeCC(char a, char b)
		{
			Assert.AreEqual(CharTests.CgeCC(a, b), Run<bool>("Mosa.Test.Collection", "CharTests", "CgeCC", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CharTests", "Newarr"));
		}

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void Ldlen(int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CharTests", "Ldlen", length));
		}

		[Test, Factory(typeof(Variations), "ISmall_C")]
		public void StelemC(int index, char value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CharTests", "Stelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_C")]
		public void LdelemC(int index, char value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CharTests", "Ldelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_C")]
		public void LdelemaC(int index, char value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CharTests", "Ldelema", index, value));
		}

	}
}
