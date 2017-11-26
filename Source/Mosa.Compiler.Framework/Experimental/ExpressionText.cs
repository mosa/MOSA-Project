// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Experimental
{
	public class ExpressionText
	{
		public void Test1()
		{
			// (Add64 <t> (Mul64 x y) (Mul64 x z))
			// Add(Mul x y)(Mul x z)

			var nodeX = new ExpressionNode(NodeType.VirtualRegister, "x");
			var nodeY = new ExpressionNode(NodeType.VirtualRegister, "y");
			var nodeZ = new ExpressionNode(NodeType.VirtualRegister, "z");

			var instruction1 = new ExpressionNode(IRInstruction.AddUnsigned);
			var instruction2 = new ExpressionNode(IRInstruction.MulSigned);
			var instruction3 = new ExpressionNode(IRInstruction.MulSigned);

			instruction1.AddNode(instruction2);
			instruction1.AddNode(instruction3);

			instruction2.AddNode(nodeX);
			instruction2.AddNode(nodeY);

			instruction3.AddNode(nodeX);
			instruction3.AddNode(nodeZ);

			var tree = new ExpressionTree(instruction1);
		}
	}
}
