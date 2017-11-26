// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Experimental
{
	public enum NodeType { Instruction, VariableConstant, FixedConstant, VirtualRegister, PhyiscalRegister }

	public class ExpressionNode
	{
		public NodeType NodeType { get; }
		public List<ExpressionNode> ParentNodes { get; } = new List<ExpressionNode>();

		public BaseInstruction Instruction { get; }
		public InstructionSize Size { get; } = InstructionSize.None;
		public ConditionCode ConditionCode { get; set; } = ConditionCode.Undefined;

		public string Alias { get; }

		public ulong ConstantUnsignedLongInteger { get; }

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
			NodeType = NodeType.FixedConstant;
			ConstantUnsignedLongInteger = constant;
		}

		public ExpressionNode(PhysicalRegister physicalRegister)
		{
			NodeType = NodeType.PhyiscalRegister;
			PhysicalRegister = physicalRegister;
		}

		public ExpressionNode(NodeType type, string alias = null)
		{
			Debug.Assert(type != NodeType.FixedConstant);
			Debug.Assert(type != NodeType.PhyiscalRegister);
			Debug.Assert(type != NodeType.Instruction);

			NodeType = type;
			Alias = alias;
		}

		public void AddNode(ExpressionNode node)
		{
			ParentNodes.Add(node);
		}

		public bool Validate(InstructionNode node)
		{
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

		public bool Validate(Operand operand)
		{
			if (NodeType == NodeType.Instruction)
				return false;

			if (NodeType == NodeType.PhyiscalRegister && operand.IsCPURegister && operand.Register == PhysicalRegister)
				return true;

			if (NodeType == NodeType.VirtualRegister && operand.IsVirtualRegister)
				return true;

			if (NodeType == NodeType.FixedConstant && operand.IsResolvedConstant && operand.ConstantUnsignedLongInteger == ConstantUnsignedLongInteger)
				return true;

			if (NodeType == NodeType.VariableConstant && operand.IsConstant)
				return true;

			//todo

			return false;
		}
	}
}
