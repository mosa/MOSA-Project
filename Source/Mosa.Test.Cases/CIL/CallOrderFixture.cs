/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman <mail.alex.lymangmail.com>
 *  Simon Wollwage (rootnode) <kintarothink-in-co.de>
 *  Michael Fröhlich (grover) <michael.ruckmichaelruck.de>
 *  Kai P. Reisert <kpreisertgooglemail.com>
 *  
 */

using MbUnit.Framework;
using Mosa.Test.Collection;
using Mosa.Test.System;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	public class CallOrderFixture : TestCompilerAdapter
	{

		public CallOrderFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void CallEmpty()
		{
			Run<object>("Mosa.Test.Collection", "CallOrderTests", "CallEmpty");
		}

		[Test]
		public void CallOrderI4()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallOrderTests", "CallOrderI4", (int)1));
		}

		[Test]
		public void CallOrderI4I4()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallOrderTests", "CallOrderI4I4", (int)1, (int)2));
		}

		[Test]
		public void CallOrderI4I4_2()
		{
			Assert.AreEqual(CallOrderTests.CallOrderI4I4_2((int)3, (int)4), Run<int>("Mosa.Test.Collection", "CallOrderTests", "CallOrderI4I4_2", (int)3, (int)4));
		}

		[Test]
		public void CallOrderU4U4()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallOrderTests", "CallOrderU4U4", (uint)1, (uint)2));
		}

		[Test]
		public void CallOrderU4U4_2()
		{
			Assert.AreEqual(CallOrderTests.CallOrderU4U4_2((uint)3, (uint)4), Run<uint>("Mosa.Test.Collection", "CallOrderTests", "CallOrderU4U4_2", (uint)3, (uint)4));
		}

		[Test]
		public void CallOrderI4I4I4()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallOrderTests", "CallOrderI4I4I4", (int)1, (int)2, (int)3));
		}

		[Test]
		public void CallOrderI4I4I4I4()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallOrderTests", "CallOrderI4I4I4I4", (int)1, (int)2, (int)3, (int)4));
		}

		[Test]
		public void CallOrderI4I4I4I4_2()
		{
			Assert.AreEqual(CallOrderTests.CallOrderI4I4I4I4_2((int)1, (int)2, (int)3, (int)4), Run<int>("Mosa.Test.Collection", "CallOrderTests", "CallOrderI4I4I4I4_2", (int)1, (int)2, (int)3, (int)4));
		}

		[Test]
		public void CallOrderU8()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallOrderTests", "CallOrderU8", (ulong)1, (ulong)2, (ulong)3, (ulong)4));
		}

		[Test]
		public void CallOrderU4_U8_U8_U8()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "CallOrderTests", "CallOrderU4_U8_U8_U8", (uint)1, (ulong)2, (ulong)3, (ulong)4));
		}

	}
}
