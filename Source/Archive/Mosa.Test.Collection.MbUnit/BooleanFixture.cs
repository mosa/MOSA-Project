/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *
 */

using MbUnit.Framework;
using Mosa.Test.Collection;
using Mosa.Test.System;
using Mosa.Test.Numbers;

namespace Mosa.Test.Collection.MbUnit
{
	[TestFixture]
	[Importance(Importance.Critical)]

	//[Category(@"Basic types")]
	//[Description(@"Tests support for the basic type System.Boolean")]
	public class BooleanFixture : TestCompilerAdapter
	{
		public BooleanFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void RetB([B] bool a)
		{
			Assert.AreEqual(BooleanTests.RetB(a), Run<bool>("Mosa.Test.Collection", "BooleanTests", "RetB", a));
		}

		[Test]
		public void AndBB([B]bool a, [B] bool b)
		{
			Assert.AreEqual(BooleanTests.AndBB(a, b), Run<bool>("Mosa.Test.Collection", "BooleanTests", "AndBB", a, b));
		}

		[Test]
		public void OrBB([B]bool a, [B] bool b)
		{
			Assert.AreEqual(BooleanTests.OrBB(a, b), Run<bool>("Mosa.Test.Collection", "BooleanTests", "OrBB", a, b));
		}

		[Test]
		public void XorBB([B]bool a, [B] bool b)
		{
			Assert.AreEqual(BooleanTests.XorBB(a, b), Run<bool>("Mosa.Test.Collection", "BooleanTests", "XorBB", a, b));
		}

		[Test]
		public void NotB([B]bool a)
		{
			Assert.AreEqual(BooleanTests.NotB(a), Run<bool>("Mosa.Test.Collection", "BooleanTests", "NotB", a));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BooleanTests", "Newarr"));
		}

		[Test]
		public void Ldlen([I4Small]int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BooleanTests", "Ldlen", length));
		}

		[Test]
		public void StelemB([I4Small]int index, [B]bool value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BooleanTests", "Stelem", index, value));
		}

		[Test]
		public void LdelemB([I4Small] int index, [B]bool value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BooleanTests", "Ldelem", index, value));
		}

		[Test]
		public void LdelemaB([I4Small]int index, [B]bool value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BooleanTests", "Ldelema", index, value));
		}
	}
}