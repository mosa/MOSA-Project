// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;

namespace Mosa.Compiler.Framework.Expression
{
	public static class ExpressionTest
	{
		public static TransformRule GetTestExpression1()
		{
			// (Add(Mul x y)(Mul x z))

			var nodeX = new Node(NodeType.OperandVariable, "x", 0);
			var nodeY = new Node(NodeType.OperandVariable, "y", 1);
			var nodeZ = new Node(NodeType.OperandVariable, "z", 2);

			//var nodeW = new Node(NodeType.OperandVariable, "w", 3);

			var instruction1 = new Node(IRInstruction.Add32);
			var instruction2 = new Node(IRInstruction.MulUnsigned32);
			var instruction3 = new Node(IRInstruction.MulUnsigned32);

			instruction1.AddNode(instruction2);
			instruction1.AddNode(instruction3);

			instruction2.AddNode(nodeX);
			instruction2.AddNode(nodeY);

			instruction3.AddNode(nodeX);
			instruction3.AddNode(nodeZ);

			var tree = new TransformRule(instruction1, null, null, 4, 0);

			return tree;
		}

		public static BasicBlocks CreateBasicBlockInstructionSet()
		{
			var basicBlocks = new BasicBlocks();

			var block = basicBlocks.CreateBlock();
			basicBlocks.AddHeadBlock(block);

			var context = new Context(block);

			var x = Operand.CreateVirtualRegister(null, 1);
			var y = Operand.CreateVirtualRegister(null, 2);
			var z = Operand.CreateVirtualRegister(null, 3);
			var r = Operand.CreateVirtualRegister(null, 4);
			var t1 = Operand.CreateVirtualRegister(null, 5);
			var t2 = Operand.CreateVirtualRegister(null, 6);
			var w = Operand.CreateVirtualRegister(null, 7);

			context.AppendInstruction(IRInstruction.MulUnsigned32, t1, x, y);
			context.AppendInstruction(IRInstruction.MulUnsigned32, t2, x, z);
			context.AppendInstruction(IRInstruction.Add32, r, t1, t2);

			return basicBlocks;
		}

		private static TransformRule GetTestExpression(string text)
		{
			var builder = new ExpressionBuilder();

			builder.AddInstructions(IRInstructionMap.Map);

			//builder.AddPhysicalRegisters();

			var expression = builder.CreateExpressionTree(text);

			return expression;
		}

		public static TransformRule GetTestExpression2()
		{
			var expression = GetTestExpression("(AddUnsigned32(MulUnsigned x y)(MulUnsigned x z)) -> (MulUnsigned x (AddUnsigned32 y z))");

			return expression;
		}

		public static TransformRule GetTestExpression3()
		{
			var expression = GetTestExpression("(MulUnsigned 1 x) -> x");

			return expression;
		}

		public static TransformRule GetTestExpression4()
		{
			var expression = GetTestExpression("(MulUnsigned (Const c1) (Const c2)) -> [c1 * c2]");

			return expression;
		}

		public static TransformRule GetTestExpression5()
		{
			var expression = GetTestExpression("(MulUnsigned 1 2) -> [1 * 2]");

			return expression;
		}
	}
}
