// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Expression;
using Mosa.Compiler.Framework.IR;
using Mosa.Platform.ARMv6;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Workspace.Experiment.Debug
{
	internal static class Program
	{
		private static void Main()
		{
			var map = new SymbolDictionary();

			map.Add(IRInstructionList.List);

			var match = new List<string> {
				"(MulUnsigned 1 x)",
				"(AddUnsigned32(MulUnsigned x y)(MulUnsigned x z))",
				"(MulUnsigned x (AddUnsigned32 y z))",
				"(MulUnsigned 1 x)",
				"x",
				"(MulUnsigned (Const c1) (Const c2))",
				"[c1 * c2]",
				"(MulUnsigned 1 2) ",
				"[1 * 2]"
			};

			var tokenized = new List<Token>[] {
				Tokenizer.Parse(match[0], ParseType.Instructions),
				Tokenizer.Parse(match[1], ParseType.Instructions),
				Tokenizer.Parse(match[2], ParseType.Instructions),
				Tokenizer.Parse(match[3], ParseType.Instructions),
				Tokenizer.Parse(match[4], ParseType.Instructions),
				Tokenizer.Parse(match[5], ParseType.Instructions),
				Tokenizer.Parse(match[6], ParseType.Instructions),
				Tokenizer.Parse(match[7], ParseType.Instructions),
				Tokenizer.Parse(match[8], ParseType.Instructions),
			};

			return;
		}

		private static void Test()
		{
			//var tree = ExpressionTest.GetTestExpression2();
			//var basicBlocks = ExpressionTest.CreateBasicBlockInstructionSet();

			//var match = tree.Transform(basicBlocks[0].Last.Previous, null);

			//ExpressionTest.GetTestExpression5();
			//ExpressionTest.GetTestExpression4();
			//ExpressionTest.GetTestExpression3();
			//ExpressionTest.GetTestExpression2();

			return;
		}
	}
}
