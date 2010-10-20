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
	[Description(@"Tests support for the basic type System.SByte")]
	public partial class SByteFixture 
	{
		private readonly ArithmeticInstructionTestRunner<int, sbyte> arithmeticTests = new ArithmeticInstructionTestRunner<int, sbyte>
		{
			ExpectedTypeName = @"int",
			TypeName = @"sbyte"
		};

		private readonly BinaryLogicInstructionTestRunner<int, sbyte, sbyte> logicTests = new BinaryLogicInstructionTestRunner<int, sbyte, sbyte>
		{
			ExpectedTypeName = @"int",
			TypeName = @"sbyte",
			ShiftTypeName = @"sbyte",
			IncludeNot = false,
			IncludeComp = false
		};

		private readonly ComparisonInstructionTestRunner<sbyte> comparisonTests = new ComparisonInstructionTestRunner<sbyte>
		{
			TypeName = @"sbyte"
		};

		private readonly SZArrayInstructionTestRunner<sbyte> arrayTests = new SZArrayInstructionTestRunner<sbyte>
		{
			TypeName = @"sbyte"
		};

	}
}
