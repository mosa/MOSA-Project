/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *
 */

using MbUnit.Framework;
using Mosa.Test.Collection;
using Mosa.Test.System;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	public class IsInstFixture : TestCompilerAdapter
	{
		public IsInstFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void IsInstAAToAA()
		{
			Assert.AreEqual(IsInstTests.IsInstAAToAA(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstAAToAA"));
		}

		[Test]
		public void IsInstBBToAA()
		{
			Assert.AreEqual(IsInstTests.IsInstBBToAA(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstBBToAA"));
		}

		[Test]
		public void IsInstCCToAA()
		{
			Assert.AreEqual(IsInstTests.IsInstCCToAA(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstCCToAA"));
		}

		[Test]
		public void IsInstCCToBB()
		{
			Assert.AreEqual(IsInstTests.IsInstCCToBB(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstCCToBB"));
		}

		[Test]
		public void IsInstDDToAA()
		{
			Assert.AreEqual(IsInstTests.IsInstDDToAA(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstDDToAA"));
		}

		[Test]
		public void IsInstDDToBB()
		{
			Assert.AreEqual(IsInstTests.IsInstDDToBB(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstDDToBB"));
		}

		[Test]
		public void IsInstDDToCC()
		{
			Assert.AreEqual(IsInstTests.IsInstDDToCC(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstDDToCC"));
		}

		[Test]
		public void IsInstAAtoIAA()
		{
			Assert.AreEqual(IsInstTests.IsInstAAtoIAA(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstAAtoIAA"));
		}

		[Test]
		public void IsInstBBToIAA()
		{
			Assert.AreEqual(IsInstTests.IsInstBBToIAA(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstBBToIAA"));
		}

		[Test]
		public void IsInstCCToIAA()
		{
			Assert.AreEqual(IsInstTests.IsInstCCToIAA(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstCCToIAA"));
		}

		[Test]
		public void IsInstCCToIBB()
		{
			Assert.AreEqual(IsInstTests.IsInstCCToIBB(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstCCToIBB"));
		}

		[Test]
		public void IsInstI4ToI4([I4] int i)
		{
			Assert.AreEqual(IsInstTests.IsInstI4ToI4(i), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstI4ToI4", i));
		}

		[Test]
		public void IsInstU4ToI4()
		{
			Assert.AreEqual(IsInstTests.IsInstU4ToI4(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstU4ToI4"));
		}

		[Test]
		public void IsInstI8ToI8()
		{
			Assert.AreEqual(IsInstTests.IsInstI8ToI8(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstI8ToI8"));
		}

		[Test]
		public void IsInstU8ToU8()
		{
			Assert.AreEqual(IsInstTests.IsInstU8ToU8(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstU8ToU8"));
		}

		[Test]
		public void IsInstI4ToU4()
		{
			Assert.AreEqual(IsInstTests.IsInstI4ToU4(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstI4ToU4"));
		}

		[Test]
		public void IsInstI1ToI1()
		{
			Assert.AreEqual(IsInstTests.IsInstI1ToI1(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstI1ToI1"));
		}

		[Test]
		public void IsInstI2ToI2()
		{
			Assert.AreEqual(IsInstTests.IsInstI2ToI2(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstI2ToI2"));
		}

		[Test]
		public void IsInstU1ToU1()
		{
			Assert.AreEqual(IsInstTests.IsInstU1ToU1(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstU1ToU1"));
		}

		[Test]
		public void IsInstU2ToU2()
		{
			Assert.AreEqual(IsInstTests.IsInstU2ToU2(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstU2ToU2"));
		}

		[Test]
		public void IsInstCToC()
		{
			Assert.AreEqual(IsInstTests.IsInstCToC(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstCToC"));
		}

		[Test]
		public void IsInstBToB([B] bool b)
		{
			Assert.AreEqual(IsInstTests.IsInstBToB(b), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstBToB", b));
		}
	}
}