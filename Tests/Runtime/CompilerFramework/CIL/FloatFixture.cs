/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
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
			ExpectedType = @"float",
			FirstType = @"float",
			SecondType = @"float",
			//IncludeAdd = false,
			//IncludeDiv = false,
			//IncludeMul = false,
			IncludeNeg = false,
			//IncludeRem = false,
			//IncludeSub = false
		};

		private readonly ComparisonInstructionTestRunner<float> comparisonTests = new ComparisonInstructionTestRunner<float>
		{
			FirstType = @"float"
		};

		private readonly SZArrayInstructionTestRunner<float> arrayTests = new SZArrayInstructionTestRunner<float>
		{
			FirstType = @"float"
		};
	}
}
