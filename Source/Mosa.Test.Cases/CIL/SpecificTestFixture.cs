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
	public class SpecificTestFixture : TestCompilerAdapter
	{

		public SpecificTestFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void Test1([I4Small] uint value)
		{
			Assert.AreEqual(SpecificTests.Test1(value), Run<uint>("Mosa.Test.Collection", "SpecificTests", "Test1", value));
		}
	}
}