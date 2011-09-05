/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
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
	[Importance(Importance.Critical)]
	//[Category(@"Compiler")]
	//[Description(@"Tests support for interfaces.")]
	public class GenericInterfaceFixture : TestCompilerAdapter
	{

		public GenericInterfaceFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void GenericInterfaceTest1([I4]int a)
		{
			Assert.AreEqual(GenericInterfaceTests.InterfaceTest1(a), Run<int>("Mosa.Test.Collection", "GenericInterfaceTests", "InterfaceTest1", a));
		}

		[Test]
		public void GenericInterfaceTest2([I4]int a)
		{
			Assert.AreEqual(GenericInterfaceTests.InterfaceTest2(a), Run<int>("Mosa.Test.Collection", "GenericInterfaceTests", "InterfaceTest2", a));
		}

		[Test]
		public void GenericInterfaceTest3([I4]int a)
		{
			Assert.AreEqual(GenericInterfaceTests.InterfaceTest3(a), Run<int>("Mosa.Test.Collection", "GenericInterfaceTests", "InterfaceTest3", a));
		}
	}
}
