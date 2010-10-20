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
	[Description(@"Tests support for the basic type System.UInt32")]
	public partial class UInt32Fixture 
	{
		private readonly ArithmeticInstructionTestRunner<uint, uint> arithmeticTests = new ArithmeticInstructionTestRunner<uint, uint>
		{
			ExpectedTypeName = @"uint",
			TypeName = @"uint"
		};

		private readonly BinaryLogicInstructionTestRunner<uint, uint, int> logicTests = new BinaryLogicInstructionTestRunner<uint, uint, int>
		{
			ExpectedTypeName = @"uint",
			TypeName = @"uint",
			ShiftTypeName = @"int",
			IncludeNot = false
		};

		private readonly ComparisonInstructionTestRunner<uint> comparisonTests = new ComparisonInstructionTestRunner<uint>
		{
			TypeName = @"uint"
		};

		private readonly SZArrayInstructionTestRunner<uint> arrayTests = new SZArrayInstructionTestRunner<uint>
		{
			TypeName = @"uint"
		};

	}
}
