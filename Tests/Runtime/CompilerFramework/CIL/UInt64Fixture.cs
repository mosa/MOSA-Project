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
	[Description(@"Tests support for the basic type System.UInt64")]
	public partial class UInt64Fixture
	{
		private readonly ArithmeticInstructionTestRunner<ulong, ulong> arithmeticTests = new ArithmeticInstructionTestRunner<ulong, ulong>
		{
			ExpectedTypeName = @"ulong",
			TypeName = @"ulong",
			IncludeNeg = false
		};

		private readonly BinaryLogicInstructionTestRunner<ulong, ulong, int> logicTests = new BinaryLogicInstructionTestRunner<ulong, ulong, int>
		{
			ExpectedTypeName = @"ulong",
			TypeName = @"ulong",
			ShiftTypeName = @"int",
			IncludeNot = false,
		};

		private readonly ComparisonInstructionTestRunner<ulong> comparisonTests = new ComparisonInstructionTestRunner<ulong>
		{
			TypeName = @"ulong"
		};

		private readonly SZArrayInstructionTestRunner<ulong> arrayTests = new SZArrayInstructionTestRunner<ulong>
		{
			TypeName = @"ulong"
		};

	}
}
