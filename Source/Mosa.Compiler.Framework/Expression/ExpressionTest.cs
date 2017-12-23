// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Expression
{
	public static class ExpressionTest
	{
		public static TransformRule GetTestExpression1()
		{
			// (Add64 <t> (Mul64 x y) (Mul64 x z))
			// (Add(Mul x y)(Mul x z))

			var nodeX = new Node(NodeType.VirtualRegister, "x");
			var nodeY = new Node(NodeType.VirtualRegister, "y");
			var nodeZ = new Node(NodeType.VirtualRegister, "z");

			var instruction1 = new Node(IRInstruction.AddUnsigned);
			var instruction2 = new Node(IRInstruction.MulSigned);
			var instruction3 = new Node(IRInstruction.MulSigned);

			instruction1.AddNode(instruction2);
			instruction1.AddNode(instruction3);

			instruction2.AddNode(nodeX);
			instruction2.AddNode(nodeY);

			instruction3.AddNode(nodeX);
			instruction3.AddNode(nodeZ);

			var tree = new TransformRule(instruction1, null);

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

			context.AppendInstruction(IRInstruction.MulSigned, t1, x, y);
			context.AppendInstruction(IRInstruction.MulSigned, t2, x, z);
			context.AppendInstruction(IRInstruction.AddUnsigned, r, t1, t2);

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
			var expression = GetTestExpression("(AddUnsigned(MulUnsigned x y)(MulUnsigned x z))");

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
