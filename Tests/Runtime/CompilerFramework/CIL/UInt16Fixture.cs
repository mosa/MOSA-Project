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
	[Description(@"Tests support for the basic type System.UInt16")]
	public partial class UInt16Fixture 
	{
		private readonly ArithmeticInstructionTestRunner<int, ushort> arithmeticTests = new ArithmeticInstructionTestRunner<int, ushort>
		{
			ExpectedType = @"int",
			FirstType = @"ushort",
			SecondType = @"ushort"
		};

		private readonly BinaryLogicInstructionTestRunner<int, ushort, ushort> logicTests = new BinaryLogicInstructionTestRunner<int, ushort, ushort>
		{
			ExpectedType = @"int",
			FirstType = @"ushort",
			SecondType = @"ushort",
			ShiftType = @"ushort",
			IncludeNot = false,
			IncludeComp = false
		};

		private readonly ComparisonInstructionTestRunner<ushort> comparisonTests = new ComparisonInstructionTestRunner<ushort>
		{
			FirstType = @"ushort"
		};

		private readonly SZArrayInstructionTestRunner<ushort> arrayTests = new SZArrayInstructionTestRunner<ushort>
		{
			FirstType = @"ushort"
		};

	}
}
