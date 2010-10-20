/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (aka grover, <mailto:sharpos@michaelruck.de>)
 *  
 */

using System;

using MbUnit.Framework;

namespace Test.Mosa.Runtime.CompilerFramework.CLI
{
	[TestFixture]
	[Importance(Importance.Critical)]
	[Category(@"Basic types")]
	[Description(@"Tests support for the basic type System.Double")]
	public partial class DoubleFixture
	{
		private readonly ArithmeticInstructionTestRunner<double, double> arithmeticTests = new ArithmeticInstructionTestRunner<double, double>
		{
			ExpectedTypeName = @"double",
			TypeName = @"double",
			IncludeRem = false
		};

		private readonly ComparisonInstructionTestRunner<double> comparisonTests = new ComparisonInstructionTestRunner<double>
		{
			TypeName = @"double"
		};

		private readonly SZArrayInstructionTestRunner<double> arrayTests = new SZArrayInstructionTestRunner<double>
		{
			TypeName = @"double"
		};

	}
}
