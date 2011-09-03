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

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MbUnit.Framework;

using Mosa.Test.System;
using Mosa.Test.Collection;

namespace Mosa.Test.Cases.IL
{
	[TestFixture]
	public class IsInst : TestCompilerAdapter
	{

		public IsInst()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void IsInstTest1()
		{
			Assert.AreEqual(IsInstTests.IsInstTest1(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstTest1"));
		}

		[Test]
		public void IsInstTest2()
		{
			Assert.AreEqual(IsInstTests.IsInstTest2(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstTest2"));
		}

		[Test]
		public void IsInstTest3()
		{
			Assert.AreEqual(IsInstTests.IsInstTest3(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstTest3"));
		}

		[Test]
		public void IsInstTest4()
		{
			Assert.AreEqual(IsInstTests.IsInstTest4(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstTest4"));
		}

		[Test]
		public void IsInstTest5()
		{
			Assert.AreEqual(IsInstTests.IsInstTest5(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstTest5"));
		}

		[Test]
		public void IsInstTest6()
		{
			Assert.AreEqual(IsInstTests.IsInstTest6(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstTest6"));
		}

		[Test]
		public void IsInstTest7()
		{
			Assert.AreEqual(IsInstTests.IsInstTest7(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstTest7"));
		}

		[Test]
		public void IsInstTest8()
		{
			Assert.AreEqual(IsInstTests.IsInstTest8(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstTest8"));
		}

		[Test]
		public void IsInstTest9()
		{
			Assert.AreEqual(IsInstTests.IsInstTest9(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstTest9"));
		}

		[Test]
		public void IsInstTest10()
		{
			Assert.AreEqual(IsInstTests.IsInstTest10(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstTest10"));
		}

		[Test]
		public void IsInstTest11()
		{
			Assert.AreEqual(IsInstTests.IsInstTest11(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstTest11"));
		}
	}
}
