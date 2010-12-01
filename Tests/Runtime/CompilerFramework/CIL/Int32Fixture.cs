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
	[Description(@"Tests support for the basic type System.Int32")]
	public partial class Int32Fixture
	{
		private readonly ArithmeticInstructionTestRunner<int, int> arithmeticTests = new ArithmeticInstructionTestRunner<int, int>
		{
			ExpectedType = "int",
			FirstType = "int",
			SecondType = "int"
		};

		private readonly BinaryLogicInstructionTestRunner<int, int, int> logicTests = new BinaryLogicInstructionTestRunner<int, int, int>
		{
			ExpectedType = "int",
			FirstType = "int",
			SecondType = "int",
			ShiftType = "int",
			IncludeNot = false
		};

		private readonly ComparisonInstructionTestRunner<int> comparisonTests = new ComparisonInstructionTestRunner<int>
		{
			FirstType = "int"
		};

		private readonly SZArrayInstructionTestRunner<int> arrayTests = new SZArrayInstructionTestRunner<int>
		{
			FirstType = "int"
		};

	}
}
