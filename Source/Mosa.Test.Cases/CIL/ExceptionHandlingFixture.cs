/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 *
 */

using System;
using MbUnit.Framework;

using Mosa.Test.Runtime.CompilerFramework;
using Mosa.Test.CodeDomCompiler;

namespace Mosa.Test.Cases.CIL
{
	//[TestFixture]
	public class ExceptionHandlingFixture : TestCompilerAdapter
	{

		public ExceptionHandlingFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		//[Test]
		[Row(1)]
		public void CatchException(int value)
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "ExceptionHandlingTests", "CatchException", value));
		}
	}
}