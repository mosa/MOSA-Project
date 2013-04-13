/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 *
 */

using MbUnit.Framework;
using Mosa.Test.Collection;
using Mosa.Test.System;

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	public class ExceptionHandlingFixture : TestCompilerAdapter
	{
		public ExceptionHandlingFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void TryFinally1()
		{
			Assert.AreEqual(ExceptionHandlingTests.TryFinally1(), Run<int>("Mosa.Test.Collection", "ExceptionHandlingTests", "TryFinally1"));
		}

		[Test]
		public void TryFinally2()
		{
			Assert.AreEqual(ExceptionHandlingTests.TryFinally2(), Run<int>("Mosa.Test.Collection", "ExceptionHandlingTests", "TryFinally2"));
		}

		[Test]
		public void TryFinally3()
		{
			Assert.AreEqual(ExceptionHandlingTests.TryFinally3(), Run<int>("Mosa.Test.Collection", "ExceptionHandlingTests", "TryFinally3"));
		}

		[Test]
		public void TryFinally4()
		{
			Assert.AreEqual(ExceptionHandlingTests.TryFinally4(), Run<int>("Mosa.Test.Collection", "ExceptionHandlingTests", "TryFinally4"));
		}
	}
}