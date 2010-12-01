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
	[Description(@"Tests support for the basic type System.UInt32")]
	public partial class UInt32Fixture 
	{
		private readonly ArithmeticInstructionTestRunner<uint, uint> arithmeticTests = new ArithmeticInstructionTestRunner<uint, uint>
		{
			ExpectedType = "uint",
			FirstType = "uint",
			SecondType = "uint"
		};

		private readonly BinaryLogicInstructionTestRunner<uint, uint, int> logicTests = new BinaryLogicInstructionTestRunner<uint, uint, int>
		{
			ExpectedType = "uint",
			FirstType = "uint",
			SecondType = "uint",
			ShiftType = "int",
			IncludeNot = false
		};

		private readonly ComparisonInstructionTestRunner<uint> comparisonTests = new ComparisonInstructionTestRunner<uint>
		{
			FirstType = "uint"
		};

		private readonly SZArrayInstructionTestRunner<uint> arrayTests = new SZArrayInstructionTestRunner<uint>
		{
			FirstType = "uint"
		};

	}
}
