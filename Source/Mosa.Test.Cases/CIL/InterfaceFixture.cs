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

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	[Importance(Importance.Critical)]
	//[Category(@"Compiler")]
	//[Description(@"Tests support for interfaces.")]
	public class InterfaceFixture : TestCompilerAdapter
	{

		public InterfaceFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void InterfaceTest1()
		{
			Assert.AreEqual(InterfaceTests.InterfaceTest1(), Run<int>("Mosa.Test.Collection", "InterfaceTests", "InterfaceTest1"));
		}

		[Test]
		public void InterfaceTest2()
		{
			Assert.AreEqual(InterfaceTests.InterfaceTest2(), Run<int>("Mosa.Test.Collection", "InterfaceTests", "InterfaceTest2"));
		}

		[Test]
		public void InterfaceTest3()
		{
			Assert.AreEqual(InterfaceTests.InterfaceTest3(), Run<int>("Mosa.Test.Collection", "InterfaceTests", "InterfaceTest3"));
		}

	}
}
