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
	[Description(@"Tests support for the basic type System.Int64")]
	public partial  class Int64Fixture 
	{
		private readonly ArithmeticInstructionTestRunner<long, long> arithmeticTests = new ArithmeticInstructionTestRunner<long, long>
		{
			ExpectedTypeName = @"long",
			TypeName = @"long"
		};

		private readonly BinaryLogicInstructionTestRunner<long, long, int> logicTests = new BinaryLogicInstructionTestRunner<long, long, int>
		{
			ExpectedTypeName = @"long",
			TypeName = @"long",
			ShiftTypeName = @"int",
			IncludeNot = false
		};

		private readonly ComparisonInstructionTestRunner<long> comparisonTests = new ComparisonInstructionTestRunner<long>
		{
			TypeName = @"long"
		};

		private readonly SZArrayInstructionTestRunner<long> arrayTests = new SZArrayInstructionTestRunner<long>
		{
			TypeName = @"long"
		};

	}
}
