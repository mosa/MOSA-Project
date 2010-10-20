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
	[Description(@"Tests support for the basic type System.Int16")]

	public partial class Int16Fixture 
	{
		private readonly ArithmeticInstructionTestRunner<int, short> arithmeticTests = new ArithmeticInstructionTestRunner<int, short>
		{
			ExpectedTypeName = @"int",
			TypeName = @"short"
		};

		private readonly BinaryLogicInstructionTestRunner<int, short, short> logicTests = new BinaryLogicInstructionTestRunner<int, short, short>
		{
			ExpectedTypeName = @"int",
			TypeName = @"short",
			ShiftTypeName = @"short",
			IncludeNot = false,
			IncludeComp = false
		};

		private readonly ComparisonInstructionTestRunner<short> comparisonTests = new ComparisonInstructionTestRunner<short>
		{
			TypeName = @"short"
		};

		private readonly SZArrayInstructionTestRunner<short> arrayTests = new SZArrayInstructionTestRunner<short>
		{
			TypeName = @"short"
		};

	}
}
