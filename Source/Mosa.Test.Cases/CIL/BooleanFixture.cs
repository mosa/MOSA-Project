/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  
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
	//[Category(@"Basic types")]
	//[Description(@"Tests support for the basic type System.Boolean")]
	public class BooleanFixture : TestCompilerAdapter
	{

		public BooleanFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test, Factory(typeof(B), "Samples")]
		public void RetB(bool a)
		{
			Assert.AreEqual(BooleanTests.RetB(a), Run<bool>("Mosa.Test.Collection", "BooleanTests", "RetB", a));
		}

		[Test, Factory(typeof(Variations), "B_B")]
		public void AndBB(bool a, bool b)
		{
			Assert.AreEqual(BooleanTests.AndBB(a, b), Run<bool>("Mosa.Test.Collection", "BooleanTests", "AndBB", a, b));
		}

		[Test, Factory(typeof(Variations), "B_B")]
		public void OrBB(bool a, bool b)
		{
			Assert.AreEqual(BooleanTests.OrBB(a, b), Run<bool>("Mosa.Test.Collection", "BooleanTests", "OrBB", a, b));
		}

		[Test, Factory(typeof(Variations), "B_B")]
		public void XorBB(bool a, bool b)
		{
			Assert.AreEqual(BooleanTests.XorBB(a, b), Run<bool>("Mosa.Test.Collection", "BooleanTests", "XorBB", a, b));
		}

		[Test, Factory(typeof(B), "Samples")]
		public void NotB(bool a)
		{
			Assert.AreEqual(BooleanTests.NotB(a), Run<bool>("Mosa.Test.Collection", "BooleanTests", "NotB", a));
		}

		[Test]
		public void Newarr()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BooleanTests", "Newarr"));
		}

		[Test, Factory(typeof(Variations), "SmallNumbers")]
		public void Ldlen(int length)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BooleanTests", "Ldlen", length));
		}

		[Test, Factory(typeof(Variations), "ISmall_B")]
		public void StelemB(int index, bool value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BooleanTests", "Stelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_B")]
		public void LdelemB(int index, bool value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BooleanTests", "Ldelem", index, value));
		}

		[Test, Factory(typeof(Variations), "ISmall_B")]
		public void LdelemaB(int index, bool value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "BooleanTests", "Ldelema", index, value));
		}
	}
}
