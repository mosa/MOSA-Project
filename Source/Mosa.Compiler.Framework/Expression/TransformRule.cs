// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Expression
{
	public class TransformRule
	{
		public Node Match { get; }
		public ExpressionNode Condition { get; }
		public Node Result { get; }

		public TransformRule(Node match, ExpressionNode condition, Node result)
		{
			Match = match;
			Condition = condition;
			Result = result;
		}

		public bool Validate(InstructionNode node)
		{
			var variables = new ExpressionVariables();

			return Validate(node, variables);
		}

		public bool Validate(InstructionNode node, ExpressionVariables variables)
		{
			// validate the instructions and operands match
			bool match = Match.Match(node, variables);

			if (!match)
				return false;

			// validate against any criteria
			if (Condition == null)
				return true;

			var result = ExpressionEvaluator.Evaluate(Condition, variables);

			return result.IsTrue;
		}

		public Node Transform(InstructionNode node, TypeSystem typeSystem, ExpressionVariables variables)
		{
			if (!Validate(node, variables))
				return null;

			return Transform(Result, node.Block, typeSystem, variables);
		}

		protected Node Transform(Node node, BasicBlock block, TypeSystem typeSystem, ExpressionVariables variables)
		{
			var newNode = new InstructionNode(node.Instruction, block);

			// todo: determine result type
			// todo: determine size

			int operandIndex = 0;
			foreach (var nodeOperand in node.ParentNodes)
			{
				if (nodeOperand.NodeType == NodeType.Expression)
				{
					// todo: expressions
				}
				else if (nodeOperand.NodeType == NodeType.FixedIntegerConstant)
				{
					// todo
				}
				else if (nodeOperand.NodeType == NodeType.FixedDoubleConstant)
				{
					// todo
				}
				else if (nodeOperand.NodeType == NodeType.ConstantVariable)
				{
					newNode.SetOperand(operandIndex++, variables.GetOperand(nodeOperand.Alias));
					continue;
				}
				else if (nodeOperand.NodeType == NodeType.PhyiscalRegister)
				{
					var type = typeSystem.BuiltIn.I4;   // todo
					Operand.CreateCPURegister(type, node.PhysicalRegister);
					continue;
				}
				else if (nodeOperand.NodeType == NodeType.VirtualRegister)
				{
					newNode.SetOperand(operandIndex++, variables.GetOperand(nodeOperand.Alias));
					continue;
				}
				else if (nodeOperand.NodeType == NodeType.OperandVariable)
				{
					newNode.SetOperand(operandIndex++, variables.GetOperand(nodeOperand.Alias));
					continue;
				}

				throw new CompilerException("Error");
			}

			return null;
		}
	}
}
