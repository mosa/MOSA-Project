// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Expression;

namespace Mosa.Workspace.Experiment.Debug
{
	internal static class Program
	{
		private static void Main()
		{
			var tree = ExpressionTest.GetTestExpression2();
			var basicBlocks = ExpressionTest.CreateBasicBlockInstructionSet();

			var match = tree.Transform(basicBlocks[0].Last.Previous, null);

			ExpressionTest.GetTestExpression5();
			ExpressionTest.GetTestExpression4();
			ExpressionTest.GetTestExpression3();
			ExpressionTest.GetTestExpression2();

			return;
		}
	}
}
