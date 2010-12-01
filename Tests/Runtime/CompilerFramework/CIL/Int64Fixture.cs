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
	[Description(@"Tests support for the basic type System.Int64")]
	public partial  class Int64Fixture 
	{
		private readonly ArithmeticInstructionTestRunner<long, long> arithmeticTests = new ArithmeticInstructionTestRunner<long, long>
		{
			ExpectedType = "long",
			FirstType = "long",
			SecondType = "long"
		};

		private readonly BinaryLogicInstructionTestRunner<long, long, int> logicTests = new BinaryLogicInstructionTestRunner<long, long, int>
		{
			ExpectedType = "long",
			FirstType = "long",
			SecondType = "long",
			ShiftType = "int",
			IncludeNot = false
		};

		private readonly ComparisonInstructionTestRunner<long> comparisonTests = new ComparisonInstructionTestRunner<long>
		{
			FirstType = "long"
		};

		private readonly SZArrayInstructionTestRunner<long> arrayTests = new SZArrayInstructionTestRunner<long>
		{
			FirstType = "long"
		};

	}
}
