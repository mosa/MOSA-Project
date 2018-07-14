// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Expression;
using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;

namespace Mosa.Workspace.Experiment.Debug
{
	internal static class Program
	{
		private static void Main()
		{
			var map = new SymbolDictionary();

			map.Add(IRInstructionList.List);

			var match = new List<string> {
				"(IR.MulUnsigned 1 x)",
				"(MulUnsigned 1 x)",
				"(IR.AddUnsigned32(IR.MulUnsigned x y)(IR.MulUnsigned x z))",
				"(AddUnsigned32(MulUnsigned x y)(MulUnsigned x z))",
				"(MulUnsigned x (AddUnsigned32 y z))",
				"(MulUnsigned 1 x)",
				"x",
				"(MulUnsigned (Const c1) (Const c2))",
				"[c1 * c2]",
				"(MulUnsigned 1 2) ",
				"[1 * 2]"
			};

			var tokenized = new List<List<Token>>();

			foreach (var m in match)
			{
				tokenized.Add(Tokenizer.Parse(m, m.StartsWith("(") ? ParseType.Instructions : ParseType.Expression));
			}

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
