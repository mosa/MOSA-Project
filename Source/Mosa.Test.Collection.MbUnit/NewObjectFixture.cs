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

using Mosa.Test.System;

namespace Mosa.Test.Collection.MbUnit
{
	[TestFixture]
	[Importance(Importance.Critical)]

	//[Category(@"Object support")]
	//[Description(@"Tests new operator, type checking and virtual method calls.")]
	public class NewObjectFixture : TestCompilerAdapter
	{
		public NewObjectFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void NewObjectWithoutArgs()
		{
			bool result = Run<bool>("Mosa.Test.Collection", @"NewObjectTests", @"WithoutArgs");
			Assert.IsTrue(result);
		}

	}
}