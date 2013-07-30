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
	public class GeneralFixture : TestCompilerAdapter
	{
		public GeneralFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void VariablePressure8()
		{
			Assert.AreEqual(GeneralTests.VariablePressure8(), Run<int>("Mosa.Test.Collection", "GeneralTests", "VariablePressure8"));
		}
	}
}