// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Expression
{
	public class TransformRule
	{
		public Node Match { get; }
		public ExpressionNode Condition { get; }
		public Node Result { get; }

		public int OperandVariableCount { get; protected set; }
		public int TypeVariableCount { get; protected set; }

		public TransformRule(Node match, ExpressionNode condition, Node result, int operandVariableCount, int typeVariableCount)
		{
			Match = match;
			Condition = condition;
			Result = result;
			TypeVariableCount = typeVariableCount;
			OperandVariableCount = operandVariableCount;
		}

		public bool Validate(InstructionNode node)
		{
			var operands = new Operand[OperandVariableCount];
			var types = new MosaType[TypeVariableCount];

			return Validate(node, operands, types);
		}

		public bool Validate(InstructionNode node, Operand[] operands, MosaType[] types)
		{
			// validate the instructions and operands match
			bool match = Match.Match(node, operands, types);

			if (!match)
				return false;

			// validate against any criteria
			if (Condition == null)
				return true;

			var result = ExpressionEvaluator.Evaluate(Condition, operands, types);

			if (result.IsTrue)
				return true;

			return false;
		}

		public InstructionNode Transform(InstructionNode node, TypeSystem typeSystem)
		{
			var operands = new Operand[OperandVariableCount];
			var types = new MosaType[TypeVariableCount];

			if (!Validate(node, operands, types))
				return null;

			return Transform(node, typeSystem, operands, types);
		}

		public InstructionNode Transform(InstructionNode baseNode, TypeSystem typeSystem, Operand[] operands, MosaType[] types)
		{
			return Transform(Result, baseNode.Block, typeSystem, operands, types);
		}

		protected InstructionNode Transform(Node node, BasicBlock block, TypeSystem typeSystem, Operand[] operands, MosaType[] types)
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
					newNode.SetOperand(operandIndex++, operands[nodeOperand.Index]);
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
					newNode.SetOperand(operandIndex++, operands[nodeOperand.Index]);
					continue;
				}
				else if (nodeOperand.NodeType == NodeType.OperandVariable)
				{
					newNode.SetOperand(operandIndex++, operands[nodeOperand.Index]);
					continue;
				}

				throw new CompilerException("Error");
			}

			return null;
		}
	}
}
