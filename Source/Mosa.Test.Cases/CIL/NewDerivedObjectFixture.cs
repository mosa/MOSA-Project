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

namespace Mosa.Test.Cases.CIL
{
	[TestFixture]
	[Importance(Importance.Critical)]

	//[Category(@"Object support")]
	//[Description(@"Tests new operator, type checking and virtual method calls.")]
	public class NewDerivedObjectFixture : TestCompilerAdapter
	{
		public NewDerivedObjectFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void NewDerivedObjectWithoutArgs()
		{
			bool result = Run<bool>("Mosa.Test.Collection", @"DerivedNewObjectTests", @"WithoutArgs");
			Assert.IsTrue(result);
		}

		[Test]
		public void NewDerivedObjectWithOneArg()
		{
			bool result = Run<bool>("Mosa.Test.Collection", @"DerivedNewObjectTests", @"WithOneArg");
			Assert.IsTrue(result);
		}

		[Test]
		public void NewDerivedObjectjWithTwoArgs()
		{
			bool result = Run<bool>("Mosa.Test.Collection", @"DerivedNewObjectTests", @"WithTwoArgs");
			Assert.IsTrue(result);
		}

		[Test]
		public void NewDerivedObjectWithThreeArgs()
		{
			bool result = Run<bool>("Mosa.Test.Collection", @"DerivedNewObjectTests", @"WithThreeArgs");
			Assert.IsTrue(result);
		}
	}
}