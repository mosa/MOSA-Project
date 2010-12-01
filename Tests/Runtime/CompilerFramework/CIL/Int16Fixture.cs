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
	[Description(@"Tests support for the basic type System.Int16")]

	public partial class Int16Fixture 
	{
		private readonly ArithmeticInstructionTestRunner<int, short> arithmeticTests = new ArithmeticInstructionTestRunner<int, short>
		{
			ExpectedType = "int",
			FirstType = @"short",
			SecondType = @"short"
		};

		private readonly BinaryLogicInstructionTestRunner<int, short, short> logicTests = new BinaryLogicInstructionTestRunner<int, short, short>
		{
			ExpectedType = "int",
			FirstType = @"short",
			SecondType = @"short",
			ShiftType = @"short",
			IncludeNot = false,
			IncludeComp = false
		};

		private readonly ComparisonInstructionTestRunner<short> comparisonTests = new ComparisonInstructionTestRunner<short>
		{
			FirstType = @"short"
		};

		private readonly SZArrayInstructionTestRunner<short> arrayTests = new SZArrayInstructionTestRunner<short>
		{
			FirstType = @"short"
		};

	}
}
