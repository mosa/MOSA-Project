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
	[Description(@"Tests support for the basic type System.Single")]
	public partial class FloatFixture 
	{
		private readonly ArithmeticInstructionTestRunner<float, float> arithmeticTests = new ArithmeticInstructionTestRunner<float, float>
		{
			ExpectedTypeName = @"float",
			TypeName = @"float",
			//IncludeAdd = false,
			//IncludeDiv = false,
			//IncludeMul = false,
			IncludeNeg = false,
			//IncludeRem = false,
			//IncludeSub = false
		};

		private readonly ComparisonInstructionTestRunner<float> comparisonTests = new ComparisonInstructionTestRunner<float>
		{
			TypeName = @"float"
		};

		private readonly SZArrayInstructionTestRunner<float> arrayTests = new SZArrayInstructionTestRunner<float>
		{
			TypeName = @"float"
		};
	}
}
