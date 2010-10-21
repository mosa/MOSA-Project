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
	[Description(@"Tests support for the basic type System.Byte")]

	public partial class ByteFixture
	{
		private readonly ArithmeticInstructionTestRunner<int, byte> arithmeticTests = new ArithmeticInstructionTestRunner<int, byte>
		{
			ExpectedType = @"int",
			FirstType = @"byte",
			SecondType = @"byte",
		};

		private readonly BinaryLogicInstructionTestRunner<int, byte, byte> logicTests = new BinaryLogicInstructionTestRunner<int, byte, byte>
		{
			ExpectedType = @"int",
			FirstType = @"byte",
			SecondType = @"byte",
			ShiftType = @"byte",
			IncludeNot = false,
			IncludeComp = false
		};

		private readonly ComparisonInstructionTestRunner<byte> comparisonTests = new ComparisonInstructionTestRunner<byte>
		{
			FirstType = @"byte"
		};

		private readonly SZArrayInstructionTestRunner<byte> arrayTests = new SZArrayInstructionTestRunner<byte>
		{
			FirstType = @"byte"
		};

	}
}
