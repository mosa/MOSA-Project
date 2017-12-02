// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Expression
{
	public enum NodeType
	{
		Instruction,
		VariableConstant,
		FixedIntegerConstant,
		FixedDoubleConstant,
		VirtualRegister,
		PhyiscalRegister,
		Variable, // can be virtual/physical register or constant
		Any
	}

	public class ExpressionNode
	{
		public NodeType NodeType { get; }
		public List<ExpressionNode> ParentNodes { get; } = new List<ExpressionNode>();

		public BaseInstruction Instruction { get; }
		public InstructionSize Size { get; } = InstructionSize.None;
		public ConditionCode ConditionCode { get; set; } = ConditionCode.Undefined;

		public string Alias { get; }

		public ulong ConstantUnsignedLongInteger { get; }
		public double ConstantDouble { get; }

		public PhysicalRegister PhysicalRegister { get; }

		public ExpressionNode(BaseInstruction instruction)
		{
			NodeType = NodeType.Instruction;
			Instruction = instruction;
		}

		public ExpressionNode(BaseInstruction instruction, InstructionSize size)
		{
			NodeType = NodeType.Instruction;
			Instruction = instruction;
			Size = size;
		}

		public ExpressionNode(BaseInstruction instruction, ConditionCode condition)
		{
			NodeType = NodeType.Instruction;
			Instruction = instruction;
			ConditionCode = condition;
		}

		public ExpressionNode(BaseInstruction instruction, ConditionCode condition, InstructionSize size)
		{
			NodeType = NodeType.Instruction;
			Instruction = instruction;
			ConditionCode = condition;
			Size = size;
		}

		public ExpressionNode(ulong constant)
		{
			NodeType = NodeType.FixedIntegerConstant;
			ConstantUnsignedLongInteger = constant;
		}

		public ExpressionNode(double constant)
		{
			NodeType = NodeType.FixedDoubleConstant;
			ConstantDouble = constant;
		}

		public ExpressionNode(PhysicalRegister physicalRegister)
		{
			NodeType = NodeType.PhyiscalRegister;
			PhysicalRegister = physicalRegister;
		}

		public ExpressionNode(NodeType type, string alias = null)
		{
			Debug.Assert(type != NodeType.FixedIntegerConstant);
			Debug.Assert(type != NodeType.PhyiscalRegister);
			Debug.Assert(type != NodeType.Instruction);

			NodeType = type;
			Alias = alias;
		}

		public void AddNode(ExpressionNode node)
		{
			ParentNodes.Add(node);
		}

		protected bool ValidateInstruction(InstructionNode node)
		{
			if (node == null)
				return false;

			if (node.IsEmpty)
				return false;

			if (NodeType != NodeType.Instruction)
				return false;

			if (node.Instruction != Instruction)
				return false;

			if (Size != InstructionSize.Native && node.Size != Size)
				return false;

			if (ConditionCode != ConditionCode.Undefined && node.ConditionCode != ConditionCode)
				return false;

			return true;
		}

		protected bool ValidateOperand(InstructionNode node, int operandIndex)
		{
			if (operandIndex > node.OperandCount)
				return false;

			return ValidateOperand(node.GetOperand(operandIndex));
		}

		protected bool ValidateOperand(Operand operand)
		{
			if (operand == null)
				return false;

			if (NodeType == NodeType.Instruction)
				return false;

			if (NodeType == NodeType.PhyiscalRegister && operand.IsCPURegister && operand.Register == PhysicalRegister)
				return true;

			if (NodeType == NodeType.VirtualRegister && operand.IsVirtualRegister)
				return true;

			if (NodeType == NodeType.FixedIntegerConstant && operand.IsResolvedConstant && operand.ConstantUnsignedLongInteger == ConstantUnsignedLongInteger)
				return true;

			if (NodeType == NodeType.VariableConstant && operand.IsConstant)
				return true;

			//todo

			return false;
		}

		public bool Validate(InstructionNode node)
		{
			if (NodeType == NodeType.Instruction)
			{
				if (!ValidateInstruction(node))
					return false;

				for (int i = 0; i < ParentNodes.Count; i++)
				{
					var parentNode = ParentNodes[i];

					if (NodeType == NodeType.Any)
						continue;

					if (parentNode.NodeType == NodeType.Instruction)
					{
						var operand = node.GetOperand(i);

						if (operand.Definitions.Count != 1)
							return false;

						var parent = operand.Definitions[0];

						if (!parentNode.Validate(parent))
							return false;
					}
					else
					{
						if (!parentNode.ValidateOperand(node, i))
							return false;
					}
				}
			}

			return true;
		}

		public override string ToString()
		{
			return NodeType.ToString() + ":" + (Instruction != null ? Instruction.BaseInstructionName : string.Empty) + Alias;
		}
	}
}
