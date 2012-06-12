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
	public class CharFixture : TestCompilerAdapter
	{

		public CharFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void AddCC([C]char a, [C]char b)
		{
			Assert.AreEqual(CharTests.AddCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "AddCC", a, b));
		}

		[Test]
		public void SubCC([C]char a, [C]char b)
		{
			Assert.AreEqual(CharTests.SubCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "SubCC", a, b));
		}

		[Test]
		public void MulCC([C]char a, [C]char b)
		{
			Assert.AreEqual(CharTests.MulCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "MulCC", a, b));
		}

		[Test]
		public void DivCC([C]char a, [CNotZero]char b)
		{
			Assert.AreEqual(CharTests.DivCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "DivCC", a, b));
		}

		[Test]
		[Pending]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivCCDivideByZeroException([C]char a)
		{
			Assert.AreEqual(CharTests.DivCC(a, (char)0), Run<int>("Mosa.Test.Collection", "CharTests", "DivCC", a, (char)0));
		}

		[Test]
		public void RemCC([C]char a, [CNotZero]char b)
		{
			Assert.AreEqual(CharTests.RemCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "RemCC", a, b));
		}

		[Test]
		[Pending]
		[ExpectedException(typeof(DivideByZeroException))]
		public void RemCCDivideByZeroException([C]char a, [C]char b)
		{
			Assert.AreEqual(CharTests.RemCC(a, (char)0), Run<int>("Mosa.Test.Collection", "CharTests", "RemCC", a, (char)0));
		}

		[Test]
		public void RetC([C]char a)
		{
			Assert.AreEqual(CharTests.RetC(a), Run<char>("Mosa.Test.Collection", "CharTests", "RetC", a));
		}

		[Test]
		public void AndCC([C]char a, [C]char b)
		{
			Assert.AreEqual(CharTests.AndCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "AndCC", a, b));
		}

		[Test]
		public void OrCC([C]char a, [C]char b)
		{
			Assert.AreEqual(CharTests.OrCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "OrCC", a, b));
		}

		[Test]
		public void XorCC([C]char a, [C]char b)
		{
			Assert.AreEqual(CharTests.XorCC(a, b), Run<int>("Mosa.Test.Collection", "CharTests", "XorCC", a, b));
		}

		[Test]
		public void CeqCC([C]char a, [C]char b)
		{
			Assert.AreEqual(CharTests.CeqCC(a, b), Run<bool>("Mosa.Test.Collection", "CharTests", "CeqCC", a, b));
		}

		[Test]
		public void CltCC([C]char a, [C]char b)
		{
			Assert.AreEqual(CharTests.CltCC(a, b), Run<bool>("Mosa.Test.Collection", "CharTests", "CltCC", a, b));
		}

		[Test]
		public void CgtCC([C]char a, [C]char b)
		{
			Assert.AreEqual(CharTests.CgtCC(a, b), Run<bool>("Mosa.Test.Collection", "CharTests", "CgtCC", a, b));
		}

		[Test]
		public void CleCC([C]char a, [C]char b)
		{
			Assert.AreEqual(CharTests.CleCC(a, b), Run<bool>("Mosa.Test.Collection", "CharTests", "CleCC", a, b));
		}

		[Test]
		public void CgeCC([C]char a, [C]char b)
		{
			Assert.AreEqual(CharTests.CgeCC(a, b), Run<bool>("Mosa.Test.Collection", "CharTests", "CgeCC", a, b));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CharTests", "Newarr"));
		}

		[Test]
		public void Ldlen([I4Small] int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CharTests", "Ldlen", length));
		}

		[Test]
		public void StelemC([I4Small] int index, [C]char value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CharTests", "Stelem", index, value));
		}

		[Test]
		public void LdelemC([I4Small] int index, [C]char value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CharTests", "Ldelem", index, value));
		}

		[Test]
		public void LdelemaC([I4Small] int index, [C]char value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CharTests", "Ldelema", index, value));
		}

	}
}
