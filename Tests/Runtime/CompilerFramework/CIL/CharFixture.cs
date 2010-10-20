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
	[Description(@"Tests support for the basic type System.Char")]
	public partial class CharFixture 
	{
		private readonly ArithmeticInstructionTestRunner<int, char> arithmeticTests = new ArithmeticInstructionTestRunner<int, char>
		{
			ExpectedTypeName = @"int",
			TypeName = @"char"
		};

		private readonly BinaryLogicInstructionTestRunner<int, char, char> logicTests = new BinaryLogicInstructionTestRunner<int, char, char>
		{
			ExpectedTypeName = @"int",
			TypeName = @"char",
			ShiftTypeName = @"char",
			IncludeComp = false,
			IncludeNot = false
		};

		private readonly ComparisonInstructionTestRunner<char> comparisonTests = new ComparisonInstructionTestRunner<char>
		{
			TypeName = @"char"
		};

		private readonly SZArrayInstructionTestRunner<char> arrayTests = new SZArrayInstructionTestRunner<char>
		{
			TypeName = @"char"
		};

	}
}
