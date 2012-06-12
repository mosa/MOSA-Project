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

		[Test]
		public void IsInstTest12()
		{
			Assert.AreEqual(IsInstTests.IsInstTest12(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstTest12"));
		}

		[Test]
		public void IsInstTest13()
		{
			Assert.AreEqual(IsInstTests.IsInstTest13(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstTest13"));
		}

		[Test]
		public void IsInstTest14()
		{
			Assert.AreEqual(IsInstTests.IsInstTest14(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstTest14"));
		}

		[Test]
		public void IsInstTest15()
		{
			Assert.AreEqual(IsInstTests.IsInstTest15(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstTest15"));
		}

		[Test]
		public void IsInstTest16()
		{
			Assert.AreEqual(IsInstTests.IsInstTest16(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstTest16"));
		}

		[Test]
		public void IsInstTest17()
		{
			Assert.AreEqual(IsInstTests.IsInstTest17(), Run<bool>("Mosa.Test.Collection", "IsInstTests", "IsInstTest17"));
		}
	}
}
